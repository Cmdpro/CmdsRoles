using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;
using Reactor.Extensions;
using System.Linq;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.CreateVent)]
    public class CreateVent : PlayerCustomRpc<RoleApiPlugin, CreateVent.Data>
    {
        
        public CreateVent(RoleApiPlugin plugin, uint id) : base(plugin, id)
        {

        }

        public readonly struct Data
        {/*
            public readonly int id;
            public readonly Vector2 position;
            public readonly float zAxis;
            public readonly int leftVent;
            public readonly int centerVent;
            public readonly int rightVent;

            public Data(int id, Vector2 position, float zAxis, int leftVent, int centerVent, int rightVent)
            {
                this.id = id;
                this.position = position;
                this.zAxis = zAxis;
                this.leftVent = leftVent;
                this.centerVent = centerVent;
                this.rightVent = rightVent;
            }*/
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

        public override void Write(MessageWriter writer, Data data)
        {/*
            writer.Write(data.id);
            writer.Write(data.position);
            writer.Write(data.zAxis);
            writer.Write(data.leftVent);
            writer.Write(data.centerVent);
            writer.Write(data.rightVent);*/
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(/*reader.ReadPackedInt32(), reader.ReadVector2(), reader.ReadSingle(), reader.ReadPackedInt32(), reader.ReadPackedInt32(), reader.ReadPackedInt32()*/);
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            var pos = innerNetObject.transform.position;
            var ventId = GetAvailableVentId();
            var leftVent = int.MaxValue;
            var centerVent = int.MaxValue;
            var rightVent = int.MaxValue;
            var zAxis = innerNetObject.transform.position.z + .001f;
            if (RoleStuff.lastVent != null)
            {
                leftVent = RoleStuff.lastVent.Id;
            }
            SpawnVent(innerNetObject, ventId, pos, zAxis, leftVent, centerVent, rightVent);
        }

        static int GetAvailableVentId()
        {
            int id = 0;

            while (true)
            {
                if (!ShipStatus.Instance.AllVents.Any(v => v.Id == id))
                {
                    return id;
                }
                id++;
            }
        }
        private static void SpawnVent(PlayerControl sender, int id, Vector2 position, float zAxis, int leftVent, int centerVent, int rightVent)
        {
            var realPos = new Vector3(position.x, position.y, zAxis);

            var ventPref = UnityEngine.Object.FindObjectOfType<Vent>();
            var vent = UnityEngine.Object.Instantiate(ventPref, ventPref.transform.parent);
            vent.Id = id;
            vent.transform.position = realPos;
            vent.Left = leftVent == int.MaxValue ? null : ShipStatus.Instance.AllVents.FirstOrDefault(v => v.Id == leftVent);
            vent.Center = centerVent == int.MaxValue ? null : ShipStatus.Instance.AllVents.FirstOrDefault(v => v.Id == centerVent);
            vent.Right = rightVent == int.MaxValue ? null : ShipStatus.Instance.AllVents.FirstOrDefault(v => v.Id == rightVent);

            var allVents = ShipStatus.Instance.AllVents.ToList();
            allVents.Add(vent);
            ShipStatus.Instance.AllVents = allVents.ToArray();

            if (vent.Left != null)
            {
                vent.Left.Right = ShipStatus.Instance.AllVents.FirstOrDefault(v => v.Id == id);
            }

            if (sender.AmOwner)
                RoleStuff.lastVent = vent;
        }
    }
}
