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
        private static readonly HashSet<PrimitiveType> supportedTypes = new HashSet<PrimitiveType>()
        {
            PrimitiveType.Lines,
            PrimitiveType.Points,
            PrimitiveType.Triangles,
            PrimitiveType.TriangleStrip,
            PrimitiveType.Quads
        };

        /// <summary>
        /// Creates a new collection of <see cref="VertexContainer{T}"/> objects with 
        /// only a single container for types that support merging.
        /// </summary>
        /// <typeparam name="T">The vertex data struct</typeparam>
        /// <param name="containers">The unoptimized vertex containers</param>
        /// <returns></returns>
        public static List<VertexContainer<T>> GroupContainersByPrimitiveType<T>(List<VertexContainer<T>> containers)
            where T : struct
        {
            // Use a single container for each primitive.
            var vertexContainersByType = new ConcurrentDictionary<PrimitiveType, List<VertexContainer<T>>>();

            foreach (var container in containers)
            {
                MergeCurrentContainer(vertexContainersByType, container);
            }

            return GetResultingContainers(vertexContainersByType);
        }

        private static List<VertexContainer<T>> GetResultingContainers<T>(ConcurrentDictionary<PrimitiveType, List<VertexContainer<T>>> vertDataByType) where T : struct
        {
            List<VertexContainer<T>> optimizedContainers = new List<VertexContainer<T>>();
            foreach (var container in vertDataByType.Values)
                optimizedContainers.AddRange(container);
            return optimizedContainers;
        }

        private static void MergeCurrentContainer<T>(ConcurrentDictionary<PrimitiveType, List<VertexContainer<T>>> vertDataByType,
            VertexContainer<T> containerToMerge) where T : struct
        {
            PrimitiveType type = containerToMerge.primitiveType;

            if (vertDataByType.ContainsKey(type))
            {
                // Combining indices isn't supported for all types currently.
                if (supportedTypes.Contains(type))
                    vertDataByType[type][0] = GetCombinedContainer(vertDataByType[type][0], containerToMerge, type);
                else
                    vertDataByType[type].Add(containerToMerge);
            }
            else
            {
                vertDataByType.TryAdd(type, new List<VertexContainer<T>>() { containerToMerge });
            }
        }

        private static VertexContainer<T> GetCombinedContainer<T>(VertexContainer<T> containerA, VertexContainer<T> containerB, 
            PrimitiveType type) where T : struct
        {
            List<T> newVertices = GetCombinedVertices(containerA, containerB);
            List<int> newIndices = GetCombinedIndices(containerA, containerB, type);

            return new VertexContainer<T>(newVertices, newIndices, type);
        }

        private static List<int> GetCombinedIndices<T>(VertexContainer<T> containerA, VertexContainer<T> containerB, PrimitiveType primitiveType) 
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

        private static List<T> GetCombinedVertices<T>(VertexContainer<T> containerA, VertexContainer<T> containerB)
            where T : struct
        {
            return containerA.vertices.Concat(containerB.vertices).ToList();
        }
    }
}
