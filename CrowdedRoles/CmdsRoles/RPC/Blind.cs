using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.Blind)]
    public class Blind : PlayerCustomRpc<RoleApiPlugin, Blind.Data>
    {
        public Blind(RoleApiPlugin plugin, uint id) : base(plugin, id)
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
                RoleStuff.Blinded = true;
                RoleStuff.OldReportRadius = PlayerControl.LocalPlayer.MaxReportDistance;
                PlayerControl.LocalPlayer.MaxReportDistance = 0f;
            }
            RoleStuff.BlindOriginalColor = GameData.Instance.GetPlayerById(data.Target).Object.nameText.color;
            if (PlayerControl.LocalPlayer.Data.IsImpostor)
            {
                GameData.Instance.GetPlayerById(data.Target).Object.nameText.color = Color.black;
            }

            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
