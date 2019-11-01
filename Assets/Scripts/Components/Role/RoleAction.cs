using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Tower.Components
{
    using Systems;
    using Utils;
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
    [RequireComponent(typeof(RoleRigidbody))]
    [RequireComponent(typeof(RoleSkills))]
    [Serializable]
    public sealed class RoleAction : MonoBehaviour
    {
        #region Types
        
        public abstract class BaseState : StateMachine
        {
            protected readonly RoleAction action;
            protected RoleSkills skills => action.GetComponent<RoleSkills>();
            protected Rigidbody2D rd => action.GetComponent<Rigidbody2D>();
            public BaseState(RoleAction roleAction) => this.action = roleAction;
            
            /// <summary>
            /// 工具函数: 尝试施放技能.
            /// </summary>
            protected bool TryUseSkill(ISkillConfig skill, KeyCode key, out StateMachine.Transfer trans)
            {
                if(CommandQueue.Get(key) && skill != null)
                {
                    if(skill.TryGetState(action.gameObject, out var stm))
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
                    (skills.primary, KeyBinding.inst.primarySkill),
                    (skills.secondary, KeyBinding.inst.secondarySkill),
                    (skills.attack, KeyBinding.inst.attack),
                    (skills.magicAttack, KeyBinding.inst.magicAttack)
                };
                
                foreach(var (skill, key) in skillList) if(TryUseSkill(skill, key, out resTrans)) return true;
                resTrans = new StateMachine.Transfer();
                return false;
            }
        }
        
        public sealed class FlyState : BaseState
        {
            public FlyState(RoleAction roleAction) : base(roleAction) { }
            
            /// <summary>
            /// 抓墙时限. 墙跳后一段时间内不允许再抓墙.
            /// </summary>
            float grabWallCooldownTimer;
            
            public override IEnumerator<Transfer> Step()
            {
                float beginTime = Time.time;
                grabWallCooldownTimer = 0f;
                while(true)
                {
                    yield return Pass();
                    if(!action.enabled) continue; 
                    
                    float t = Time.time - beginTime;
                    grabWallCooldownTimer = (grabWallCooldownTimer - Time.deltaTime).Max(0f);
                    
                    // 在起跳之后的一段时间内, 不能切换为落地状态.
                    if(action.touchingGround && t > action.minJumpTime)
                    {
                        yield return Trans(new MoveState(action));
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
                    
                    // 尝试抓墙.
                    var grabDir = 0;
                    if(grabWallCooldownTimer == 0f) grabDir = action.TryAttachWall();
                    
                    // 冲刺和跳跃是同一个按键.
                    if(CommandQueue.Get(KeyBinding.inst.rush))
                    {
                        // 如果抓墙成功, 那么支持墙跳.
                        if(grabDir != 0)
                        {
                            grabWallCooldownTimer = action.grabWallCooldown;
                            action.WallJump(grabDir);
                        }
                        
                        // 否则, 处理冲刺.
                        else if(TryUseSkill(skills.rush, KeyBinding.inst.rush, out var transJump))
                        {
                            yield return transJump;
                        }
                    }
                    
                    // 限制竖直速度.
                    rd.velocity = rd.velocity.Y(rd.velocity.y.Clamp(-action.maxVerticalSpeed, action.maxVerticalSpeed));
                    
                    // 如果抓墙方向和当前按键方向不一致, 或者当前没有抓墙.
                    if(!((left && grabDir == -1) || (right && grabDir == 1)))
                    {
                        // 当前正在抓墙, 但是想要离开.
                        if((left && grabDir == 1) || (right && grabDir == -1))
                        {
                            grabWallCooldownTimer = action.grabWallCooldown;
                            action.WallJump(-grabDir);
                        }
                        
                        // 处理在空中的移动.
                        if(left == right && grabDir == 0) action.MoveInTheAir(0);
                        else if(left) action.MoveInTheAir(-1);
                        else if(right) action.MoveInTheAir(1);
                    }
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
                    if(!action.enabled) continue; 
                    
                    bool attachedGround = action.TryAttachGround();

                    // 起跳. 不需要前置条件.
                    if(CommandQueue.Get(KeyBinding.inst.rush))
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
        
        [Tooltip("墙跳速度.")]
        public float wallJumpSpeed;
        
        [Tooltip("墙跳减速倍率.")]
        public float wallJumpSpeedMult;
        
        [Tooltip("成功抓墙时的目标速度.")]
        public float grabWallMaxSpeed;
        
        [Tooltip("墙跳后这么长时间内不允许再抓墙.")]
        public float grabWallCooldown;
        
        [Tooltip("最大允许的\"地面\"倾角.大于等于这个倾角会被认为不是地面. 单位:角度.")]
        public float groundAngle;

        [Tooltip("与竖直方向的夹角小于等于这个角度会被认为是\"墙壁\". 单位:角度.")]
        public float wallAngle;
        
        [Tooltip("在离开地面多少秒以后仍然可以跳跃.")]
        public float minJumpTime;

        [Tooltip("在跳跃时, 获得一个系数为该变量的额外速度加成.")]
        public float jumpHoriSpeedMult;

        [Tooltip("允许贴地的最大距离.")]
        public float attachGroundDist;
        
        [Tooltip("允许抓墙的最大距离.")]
        public float grabWallDist;
        
        [Tooltip("最大竖直速度.")]
        public float maxVerticalSpeed;
        
        public Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
        
        public RoleSkills skills => this.GetComponent<RoleSkills>();
        
        public MoveState GetMoveState() => new MoveState(this);
        
        public FlyState GetFlyState() => new FlyState(this);
        
        /// <summary>
        /// 起跳事件的回调函数.
        /// </summary>
        public Action<GameObject> JumpCallbacks = x => { };
        
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
                int cnt = rd.GetContacts(contactBuffer);
                return contactBuffer.ToEnumerable(cnt);
            }
        }

        /// <summary>
        /// 是否触地.
        /// </summary>
        public bool touchingGround => contacts.Aggregate(false, (a, x) => a || InGroundNormalRange(x.normal));
        
        /// <summary>
        /// 是否触墙.
        /// </summary>
        public bool touchingWall => contacts.Aggregate(false, (a, x) => a || InWallNormalRange(x.normal));
        
        /// <summary>
        /// 目前所站位置的地面法线. 取离竖直向上的方向最近的接触点向量.
        /// </summary>
        Vector2 standingNormal => contacts.Aggregate(
            Vector2.down, 
            (a, x) => Vector2.Angle(Vector2.up, a) < Vector2.Angle(Vector2.up, x.normal) ? a : x.normal 
        );
        
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
        ///  收集该角色的所有多边形碰撞盒, 从每个顶点往下打出射线, 取最短的命中距离作为下移的距离.
        /// 顺便记录平均法线, 用于后续操作.
        /// 返回向量的方向表示[平均法线方向, 返回的长度表示离表面的距离.
        /// </summary>
        (Vector2? left, Vector2? right) GetWallAttachInfo()
        {
            var colliders = new List<Collider2D>();
            rd.GetAttachedColliders(colliders);
            const float inf = 1e10f;
            var (leftDist, rightDist) = (inf, inf);
            var (leftSum, rightSum) = (Vector2.zero, Vector2.zero);
            var (leftCnt, rightCnt) = (0, 0);
            foreach(var col in colliders)
            {
                if(!(col is PolygonCollider2D poly)) continue;
                foreach(var pt in poly.GetPath(0))
                {
                    var origin = this.transform.position.ToVec2() + pt;
                    var (leftHit, rightHit) = (
                        Physics2D.Raycast(origin, Vector2.left, grabWallDist, LayerMask.GetMask("Terrain")),
                        Physics2D.Raycast(origin, Vector2.right, grabWallDist, LayerMask.GetMask("Terrain"))
                    );
                    if(leftHit && InWallNormalRange(leftHit.normal))
                    {
                        leftDist.UpdMin(leftHit.distance);
                        leftSum += leftHit.normal;
                        leftCnt += 1;
                    }
                    if(rightHit && InWallNormalRange(rightHit.normal))
                    {
                        rightDist.UpdMin(rightHit.distance);
                        rightSum += rightHit.normal;
                        rightCnt += 1;
                    }
                }
            }
            
            return (
                leftCnt == 0 ? (Vector2?)null : (leftSum / leftCnt).normalized * leftDist,
                rightCnt == 0 ? (Vector2?)null : (rightSum / rightCnt).normalized * rightDist
            );
        }
        
        /// <summary>
        /// 墙跳动作. 数值速度减半, 朝法线方向跳跃.
        /// 如果两边都有触墙, 什么都不做.
        /// </summary>
        void WallJump(int dir)
        {
            var (leftAttach, rightAttach) = GetWallAttachInfo();
            if(leftAttach.HasValue && rightAttach.HasValue) return;
            
            var jumpDir = leftAttach.HasValue ? leftAttach.Value.normalized : rightAttach.Value.normalized;
            
            rd.velocity = rd.velocity * wallJumpSpeedMult + jumpDir * wallJumpSpeed;
        }
        
        
        /// <summary>
        /// 尝试抓墙.
        /// dir 返回 -1 表示墙在左边, 1 表示墙在右边. 失败, 或者已经接触墙壁, 则返回 0.
        /// </summary>
        int TryAttachWall()
        {
            // 判断应该向哪个方向贴墙.
            int attachDir = 0;
            var (leftAttach, rightAttach) = GetWallAttachInfo();
            
            // 取方向.
            if(leftAttach.HasValue && rightAttach.HasValue) attachDir = UnityEngine.Random.Range(0, 1) * 2 - 1;
            else if(leftAttach.HasValue) attachDir = -1;
            else if(rightAttach.HasValue) attachDir = 1;
            else return 0;
            
            Debug.Assert(attachDir != 0);
            
            // 贴墙操作.
            float attachDist = leftAttach != null ? leftAttach.Value.magnitude : rightAttach.Value.magnitude;
            rd.position += attachDir * attachDist * Vector2.right;
            
            // 把速度改换成沿着墙面切线的方向, 以保证在结束抓墙时的速度顺着墙的切线方向.
            var normal = leftAttach != null ? leftAttach.Value.normalized : rightAttach.Value.normalized;
            var tangent = normal.RotHalfPi();
            if(rd.velocity.Dot(tangent) > 0) rd.velocity = rd.velocity.magnitude * tangent;
            else rd.velocity = rd.velocity.magnitude * -tangent;
            Debug.DrawRay(this.transform.position, normal, Color.green);
            Debug.DrawRay(this.transform.position, tangent, Color.yellow);
            
            // 限制贴墙速度.
            rd.velocity = rd.velocity.Len(rd.velocity.magnitude.Min(grabWallMaxSpeed));
            
            return attachDir;
        }
        
        /// <summary>
        /// 强制贴地, 避免惯性导致的短暂的贴地"飞行".
        /// 当角色离地时, 应当检查其是否能够直接贴向地面.
        /// 当角色已经触地时, 直接返回 true.
        /// </summary>
        bool TryAttachGround()
        {
            if(touchingGround) return true;
            
            const float inf = 1e10f;
            var dist = inf;
            
            // 收集该角色的所有多边形碰撞盒, 从每个顶点往下打出射线, 取最短的命中距离作为下移的距离.
            var colliders = new List<Collider2D>();
            rd.GetAttachedColliders(colliders);
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
            
            if(dist < attachGroundDist)
            {
                rd.position += Vector2.down * dist;
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
            var curVx = rd.velocity.x;
            rd.velocity = rd.velocity.X(Maths.PowerStep(curVx, targetVx, airAccRate, Time.deltaTime));
        }
        
        /// <summary>
        /// 强制主角停止在地面上.
        /// </summary>
        void StayOnTheGround()
        {
            rd.velocity = rd.velocity.X(0f);
        }
        
        /// <summary>
        /// 在地面移动 默认 dir = 1 为右. 校正移动速度的方向为地面的切线方向.
        /// </summary>
        void MoveOnTheGround(int dir, Vector2 groundNormal)
        {
            var curV = rd.velocity;
            Debug.DrawRay(this.transform.position, groundNormal, Color.red);
            Debug.DrawRay(this.transform.position, groundNormal.RotHalfPi(), Color.green);
            Debug.DrawRay(this.transform.position, -groundNormal.RotHalfPi(), Color.blue);
            var targetV = groundNormal.RotHalfPi().Dot(Vector2.right * dir) > 0f
                ? groundNormal.RotHalfPi() * groundHoriSpeed
                : -groundNormal.RotHalfPi() * groundHoriSpeed;
            Debug.DrawRay(this.transform.position, targetV, Color.black);
            if(curV.Dot(targetV) < 0f) curV = Vector2.zero;
            rd.velocity = Maths.PowerStep(curV, targetV, groundAccRate, Time.deltaTime);
        }

        /// <summary>
        /// 判定该法线是不是"地面"的法线.
        /// </summary>
        bool InGroundNormalRange(Vector2 normal) => Vector2.Angle(normal, Vector2.up) <= groundAngle;
        
        /// <summary>
        /// 判定该法线是不是"墙面"法线.
        /// </summary>
        bool InWallNormalRange(Vector2 normal) => Vector2.Angle(normal, Vector2.right).Min(Vector2.Angle(normal, Vector2.left)) <= wallAngle;
        
        /// <summary>
        /// 起跳.
        /// </summary>
        void Jump()
        {
            JumpCallbacks?.Invoke(this.gameObject);
            
            // 先处理位移, 保证它在下一帧会离开地面.
            rd.position += Vector2.up * jumpSpeed * Time.deltaTime;
            rd.velocity += new Vector2(
                rd.velocity.x * jumpHoriSpeedMult,
                jumpSpeed
            );
        }


        #endregion
    }
}
