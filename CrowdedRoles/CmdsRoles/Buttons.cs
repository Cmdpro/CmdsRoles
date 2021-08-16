using System;
using System.Collections.Generic;
using System.Text;
using Reactor;
using CrowdedRoles.Rpc;
using Reactor.Networking;
using CrowdedRoles.Extensions;
using CrowdedRoles;
using UnityEngine;
using CmdsRoles;
using CrowdedRoles.Attributes;
using CrowdedRoles.GameOverReasons;
using System.Linq;
using BepInEx.IL2CPP;
using CrowdedRoles.Roles;

namespace CrowdedRoles
{
    public partial class CrowdedRoles
    {

        public static class Buttons
        {
            

            public static class CurseButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = PlayerControl.LocalPlayer.FindClosestTarget();
                            if (closestplr != null)
                            {
                                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                                Rpc<Curse>.Instance.Send(new Curse.Data(closestplr.PlayerId));
                            }
                        },

                        cooldown: 30f,
                        image: Properties.Resources.Curse,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Curser>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }
            public static class DisableKillButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                Rpc<AntiKill>.Instance.Send(new AntiKill.Data(closestplr.PlayerId));
                            }
                        },

                        cooldown: 15f,
                        image: Properties.Resources.AntiKill,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Revenger>() && PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;


                        },
                        hudManager: hudManager,
                        effectDuration: 10f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }
            public static class TeleportToPersonSelectButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                RoleStuff.RevengeSelected = closestplr;
                            }
                        },

                        cooldown: 45f,
                        image: Properties.Resources.SelectTeleport,
                        positionOffset: new UnityEngine.Vector2(1.5f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Revenger>() && PlayerControl.LocalPlayer.Data.IsDead && RoleStuff.RevengeSelected == null && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;


                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }
            public static class TeleportToPersonButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                Vector3 pos = closestplr.transform.position;
                                Rpc<TeleportPersonToPerson>.Instance.Send(new TeleportPersonToPerson.Data(RoleStuff.RevengeSelected.PlayerId, closestplr.PlayerId));
                                RoleStuff.RevengeSelected = null;
                            }
                        },

                        cooldown: 0f,
                        image: Properties.Resources.TpToPlayer,
                        positionOffset: new UnityEngine.Vector2(1.5f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Revenger>() && PlayerControl.LocalPlayer.Data.IsDead && RoleStuff.RevengeSelected != null && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;


                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }
            public static class DissapearButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = PlayerControl.LocalPlayer.FindClosestTarget();
                            if (closestplr != null)
                            {
                                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                                closestplr.Die(DeathReason.Kill);
                                Rpc<KillNoBody>.Instance.Send(new KillNoBody.Data(closestplr.PlayerId));
                            }
                        },

                        cooldown: 45f,
                        image: Properties.Resources.NinjaInvis,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Dissapearer>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;


                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }

            public static class StunPlayerButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {

                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                Rpc<Stun>.Instance.Send(new Stun.Data(closestplr.PlayerId, 0));
                                RoleStuff.StunnedPerson = closestplr.PlayerId;
                            }
                        },

                        cooldown: 15f,
                        image: Properties.Resources.Stun,
                        positionOffset: new UnityEngine.Vector2(0.125f, 1.5f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Revenger>() && PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;


                        },
                        hudManager: hudManager,
                        effectDuration: 5f,
                        onEffectEnd: () =>
                        {
                            Rpc<UnStun>.Instance.Send(new UnStun.Data(RoleStuff.StunnedPerson, 0));
                        }
                    );

                    return button;
                }
            }
            public static class ProtectButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {
                    PlayerControl closestplr = null;
                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            closestplr = FindClosestTargetAll();
                            RoleStuff.hasProtected = true;
                            Rpc<Protect>.Instance.Send(new Protect.Data(closestplr.PlayerId));
                        },

                        cooldown: 15f,
                        image: Properties.Resources.Protect,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Protector>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance && !RoleStuff.hasProtected;

                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {
                            
                        }
                    );

                    return button;
                }

            }
            private static PlayerControl FindClosestTargetAll()
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
            public static class TeleportButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();

                            RoleStuff.Selected = closestplr;
                            RoleStuff.hasSelected = true;
                        },

                        cooldown: 30f,
                        image: Properties.Resources.Teleport,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Teleporter>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 25f,
                        onEffectEnd: () =>
                        {
                            Vector3 pos = RoleStuff.Selected.transform.position;
                            PlayerControl.LocalPlayer.gameObject.transform.position = pos;
                            Rpc<TeleportPlayer>.Instance.Send(new TeleportPlayer.Data(pos, PlayerControl.LocalPlayer.PlayerId));
                        }
                    );

                    return button;
                }

            }

            public static class TrackingButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();

                            RoleStuff.TrackingPerson = closestplr.PlayerId;
                            RoleStuff.Tracking = true;
                        },

                        cooldown: 30f,
                        image: Properties.Resources.Track,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Tracker>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 15f,
                        onEffectEnd: () =>
                        {
                            RoleStuff.Tracking = false;
                            RoleStuff.TrackingPerson = 255;
                            HudManager.Instance.PlayerCam.Target = PlayerControl.LocalPlayer;
                            PlayerControl.LocalPlayer.myLight.transform.position = PlayerControl.LocalPlayer.transform.position;
                        }
                    );

                    return button;
                }

            }
            public static class CopyButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                if (closestplr.Data.IsImpostor)
                                {
                                    HudManager.Instance.KillButton.gameObject.SetActive(true);
                                }
                                Rpc<CopyRole>.Instance.Send(new CopyRole.Data(closestplr.PlayerId));
                            }
                        },

                        cooldown: 0f,
                        image: Properties.Resources.Copy,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return false;

                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }
            public static class PlaceVentButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            Rpc<CreateVent>.Instance.Send(new CreateVent.Data());
                            
                        },

                        cooldown: 25f,
                        image: Properties.Resources.PlaceVent,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Engineer>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }

            

            public static class ElectrocuteButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {

                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                Rpc<Stun>.Instance.Send(new Stun.Data(closestplr.PlayerId, 1));
                                RoleStuff.StunnedPerson = closestplr.PlayerId;
                            }
                        },

                        cooldown: 15f,
                        image: Properties.Resources.Shock,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Electrician>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;
                        },
                        hudManager: hudManager,
                        effectDuration: 7f,
                        onEffectEnd: () =>
                        {
                            Rpc<UnStun>.Instance.Send(new UnStun.Data(RoleStuff.StunnedPerson, 1));
                        }
                    );

                    return button;
                }
            }
            public static class InvisibleButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            Rpc<ToggleInvisible>.Instance.Send(new ToggleInvisible.Data(1, 1));
                        },

                        cooldown: 30f,
                        image: Properties.Resources.NinjaInvis,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Ninja>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;
                        },
                        hudManager: hudManager,
                        effectDuration: 15f,
                        onEffectEnd: () =>
                        {
                            Rpc<ToggleInvisible>.Instance.Send(new ToggleInvisible.Data(0, 1));
                        }
                    );

                    return button;
                }
            }
            public static class DetSearchButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {

                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                RoleStuff.SearchingPerson = closestplr;
                            }
                        },

                        cooldown: 35f,
                        image: Properties.Resources.DetectiveSearch,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Detective>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;
                        },
                        hudManager: hudManager,
                        effectDuration: 10f,
                        onEffectEnd: () =>
                        {
                            if (RoleStuff.SearchingPerson != null)
                            {
                                Rpc<Search>.Instance.Send(new Search.Data(RoleStuff.SearchingPerson.PlayerId));
                                RoleStuff.SearchingPerson.SetName(RoleStuff.SearchingPerson.Data.PlayerName + " (" + RoleStuff.SearchingPerson.GetRole().Name + ")");
                                if (RoleStuff.SearchingPerson.Data.IsImpostor)
                                {
                                    RoleStuff.SearchingPerson.nameText.color = Palette.ImpostorRed;
                                }
                                else
                                {
                                    RoleStuff.SearchingPerson.nameText.color = Palette.CrewmateBlue;
                                }
                                RoleStuff.SearchingPerson = null;

                            }
                        }
                    );

                    return button;
                }

            }
            public static class HelpSearchButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {

                            var closestplr = FindClosestTargetAll();
                            if (closestplr != null)
                            {
                                RoleStuff.SearchingPerson = closestplr;
                            }
                        },

                        cooldown: 35f,
                        image: Properties.Resources.DetectiveSearch,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Helper>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;
                        },
                        hudManager: hudManager,
                        effectDuration: 20f,
                        onEffectEnd: () =>
                        {
                            if (RoleStuff.SearchingPerson != null)
                            {
                                Rpc<Search>.Instance.Send(new Search.Data(RoleStuff.SearchingPerson.PlayerId));
                                RoleStuff.SearchingPerson.SetName(RoleStuff.SearchingPerson.Data.PlayerName + " (" + RoleStuff.SearchingPerson.GetRole().Name + ")");
                                if (RoleStuff.SearchingPerson.Data.IsImpostor)
                                {
                                    RoleStuff.SearchingPerson.nameText.color = Palette.ImpostorRed;
                                }
                                else
                                {
                                    RoleStuff.SearchingPerson.nameText.color = Palette.CrewmateBlue;
                                }
                                RoleStuff.SearchingPerson = null;

                            }
                        }
                    );

                    return button;
                }

            }
            public static class BlindPlayerButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {

                            var closestplr = PlayerControl.LocalPlayer.FindClosestTarget();
                            if (closestplr != null)
                            {
                                RoleStuff.BlindTimer = 0f;
                                RoleStuff.ReadyToBlind = true;
                                RoleStuff.BlindedPerson = closestplr.PlayerId;
                            }
                        },

                        cooldown: 20f,
                        image: Properties.Resources.Blindfold,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Blinder>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;


                        },
                        hudManager: hudManager,
                        effectDuration: 20f,
                        onEffectEnd: () =>
                        {
                            RoleStuff.ReadyToBlind = false;
                            Rpc<UnBlind>.Instance.Send(new UnBlind.Data(RoleStuff.BlindedPerson));
                        }
                    );

                    return button;
                }
            }
            public static class ShootButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();
                            if (closestplr.Data.IsImpostor)
                            {
                                Rpc<Kill>.Instance.Send(new Kill.Data(closestplr.PlayerId));
                            } else
                            {
                                
                                Rpc<SelfDestruct>.Instance.Send(new SelfDestruct.Data(PlayerControl.LocalPlayer.PlayerId, 1));
                                

                            }

                        },

                        cooldown: 35f,
                        image: Properties.Resources.Shoot,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Sheriff>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {

                        }
                    );

                    return button;
                }
            }
           
            public static class ReviveButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            RoleStuff.Reviving = true;
                            
                        },

                        cooldown: 35f,
                        image: Properties.Resources.Revive,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Doctor>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 25f,
                        onEffectEnd: () =>
                        {
                            var closestbody = RoleStuff.ClosestBody(PlayerControl.LocalPlayer.MaxReportDistance);
                            Rpc<Revive>.Instance.Send(new Revive.Data(closestbody.ParentId, 2, closestbody.transform.position));
                            Rpc<CleanupBody>.Instance.Send(new CleanupBody.Data(closestbody.ParentId));
                            RoleStuff.Reviving = false;
                        }
                    );

                    return button;
                }
            }
            public static class ShrinkButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            RoleStuff.OldSize = PlayerControl.LocalPlayer.gameObject.transform.localScale;
                            RoleStuff.OldSpeed = PlayerControl.GameOptions.PlayerSpeedMod;
                            PlayerControl.GameOptions.PlayerSpeedMod = RoleStuff.OldSpeed * 1.5f;
                            Vector2 HalfOldSize = new Vector2(RoleStuff.OldSize.x / 2, RoleStuff.OldSize.y / 2);
                            Rpc<ResizePlayer>.Instance.Send(new ResizePlayer.Data(HalfOldSize, PlayerControl.LocalPlayer.PlayerId));
                            RoleStuff.ShrinkerShrunken = true;

                        },

                        cooldown: 25f,
                        image: Properties.Resources.Shrink,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Shrinker>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 10f,
                        onEffectEnd: () =>
                        {
                            Rpc<ResizePlayer>.Instance.Send(new ResizePlayer.Data(RoleStuff.OldSize, PlayerControl.LocalPlayer.PlayerId));
                            PlayerControl.LocalPlayer.gameObject.transform.position = new Vector3(PlayerControl.LocalPlayer.gameObject.transform.position.x, PlayerControl.LocalPlayer.gameObject.transform.position.y + 0.2f);
                            Rpc<TeleportPlayer>.Instance.Send(new TeleportPlayer.Data(new Vector2(PlayerControl.LocalPlayer.gameObject.transform.position.x, PlayerControl.LocalPlayer.gameObject.transform.position.y), PlayerControl.LocalPlayer.PlayerId));
                            PlayerControl.GameOptions.PlayerSpeedMod = RoleStuff.OldSpeed;
                            RoleStuff.ShrinkerShrunken = false;
                        }
                    );

                    return button;
                }
            }
            public static class ShapeshiftButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            byte target = PlayerControl.LocalPlayer.FindClosestTarget().PlayerId;
                            Rpc<SwapEverything>.Instance.Send(new SwapEverything.Data(PlayerControl.LocalPlayer.PlayerId, target));
                            if (GameData.Instance.GetPlayerById(target).Object.Is<Reflector>() && RoleStuff.ReflectorReflecting)
                            {
                                Rpc<SelfDestruct>.Instance.Send(new SelfDestruct.Data());
                            }
                            else
                            {
                                Rpc<Kill>.Instance.Send(new Kill.Data(target));
                            }
                            PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);

                        },

                        cooldown: 25f,
                        image: Properties.Resources.Shapeshift,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Shapeshifter>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 0f,
                        onEffectEnd: () =>
                        {
                            
                        }
                    );

                    return button;
                }
            }
            public static class RubberbandButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var closestplr = FindClosestTargetAll();
                            RoleStuff.OldPos = closestplr.transform.position;
                            RoleStuff.TrollRubberbandPlayer = closestplr;
                        },

                        cooldown: 8f,
                        image: Properties.Resources.TrollTp,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Troll>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 7f,
                        onEffectEnd: () =>
                        {
                            RoleStuff.TrollRubberbandPlayer.transform.position = RoleStuff.OldPos;
                            Rpc<TeleportPlayer>.Instance.Send(new TeleportPlayer.Data(RoleStuff.OldPos, RoleStuff.TrollRubberbandPlayer.PlayerId));
                        }
                    );

                    return button;
                }
            }
            public static class ReflectButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            Rpc<ToggleReflect>.Instance.Send(new ToggleReflect.Data(1));
                        },

                        cooldown: 35f,
                        image: Properties.Resources.Reflect,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Reflector>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 15f,
                        onEffectEnd: () =>
                        {
                            Rpc<ToggleReflect>.Instance.Send(new ToggleReflect.Data(0));
                        }
                    );

                    return button;
                }
            }
            public static class SlowButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            RoleStuff.OldSpeedTroll = PlayerControl.GameOptions.PlayerSpeedMod;
                            RoleStuff.TrollSpeedActive = true;
                            Rpc<SetSpeed>.Instance.Send(new SetSpeed.Data(PlayerControl.GameOptions.PlayerSpeedMod / 2));
                        },

                        cooldown: 8f,
                        image: Properties.Resources.SlowEveryone,
                        positionOffset: new UnityEngine.Vector2(0.125f, 1.5f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Troll>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 7f,
                        onEffectEnd: () =>
                        {
                            RoleStuff.TrollSpeedActive = false;
                            Rpc<SetSpeed>.Instance.Send(new SetSpeed.Data(RoleStuff.OldSpeedTroll));
                        }
                    );

                    return button;
                }
            }
            public static class HackButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            foreach (PlayerControl i in PlayerControl.AllPlayerControls)
                            {
                                if (i.PlayerId >= 0 && i.PlayerId <= GameData.Instance.PlayerCount - 1 && !i.Data.IsDead)
                                {
                                    var Arrow = new GameObject("Arrow" + i.PlayerId);
                                    Arrow.AddComponent<SpriteRenderer>().sprite = RoleStuff.ConvertToSprite(Properties.Resources.HackerArrow, 100, new Vector2(0.5f, 0.5f));
                                    Arrow.AddComponent<ArrowBehaviour>();
                                    Arrow.GetComponent<ArrowBehaviour>().target = i.transform.position;

                                    Arrow.GetComponent<SpriteRenderer>().sortingOrder = 255;
                                    Vector3 pos = PlayerControl.LocalPlayer.transform.position;
                                    //Arrow.transform.position = new Vector3(pos.x + 5, pos.y + 5, pos.z);
                                    RoleStuff.ArrowList.Add(Arrow.GetComponent<ArrowBehaviour>());
                                    RoleStuff.PlayerList.Add(i);
                                }
                            }
                            
                        },

                        cooldown: 35f,
                        image: Properties.Resources.HackerHack,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<Hacker>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 20f,
                        onEffectEnd: () =>
                        {
                            foreach (ArrowBehaviour i in RoleStuff.ArrowList)
                            {
                                i.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                                GameObject.Destroy(i.gameObject);
                                
                            }
                            RoleStuff.ArrowList.Clear();
                            RoleStuff.PlayerList.Clear();
                        }
                    );

                    return button;
                }
            }
            public static class BodyFindButton
            {
                public static Reactor.Button.CooldownButton button(HudManager hudManager)
                {

                    Reactor.Button.CooldownButton button = new Reactor.Button.CooldownButton(
                        onClick: () =>
                        {
                            var foundBodies = GameObject.FindObjectsOfType<DeadBody>();
                            foreach (DeadBody i in foundBodies)
                            {
                                var Arrow = new GameObject("Arrow" + i.ParentId);
                                Arrow.AddComponent<SpriteRenderer>().sprite = RoleStuff.ConvertToSprite(Properties.Resources.Arrow, 100, new Vector2(0.5f, 0.5f));
                                Arrow.AddComponent<ArrowBehaviour>();
                                Arrow.GetComponent<ArrowBehaviour>().target = i.transform.position;

                                Arrow.GetComponent<SpriteRenderer>().sortingOrder = 255;
                                Vector3 pos = PlayerControl.LocalPlayer.transform.position;
                                //Arrow.transform.position = new Vector3(pos.x + 5, pos.y + 5, pos.z);
                                RoleStuff.ArrowBodyList.Add(Arrow.GetComponent<ArrowBehaviour>());
                            }

                        },

                        cooldown: 35f,
                        image: Properties.Resources.BodyFind,
                        positionOffset: new UnityEngine.Vector2(0.125f, 0.125f),
                        () =>
                        {
                            return PlayerControl.LocalPlayer.Is<BodyFinder>() && !PlayerControl.LocalPlayer.Data.IsDead && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && !MeetingHud.Instance;

                        },
                        hudManager: hudManager,
                        effectDuration: 15f,
                        onEffectEnd: () =>
                        {
                            foreach (ArrowBehaviour i in RoleStuff.ArrowBodyList)
                            {
                                i.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                                GameObject.Destroy(i.gameObject);
                            }
                            RoleStuff.ArrowList.Clear();
                        }
                    );

                    return button;
                }
            }
        }
    }
}
