#if DEBUG
using System.Collections.Generic;
using System.Linq;
using BepInEx.IL2CPP;
using CrowdedRoles.Attributes;
using CrowdedRoles.Extensions;
using CrowdedRoles.GameOverReasons;
using CrowdedRoles.Options;
using CrowdedRoles.Roles;
using HarmonyLib;
using UnityEngine;
using CmdsRoles;
using Reactor.Networking;
using CrowdedRoles.Components;

namespace CrowdedRoles
{
    [RegisterCustomRole]
    public class Ninja : BaseRole
    {
        public Ninja(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Ninja";
        public override Color Color { get; } = Color.gray;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Go invisible and kill the impostors";
        public override bool CanKill(PlayerControl? target) => true;
        public override bool KillConditions(PlayerControl? target) => !target.Data.IsDead && target != PlayerControl.LocalPlayer && target != RoleStuff.InvisibleNinja;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => false;
        public override Team Team { get; } = Team.Crewmate;

        public override void OnRoleAssign(PlayerControl player)
        {
            RoleStuff.Invisible = false;
        }
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0;
            if (RoleActive.NinjaActive.Value == true)
            {
                active = 1;
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }

        

    }

    

    

    


    
}
#endif