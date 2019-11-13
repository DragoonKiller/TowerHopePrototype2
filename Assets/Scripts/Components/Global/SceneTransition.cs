using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tower.Components
{
    using Systems;
    using Utils;
    
    /// <summary>
    /// 处理整个转场过程.
    /// 转场过程是这样的:
    /// 时间点 1 :
    ///   设置玩家角色为不可控制
    ///   开始 进入转场计时
    ///   开始 拉下幕布
    /// 时间点 2 : 幕布完全拉下
    ///   开始 异步删除当前关卡
    ///   开始 异步加载当前关卡
    /// 时间点 3 : 异步加载完成 且 异步删除完成
    ///   开始 拉开幕布
    /// 时间点 4 : 幕布完全拉开
    ///   恢复玩家角色为可控制
    /// 这个脚本控制从"异步加载转场场景"开始, 到"幕布完全拉开"的整个过程.
    /// 并且提供回调函数来告知场景是否加载完毕.
    /// 这个脚本应该挂在永远存在于游戏世界的对象中.
    /// </summary>
    public sealed class SceneTransition : MonoBehaviour
    {
        public static SceneTransition inst;
        
        [Tooltip("转场特效的淡入淡出时间.")]
        public float fadeTime;
        
        [Tooltip("当前关卡场景.")]
        public string curSceneName;
        
        [Tooltip("淡入淡出进度. 其它对象根据该值渲染幕布.")]
        public float curtain;
        
        string changeSceneName;
        
        void Awake() => inst = this;
        
        
        public void ChangeScene(string name)
        {
            changeSceneName = name;
            StateMachine.Register(new SceneTransitionProcess(this));
        }
        
        public sealed class SceneTransitionProcess : StateMachine
        {
            SceneTransition trans;

            public SceneTransitionProcess(SceneTransition trans) => this.trans = trans;
            
            public override IEnumerator<Transfer> Step()
            {
                // 幕布拉下.
                float t = Time.time;
                while(Time.time - t < trans.fadeTime)
                {
                    trans.curtain = (Time.time - t) / trans.fadeTime;
                    yield return Pass();
                }
                trans.curtain = 1;
                
                // 场景切换.
                var nxt = SceneManager.LoadSceneAsync(trans.changeSceneName, LoadSceneMode.Additive);
                nxt.allowSceneActivation = true;
                var cur = SceneManager.UnloadSceneAsync(trans.curSceneName);
                while(!(nxt.isDone && cur.isDone)) yield return Pass();
                
                // 幕布升起.
                t = Time.time;
                while(Time.time - t < trans.fadeTime)
                {
                    trans.curtain = 1 - (Time.time - t) / trans.fadeTime;
                    yield return Pass();
                }
                trans.curtain = 0;
            }
        }
    }
}
