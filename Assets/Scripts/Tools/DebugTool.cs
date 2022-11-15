
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class GizmosTool
    {
        public static void OnDrawGridCenter(float3 center, float3 size, float3 spacing)
        {

        }
        public static void OnDrawGrid(float3 min, float3 max, float3 spacing)
        {
            var size = (max - min) / spacing;
            foreach (var x in Enumerable.Range(0, Mathf.CeilToInt(size.x)))
            {
                foreach (var y in Enumerable.Range(0, Mathf.CeilToInt(size.y)))
                {
                    var s = new float3(spacing.x, spacing.y, 0);
                    var center = min + s * 0.5f + s * new float3(x, y, 0);
                    Gizmos.DrawWireCube(center, s);
                }
            }
            foreach (var y in Enumerable.Range(0, Mathf.CeilToInt(size.y)))
            {
                foreach (var z in Enumerable.Range(0, Mathf.CeilToInt(size.z)))
                {
                    var s = new float3(0, spacing.y, spacing.z);
                    var center = min + s * 0.5f + s * new float3(0, y, z);
                    Gizmos.DrawWireCube(center, s);
                }
            }
            foreach (var x in Enumerable.Range(0, Mathf.CeilToInt(size.x)))
            {
                foreach (var z in Enumerable.Range(0, Mathf.CeilToInt(size.z)))
                {
                    var s = new float3(spacing.x, 0, spacing.z);
                    var center = min + s * 0.5f + s * new float3(x, 0, z);
                    Gizmos.DrawWireCube(center, s);
                }
            }
        }
    }
}