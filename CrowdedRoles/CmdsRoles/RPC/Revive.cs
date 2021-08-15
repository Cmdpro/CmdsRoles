using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using CrowdedRoles.Extensions;
using Reactor.Extensions;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.Revive)]
    public class Revive : PlayerCustomRpc<RoleApiPlugin, Revive.Data>
    {
        public Revive(RoleApiPlugin plugin, uint id) : base(plugin, id)
        {

        }

        public readonly struct Data
        {
            public readonly byte Target;
            public readonly byte Type;
            public readonly Vector2 RevivePos;

            public Data(byte Target, byte Type, Vector2 RevivePos)
            {
                this.Type = Type;
                this.Target = Target;
                this.RevivePos = RevivePos;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Target);
            writer.Write(data.Type);
            writer.Write(data.RevivePos);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadByte(), reader.ReadByte(), reader.ReadVector2());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {

            //GameOptions.PlayerSpeedMod
            
            GameData.Instance.GetPlayerById(data.Target).Object.Revive();
            GameData.Instance.GetPlayerById(data.Target).Object.nameText.color = new Color((0.0f / 0.0f), (232.0f / 232.0f), (46.0f / 46.0f), 1);
            if (data.Type == 1)
            {
                if (PlayerControl.LocalPlayer.Is<Protector>())
                {
                    Rpc<SelfDestruct>.Instance.Send(new SelfDestruct.Data(PlayerControl.LocalPlayer.PlayerId, 0));
                }
            }
            if (data.Type == 2)
            {
                if (PlayerControl.LocalPlayer.Data.PlayerId == data.Target)
                {

                    PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(new Vector2(data.RevivePos.x, data.RevivePos.y + 0.25f));
                }
            }
            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
