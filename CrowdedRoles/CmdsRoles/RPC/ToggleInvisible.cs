using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.ToggleInvisible)]
    public class ToggleInvisible : PlayerCustomRpc<RoleApiPlugin, ToggleInvisible.Data>
    {
        public ToggleInvisible(RoleApiPlugin plugin, uint id) : base(plugin, id)
        {

        }

        public readonly struct Data
        {
            public readonly byte Target;
            public readonly byte Type;

            public Data(byte Target, byte Type)
            {
                this.Target = Target;
                this.Type = Type;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Target);
            writer.Write(data.Type);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadByte(), reader.ReadByte());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {

            //GameOptions.PlayerSpeedMod
            if (data.Target == 1)
            {
                innerNetObject.Visible = false;
                if (data.Type == 1)
                {
                    RoleStuff.InvisibleNinja = innerNetObject;
                }
            }
            if (data.Target == 0)
            {
                innerNetObject.Visible = true;
                if (data.Type == 1)
                {
                    RoleStuff.InvisibleNinja = null;
                }
            }
            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
