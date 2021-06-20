using Reactor;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
#if DEBUG
using CrowdedRoles.Attributes;
#endif
using CrowdedRoles.Options;
using HarmonyLib;
using CmdsRoles;
using Reactor.Networking;
using System;
using UnityEngine;
using UnhollowerBaseLib.Attributes;
using Hazel.Udp;
using Reactor.Extensions;
using static CrowdedRoles.CrowdedRoles;
using CrowdedRoles.Extensions;
using CrowdedRoles.Components;

namespace CrowdedRoles
{
    [BepInPlugin(Id)]
    [BepInDependency(ReactorPlugin.Id)]
    [ReactorPluginSide(PluginSide.ClientOnly)]
    public class RoleApiPlugin : BasePlugin
    {
        public const string Id = "xyz.crowdedmods.crowdedroles";
        private Harmony Harmony { get; } = new(Id);
        public static ManualLogSource Logger { get; private set; } = null!;

        public override void Load()
        {

            BepInPlugin metadata = MetadataHelper.GetMetadata(this);
            OptionsManager.SaveOptionsFile = new ConfigFile(Utility.CombinePaths(Paths.ConfigPath, Id + ".options.cfg"), false, metadata);
            Harmony.Unpatch(typeof(UdpConnection).GetMethod("HandleSend"), HarmonyPatchType.Prefix,
    ReactorPlugin.Id);
            var gameObject = new GameObject(nameof(ReactorPlugin)).DontDestroy();
            gameObject.AddComponent<UpdateClass>().Plugin = this;
#if DEBUG
            RegisterCustomRoleAttribute.Register(this);
            RoleActive.RegisterOptions(this);
            RoleSettings.RegisterOptions(this);
            RegisterCustomGameOverReasonAttribute.Register(this);
#endif
            Harmony.PatchAll();
            Logger = Log;
        }


    }
    [RegisterInIl2Cpp]
    public class UpdateClass : MonoBehaviour
    {
        [HideFromIl2Cpp]
        public RoleApiPlugin Plugin { get; internal set; }

        public UpdateClass(IntPtr ptr) : base(ptr)
        {
        }
        public void Update()
        {
            if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
            {

                if (RoleStuff.isProtected == true)
                {

                    if (PlayerControl.LocalPlayer.Data.IsDead == true)
                    {
                        PlayerControl.LocalPlayer.Revive();
                        PlayerControl.LocalPlayer.nameText.color = new Color((0.0f / 0.0f), (232.0f / 232.0f), (46.0f / 46.0f), 1);
                        Rpc<Revive>.Instance.Send(new Revive.Data(PlayerControl.LocalPlayer.PlayerId, 1, new Vector2(0, 0)));
                        RoleStuff.isProtected = false;

                    }
                }
                if (PlayerControl.LocalPlayer.Data.IsDead)
                {
                    RoleStuff.DeadTime += Time.deltaTime;
                } else
                {
                    RoleStuff.DeadTime = 0f;
                }
                if (PlayerControl.LocalPlayer.Data.IsDead)
                {
                    RoleStuff.TrackingPerson = PlayerControl.LocalPlayer.PlayerId;
                }
                if (PlayerControl.LocalPlayer.Is<Shrinker>() && RoleStuff.ShrinkerShrunken)
                {
                    Rpc<TeleportPlayer>.Instance.Send(new TeleportPlayer.Data(PlayerControl.LocalPlayer.transform.position, PlayerControl.LocalPlayer.PlayerId));
                }
                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started || MeetingHud.Instance)
                {
                    RoleStuff.Blinded = false;
                }
                if (RoleStuff.Tracking)
                {
                    HudManager.Instance.PlayerCam.Target = GameData.Instance.GetPlayerById(RoleStuff.TrackingPerson).Object;
                    PlayerControl.LocalPlayer.myLight.transform.position = GameData.Instance.GetPlayerById(RoleStuff.TrackingPerson).Object.transform.position;
                }

                if (RoleStuff.isCursed == true)
                {
                    
                    var closestplayer = PlayerControl.LocalPlayer.FindClosestTarget();
                    if (closestplayer != null && closestplayer.Data.IsImpostor == false && closestplayer.Data.IsDead == false)
                    {
                        closestplayer.nameText.color = new Color((0.0f), (0.0f), (0.0f), 1);
                        Rpc<Kill>.Instance.Send(new Kill.Data(closestplayer.PlayerId));
                        PlayerControl.LocalPlayer.StartCoroutine(PlayerControl.LocalPlayer.KillAnimations.Random<KillAnimation>().CoPerformKill(PlayerControl.LocalPlayer, closestplayer));
                        RoleStuff.isCursed = false;

                    }
                }
                if (RoleStuff.RevengeIsStunned)
                {
                    PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(RoleStuff.RevengeStunPos);
                }
                if (RoleStuff.ElecIsStunned)
                {
                    PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(RoleStuff.ElecStunPos);
                }
                if (RoleStuff.BlindTimer > 0)
                {
                    RoleStuff.BlindTimer -= Time.deltaTime;
                } else if (RoleStuff.ReadyToBlind == true)
                {
                    Rpc<Blind>.Instance.Send(new Blind.Data(RoleStuff.BlindedPerson));
                    RoleStuff.ReadyToBlind = false;
                }
                if (RoleStuff.AntiKillTimer > 0)
                {
                    RoleStuff.AntiKillTimer -= Time.deltaTime;
                }
                if (PlayerControl.LocalPlayer.Is<Hacker>())
                {
                    foreach (GameObject i in RoleStuff.ArrowList)
                    {
                        i.GetComponent<ArrowBehaviour>().target = RoleStuff.PlayerList[RoleStuff.ArrowList.IndexOf(i)].transform.position;
                        Vector3 pos = PlayerControl.LocalPlayer.transform.position;
                        i.GetComponent<ArrowBehaviour>().transform.position = new Vector3(Math.Clamp(pos.x, pos.x - 1, pos.x + 1), Math.Clamp(pos.y, pos.y - 1, pos.y + 1), pos.z);
                    }
                }

                var closestarget = PlayerControl.LocalPlayer.FindClosestTarget();
                if (closestarget == null)
                {
                    HudPatch.Curse.CanUse_ = false;
                    HudPatch.Dissapear.CanUse_ = false;
                    HudPatch.Blind.CanUse_ = false;
                }
                else
                {
                    HudPatch.Curse.CanUse_ = true;
                    HudPatch.Dissapear.CanUse_ = true;
                    HudPatch.Blind.CanUse_ = true;
                }
                var closestargetall = RoleStuff.FindClosestTargetAll();
                if (closestargetall == null)
                {
                    HudPatch.Protect.CanUse_ = false;
                    HudPatch.Teleport.CanUse_ = false;
                    HudPatch.Track.CanUse_ = false;
                    HudPatch.Stun.CanUse_ = false;
                    HudPatch.TpToPlayer.CanUse_ = false;
                    HudPatch.TpToPlayerSelect.CanUse_ = false;
                    HudPatch.Copy.CanUse_ = false;
                    HudPatch.Electrocute.CanUse_ = false;
                    HudPatch.DetSearch.CanUse_ = false;
                    HudPatch.HelpSearch.CanUse_ = false;
                    HudPatch.Shoot.CanUse_ = false;
                    HudPatch.Rubberband.CanUse_ = false;
                }
                else
                {
                    HudPatch.Protect.CanUse_ = true;
                    HudPatch.Teleport.CanUse_ = true;
                    HudPatch.Track.CanUse_ = true;
                    HudPatch.Stun.CanUse_ = true;
                    HudPatch.TpToPlayer.CanUse_ = true;
                    HudPatch.Copy.CanUse_ = true;
                    HudPatch.Electrocute.CanUse_ = true;
                    HudPatch.DetSearch.CanUse_ = true;
                    HudPatch.HelpSearch.CanUse_ = true;
                    HudPatch.TpToPlayerSelect.CanUse_ = true;
                    HudPatch.Shoot.CanUse_ = true;
                    HudPatch.Rubberband.CanUse_ = true;
                }
                var detfindclosest = RoleStuff.FindClosestTargetDet();
                if (detfindclosest == null)
                {
                    if (RoleStuff.SearchingPerson != null && (PlayerControl.LocalPlayer.Is<Detective>() || PlayerControl.LocalPlayer.Is<Helper>()))
                    {
                        RoleStuff.SearchingPerson = null;
                        RoleStuff.ResetButton(HudPatch.DetSearch);
                        RoleStuff.ResetButton(HudPatch.HelpSearch);
                    }
                }
                if (PlayerControl.LocalPlayer.Is<Doctor>())
                {
                    var closebody = RoleStuff.ClosestBody(PlayerControl.LocalPlayer.MaxReportDistance);
                    if (closebody == null)
                    {
                        HudPatch.Revive.CanUse_ = false;
                        if (RoleStuff.Reviving)
                        {
                            RoleStuff.ResetButton(HudPatch.Revive);
                            RoleStuff.Reviving = false;
                        }
                    } else
                    {
                        HudPatch.Revive.CanUse_ = true;
                    }
                }
                if (PlayerControl.LocalPlayer.Is<Vampire>() && RoleStuff.MaxVampireCooldown > 3)
                {
                    if (PlayerControl.LocalPlayer.killTimer > RoleStuff.MaxVampireCooldown)
                    {
                        PlayerControl.LocalPlayer.SetKillTimer(RoleStuff.MaxVampireCooldown);
                    }
                }
                


                //if (CrowdedRoles.askForCooldown == true)
                //{
                //    CmdsRoles.HudPatch.Curse.IsEffectActive = false;
                //    CmdsRoles.HudPatch.Curse.KillButtonManager.TimerText.Color = new Color((255f), (255f), (255f), 1);
                //    CmdsRoles.HudPatch.Curse.Timer = 30f;
                //    
                //    CrowdedRoles.askForCooldown = false;
                //}
            }
        }
        
        
    }
    
}