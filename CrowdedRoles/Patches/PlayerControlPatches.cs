using CrowdedRoles.Extensions;
using CrowdedRoles.Roles;
using HarmonyLib;
using UnityEngine;

namespace CrowdedRoles.Patches
{
    [HarmonyPatch(typeof(PlayerControl))]
    internal static class PlayerControlPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
        public static void FixedUpdate_Postfix(ref PlayerControl __instance)
        {
            if (GameData.Instance == null || __instance.Data == null)
            {
                return;
            }
            
            BaseRole? role = __instance.GetRole();

            if (__instance.AmOwner && role != null) // probably will be reworked
            {
                //idk why this isnt working
                if (role.CanKill(null) &&  __instance.CanMove && !__instance.Data.IsDead)
                {
                    if (!PlayerControl.LocalPlayer.Data.IsImpostor)
                    {
                        __instance.SetKillTimer(Mathf.Max(0, __instance.killTimer - Time.fixedDeltaTime));
                    }
                    HudManager.Instance.KillButton.SetTarget(__instance.CustomFindClosetTarget());
                }
                else
                {
                    HudManager.Instance.KillButton.SetTarget(null);
                }
            }
            if (HudManager.Instance.KillButton.CurrentTarget == RoleStuff.InvisibleNinja)
            {
                HudManager.Instance.KillButton.SetTarget(null);
            }
        }
        
    }
}