using Reactor;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using CrowdedRoles;
using System.Collections;
using PowerTools;
using Reactor.Extensions;
using Reactor.Networking;

namespace CmdsRoles
{
    [RegisterCustomRpc((uint)CrowdedRoles.Rpc.CustomRpcCalls.SelfDestruct)]
    public class SelfDestruct : PlayerCustomRpc<RoleApiPlugin, SelfDestruct.Data>
    {
        public SelfDestruct(RoleApiPlugin plugin, uint id) : base(plugin, id)
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

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

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
            var target = GameData.Instance.GetPlayerById(data.Target);
            //GameOptions.PlayerSpeedMod
            innerNetObject.MurderPlayer(target.Object);
            PlayerControl.LocalPlayer.StartCoroutine(PlayerControl.LocalPlayer.KillAnimations.Random<KillAnimation>().CoPerformKill(target.Object, target.Object));
            if (data.Type == 1)
            {
                var gameObject = new GameObject("Gun");
                gameObject.AddComponent<SpriteRenderer>().sprite = RoleStuff.ConvertToSprite(CrowdedRoles.Properties.Resources.Gun, 200, Vector2.zero);
                gameObject.transform.position = new Vector2(innerNetObject.transform.position.x, innerNetObject.transform.position.y - innerNetObject.transform.localScale.y);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            //.SetActive(false);
            //PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
            // System.Environment.Exit(1);
            System.Console.WriteLine($"TEST {data.Target}");

        }
        public AnimationClip BlurAnim;

        // Token: 0x0400068C RID: 1676
        public DeadBody bodyPrefab;

        // Token: 0x0400068D RID: 1677
        public Vector3 BodyOffset;
        public IEnumerator CoPerformKill(PlayerControl source, PlayerControl target)
        {
            FollowerCamera cam = Camera.main.GetComponent<FollowerCamera>();
            bool isParticipant = PlayerControl.LocalPlayer == source || PlayerControl.LocalPlayer == target;
            PlayerPhysics sourcePhys = source.MyPhysics;
            KillAnimation.SetMovement(source, false);
            KillAnimation.SetMovement(target, false);
            if (isParticipant)
            {
                cam.Locked = true;
            }
            target.Die(DeathReason.Kill);
            SpriteAnim sourceAnim = source.GetComponent<SpriteAnim>();
            yield return new WaitForAnimationFinish(sourceAnim, this.BlurAnim);
            source.NetTransform.SnapTo(target.transform.position);
            sourceAnim.Play(sourcePhys.IdleAnim, 1f);
            KillAnimation.SetMovement(source, true);
            DeadBody deadBody = UnityEngine.Object.Instantiate<DeadBody>(this.bodyPrefab);
            Vector3 vector = target.transform.position + this.BodyOffset;
            vector.z = vector.y / 1000f;
            deadBody.transform.position = vector;
            deadBody.ParentId = target.PlayerId;
            target.SetPlayerMaterialColors(deadBody.GetComponent<Renderer>());
            KillAnimation.SetMovement(target, true);
            if (isParticipant)
            {
                cam.Locked = false;
            }
            yield break;
        }

    }
}
