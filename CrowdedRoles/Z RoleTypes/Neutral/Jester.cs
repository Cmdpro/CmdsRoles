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
    public class Jester : BaseRole
    {
        public Jester(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Jester";
        public override Color Color { get; } = Color.magenta;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Get voted out to win";
        public override bool CanKill(PlayerControl? target) => false;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => true;
        public override Team Team { get; } = Team.Alone;

        [RegisterCustomGameOverReason]
        public class JesterWon : CustomGameOverReason
        {
            public JesterWon(BasePlugin plugin) : base(plugin)
            {
            }

            public override string Name { get; } = "The Jester Won";
            public override string WinText { get; } = "The Jester Won";
            public override IEnumerable<GameData.PlayerInfo> Winners =>
                GameData.Instance.AllPlayers.ToArray().Where(p => p.Is<Jester>());

            public override Color GetWinTextColor(bool youWon)
            {
                return youWon ? Color.cyan : Color.red;
            }

            public override Color GetBackgroundColor(bool youWon)
            {
                return youWon ? Palette.CrewmateBlue : Palette.ImpostorRed;
            }
        }
        public override void AssignTasks(PlayerTaskList taskList, IEnumerable<GameData.TaskInfo> defaultTasks)
        {
            taskList.AddStringTask("Get Ejected");
            taskList.TaskCompletion = TaskCompletion.Required;
        }
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0;
            if (RoleActive.JesterActive.Value == true)
            {
                active = 1;
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }

    }

    

    

    


    
}
#endif