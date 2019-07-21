using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Utils
{
    public class UnitTestWindow : EditorWindow
    {
        class InfoState
        {
            public MethodInfo method;
            public bool shoudBeTested;
        }

        [MenuItem("UnitTest/UnitTest")]
        public static void ShowWindow() => GetWindow(typeof(UnitTestWindow));

        public const string systemsAssemblyName = "Assembly-CSharp";
        
        /// <summary>
        /// 当前过滤器.
        /// </summary>
        public string filter;

        /// <summary>
        /// 列表的视图状态.
        /// </summary>
        public Vector2 scrollState;

        /// <summary>
        /// 最大显示的方法数量.
        /// </summary>
        public const int maxCount = 200;

        public static Assembly systemAssembly;

        /// <summary>
        /// 静态存储所有测试方法.
        /// Clear 会保留可用内存,
        /// 可以避免每次申请一个容器带来的内存开销.
        /// </summary>
        static readonly List<InfoState> methods = new List<InfoState>();

        void OnEnable()
        {
            titleContent = new GUIContent("Unit Test");
        }

        void OnGUI()
        {
            if(systemAssembly == null)
            {
                systemAssembly = FindAssembly();
                SetupCache();

                filter = "";
                scrollState = Vector2.zero;
            }
            UIProcess();
        }

        /// <summary>
        /// 找到包含了自定义脚本的程序集.
        /// </summary>
        /// <returns></returns>
        Assembly FindAssembly()
        {
            var methods = new List<MethodInfo>();
            var systemAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name == systemsAssemblyName).GetEnumerator();
            systemAssemblies.MoveNext();
            return systemAssembly = systemAssemblies.Current;
        }

        /// <summary>
        /// 找到所有标记了 Tower.Systems.UnitTest 的方法.
        /// </summary>
        /// <param name="a"></param>
        void SetupCache()
        {
            methods.Clear();
            foreach(var type in systemAssembly.GetTypes())
            {
                foreach(var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                {
                    var x = method.GetCustomAttribute<UnitTest>();
                    if(x == null) continue;

                    // 默认全部勾选.
                    methods.Add(new InfoState() { method = method, shoudBeTested = true });
                }
            }
        }

        /// <summary>
        /// 绘制 UI 并执行响应操作.
        /// </summary>
        void UIProcess()
        {
            GUILayout.Label("Unit Test");

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("run all", GUILayout.MinWidth(40))) RunAll();

            GUILayout.Space(20);
            if(GUILayout.Button("toggle all")) ActionToggleAll();
            if(GUILayout.Button("un-toggle all")) ActionUntoggleAll();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter", GUILayout.MaxWidth(60));
            filter = GUILayout.TextField(filter);
            GUILayout.EndHorizontal();

            scrollState = GUILayout.BeginScrollView(scrollState);

            int curCount = 0;
            foreach(var method in methods)
            {
                if(curCount == maxCount)
                {
                    var boldStyle = new GUIStyle();
                    boldStyle.fontStyle = FontStyle.Bold;

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Label("... too many items!", boldStyle);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Label("(use filter to access other test items)");
                    GUILayout.EndHorizontal();

                    break;
                }
                var name = FullName(method);
                if(!name.Contains(filter)) continue;
                method.shoudBeTested = GUILayout.Toggle(method.shoudBeTested, name);
                curCount += 1;
            }

            GUILayout.EndScrollView();
        }

        
        string FullName(MethodInfo method)
            => method.DeclaringType.FullName + "." + method.Name;

        string FullName(InfoState method)
            => FullName(method.method);

        void ActionToggleAll()
        {
            foreach(var method in methods)
                if(FullName(method).Contains(filter))
                    method.shoudBeTested = true;
        }

        void ActionUntoggleAll()
        {
            foreach(var method in methods)
                if(FullName(method).Contains(filter))
                method.shoudBeTested = false;
        }

        void RunAll()
        {
            Debug.Log("Test begin");

            var nothingParams = new object[0];
            int succ = 0;
            int fail = 0;
            foreach(var method in methods)
            {
                if(!method.shoudBeTested) continue;
                try
                {
                    method.method.Invoke(null, nothingParams);
                    succ += 1;
                }
                catch(Exception e)
                {
                    Debug.LogWarning("Test fail : " + e.Message);
                    fail += 1;
                }
            }

            Debug.Log($"Test end [success: { succ }, fail: { fail }]");

            GC.Collect(1);
        }
    }
}