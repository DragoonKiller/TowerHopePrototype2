using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Utils;
using static Utils.Algorthms;

namespace Tower.Components
{
    using Systems;

    /// <summary>
    /// 实时读取 Polygon Collider 数据并生成 mesh.
    /// </summary>
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteAlways]
    public class PolyMesh : MonoBehaviour
    {
        //=====================================================================
        // 关联组件
        //=====================================================================

        PolygonCollider2D pc => GetComponent<PolygonCollider2D>();
        MeshFilter ms => GetComponent<MeshFilter>();

        //=====================================================================
        // Unity API函数
        //=====================================================================

        /// <summary>
        /// 这个函数要在编辑器中运行.
        /// </summary>
        void Update()
        {
            UpdateMesh();
        }
        
        void UpdateMesh()
        {
            if(Application.isPlaying) return;
            if(!Application.isEditor) return;
            ms.mesh = ToMesh(pc);
        }
        
        void Start()
        {
            ms.mesh = ToMesh(pc);
        }

        //=====================================================================
        // 内部函数
        //=====================================================================


        /// <summary>
        /// 从自定义 polygon collider 生成一个 mesh.
        /// </summary>
        static Mesh ToMesh(PolygonCollider2D collider)
        {
            var trs = new List<Vector2>(collider.GetPath(0)).Triangulation();
            var vts = new List<Vector3>(trs.Count * 3);
            var ids = new List<int>(trs.Count * 3);
            for(int i = 0; i < trs.Count; i++)
            {
                vts.Add(trs[i].a, trs[i].b, trs[i].c);
                ids.Add(i * 3, i * 3 + 1, i * 3 + 2);
            }
            var mesh = new Mesh();
            mesh.SetVertices(vts);
            mesh.SetIndices(ids.ToArray(), MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();
            mesh.name = "Generated Mesh (PolyMesh)";
            return mesh;
        }
    }
}
