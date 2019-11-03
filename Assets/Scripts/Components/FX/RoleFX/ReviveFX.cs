using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Utils;
    
    /// <summary>
    /// 角色死亡复活时的特效脚本.
    /// 使用脚本控制一些粒子特效而不使用内置的粒子组件.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RoleDeathRevive))]
    public class ReviveFX : MonoBehaviour
    {
        [Tooltip("负责复活特效的粒子系统.")]
        public new ParticleSystem particleSystem;
        
        [Tooltip("发生器半径.")]
        public float radius;
        
        [Tooltip("逼近曲线.")]
        public AnimationCurve approchCurve;
        
        [Tooltip("最大缩放.")]
        public float maxScale;
        
        [Tooltip("缩放曲线.")]
        public AnimationCurve scaleCurve;
        
        [Tooltip("控制角色复活的脚本.")]
        public RoleDeathRevive revive;
        
        [Header("Debug")]
        [SerializeField] public ParticleSystem.Particle[] particles;
        [SerializeField] public List<ParticleSystem.Particle> nextParticles = new List<ParticleSystem.Particle>();
        
        void Update()
        {
            AdjustParticleSystem();
        }
        
        void AdjustParticleSystem()
        {
            var emission = particleSystem.emission;
            emission.enabled = revive.isDeath;
            
            // 复活后不再显示该特效, 即使粒子还没达到消失时间.
            if(!revive.isDeath)
            {
                particleSystem.SetParticles(particles, 0, 0);
                return;
            }
            
            if(particles == null || particleSystem.particleCount > particles.Length)
            {
                particles = new ParticleSystem.Particle[(particleSystem.particleCount * 1.2f).CeilToInt()];
            }
            
            particleSystem.GetParticles(particles);
            for(int i = 0; i < particleSystem.particleCount; i++)
            {
                // 如果是新生成的粒子, 把它的位置放到半径上.
                if(particles[i].startLifetime - particles[i].remainingLifetime <= Time.deltaTime)
                {
                    particles[i].position = (this.transform.position.ToVec2() + Vector2.up.Rot(Random.Range(0, 2 * Mathf.PI)) * radius).ToVec3(particles[i].position.z);
                }
                
                var pos = particles[i].position.ToVec2();
                var dir = pos.To(this.transform.position.ToVec2());
                var process = 1 - particles[i].remainingLifetime / particles[i].startLifetime;
                var nextProcess = 1 - (particles[i].remainingLifetime - Time.deltaTime) / particles[i].startLifetime;
                
                // 将粒子对准角色中心.
                particles[i].rotation = -dir.Angle() * Mathf.Rad2Deg - 90;
                
                // 把粒子往角色中心移动.
                var curDist = dir.magnitude;
                var curApproch = approchCurve.Evaluate(process);
                var nxtApproch = approchCurve.Evaluate(nextProcess);
                var nxtDist = curDist / (1 - curApproch) * (1 - nxtApproch);
                var move = dir.Len(curDist - nxtDist);
                
                // 设置粒子的速度, 用于插值.
                particles[i].velocity = move / Time.deltaTime;
                
                // 设置粒子大小.
                particles[i].startSize = maxScale * scaleCurve.Evaluate(process);
            }
            
            particleSystem.SetParticles(particles);
        }
    }

}
