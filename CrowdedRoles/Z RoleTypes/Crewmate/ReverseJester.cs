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
    public class ReverseJester : BaseRole
    {
        public ReverseJester(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Reverse\nJester";
        public override Color Color { get; } = Color.magenta;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Dont get voted out or you loose";
        public override bool CanKill(PlayerControl? target) => false;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => true;
        public override Team Team { get; } = Team.Crewmate;

        [RegisterCustomGameOverReason]
        public class ReverseJesterLost : CustomGameOverReason
        {
            public ReverseJesterLost(BasePlugin plugin) : base(plugin)
            {
            }

            public override string Name { get; } = "The Reverse\n Jester Lost";
            public override string WinText { get; } = "The Reverse\n Jester Lost";
            public override IEnumerable<GameData.PlayerInfo> Winners =>
                GameData.Instance.AllPlayers.ToArray().Where(p => !p.Is<ReverseJester>() && p.IsImpostor);

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
            taskList.AddStringTask("Dont Get Ejected");
            taskList.AddNormalTasks(defaultTasks);
            taskList.TaskCompletion = TaskCompletion.Required;
        }
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0;
            if (RoleActive.ReverseJesterActive.Value == true)
            {
                active = 1;
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }

    }

    

    

    


    
}
#endif