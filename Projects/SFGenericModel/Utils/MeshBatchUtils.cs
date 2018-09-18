using OpenTK.Graphics.OpenGL;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SFGenericModel.Utils
{
    /// <summary>
    /// Contains methods for grouping vertex data to reduce draw calls and improve performance.
    /// </summary>
    public static class MeshBatchUtils
    {
        /// <summary>
        /// Creates a single <see cref="VertexContainer{T}"/> for each PrimitiveType to 
        /// reduce draw calls and improve performance. 
        /// <para></para><para></para>
        /// Unsupported primite types will remain ungrouped.
        /// </summary>
        /// <typeparam name="T">The struct used to store a single vertex</typeparam>
        /// <param name="containers">The unoptimized vertex containers</param>
        /// <returns></returns>
        public static List<VertexContainer<T>> GroupContainersByPrimitiveType<T>(List<VertexContainer<T>> containers)
            where T : struct
        {
            // Use a single container for each primitive.
            var vertDataByType = new ConcurrentDictionary<PrimitiveType, List<VertexContainer<T>>>();

            foreach (var container in containers)
            {
                PrimitiveType primitiveType = container.primitiveType;
                MergeCurrentContainer(vertDataByType, container, primitiveType);
            }

            List<VertexContainer<T>> optimizedContainers = new List<VertexContainer<T>>();
            foreach (var container in vertDataByType.Values)
                optimizedContainers.AddRange(container);
            return optimizedContainers;
        }

        private static void MergeCurrentContainer<T>(ConcurrentDictionary<PrimitiveType, List<VertexContainer<T>>> vertDataByType, 
            VertexContainer<T> containerToMerge, PrimitiveType type) where T : struct
        {
            // Combining indices isn't supported for all types currently.
            if (IsSupportedPrimitiveType(type))
            {
                if (vertDataByType.ContainsKey(type))
                    vertDataByType[type][0] = CreateCombinedContainer(vertDataByType[type][0], containerToMerge, type);
                else
                    vertDataByType.TryAdd(type, new List<VertexContainer<T>>() { containerToMerge });
            }
            else
            {
                if (vertDataByType.ContainsKey(type))
                    vertDataByType[type].Add(containerToMerge);
                else
                    vertDataByType.TryAdd(type, new List<VertexContainer<T>>() { containerToMerge });
            }
        }

        private static VertexContainer<T> CreateCombinedContainer<T>(VertexContainer<T> containerA, VertexContainer<T> containerB, 
            PrimitiveType type) where T : struct
        {
            List<T> newVertices = CombineVertices(containerA, containerB, type);
            List<int> newIndices = CombineIndices(containerA, containerB, type);

            return new VertexContainer<T>(newVertices, newIndices, type);
        }

        private static List<int> CombineIndices<T>(VertexContainer<T> containerA, VertexContainer<T> containerB, PrimitiveType primitiveType) 
            where T : struct
        {
            List<int> newIndices = new List<int>();
            newIndices.AddRange(containerA.vertexIndices);

            int firstContainerOffset = containerA.vertices.Count;

            if (primitiveType == PrimitiveType.TriangleStrip)
            {
                // Create a degenerate triangle to combine the two lists.
                newIndices.Add(containerA.vertexIndices.Last());
                newIndices.Add(containerB.vertexIndices.First() + firstContainerOffset);
            }

            // HACK: Assume no shared vertices between containers.
            foreach (int index in containerB.vertexIndices)
            {
                newIndices.Add(index + firstContainerOffset);
            }

            return newIndices;
        }

        private static List<T> CombineVertices<T>(VertexContainer<T> containerA, VertexContainer<T> containerB, PrimitiveType type)
            where T : struct
        {
            List<T> newVertices = new List<T>();
            newVertices.AddRange(containerA.vertices);
            newVertices.AddRange(containerB.vertices);
            return newVertices;
        }

        private static bool IsSupportedPrimitiveType(PrimitiveType type)
        {
            return type == PrimitiveType.Lines
                || type == PrimitiveType.Points
                || type == PrimitiveType.Triangles
                || type == PrimitiveType.TriangleStrip
                || type == PrimitiveType.Quads;
        }
    }
}
