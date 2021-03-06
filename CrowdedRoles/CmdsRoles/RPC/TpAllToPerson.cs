using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.TpAllToPerson)]
    public class TpAllToPerson : PlayerCustomRpc<RoleApiPlugin, TpAllToPerson.Data>
    {
        public TpAllToPerson(RoleApiPlugin plugin, uint id) : base(plugin, id)
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
            Vector2 pos = GameData.Instance.GetPlayerById(data.Target).Object.transform.position;
            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(pos);


            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
