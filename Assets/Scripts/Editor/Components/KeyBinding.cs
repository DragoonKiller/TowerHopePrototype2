using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;
using Systems;

namespace Tower.Global
{
    [CustomEditor(typeof(KeyBinding))]
    public class KeyBindingWindow : Editor
    {
        bool useEnum;

        public override void OnInspectorGUI()
        {
            var x = serializedObject.targetObject as KeyBinding;
            if(x == null) return;

            var headerStyle = new GUIStyle();
            headerStyle.fontSize = 12;
            headerStyle.alignment = TextAnchor.MiddleRight;
            headerStyle.fontStyle = FontStyle.Bold;

            var additionalTagStyle = new GUIStyle();
            additionalTagStyle.fontSize = 11;
            additionalTagStyle.alignment = TextAnchor.MiddleRight;

            GUILayout.Space(8);
            useEnum = GUILayout.Toggle(useEnum, "use enum for keys", GUILayout.Width(200));
            GUILayout.Space(8);

            // 反射取出 KeyBinding.Setting 变量.
            foreach(var kb in typeof(KeyBinding).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if(kb.FieldType != typeof(KeyBinding.Setting)) continue;
                var val = (KeyBinding.Setting)kb.GetValue(x);

                var name = kb.Name;
                if(name.EndsWith("Setting")) name = name.Remove(name.Length - "Setting".Length);
                else if(name.EndsWith("Settings")) name = name.Remove(name.Length - "Settings".Length);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(name, headerStyle, GUILayout.Width(90));
                GUILayout.Space(10);
                val.type = (CommandType)EditorGUILayout.EnumPopup(val.type, GUILayout.MaxWidth(100));
                GUILayout.Space(10);
                GUILayout.Label("priority", GUILayout.Width(50));
                val.priority = EditorGUILayout.IntField(val.priority, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();

                if(val.type == CommandType.Timeout)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("duration", additionalTagStyle, GUILayout.Width(260));
                    GUILayout.Space(8);
                    val.timeout = EditorGUILayout.FloatField(val.timeout, GUILayout.Width(80));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(104);
                if(useEnum)
                {
                    val.key = (KeyCode)EditorGUILayout.EnumPopup(val.key, GUILayout.MaxWidth(100));
                    GUILayout.Space(10);
                }
                else
                {
                    var newVal = EditorGUILayout.TextField(val.key.ToString(), GUILayout.Width(100));
                    if(Enum.TryParse<KeyCode>(newVal, true, out var parsedRes)) val.key = parsedRes;
                    else val.key = KeyCode.None;
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);
                kb.SetValue(x, val);

            }
        }

    }
}