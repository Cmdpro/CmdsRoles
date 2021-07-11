using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using CrowdedRoles.Roles;
using CrowdedRoles.Extensions;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.CopyRole)]
    public class CopyRole : PlayerCustomRpc<RoleApiPlugin, CopyRole.Data>
    {
        public CopyRole(RoleApiPlugin plugin, uint id) : base(plugin, id)
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

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

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
            RoleManager.PlayerRoles[innerNetObject.PlayerId] = GameData.Instance.GetPlayerById(data.Target).GetRole();
            if (GameData.Instance.GetPlayerById(data.Target).IsImpostor)
            {
                //innerNetObject.Data.IsImpostor = true;
            }


            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
