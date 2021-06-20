using HarmonyLib;
using Hazel;
using Reactor;
using System;
using UnityEngine;
using CmdsRoles;
using Il2CppSystem.Collections.Generic;

namespace CrowdedRoles
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class PlayerControlFixedUpdatePatch
    {
        public static void Postfix(ref PlayerControl __instance)
        {
			if (RoleStuff.Blinded)
            {
                __instance.myLight.LightRadius = 0f;
            }
		}
    }
}