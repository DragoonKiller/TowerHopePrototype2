using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    using Utils;
    
    /// <summary>
    /// 控制角色死亡复活.
    /// </summary>
    [RequireComponent(typeof(RolePlayerControl))]
    [RequireComponent(typeof(RoleHealth))]
    [RequireComponent(typeof(RoleReviveStone))]
    public class RoleDeathRevive : MonoBehaviour
    {
        [Tooltip("保留复活点. 如果没有复活石, 则使用该复活点.")]
        public Vector2 reservedRevivePoint;
        
        [Tooltip("复活耗时.")]
        public float duration;
        
        [Tooltip("最大位置变化.")]
        public Vector2 maxOffset;
        
        [Tooltip("位置变化曲线.")]
        public AnimationCurve offsetCurve;
        
        [Tooltip("复活特效控制脚本.")]
        public ReviveFX fx;
        
        /// <summary>
        /// 死亡动作进度.
        /// </summary>
        public float deathProcess { get; private set; }
        
        /// <summary>
        /// 是否正在处理死亡过程.
        /// </summary>
        public bool isDeath { get; private set; }
        
        RolePlayerControl action => this.GetComponent<RolePlayerControl>();
        RoleHealth health => this.GetComponent<RoleHealth>();
        RoleReviveStone reviveStones => this.GetComponent<RoleReviveStone>();
        RoleTrail trail => this.GetComponent<RoleTrail>();
        
        StateMachine.Tag stateTag;

        sealed class WaitForDeathState : StateMachine
        {
            public RoleDeathRevive revive;
            
            public override IEnumerator<Transfer> Step()
            {
                while(true)
                {
                    yield return Pass();
                    
                    if(revive.health.curHealth <= 0)
                    {
                        // 关闭玩家输入.
                        revive.action.enabled = false;
                        
                        // 转移到死亡状态.
                        yield return Trans(new DeathState() { revive = revive });
                    }
                }
            }
        }

        sealed class DeathState : StateMachine
        {
            public RoleDeathRevive revive;
            
            public override IEnumerator<Transfer> Step()
            {
                var beginTime = Time.time;
                var basePos = revive.transform.position;
                
                revive.isDeath = true;
                revive.deathProcess = 0;
                
                while(true)
                {
                    yield return Pass();
                    var totalTime = Time.time - beginTime;
                    
                    revive.deathProcess = totalTime / revive.duration;
                    
                    if(totalTime < revive.duration)
                    {
                        var offset = revive.maxOffset * revive.offsetCurve.Evaluate(revive.deathProcess);
                        revive.transform.position = (basePos.ToVec2() + offset).ToVec3(basePos.z);
                    }
                    else
                    {
                        // 回满血.
                        revive.health.curHealth = revive.health.maxHealth;
                        
                        // 打开玩家输入.
                        revive.action.enabled = true;
                        
                        // 设置玩家位置.
                        if(revive.reviveStones.revivePoints.Count != 0)
                        {
                            revive.transform.position = revive.reviveStones.revivePoints.Last().transform.position.Z(revive.transform.position.z);
                        }
                        else
                        {
                            revive.transform.position = revive.reservedRevivePoint.ToVec3(revive.transform.position.z);
                        }
                        
                        // 设置特效.
                        if(revive.trail) revive.trail.Reset();
                        
                        revive.isDeath = false;
                        revive.deathProcess = 0;
                        
                        // 转移到等待状态.
                        yield return Trans(new WaitForDeathState() { revive = revive });
                    }
                }
            }
        }
        
        void Start()
        {
            var stm = new WaitForDeathState() { revive = this };
            stateTag = stm.tag;
            StateMachine.Register(stm);
        }
        
        void OnDestroy()
        {
            StateMachine.Remove(stateTag);
        }
    }
}
