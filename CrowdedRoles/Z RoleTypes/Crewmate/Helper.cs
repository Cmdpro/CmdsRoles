#if DEBUG
using System.Collections.Generic;
using System.Linq;
using BepInEx.IL2CPP;
using CrowdedRoles.Attributes;
using CrowdedRoles.Extensions;
using CrowdedRoles.GameOverReasons;
using CrowdedRoles.Options;
using CrowdedRoles.Roles;
using CrowdedRoles.Components;
using HarmonyLib;
using UnityEngine;
using CmdsRoles;
using Reactor.Networking;

namespace CrowdedRoles
{
    [RegisterCustomRole]
    public class Helper : BaseRole
    {
        public Helper(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Helper";
        public override Color Color { get; } = Color.blue;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Help the detective";
        public override bool CanKill(PlayerControl? target) => false;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => false;
        public override Team Team { get; } = Team.Crewmate;
        public override void OnRoleAssign(PlayerControl player)
        {
            RoleStuff.SearchingPerson = null;
            
        }
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0; 
            if (RoleActive.HelperActive.Value == true)
            {
                active = 1;
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }


    }

    

    

    


    
}
#endif