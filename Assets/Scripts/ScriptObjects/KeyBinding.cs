using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System;

using Systems;

// 警告: 变量的值恒为 0.
// 然而这个值可以通过编辑器修改.
#pragma warning disable CS0649

namespace Tower.Global
{
    [CreateAssetMenu(fileName = "KeyBinding", menuName = "Global/KeyBinding", order = 199)]
    public class KeyBinding : ScriptableObject
    {
        [Serializable]
        public struct Setting
        {
            public CommandKey key;
            public CommandType type;

            [Tooltip("事件优先级. 同时存在的事件将会按照优先级排序.")]
            public int priority;

            [Tooltip("事件的触发时间. 只对 CommandType.Timeout 有效.")]
            public float timeout;
        }

        public static KeyBinding inst;

        // 注意: 命名后缀 Setting 会被反射读取.

        [SerializeField] Setting moveLeftSetting;
        [SerializeField] Setting moveRightSetting;
        [SerializeField] Setting jumpSetting;
        [SerializeField] Setting crouchSetting;

        [SerializeField] Setting rushSetting;
        [SerializeField] Setting attackSetting;
        [SerializeField] Setting magicAttackSetting;
        [SerializeField] Setting primiarySkillSetting;
        [SerializeField] Setting secondarySkillSetting;

        [SerializeField] Setting menuSetting;
        [SerializeField] Setting inventorySetting;

        // 注意: 名称需要和 KeySetting 一致, 以便反射读取.

        public CommandKey moveLeft { get; private set; }
        public CommandKey moveRight { get; private set; }
        public CommandKey jump { get; private set; }
        public CommandKey crouch { get; private set; }

        public CommandKey rush { get; private set; }
        public CommandKey attack { get; private set; }
        public CommandKey magicAttack { get; private set; }
        public CommandKey primiarySkill { get; private set; }
        public CommandKey secondarySkill { get; private set; }

        public CommandKey menu { get; private set; }
        public CommandKey inventory { get; private set; }

        readonly static List<CommandKey> bindedKeys = new List<CommandKey>();

        public KeyBinding()
        {
            inst = this;
        }

        public void Load()
        {
            foreach(var key in bindedKeys) CommandQueue.Unbind(key);
            bindedKeys.Clear();

            foreach(var cmdSetting in 
                this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => (x.GetValue(this) as Setting?) != null))
            {
                var setting = (Setting)cmdSetting.GetValue(this);
                bindedKeys.Add(setting.key);

                switch(setting.type)
                {
                case CommandType.Instant:
                    CommandQueue.BindInstant(setting.key, setting.priority);
                    break;

                case CommandType.Timeout:
                    CommandQueue.BindTimeout(setting.key, setting.timeout, setting.priority);
                    break;

                case CommandType.Interval:
                    CommandQueue.BindInterval(setting.key, setting.priority);
                    break;

                default:
                    throw new ArgumentException("Not a valid enum (CommandType)");
                }

                var name = cmdSetting.Name;
                var prpName = name.Substring(0, name.Length - "Setting".Length);
                var prp = this.GetType().GetProperty(prpName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                prp.SetValue(this, setting.key);
            }
        }
    }
}