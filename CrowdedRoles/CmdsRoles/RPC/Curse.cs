using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using CrowdedRoles.Rpc;
using Reactor.Networking;
using static CrowdedRoles.CrowdedRoles;

namespace CmdsRoles
{
        [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.Curse)]
        public class Curse : PlayerCustomRpc<RoleApiPlugin, Curse.Data>
        {
            public Curse(RoleApiPlugin plugin, uint id) : base(plugin, id)
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

                //GameOptions.PlayerSpeedMod
                if (PlayerControl.LocalPlayer.PlayerId == data.Target)
                {
                    RoleStuff.isCursed = true;
                }
                //.SetActive(false);
                //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                // System.Environment.Exit(1);
                System.Console.WriteLine($"TEST {data.Target}");

            }
        }
}
