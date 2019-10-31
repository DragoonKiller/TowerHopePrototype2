using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tower.Components
{
    using Tower.Global;
    using Systems;
    using Utils;
    
    /// <summary>
    /// 控制角色放置复活点, 存储所有有效复活点.
    /// 这个动作和技能与角色动作控制是独立的.
    /// </summary>
    [RequireComponent(typeof(Role))]
    public sealed class RoleReviveStone : MonoBehaviour
    {
        [Tooltip("复活石放置对象的prefab.")]
        public ReviveStone sourceStone;
        
        [Tooltip("复活石指示对象的prefab.")]
        public ReviveStoneConnection sourceConnection;
        
        [Tooltip("存储了复活石的背包.")]
        public Transform inventory;
        
        [Tooltip("角色放置复活点的耗时.")]
        public float placeTime;
        
        [Tooltip("放置石头时的特效控制脚本.")]
        public ReviveStonePlaceFX reviveStonePlaceFX;
        
        [Header("Debug")]
        
        [Tooltip("复活点列表.")]
        public List<ReviveStone> revivePoints;
        
        [Tooltip("复活点连接列表.")]
        public List<ReviveStoneConnection> reviveConnections;
        
        [Tooltip("角色按下放置复活点按钮的计时.")]
        [SerializeField] float process;
        
        Role role => this.GetComponent<Role>();
        
        bool hasReviveStone => inventory.childCount != 0;
        
        /// <summary>
        /// 删掉所有复活点.
        /// </summary>
        public void Clear()
        {
            revivePoints.Clear();
        }
        
        void Update()
        {
            Settle();
            SetFX();
            CleanRevivePoints();
            ResetReviveConnection();
        }
        
        /// <summary>
        /// 根据玩家输入, 设置复活点.
        /// </summary>
        void Settle()
        {
            if(!CommandQueue.Get(KeyBinding.inst.setRevive) || !hasReviveStone)
            {
                process = 0;
                return;
            }
            
            process += Time.deltaTime;
            if(process <= placeTime) return;
            
            if(!TryConsumeReviveStone()) return;
            
            GenerateRevivePoint();
            process = 0;
        }
        
        /// <summary>
        /// 设置特效脚本.
        /// </summary>
        void SetFX()
        {
            reviveStonePlaceFX.process = process / placeTime;
        }
        
        /// <summary>
        /// 删掉列表中所有失效的复活点.
        /// </summary>
        void CleanRevivePoints()
        {
            revivePoints.RemoveAll(x => x == null);
        }
        
        /// <summary>
        /// 生成复活点对象, 把它加入自己的复活点列表.
        /// </summary>
        void GenerateRevivePoint()
        {
            var x = Instantiate(sourceStone.gameObject).GetComponent<ReviveStone>();
            x.transform.position = this.gameObject.transform.position;
            revivePoints.Add(x);
        }
        
        /// <summary>
        /// 尝试消耗一个地图石. 地图石的消耗脚本会把自己删除.
        /// </summary>
        bool TryConsumeReviveStone()
        {
            if(!hasReviveStone) return false;
            if(!inventory.GetChild(0).TryGetComponent<ReviveStonePickedItem>(out var reviveStone)) return false;
            reviveStone.Consume();
            return true;
        }
        
        /// <summary>
        /// 同步复活石之间的连接.
        /// </summary>
        void ResetReviveConnection()
        {
            while(reviveConnections.Count < revivePoints.Count - 1)
            {
                reviveConnections.Add(Instantiate(sourceConnection));
            }
            
            while(reviveConnections.Count != 0 && reviveConnections.Count > revivePoints.Count - 1)
            {
                DestroyImmediate(reviveConnections.Last().gameObject);
                reviveConnections.RemoveLast();
            }
            
            for(int i = 0; i < reviveConnections.Count; i++)
            {
                var from = revivePoints[i].transform.position.ToVec2();
                var to = revivePoints[i + 1].transform.position.ToVec2();
                reviveConnections[i].SetFromTo(from, to);
            }
        }
    }
}
