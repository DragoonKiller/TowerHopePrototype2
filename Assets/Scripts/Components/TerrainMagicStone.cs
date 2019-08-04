using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
// 魔法石
// 角色与魔法石接触之后, 恢复全部法力.
// 一些角色与魔法石接触之后会触发其特殊能力.
// 

namespace Tower.Components
{
    using Systems;
    using Utils;

    [RequireComponent(typeof(Collider2D))]
    public class TerrainMagicStone : MonoBehaviour
    {
        [Tooltip("普通情况下, 魔法恢复速率乘数.")]
        public float magicRecoverMult;

        [Tooltip("如果碰撞者的*法向速度*超过这个数值, 会直接回满法力并播放特效")]
        public float fullMagicRecoverSpeed;

        Collider2D cd => this.GetComponent<Collider2D>();
        readonly List<ContactPoint2D> contacts = new List<ContactPoint2D>();

        void Start()
        {
            Signal<Signals.RenderUpdate>.Listen(Step);
        }

        void OnDestroy()
        {
            Signal<Signals.RenderUpdate>.Remove(Step);
        }

        void Step(Signals.RenderUpdate e)
        {
            // 注意该函数会 clear 这个 list.
            cd.GetContacts(contacts);

            var magicRecovered = new List<GameObject>();
            foreach(var ct in contacts)
            {
                // 每帧每个 GameObject 最多回复一次.
                if(TryRecoverRoleMagic(ct, e.dt) && magicRecovered.Contains(ct.collider.gameObject)) break;
                else magicRecovered.Add(ct.collider.gameObject);
            }
        }

        bool TryRecoverRoleMagic(ContactPoint2D ct, float dt)
        {
            var role = ct.collider.gameObject.GetComponent<Role>();
            if(role == null) return false;
            var magic = role.magic;
            if(magic == null) return false;

            // 检查角色的法向速度是否大于给定值.
            // 如果是, 直接回满魔法并触发特效;
            // 否则, 按照 magicRecoverMult * recoverRate 回复.
            if(TryFullRecover(role, ct.normal,ct.relativeVelocity))
            {
                CreateFullRecoverFX();
                // 如果触发了满回复, 就不再进行其它动作.
                return true;
            }

            return TryNormalRecover(role, dt);
        }

        bool TryNormalRecover(Role role, float dt)
        {
            Debug.Assert(role != null);
            Debug.Assert(role.magic != null);
            role.magic.RecoverMagic(dt * magicRecoverMult);
            return true;
        }

        bool TryFullRecover(Role role, Vector2 normal, Vector2 v)
        {
            // 法线方向是朝内的.
            var speed = normal.normalized.Dot(v);
            if(speed > 1) Debug.Log(speed);
            if(speed >= fullMagicRecoverSpeed)
            {
                // 直接回复无穷法力. 多余的法力会被 clamp 掉.
                role.magic.RecoverMagic(1e8f);
                return true;
            }
            return false;
        }

        void CreateFullRecoverFX()
        {
            // TODO!
        }

    }

    
}
