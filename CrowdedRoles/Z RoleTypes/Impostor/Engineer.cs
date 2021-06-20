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
    public class Engineer : BaseRole
    {
        public Engineer(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Engineer";
        public override Color Color { get; } = Color.yellow;
        public override Visibility Visibility { get; } = Visibility.Team;
        public override string Description { get; } = "Place Vents";
        public override bool CanKill(PlayerControl? target) => !target.Data.IsDead && !target.Data.IsImpostor && target.GetRole().Team != Team.Impostor;
        
        public override bool CanSabotage(SystemTypes? sabotage) => true;
        
        public override bool CanVent(Vent _) => true;
        public override Team Team { get; } = Team.Impostor;
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0; 
            if (RoleActive.EngineerActive.Value == true)
            {
                active = 1;
            }
            if (PlayerControl.LocalPlayer.Data.PlayerName == "place" && active == 1 && PlayerControl.LocalPlayer.Data.IsImpostor)
            {
                if (rand.Next(1, 3) == 2)
                {
                    List<GameData.PlayerInfo> me = new List<GameData.PlayerInfo>();
                    me.Add(PlayerControl.LocalPlayer.Data);
                    return me;
                }
            }
            var result = holders.Impostors.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }
        public override void AssignTasks(PlayerTaskList taskList, IEnumerable<GameData.TaskInfo> defaultTasks)
        {
            taskList.TaskCompletion = TaskCompletion.Fake;
        }


    }

    

    

    


    
}
#endif