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
    public class Hacker : BaseRole
    {
        public Hacker(BasePlugin plugin) : base(plugin)
        {
        }

        public override string Name { get; } = "Hacker";
        public override Color Color { get; } = Color.green;
        public override Visibility Visibility { get; } = Visibility.Myself;
        public override string Description { get; } = "Create Portals";
        public override bool CanKill(PlayerControl? target) => false;
        public override bool CanSabotage(SystemTypes? sabotage) => false;
        public override bool CanVent(Vent _) => false;
        public override Team Team { get; } = Team.Crewmate;

        public override void OnRoleAssign(PlayerControl player)
        {
            foreach (PlayerControl i in PlayerControl.AllPlayerControls) {
                if (i != PlayerControl.LocalPlayer)
                {
                    var gameObject = new GameObject("Arrow" + i.PlayerId);
                    gameObject.AddComponent<SpriteRenderer>().sprite = RoleStuff.ConvertToSprite(Properties.Resources.HackerArrow, 100);
                    gameObject.AddComponent<ArrowBehaviour>();
                    gameObject.GetComponent<ArrowBehaviour>().target = i.transform.position;
                    Vector3 pos = PlayerControl.LocalPlayer.transform.position;
                    gameObject.transform.position = new Vector3(pos.x + 5, pos.y + 5, pos.z);
                    RoleStuff.ArrowList.Add(gameObject);
                    RoleStuff.PlayerList.Add(i);
                }
            }
        }
        public override IEnumerable<GameData.PlayerInfo> SelectHolders(RoleHolders holders, byte limit)
        {
            var rand = new System.Random();
            int active = 0;
            if (RoleActive.HackerActive.Value == true)
            {
                active = 1;
            }
            var result = holders.Crewmates.OrderBy(_ => rand.Next()).Take(active).ToList();
            return result;
        }

        

    }

    

    

    


    
}
#endif