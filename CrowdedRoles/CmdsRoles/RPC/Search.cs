using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using Reactor.Extensions;
using System.Linq;
using CrowdedRoles.Extensions;
using CrowdedRoles.Components;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.Search)]
    public class Search : PlayerCustomRpc<RoleApiPlugin, Search.Data>
    {
        
        public Search(RoleApiPlugin plugin, uint id) : base(plugin, id)
        {

        }

        public readonly struct Data
        {
            public readonly byte Target;

            public Data(byte Target)
            {
                this.Target = Target;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Target);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadByte());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            if (RoleSettings.DetHelpShare.Value)
            {
                if (PlayerControl.LocalPlayer.Is<Detective>() || PlayerControl.LocalPlayer.Is<Helper>())
                {
                    GameData.Instance.GetPlayerById(data.Target).Object.SetName(GameData.Instance.GetPlayerById(data.Target).PlayerName + " (" + GameData.Instance.GetPlayerById(data.Target).GetRole().Name + ")");
                    if (GameData.Instance.GetPlayerById(data.Target).IsImpostor)
                    {
                        GameData.Instance.GetPlayerById(data.Target).Object.nameText.color = Palette.ImpostorRed;
                    }
                    else
                    {
                        GameData.Instance.GetPlayerById(data.Target).Object.nameText.color = Palette.CrewmateBlue;
                    }
                }
            }
        }
    }
}
