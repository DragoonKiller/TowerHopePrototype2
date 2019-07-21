using System;
using System.Collections.Generic;
using UnityEngine;

using Utils;
using static Utils.Algorthms;

namespace Tower.Systems
{

    /// <summary>
    /// 使用 Polygon Collider 手动调整的 mesh.
    /// </summary>
    public class PolyMesh : MonoBehaviour
    {






        /// <summary>
        /// 从自定义 polygon collider 生成一个 mesh.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="meshFilter"></param>
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
            mesh.name = "Generated tarrain mesh";
            return mesh;
        }
    }
}