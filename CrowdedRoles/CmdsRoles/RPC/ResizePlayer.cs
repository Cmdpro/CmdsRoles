using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using Reactor.Extensions;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.ResizePlayer)]
    public class ResizePlayer : PlayerCustomRpc<RoleApiPlugin, ResizePlayer.Data>
    {
        public ResizePlayer(RoleApiPlugin plugin, uint id) : base(plugin, id)
        {

        }

        public readonly struct Data
        {
            public readonly Vector2 Pos;
            public readonly byte Target;

            public Data(Vector2 Pos, byte Target)
            {
                this.Pos = Pos;
                this.Target = Target;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Pos);
            writer.Write(data.Target);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadVector2(), reader.ReadByte());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            var Player = GameData.Instance.GetPlayerById(data.Target);
            //GameOptions.PlayerSpeedMod
            Player.Object.gameObject.transform.localScale = data.Pos;
            


            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
