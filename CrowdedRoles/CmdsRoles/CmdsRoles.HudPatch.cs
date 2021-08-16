using HarmonyLib;
using Reactor.Button;
using UnityEngine;
using CmdsRoles;
using System;
using System.Collections;
using static CrowdedRoles.CrowdedRoles.Buttons;
using CrowdedRoles.Components;

namespace CrowdedRoles
{
    public partial class CrowdedRoles
    {
        public static class HudPatch
        {
            
            public static CooldownButton Stun;
            public static CooldownButton Protect;
            public static CooldownButton Curse;
            public static CooldownButton Track;
            public static CooldownButton Teleport;
            public static CooldownButton AntiKill;
            public static CooldownButton TpToPlayer;
            public static CooldownButton Dissapear;
            public static CooldownButton Copy;
            public static CooldownButton Electrocute;
            public static CooldownButton DetSearch;
            public static CooldownButton TpToPlayerSelect;
            public static CooldownButton Blind;
            public static CooldownButton HelpSearch;
            public static CooldownButton PlaceVent;
            public static CooldownButton Shoot;
            public static CooldownButton Revive;
            public static CooldownButton Shrink;
            public static CooldownButton Slow;
            public static CooldownButton Rubberband;
            public static CooldownButton Reflect;
            public static CooldownButton Hack;
            public static CooldownButton Shapeshift;
            public static CooldownButton NinjaInvis;
            public static CooldownButton BodyFind;

            [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
            public static class HudManagerStart
            {
                public static void Postfix(HudManager __instance)
                {
                    
                    Stun = StunPlayerButton.button(__instance);
                    Protect = ProtectButton.button(__instance);
                    Curse = CurseButton.button(__instance);
                    Track = TrackingButton.button(__instance);
                    Teleport = TeleportButton.button(__instance);
                    Dissapear = DissapearButton.button(__instance);
                    AntiKill = DisableKillButton.button(__instance);
                    TpToPlayerSelect = TeleportToPersonSelectButton.button(__instance);
                    TpToPlayer = TeleportToPersonButton.button(__instance);
                    Copy = CopyButton.button(__instance);
                    Electrocute = ElectrocuteButton.button(__instance);
                    DetSearch = DetSearchButton.button(__instance);
                    HelpSearch = HelpSearchButton.button(__instance);
                    Blind = BlindPlayerButton.button(__instance);
                    PlaceVent = PlaceVentButton.button(__instance);
                    Shoot = ShootButton.button(__instance);
                    Revive = ReviveButton.button(__instance);
                    Shrink = ShrinkButton.button(__instance);
                    Slow = SlowButton.button(__instance);
                    Rubberband = RubberbandButton.button(__instance);
                    Reflect = ReflectButton.button(__instance);
                    Hack = HackButton.button(__instance);
                    Shapeshift = ShapeshiftButton.button(__instance);
                    NinjaInvis = InvisibleButton.button(__instance);
                    BodyFind = BodyFindButton.button(__instance);
                }
            }

            /*[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
            public static class VotePatch
            {
                public static void Postfix(MeetingHud __instance)
                {
                    if (RoleSettings.ExitMeetingVote.Value)
                    {
                        GameData.PlayerInfo data = PlayerControl.LocalPlayer.Data;
                        DestroyableSingleton<HudManager>.Instance.Chat.SetPosition(null);
                        DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(data.IsDead);
                        DestroyableSingleton<HudManager>.Instance.Chat.BanButton.Hide();
                    }
                }
            }*/
        }
    }
}
