using System;
using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

namespace CrowdedRoles.Roles
{
    public static class RoleManager
    {
        public static Dictionary<BaseRole, byte> Limits { get; } = new();
        public static Dictionary<BaseRole, byte> EditableLimits = new();

        public static Dictionary<byte, BaseRole> PlayerRoles { get; } = new();
        internal static Dictionary<string, Dictionary<byte, BaseRole>> Roles { get; } = new();
        internal static Dictionary<byte, TaskCompletion> TaskCompletions { get; } = new ();
        public static bool RolesSet { get; internal set; }

        internal static BaseRole? GetRoleByData(RoleData data)
        {
            return Roles[data.pluginId]?[data.localId];
        }
        
        public static void GameEnded()
        {
            PlayerRoles.Clear();
            TaskCompletions.Clear();
            RolesSet = false;
        }
    }

    public class RoleSingleton<T> where T : BaseRole
    {
        private T? _instance;

        public T Instance =>
            (_instance ??= RoleManager.Roles.SelectMany(p => p.Value.Select(d => d.Value)).Single(r => r is T) as T) ??
            throw new NullReferenceException($"{typeof(T).FullDescription()} is not registered");
    }
}
