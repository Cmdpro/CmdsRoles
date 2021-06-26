using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.Portal)]
    public class Portal : PlayerCustomRpc<RoleApiPlugin, Portal.Data>
    {
        public Portal(RoleApiPlugin plugin, uint id) : base(plugin, id)
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
            if (data.Target == 0)
            {
                GameObject.Destroy(RoleStuff.Portal1);
                GameObject.Destroy(RoleStuff.Portal2);
                RoleStuff.Portal1 = null;
                RoleStuff.Portal2 = null;
            }
            if (data.Target == 1)
            {
                var gameObject = new GameObject("Portal1");
                gameObject.AddComponent<SpriteRenderer>().sprite = RoleStuff.ConvertToSprite(CrowdedRoles.Properties.Resources.Portal, 190, Vector2.zero);
                gameObject.transform.position = new Vector2(innerNetObject.transform.position.x, innerNetObject.transform.position.y - innerNetObject.transform.localScale.y);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            if (data.Target == 2)
            {
                var gameObject = new GameObject("Portal2");
                gameObject.AddComponent<SpriteRenderer>().sprite = RoleStuff.ConvertToSprite(CrowdedRoles.Properties.Resources.Portal, 190, Vector2.zero);
                gameObject.transform.position = new Vector2(innerNetObject.transform.position.x - innerNetObject.transform.localScale.x, innerNetObject.transform.position.y - innerNetObject.transform.localScale.y);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                
            }
            RoleStuff.PortalImmuneTime = 1f;

            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
    }
}
