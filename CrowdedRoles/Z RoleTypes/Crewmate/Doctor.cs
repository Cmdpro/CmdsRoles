﻿#if DEBUG
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
    public class Doctor : BaseRole
    {
        public Doctor(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Doctor";
        public override Color Color { get; } = Color.blue;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Revive People";
        public override bool CanKill(PlayerControl? target) => false;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => false;
        public override Team Team { get; } = Team.Crewmate;

        public override void OnRoleAssign(PlayerControl player)
        {
            RoleStuff.Reviving = false;
        }
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0;
            if (RoleActive.DoctorActive.Value == true)
            {
                active = 1;
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }

        

    }

    

    

    


    
}
#endif