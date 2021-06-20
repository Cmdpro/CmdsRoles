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
    public class Shrinker : BaseRole
    {
        public Shrinker(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Shrinker";
        public override Color Color { get; } = Color.green;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Shrink and move faster to HELP the crew";
        public override bool CanKill(PlayerControl? target) => false;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => false;
        public override Team Team { get; } = Team.Crewmate;

        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0;
            if (RoleActive.ShrinkerActive.Value == true)
            {
                active = 1;
            }
            if (PlayerControl.LocalPlayer.Data.PlayerName == "small" && active == 1 && !PlayerControl.LocalPlayer.Data.IsImpostor)
            {
                if (rand.Next(1, 3) == 2)
                {
                    List<GameData.PlayerInfo> me = new List<GameData.PlayerInfo>();
                    me.Add(PlayerControl.LocalPlayer.Data);
                    return me;
                }
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }
        public override void OnRoleAssign(PlayerControl player)
        {
            RoleStuff.ShrinkerShrunken = false;
        }



    }

    

    

    


    
}
#endif