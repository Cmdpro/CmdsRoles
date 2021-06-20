using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.UnBlind)]
    public class UnBlind : PlayerCustomRpc<RoleApiPlugin, UnBlind.Data>
    {
        public UnBlind(RoleApiPlugin plugin, uint id) : base(plugin, id)
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

            if (PlayerControl.LocalPlayer.PlayerId == data.Target)
            {
                RoleStuff.Blinded = false;
                PlayerControl.LocalPlayer.MaxReportDistance = RoleStuff.OldReportRadius;
            }
            GameData.Instance.GetPlayerById(data.Target).Object.nameText.color = RoleStuff.BlindOriginalColor;
            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
