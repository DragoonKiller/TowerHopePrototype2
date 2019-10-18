using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;
using Utils;

namespace Tower.Components
{
    using Global;
    using Skills;

    /// <summary>
    /// 定义了角色的移动行为.
    /// </summary>
    // 需要处理的角色移动机制包括:
    // * [地面] 在地面上站立不动.
    // * [地面] 在地面上左右跑动.
    // * [地面] 在地面上跑动时, 贴地.
    // * [地面] 在地面施放普通攻击.
    // * [地面 -> 空中] 从地面跳起, 进入空中.
    // * [地面 -> 空中] 因为踩空或地形破坏等原因离开地面进入空中.
    // * [空中] 在空中左右移动.
    // * [空中] 在空中贴墙移动.
    // * [空中] 在空中贴墙跳跃.
    // * [空中 -> 地面] 落地.
    // * [空中 -> 技能] 在空中施放技能. 技能不能在地面施放.
    // * [技能 -> 空中] 技能施放完毕后, 回到空中状态或者地面状态, 取决于技能结束时的状态.
    // 为了实现这些机制, 约定:
    // 在地面和在空中各使用一个状态机. 每个技能使用一个状态机.
    // 地面和空中的状态机处理自己的特殊机制, 包括左右移动, 抓地, 抓墙, 转换到另一个状态等.
    // 技能状态机处理自己的特殊机制.
    // 技能执行结束时, 其状态机通过 Trans 切换到 地面或空中状态. 一般都是空中状态, 因为不能起跳.
    // 为了保证最终的手感, 需要额外的机制:
    // * 空中状态有一个跳跃计数. 从地面起跳变为空中状态, 或者技能结束变为空中状态, 跳跃计数初始化为 0. 
    //   其它情况离开地面, 空中状态跳跃计数初始化为 1, 一段时间后 -1. 用这个机制实现短时离地起跳. 
    // * 地面状态有一个"跳跃计数", 跳起后 -1, 放开跳跃键后 +1, 这样能保证长按空格时不会一直跳.
    [RequireComponent(typeof(Role))]
    [RequireComponent(typeof(Rigidbody2D))]
    [Serializable]
    public sealed class RoleAction : MonoBehaviour
    {
        
        #region Types

        public abstract class BaseState : StateMachine
        {
            protected readonly RoleAction action;
            protected Role role => action.role;
            protected RoleSkills skills => role.skills;
            public BaseState(RoleAction roleAction) => this.action = roleAction;
            
            /// <summary>
            /// 工具函数: 尝试施放技能.
            /// </summary>
            protected bool TryUseSkill(ISkillConfig skill, KeyCode key, out StateMachine.Transfer trans)
            {
                if(CommandQueue.Get(key) && skill != null)
                {
                    if(skill.TryGetState(role, out var stm))
                    {
                        trans = Trans(stm);
                        return true;
                    }
                }
                trans = new StateMachine.Transfer();
                return false;
            }
            
            /// <summary>
            /// 工具函数: 检测某个技能并释放.
            /// </summary>
            protected bool TryUseAnySkill(out StateMachine.Transfer resTrans)
            {
                var skillList = new (ISkillConfig skill, KeyCode key)[] {
                    (role.skills.primary, KeyBinding.inst.primarySkill),
                    (role.skills.secondary, KeyBinding.inst.secondarySkill),
                    (role.skills.attack, KeyBinding.inst.attack),
                    (role.skills.magicAttack, KeyBinding.inst.magicAttack)
                };
                
                foreach(var (skill, key) in skillList) if(TryUseSkill(skill, key, out resTrans)) return true;
                resTrans = new StateMachine.Transfer();
                return false;
            }
        }
        
        public sealed class FlyState : BaseState
        {
            public FlyState(RoleAction roleAction) : base(roleAction) { }
            
            public override IEnumerator<Transfer> Step()
            {
                float beginTime = Time.time;
                while(true)
                {
                    yield return Pass();
                    float t = Time.time - beginTime;
                    
                    // 在起跳之后的一段时间内, 不能切换为落地状态.
                    if(action.landing && t > action.minJumpTime)
                    {
                        yield return Trans(new MoveState(action));
                    }
                    
                    // 冲刺.
                    if(TryUseSkill(role.skills.rush, KeyBinding.inst.rush, out var transJump))
                    {
                        yield return transJump;
                    }
                    
                    // 放技能.
                    if(TryUseAnySkill(out var trans))
                    {
                        yield return trans;
                        continue;
                    }
            
                    // 在空中左右移动.
                    bool left = CommandQueue.Get(KeyBinding.inst.moveLeft);
                    bool right = CommandQueue.Get(KeyBinding.inst.moveRight);
                    if(left == right) action.MoveInTheAir(0);
                    else if(left) action.MoveInTheAir(-1);
                    else if(right) action.MoveInTheAir(1);
                    
                    // 限制竖直速度.
                    role.rd.velocity = role.rd.velocity.Y(role.rd.velocity.y.Clamp(-role.action.maxVerticalSpeed, role.action.maxVerticalSpeed));
                }
            }
        }
        
        public sealed class MoveState : BaseState
        {
            public float leavingGroundTime = 0;
            
            public MoveState(RoleAction roleAction) : base(roleAction) { }
            
            public override IEnumerator<Transfer> Step()
            {
                while(true)
                {
                    yield return Pass();

                    bool attachedGround = action.TryAttachGround();

                    // 起跳. 不需要前置条件.
                    bool jump = CommandQueue.Get(KeyBinding.inst.rush);
                    if(jump)
                    {
                        action.Jump();
                        yield return Trans(new FlyState(action));
                    }

                    // 走离地面, 或者因环境原因强制离开地面. 过一段时间之后才会转换为飞行状态.
                    if(!attachedGround)
                    {
                        leavingGroundTime += Time.deltaTime;
                        if(leavingGroundTime >= action.minJumpTime)
                        {
                            yield return Trans(new FlyState(action));
                        }
                    }
                    else
                    {
                        leavingGroundTime = 0;
                    }

                    // 放技能.
                    if(TryUseAnySkill(out var trans))
                    {
                        yield return trans;
                        continue;
                    }

                    // 在地上左右移动.
                    bool left = CommandQueue.Get(KeyBinding.inst.moveLeft);
                    bool right = CommandQueue.Get(KeyBinding.inst.moveRight);
                    if(left == right) action.StayOnTheGround();
                    else if(left) action.MoveOnTheGround(-1, action.standingNormal);
                    else if(right) action.MoveOnTheGround(1, action.standingNormal);
                }
            }
        }

        #endregion
        // ============================================================================================================
        #region Properties 

        [Tooltip("空中的加比例.")]
        [Range(0, 10)] public float airAccRate;

        [Tooltip("在空中移动时, 水平飞行的额定速度.")]
        public float airHoriSpeed;

        [Tooltip("地面的加速比例.")]
        [Range(0, 10)] public float groundAccRate;

        [Tooltip("在地面移动时, 沿地面的额定速度.")]
        public float groundHoriSpeed;

        [Tooltip("跳跃动作产生的向上的速度.")]
        public float jumpSpeed;

        [Tooltip("最大允许的\"地面\"倾角.大于等于这个倾角会被认为不是地面. 单位:角度.")]
        public float groundAngle;

        [Tooltip("与竖直方向的夹角小于等于这个角度会被认为是\"墙壁\". 单位:角度.")]
        public float wallAngle;
        
        [Tooltip("在离开地面多少秒以后仍然可以跳跃.")]
        public float minJumpTime;

        [Tooltip("在跳跃时, 获得一个系数为该变量的额外速度加成.")]
        public float jumpHoriSpeedMult;

        [Tooltip("允许贴地的最大距离.")]
        public float attachDistance;
        
        [Tooltip("最大竖直速度.")]
        public float maxVerticalSpeed;
        
        
        
        public Role role => GetComponent<Role>();
        
        public MoveState GetMoveState() => new MoveState(this);
        
        public FlyState GetFlyState() => new FlyState(this);
        
        /// <summary>
        /// 起跳事件的回调函数.
        /// </summary>
        public Action<Role> JumpCallbacks = x => { };
        
        #endregion
        // ============================================================================================================
        #region Tool properties
        
        // 这些 tag 用于删除状态机.
        StateMachine.Tag stateTag;

        const int maxContactRetrive = 50;

        static readonly ContactPoint2D[] contactBuffer = new ContactPoint2D[maxContactRetrive];
        IEnumerable<ContactPoint2D> contacts
        {
            get
            {
                int cnt = role.rd.GetContacts(contactBuffer);
                return contactBuffer.ToEnumerable(cnt);
            }
        }

        /// <summary>
        /// 是否触地.
        /// </summary>
        bool touchingGround
        {
            get
            {
                foreach(var i in contacts) if(InGroundNormalRange(i.normal)) return true;
                return false;
            }
        }

        bool landing
        {
            get
            {
                int cc = 0;
                foreach(var i in contacts) if(InGroundNormalRange(i.normal)) cc++;
                return cc > 0;
            }
        }

        Vector2 standingNormal
        {
            get
            {
                foreach(var i in contacts) Debug.DrawRay(i.point, i.normal, Color.red);
                return Vector2.up;
            }
        }

        #endregion
        // ============================================================================================================
        #region Built-in & public methods
        
        void Start()
        {
            stateTag = StateMachine.Register(new FlyState(this)).tag;
        }

        void OnDestroy()
        {
            StateMachine.Remove(stateTag);
        }

        #endregion
        // ============================================================================================================
        #region Tool methods
        
        /// <summary>
        /// 强制贴地, 避免惯性导致的短暂的贴地"飞行".
        /// 当角色离地时, 应当检查其是否能够直接贴向地面.
        /// </summary>
        bool TryAttachGround()
        {
            if(touchingGround) return true;

            var colliders = new List<Collider2D>();
            role.rd.GetAttachedColliders(colliders);
            var inf = 1e10f;
            var dist = inf;
            
            // 收集该角色的所有多边形碰撞盒, 从每个顶点往下打出射线, 取最短的命中距离作为下移的距离.
            foreach(var col in colliders)
            {
                if(!(col is PolygonCollider2D poly)) continue;
                foreach(var pt in poly.GetPath(0))
                {
                    var origin = this.transform.position.ToVec2() + pt;
                    var hit = Physics2D.Raycast(origin, Vector2.down, inf, LayerMask.GetMask("Terrain"));
                    if(!hit) continue;
                    if(!InGroundNormalRange(hit.normal)) continue;
                    dist.UpdMin(hit.distance);
                }
            }
            
            if(dist < attachDistance)
            {
                role.rd.position += Vector2.down * dist;
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 在空气中进行左右移动. 默认 dir = 1 为右.
        /// </summary>
        void MoveInTheAir(int dir)
        {
            var targetVx = dir * airHoriSpeed;
            var curVx = role.rd.velocity.x;
            role.rd.velocity = role.rd.velocity.X(NextVelocity(curVx, targetVx, airAccRate, Time.deltaTime));
        }
        
        /// <summary>
        /// 强制主角停止在地面上.
        /// </summary>
        void StayOnTheGround()
        {
            role.rd.velocity = role.rd.velocity.X(0f);
        }
        
        /// <summary>
        /// 在地面移动 默认 dir = 1 为右. 校正移动速度的方向为地面的切线方向.
        /// </summary>
        void MoveOnTheGround(int dir, Vector2 groundNormal)
        {
            var curV = role.rd.velocity;
            Debug.DrawRay(this.transform.position, groundNormal, Color.red);
            Debug.DrawRay(this.transform.position, groundNormal.RotHalfPi(), Color.green);
            Debug.DrawRay(this.transform.position, -groundNormal.RotHalfPi(), Color.blue);
            var targetV = groundNormal.RotHalfPi().Dot(Vector2.right * dir) > 0f
                ? groundNormal.RotHalfPi() * groundHoriSpeed
                : -groundNormal.RotHalfPi() * groundHoriSpeed;
            Debug.DrawRay(this.transform.position, targetV, Color.black);
            if(curV.Dot(targetV) < 0f) curV = Vector2.zero;
            role.rd.velocity = NextVelocity(curV, targetV, groundAccRate, Time.deltaTime);
        }

        /// <summary>
        /// 判定该法线是不是"地面"的法线.
        /// </summary>
        bool InGroundNormalRange(Vector2 normal) => Vector2.Angle(normal, Vector2.up) <= groundAngle;
        
        /// <summary>
        /// 起跳.
        /// </summary>
        void Jump()
        {
            JumpCallbacks?.Invoke(role);
            
            // 先处理位移, 保证它在下一帧会离开地面.
            role.rd.position += Vector2.up * jumpSpeed * Time.deltaTime;
            role.rd.velocity += new Vector2(
                role.rd.velocity.x * jumpHoriSpeedMult,
                jumpSpeed
            );
        }


        // 速度随时间变化公式, 其中 H是额定速度, a是功率, C是当前速度:
        // f(t) = H - 2^-at * (H - C)
        // 就有:
        // f(r) - f(l) = (H - C) (- 2^-ar + 2^-al)
        //   = (H - C) a^-l (- 2^-a(r-l) + 1 )
        //   = (H - f(l)) (-2^-adt + 1)
        // 所以欧拉插值法用递推式: v(r) = v(l) + (H - v(l)) (1 - 2^-adt)
        public float NextVelocity(float curV, float targetV, float acc, float dt) => curV + (targetV - curV) * (1f - 2.Pow(-acc * dt));
        public Vector2 NextVelocity(Vector2 curV, Vector2 targetV, float acc, float dt) => curV + (targetV - curV) * (1f - 2.Pow(-acc * dt));
        
        #endregion
    }
}
