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

        public static Sprite ConvertToSprite(Byte[] bytes, int PixelsPerUnit, Vector2 pivot)
        {
            // create a Texture2D object that is used to stream data into Texture2D
            Texture2D texture = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture, bytes); // stream data into Texture2D
                                                       // Create a Sprite, to Texture2D object basis
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, PixelsPerUnit);
            return sp;
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
}
