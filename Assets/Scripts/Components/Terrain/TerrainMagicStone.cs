using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        readonly List<Role> prevRoles = new List<Role>();
        readonly List<Role> roles = new List<Role>();
        readonly List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        readonly List<RoleMagic> recovered = new List<RoleMagic>(); 

        void Start()
        {
            Signal<Signals.PostUpdate>.Listen(Step);
        }

        void OnDestroy()
        {
            Signal<Signals.PostUpdate>.Remove(Step);
        }
        
        void Step(Signals.PostUpdate e)
        {
            // 注意该函数会 clear 这个 list.
            cd.GetContacts(contacts);
            roles.Clear();
            roles.AddRange(contacts.Select(x => x.collider.gameObject.GetComponent<Role>()).Where(x => x != null).Distinct());
            
            // 角色在魔法石上跳跃会立即回满法力.
            var (added, removed) = prevRoles.FowardComapre(roles);
            foreach(var r in added) r.action.JumpCallbacks += this.FullRecover;
            foreach(var r in removed) r.action.JumpCallbacks -= this.FullRecover;
            
            // 角色撞击魔法石能够立即回满法力.
            // 检查角色的法向速度是否大于给定值. 如果是, 直接回满魔法并触发特效.
            foreach(var ct in contacts)
            {
                var role = ct.collider.GetComponent<Role>();
                if(!role) continue;
                var magic = role.magic;
                if(!magic) continue;
                
                // 法线方向是朝内的.
                var speed = ct.normal.normalized.Dot(ct.relativeVelocity);
                if(speed >= fullMagicRecoverSpeed) magic.RecoverMagic(1e5f);
            }
            
            // 常规回复. 按照 magicRecoverMult * recoverRate 回复魔法.    
            foreach(var r in roles)
            {
                if(!r.magic) continue;
                // 考虑到角色本身也会回复法力, 这里减掉角色自身的法力回复值.
                r.magic.RecoverMagic(e.dt * (magicRecoverMult - 1));
            }
            
            prevRoles.Clear();
            foreach(var i in roles) prevRoles.Add(i);
        }
        
        void FullRecover(Role role)
        {
            if(!role.magic) return;
            role.magic.RecoverMagic(1e5f);
        }

    }

    
}
