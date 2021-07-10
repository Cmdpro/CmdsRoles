using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using System.Collections;
using PowerTools;
using Reactor.Extensions;
using Reactor.Networking;

namespace CrowdedRoles
{
    [RegisterCustomRpc((uint)Rpc.CustomRpcCalls.SwapEverything)]
    public class SwapEverything : PlayerCustomRpc<RoleApiPlugin, SwapEverything.Data>
    {
        public SwapEverything(RoleApiPlugin plugin, uint id) : base(plugin, id)
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

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

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
            PlayerControl? T1 = GameData.Instance.GetPlayerById(data.Target).Object;
            PlayerControl? T2 = GameData.Instance.GetPlayerById(data.Target2).Object;
            int T1Color = T1.Data.ColorId;
            uint T1Hat = T1.Data.HatId;
            uint T1Skin = T1.Data.SkinId;
            uint T1Pet = T1.Data.PetId;
            string T1Name = T1.Data.PlayerName;
            T1.SetColor(T2.Data.ColorId);
            T1.SetHat(T2.Data.HatId, T2.Data.ColorId);
            T1.SetSkin(T2.Data.SkinId);
            T1.SetPet(T2.Data.PetId);
            T1.SetName(T2.Data.PlayerName);

            T2.SetColor(T1Color);
            T2.SetHat(T1Hat, T1Color);
            T2.SetSkin(T1Skin);
            T2.SetPet(T1Pet);
            T2.SetName(T1Name);
            System.Console.WriteLine($"TEST {data.Target}");

        }
        
        

    }
}
