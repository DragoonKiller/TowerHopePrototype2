using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;
using Utils;

namespace Tower.Components
{
    // ============================================================================================================
    // Types
    // ============================================================================================================

    /// <summary>
    /// 定义了角色的移动行为.
    /// 角色移动的行为是独立的.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [System.Serializable]
    public class RoleAction : MonoBehaviour
    {
        sealed class FlyState : StateMachine
        {
            readonly RoleAction role;
            public FlyState(RoleAction roleAction) => this.role = roleAction;

            public override IEnumerator<Transfer> Step()
            {
                float t = 0;
                while(true)
                {
                    yield return Pass();

                    t += Time.deltaTime;
                    if(t < role.jumpTimeExpend &&
                        Input.GetKeyDown(KeyCode.W) &&
                        Time.time - role.lastJumpTime > role.jumpTimeExpend)
                    {
                        role.Jump();
                        yield return Pass();
                    }

                    if(role.landing)
                    {
                        yield return Trans(new MoveState(role));
                    }

                    if(Input.GetKeyDown(KeyCode.Space) && role.rushSkillCreator != null)
                        role.rushSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.rushAdvSkillCreator != null)
                        role.rushAdvSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.primarySkillCreator != null)
                        role.primarySkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.primaryAdvSkillCreator != null)
                        role.primaryAdvSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.secondarySkillCreator != null)
                        role.secondarySkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.secondaryAdvSkillCreator != null)
                        role.secondaryAdvSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.primaryAttackCreator != null)
                        role.primaryAttackCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.primaryAdvAttackCreator != null)
                        role.primaryAdvAttackCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.secondaryAttackCreator != null)
                        role.secondaryAttackCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.secondaryAdvAttackCreator != null)
                        role.secondaryAdvAttackCreator(role);

                    bool left = Input.GetKey(KeyCode.A);
                    bool right = Input.GetKey(KeyCode.D);
                    if(left == right) role.MoveInTheAir(0);
                    else if(left) role.MoveInTheAir(-1);
                    else if(right) role.MoveInTheAir(1);
                }
            }
        }

        sealed class MoveState : StateMachine
        {
            readonly RoleAction role;
            public MoveState(RoleAction roleAction) => this.role = roleAction;

            public override IEnumerator<Transfer> Step()
            {
                while(true)
                {
                    yield return Pass();

                    bool jump = Input.GetKeyDown(KeyCode.W);
                    if(jump && Time.time - role.lastJumpTime > role.jumpTimeExpend)
                    {
                        role.Jump();
                        yield return Trans(new FlyState(role));
                    }

                    if(role.leavingGround)
                    {
                        if(!role.TryAttachGround())
                        {
                            yield return Trans(new FlyState(role));
                        }
                    }


                    if(Input.GetKeyDown(KeyCode.Space) && role.rushSkillCreator != null)
                        role.rushSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.rushAdvSkillCreator != null)
                        role.rushAdvSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.primarySkillCreator != null)
                        role.primarySkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.primaryAdvSkillCreator != null)
                        role.primaryAdvSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.secondarySkillCreator != null)
                        role.secondarySkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.secondaryAdvSkillCreator != null)
                        role.secondaryAdvSkillCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.primaryAttackCreator != null)
                        role.primaryAttackCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.primaryAdvAttackCreator != null)
                        role.primaryAdvAttackCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && role.secondaryAttackCreator != null)
                        role.secondaryAttackCreator(role);
                    if(Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift) && role.secondaryAdvAttackCreator != null)
                        role.secondaryAdvAttackCreator(role);

                    bool left = Input.GetKey(KeyCode.A);
                    bool right = Input.GetKey(KeyCode.D);
                    if(left == right) role.StayOnTheGround();
                    else if(left) role.MoveOnTheGround(-1, role.standingNormal);
                    else if(right) role.MoveOnTheGround(1, role.standingNormal);
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

        // 下面是各种技能.

        public Func<RoleAction, Skill> rushSkillCreator;
        public Func<RoleAction, Skill> rushAdvSkillCreator;
        public Func<RoleAction, Skill> primarySkillCreator;
        public Func<RoleAction, Skill> primaryAdvSkillCreator;
        public Func<RoleAction, Skill> secondarySkillCreator;
        public Func<RoleAction, Skill> secondaryAdvSkillCreator;
        public Func<RoleAction, Skill> primaryAttackCreator;
        public Func<RoleAction, Skill> primaryAdvAttackCreator;
        public Func<RoleAction, Skill> secondaryAttackCreator;
        public Func<RoleAction, Skill> secondaryAdvAttackCreator;

        // ============================================================================================================
        // Tool properties
        // ============================================================================================================

        Rigidbody2D rd => GetComponent<Rigidbody2D>();

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
                int cnt = rd.GetContacts(contactBuffer);
                return contactBuffer.ToEnumerable(cnt);
            }
        }

        bool leavingGround
        {
            get
            {
                foreach(var i in contacts) if(InGroundNormalRange(i.normal)) return false;
                return true;
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

        CommandKey attackKey = new CommandKey() {
            key = KeyCode.Mouse0,
            ctrl = false,
            shift = false,
        };

        CommandKey magicAttackKey = new CommandKey() {
            key = KeyCode.Mouse1,
            ctrl = false,
            shift = false,
        };

        // ============================================================================================================
        // Built-in & public methods
        // ============================================================================================================

        void Start()
        {
            stateTag = StateMachine.Register(new FlyState(this)).tag;

            CommandQueue.BindInterval(
                attackKey,
                10,
                new CommandQueue.Callbacks() {
                    create = () => "Create!".Log(),
                    delete = () => "Delete!".Log(),
                }
            );

            CommandQueue.BindTimeout(
                magicAttackKey,
                0.2f,
                10,
                new CommandQueue.Callbacks()
                {
                    create = () => "Magic Create!".Log(),
                    delete = () => "Magic Delete!".Log(),
                }
            );
        }

        void OnDestroy()
        {
            StateMachine.Remove(stateTag);
            CommandQueue.Unbind(attackKey);
            CommandQueue.Unbind(magicAttackKey);
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
            if(!leavingGround) return true;

            var colliders = new List<Collider2D>();
            rd.GetAttachedColliders(colliders);
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
                    dist.UpdMin(hit.distance);
                }
            }
            if(dist != inf)
            {
                rd.position += Vector2.down * dist;
                return true;
            }
            return false;
        }

        void MoveInTheAir(int dir)
        {
            var targetVx = dir * airHoriSpeed;
            var curVx = rd.velocity.x;
            rd.velocity = rd.velocity.X(NextVelocity(curVx, targetVx, airAccRate, Time.deltaTime));
        }

        void StayOnTheGround()
        {
            rd.velocity = Vector2.zero;
        }

        void MoveOnTheGround(int dir, Vector2 groundNormal)
        {
            var targetVx = dir * groundHoriSpeed;
            var curVx = rd.velocity.x * rd.velocity.normalized.Cross(groundNormal.normalized).Abs();
            rd.velocity = rd.velocity.X(NextVelocity(curVx, targetVx, airAccRate, Time.deltaTime));
        }

        /// <summary>
        /// 判定该法线是不是"地面"的法线.
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        bool InGroundNormalRange(Vector2 normal)
             => Vector2.Angle(normal, Vector2.up) < groundAngle;

        void Jump()
        {
            lastJumpTime = Time.time;
            // 先处理位移, 保证它在下一帧会离开地面.
            rd.position += Vector2.up * jumpSpeed * Time.deltaTime;
            rd.velocity += new Vector2(
                rd.velocity.x * jumpHoriSpeedMult,
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
        float NextVelocity(float curV, float targetV, float acc, float dt)
            => curV + (targetV - curV) * (1f - 2.Pow(-acc * dt));
    }
}
