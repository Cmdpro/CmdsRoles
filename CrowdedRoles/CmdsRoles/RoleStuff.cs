using HarmonyLib;
using System;
using System.Text;
using UnityEngine;
using Il2CppSystem.Collections;
using System.Collections.Generic;
using Reactor.Button;
using Assets.CoreScripts;
using CrowdedRoles.Extensions;
using Reactor.Networking;
using CmdsRoles;
using CrowdedRoles.Components;
using static CrowdedRoles.CrowdedRoles;
using System.Linq;
using CrowdedRoles.Roles;

namespace CrowdedRoles
{
    static class RoleStuff
    {

        public static bool isProtected;
        public static bool hasProtected;
        public static byte protectedID;
        public static bool isCursed;
        public static bool NinjaSelected;
        public static bool Invisible;
        public static bool Tracking;
        public static byte TrackingPerson;
        public static byte RoleId;
        public static bool hasSelected;
        public static PlayerControl? Selected;
        public static PlayerControl? RevengeSelected;
        public static bool DevMode;
        public static uint oldpet;
        public static bool OutOfBody;
        public static bool ElecIsStunned;
        public static bool RevengeIsStunned;
        public static byte StunnedPerson;
        public static List<PlayerControl> Poisoned = new List<PlayerControl>();
        public static Vector2 RevengeStunPos;
        public static Vector2 ElecStunPos;
        public static float OldSpeedTroll;
        public static bool ReflectorReflecting;
        public static float AntiKillTimer;
        public static PlayerControl? SearchingPerson;
        public static float OldReportRadius;
        public static Vent? lastVent;
        public static Color BlindOriginalColor;
        public static byte BlindedPerson;
        public static float BlindTimer;
        public static bool ReadyToBlind;
        public static bool Blinded;
        public static bool Reviving;
        public static float DeadTime = 0f;
        public static string OldName = "";
        public static byte MeetingPlayer = 255;
        public static float MaxVampireCooldown;
        public static byte Copier = 255;
        public static bool BasicButtonConditions = false;
        public static Vector2 OldSize;
        public static float OldSpeed;
        public static bool ShrinkerShrunken;
        public static Vector2 OldPos;
        public static PlayerControl? TrollRubberbandPlayer;
        public static float PortalImmuneTime;
        public static List<ArrowBehaviour> ArrowList = new List<ArrowBehaviour>();
        public static List<PlayerControl> PlayerList = new List<PlayerControl>();
        public static GameObject? Portal1;
        public static GameObject? Portal2;
        public static float TrollSpeed;
        public static float ShrinkerSpeed;
        public static bool TrollSpeedActive;
        public static bool ShrinkerSpeedActive;
        public static PlayerControl? InvisibleNinja;

        public static Sprite ConvertToSprite(Byte[] bytes, int PixelsPerUnit, Vector2 pivot)
        {
            // create a Texture2D object that is used to stream data into Texture2D
            Texture2D texture = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture, bytes); // stream data into Texture2D
                                                       // Create a Sprite, to Texture2D object basis
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, PixelsPerUnit);
            return sp;
        }
        public static PlayerControl FindRole<T>() where T : BaseRole
        {
            foreach (PlayerControl i in PlayerControl.AllPlayerControls)
            {
                if (i.Is<T>())
                {
                    return i;
                }
            }
            return null;
        }
        public static void StartGameStuff()
        {
            BothGameStuff();
            
        }
        public static void BothGameStuff()
        {
            RoleStuff.Blinded = false;
            RoleStuff.ElecIsStunned = false;
            RoleStuff.RevengeIsStunned = false;
            RoleStuff.Tracking = false;
            RoleStuff.PlayerList.Clear();
            RoleStuff.ArrowList.Clear();
            PlayerControl.LocalPlayer.moveable = true;
        }
        public static void EndGameStuff()
        {
            BothGameStuff();
            if (TrollSpeedActive)
            {
                Rpc<SetSpeed>.Instance.Send(new SetSpeed.Data(RoleStuff.OldSpeedTroll));
            }
            if (ShrinkerShrunken)
            {
                PlayerControl.GameOptions.PlayerSpeedMod = OldSpeed;
            }
        }
        public static void DoClientKillStuff(byte target)
        {
            if (PlayerControl.LocalPlayer.Is<Vampire>())
            {
                if (RoleStuff.MaxVampireCooldown > RoleSettings.VampireMin.Value)
                {
                    RoleStuff.MaxVampireCooldown -= 1;
                }
                if (RoleStuff.MaxVampireCooldown > RoleSettings.VampireMin.Value)
                {
                    RoleStuff.MaxVampireCooldown -= 1;
                }
            }
            PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
            HudPatch.Dissapear.Timer = HudPatch.Dissapear.MaxTimer;
            HudPatch.Curse.Timer = HudPatch.Curse.MaxTimer;
            HudPatch.Shapeshift.Timer = HudPatch.Shapeshift.MaxTimer;
        }
        public static void DoServerKillStuff(byte target)
        {

        }
        public static void ResetButton(CooldownButton button)
        {
            float SearchOriginalEffectDuration = button.EffectDuration;
            button.EffectDuration = 0;
            button.IsEffectActive = false;
            button.Timer = 0.01f;
            button.EffectDuration = SearchOriginalEffectDuration;
        }
        public static bool ClientSendChat(string chatText)
        {
            if (string.IsNullOrWhiteSpace(chatText))
            {
                return false;
            }
            if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance)
            {
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, chatText);
            }
            if (chatText.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                DestroyableSingleton<Telemetry>.Instance.SendWho();
            }
            return true;
        }
        public static DeadBody ClosestBody(float FindRadius)
        {
            foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), FindRadius, Constants.PlayersOnlyMask))
            {
                if (!(collider2D.tag != "DeadBody"))
                {
                    DeadBody component = collider2D.GetComponent<DeadBody>();
                    if (component && !component.Reported)
                    {
                        return component;
                    }
                }
            }
            return null;
        }

        public static PlayerControl FindClosestTargetAll()
        {
            PlayerControl result = null;
            float num = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
            if (!ShipStatus.Instance)
            {
                return null;
            }
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                GameData.PlayerInfo playerInfo = allPlayers[i];

                if (!playerInfo.Disconnected && playerInfo.PlayerId != PlayerControl.LocalPlayer.PlayerId && !playerInfo.IsDead)
                {
                    PlayerControl @object = playerInfo.Object;
                    if (@object)
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            return result;
        }
        public static PlayerControl FindClosestCrewmate()
        {
            PlayerControl result = null;
            float num = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
            if (!ShipStatus.Instance)
            {
                return null;
            }
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                GameData.PlayerInfo playerInfo = allPlayers[i];

                if (!playerInfo.Disconnected && playerInfo.PlayerId != PlayerControl.LocalPlayer.PlayerId && !playerInfo.IsDead && !playerInfo.IsImpostor)
                {
                    PlayerControl @object = playerInfo.Object;
                    if (@object)
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            return result;
        }
        public static PlayerControl FindClosestTargetDet()
        {
            PlayerControl result = null;
            float num = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
            if (!ShipStatus.Instance)
            {
                return null;
            }
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                GameData.PlayerInfo playerInfo = allPlayers[i];

                if (!playerInfo.Disconnected && playerInfo.PlayerId != PlayerControl.LocalPlayer.PlayerId && !playerInfo.IsDead && SearchingPerson == playerInfo.Object)
                {
                    PlayerControl @object = playerInfo.Object;
                    if (@object)
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            return result;
        }
        
    }
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    class CheckEndCriteriaPatch
    {
        
        public static bool Prefix(ShipStatus __instance)
        {
            Criteria(__instance);
            if (__instance.gameObject.GetComponent<InnerNet.InnerNetObject>() == null)
            {
                UnityEngine.Debug.Log("There is no component");
            }
            return false;
        }
        public static void Criteria(ShipStatus __instance)
        {
            if (!GameData.Instance)
            {
                return;
            }
            if (__instance.Systems.ContainsKey(SystemTypes.LifeSupp))
            {
                LifeSuppSystemType lifeSuppSystemType = __instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                if (lifeSuppSystemType.Countdown < 0f)
                {
                    __instance.EndGameForSabotage();
                    lifeSuppSystemType.Countdown = 10000f;
                }
            }
            ISystemType systemType2;

            if ((__instance.Systems.ContainsKey(SystemTypes.Reactor) &&
                 (systemType2 = __instance.Systems[SystemTypes.Reactor]) != null || __instance.Systems.ContainsKey(SystemTypes.Laboratory) && (systemType2 = __instance.Systems[SystemTypes.Laboratory]) != null) && systemType2.TryCast<ICriticalSabotage>() != null)
            {
                ICriticalSabotage criticalSabotage = systemType2.Cast<ICriticalSabotage>();
                if (criticalSabotage.Countdown < 0f)
                {
                    __instance.EndGameForSabotage();
                    criticalSabotage.ClearSabotage();
                }
            }
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            for (int i = 0; i < GameData.Instance.PlayerCount; i++)
            {
                GameData.PlayerInfo playerInfo = GameData.Instance.AllPlayers[i];
                if (!playerInfo.Disconnected)
                {
                    if (playerInfo.IsImpostor)
                    {
                        num3++;
                    }
                    if (!playerInfo.IsDead)
                    {
                        if (playerInfo.IsImpostor)
                        {
                            num2++;
                        }
                        else
                        {
                            num++;
                        }
                    }
                }
            }
            if (num2 <= 0 && (!DestroyableSingleton<TutorialManager>.InstanceExists || num3 > 0))
            {
                if (!DestroyableSingleton<TutorialManager>.InstanceExists)
                {
                    __instance.gameObject.GetComponent<InnerNet.InnerNetObject>().enabled = false;
                    ShipStatus.RpcEndGame(GameOverReason.HumansByVote, !SaveManager.BoughtNoAds);
                    return;
                }
                DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverImpostorDead, Array.Empty<Il2CppSystem.Object>()));
                ShipStatus.ReviveEveryone();
                return;
            }
            else
            {
                if (num > num2)
                {
                    if (!DestroyableSingleton<TutorialManager>.InstanceExists)
                    {
                        if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
                        {
                            __instance.gameObject.GetComponent<InnerNet.InnerNetObject>().enabled = false;
                            ShipStatus.RpcEndGame(GameOverReason.HumansByTask, !SaveManager.BoughtNoAds);
                            return;
                        }
                    }
                    else if (PlayerControl.LocalPlayer.myTasks.ToArray().All((PlayerTask t) => t.IsComplete))
                    {
                        DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverTaskWin, Array.Empty<Il2CppSystem.Object>()));
                        __instance.Begin();
                    }
                    return;
                }
                if (num2 <= num)
                {
                    __instance.gameObject.GetComponent<InnerNet.InnerNetObject>().enabled = false;
                    GameOverReason endReason;
                    switch (TempData.LastDeathReason)
                    {
                        case DeathReason.Exile:
                            endReason = GameOverReason.ImpostorByVote;
                            break;
                        case DeathReason.Kill:
                            endReason = GameOverReason.ImpostorByKill;
                            break;
                        default:
                            endReason = GameOverReason.ImpostorByVote;
                            break;
                    }
                    if (!RoleStuff.FindRole<Survivor>().Data.IsDead)
                    {
                        PlayerControl.LocalPlayer.RpcCustomEndGame<Survivor.SurvivorWon>();
                    }
                    else
                    {
                        ShipStatus.RpcEndGame(endReason, !SaveManager.BoughtNoAds);
                    }
                    return;
                }
                DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverImpostorKills, Array.Empty<Il2CppSystem.Object>()));
                ShipStatus.ReviveEveryone();
                return;
            }
        }
    }
}
