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

    // ============================================================================================================
    // Types
    // ============================================================================================================

    /// <summary>
    /// 定义了角色的移动行为.
    /// 角色移动的行为是独立的.
    /// </summary>
    [RequireComponent(typeof(Role))]
    [RequireComponent(typeof(Rigidbody2D))]
    [Serializable]
    public sealed class RoleAction : MonoBehaviour
    {
        sealed class FlyState : StateMachine
        {
            readonly RoleAction action;
            Role role => action.role;
            RoleSkills skills => role.skills;
            public FlyState(RoleAction roleAction) => this.action = roleAction;

            public override IEnumerator<Transfer> Step()
            {
                float t = 0;
                while(true)
                {
                    yield return Pass();

                    t += Time.deltaTime;
                    if(t < action.jumpTimeExpend &&
                        CommandQueue.Top(KeyBinding.inst.jump) &&
                        Time.time - action.lastJumpTime > action.jumpTimeExpend)
                    {
                        action.Jump();
                        yield return Pass();
                    }

                    // 在起跳之后的一段时间内, 不能切换为落地状态.
                    if(action.landing && t > action.jumpTimeExpend)
                    {
                        yield return Trans(new MoveState(action));
                    }

                    if(CommandQueue.Get(KeyBinding.inst.rush) && skills.rush != null)
                        if(skills.rush.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.primarySkill) && skills.primary != null)
                        if(skills.primary.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.secondarySkill) && skills.secondary != null)
                            if(skills.secondary.TryGetState(role, out var stm))
                             yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.attack) && skills.attack != null)
                        if(skills.attack.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.magicAttack) && skills.attack != null)
                        if(skills.magicAttack.TryGetState(role, out var stm))
                            yield return Call(stm);

                    bool left = CommandQueue.Get(KeyBinding.inst.moveLeft);
                    bool right = CommandQueue.Get(KeyBinding.inst.moveRight);
                    if(left == right) action.MoveInTheAir(0);
                    else if(left) action.MoveInTheAir(-1);
                    else if(right) action.MoveInTheAir(1);
                }
            }
        }

        sealed class MoveState : StateMachine
        {
            readonly RoleAction action;
            Role role => action.role;
            RoleSkills skills => role.skills;
            public MoveState(RoleAction roleAction) => this.action = roleAction;

            public override IEnumerator<Transfer> Step()
            {
                while(true)
                {
                    yield return Pass();

                    bool attachedGround = action.TryAttachGround();

                    bool jump = CommandQueue.Top(KeyBinding.inst.jump);
                    if(jump && Time.time - action.lastJumpTime > action.jumpTimeExpend && attachedGround)
                    {
                        action.Jump();
                        yield return Trans(new FlyState(action));
                    }

                    if(!attachedGround)
                    {
                        yield return Trans(new FlyState(action));
                    }

                    if(CommandQueue.Get(KeyBinding.inst.rush) && skills.rush != null)
                        if(skills.rush.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.primarySkill) && skills.primary != null)
                        if(skills.primary.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.secondarySkill) && skills.secondary != null)
                        if(skills.secondary.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.attack) && skills.attack != null)
                        if(skills.attack.TryGetState(role, out var stm))
                            yield return Call(stm);

                    if(CommandQueue.Get(KeyBinding.inst.magicAttack) && skills.attack != null)
                        if(skills.magicAttack.TryGetState(role, out var stm))
                            yield return Call(stm);

                    bool left = CommandQueue.Get(KeyBinding.inst.moveLeft);
                    bool right = CommandQueue.Get(KeyBinding.inst.moveRight);
                    if(left == right) action.StayOnTheGround();
                    else if(left) action.MoveOnTheGround(-1, action.standingNormal);
                    else if(right) action.MoveOnTheGround(1, action.standingNormal);
                }
            }
        }


        // ============================================================================================================
        // Properties
        // ============================================================================================================

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

        [Tooltip("最大允许的\"地面\"倾角.大于等于这个倾角会被认为不是地面.")]
        public float groundAngle;

        [Tooltip("在离开地面多少秒以后仍然可以跳跃.")]
        public float jumpTimeExpend;

        [Tooltip("在跳跃时, 获得一个系数为该变量的额外速度加成.")]
        public float jumpHoriSpeedMult;

        [Tooltip("允许贴地的最大距离.")]
        public float attachDistance;

        // 下面是给其它组件使用的部分.

        public Role role => GetComponent<Role>();

        // ============================================================================================================
        // Tool properties
        // ============================================================================================================

        // 这些 tag 用于删除状态机.
        StateMachine.Tag stateTag;

        /// <summary>
        /// 上次跳跃的时间.
        /// </summary>
        float lastJumpTime;

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

        // ============================================================================================================
        // Built-in & public methods
        // ============================================================================================================

        void Start()
        {
            stateTag = StateMachine.Register(new FlyState(this)).tag;
        }

        void OnDestroy()
        {
            StateMachine.Remove(stateTag);
        }


        // ============================================================================================================
        // Tool methods
        // ============================================================================================================


        /// <summary>
        /// 避免惯性导致的短暂的贴地"飞行".
        /// 当角色离地时, 应当检查其是否能够直接贴向地面.
        /// </summary>
        bool TryAttachGround()
        {
            if(touchingGround) return true;

            var colliders = new List<Collider2D>();
            role.rd.GetAttachedColliders(colliders);
            var inf = 1e10f;
            var dist = inf;
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

        void MoveInTheAir(int dir)
        {
            var targetVx = dir * airHoriSpeed;
            var curVx = role.rd.velocity.x;
            role.rd.velocity = role.rd.velocity.X(NextVelocity(curVx, targetVx, airAccRate, Time.deltaTime));
        }

        void StayOnTheGround()
        {
            role.rd.velocity = Vector2.zero;
        }

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
        /// <param name="normal"></param>
        /// <returns></returns>
        bool InGroundNormalRange(Vector2 normal)
             => Vector2.Angle(normal, Vector2.up) <= groundAngle;

        void Jump()
        {
            lastJumpTime = Time.time;
            // 先处理位移, 保证它在下一帧会离开地面.
            role.rd.position += Vector2.up * jumpSpeed * Time.deltaTime;
            role.rd.velocity += new Vector2(
                role.rd.velocity.x * jumpHoriSpeedMult,
                jumpSpeed
            );
        }

        // 速度随时间变化公式,其中 H是额定速度,a是功率,C是当前速度:
        // f(t) = H - 2^-at * (H - C)
        // 就有:
        // f(r) - f(l) = (H - C) (- 2^-ar + 2^-al)
        //   = (H - C) a^-l (- 2^-a(r-l) + 1 )
        //   = (H - f(l)) (-2^-adt + 1)
        // 所以欧拉插值法用递推式: v(r) = v(l) + (H - v(l)) (1 - 2^-adt)
        float NextVelocity(float curV, float targetV, float acc, float dt) => curV + (targetV - curV) * (1f - 2.Pow(-acc * dt));
        Vector2 NextVelocity(Vector2 curV, Vector2 targetV, float acc, float dt) => curV + (targetV - curV) * (1f - 2.Pow(-acc * dt));
    }
}
