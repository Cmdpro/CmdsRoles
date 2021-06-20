using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using CrowdedRoles.Extensions;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.CleanupBody)]
    public class CleanupBody : PlayerCustomRpc<RoleApiPlugin, CleanupBody.Data>
    {
        public CleanupBody(RoleApiPlugin plugin, uint id) : base(plugin, id)
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

            //GameOptions.PlayerSpeedMod
            foreach (DeadBody i in GameObject.FindObjectsOfType<DeadBody>())
            {
                if (i.ParentId == data.Target)
                {
                    GameObject.Destroy(i.gameObject);
                }
            }
            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
