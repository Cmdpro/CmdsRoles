using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using static CrowdedRoles.CrowdedRoles;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.UnProtect)]
    public class UnProtect : PlayerCustomRpc<RoleApiPlugin, UnProtect.Data>
    {
        public UnProtect(RoleApiPlugin plugin, uint id) : base(plugin, id)
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
            var person = GameData.Instance.GetPlayerById(data.Target);
            RoleStuff.protectedID = 255;
            person.Object.nameText.color = new Color((0.0f / 0.0f), (232.0f / 232.0f), (46.0f / 46.0f), 1);
            

            if (PlayerControl.LocalPlayer.PlayerId == data.Target)
            {
                RoleStuff.isProtected = false;
                
            }

        }
    }
}
