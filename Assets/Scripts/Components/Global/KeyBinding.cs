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
    [Serializable]
    public class KeyBinding : MonoBehaviour
    {
        [Serializable]
        public class Setting : IEquatable<Setting>
        {
            public KeyCode key;
            public CommandType type;

            [Tooltip("事件优先级. 同时存在的事件将会按照优先级排序.")]
            public int priority;

            [Tooltip("事件的触发时间. 只对 CommandType.Timeout 有效.")]
            public float timeout;

            public bool Equals(Setting other)
            {
                return key == other.key
                    && type == other.type
                    && priority == other.priority
                    && timeout == other.timeout;
            }
        }

        public static KeyBinding inst;

        // 注意: 命名后缀 Setting 会被反射读取.

        [SerializeField] Setting advancedSkillSetting;

        [SerializeField] Setting moveLeftSetting;
        [SerializeField] Setting moveRightSetting;
        [SerializeField] Setting jumpSetting;
        [SerializeField] Setting crouchSetting;

        [SerializeField] Setting rushSetting;
        [SerializeField] Setting attackSetting;
        [SerializeField] Setting magicAttackSetting;
        [SerializeField] Setting primarySkillSetting;
        [SerializeField] Setting secondarySkillSetting;

        [SerializeField] Setting menuSetting;
        [SerializeField] Setting inventorySetting;

        // 注意: 名称需要和 KeySetting 一致, 以便反射读取.

        public KeyCode advancedSkill { get; private set; }

        public KeyCode moveLeft { get; private set; }
        public KeyCode moveRight { get; private set; }
        public KeyCode jump { get; private set; }
        public KeyCode crouch { get; private set; }

        public KeyCode rush { get; private set; }
        public KeyCode attack { get; private set; }
        public KeyCode magicAttack { get; private set; }
        public KeyCode primarySkill { get; private set; }
        public KeyCode secondarySkill { get; private set; }

        public KeyCode menu { get; private set; }
        public KeyCode inventory { get; private set; }

        readonly static List<KeyCode> bindedKeys = new List<KeyCode>();

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
                .Where(x => x.GetValue(this) as Setting != null))
            {
                var setting = cmdSetting.GetValue(this) as Setting;
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