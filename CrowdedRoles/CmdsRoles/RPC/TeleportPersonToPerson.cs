using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.TeleportPersonToPerson)]
    public class TeleportPersonToPerson : PlayerCustomRpc<RoleApiPlugin, TeleportPersonToPerson.Data>
    {
        public TeleportPersonToPerson(RoleApiPlugin plugin, uint id) : base(plugin, id)
        {

        }

        public readonly struct Data
        {
            public readonly byte Target;
            public readonly byte Target2;

            public Data(byte Target, byte Target2)
            {
                this.Target = Target;
                this.Target2 = Target2;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Target);
            writer.Write(data.Target2);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadByte(), reader.ReadByte());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            if (PlayerControl.LocalPlayer.PlayerId == data.Target)
            {
                PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(GameData.Instance.GetPlayerById(data.Target2).Object.transform.position);
            }

            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
