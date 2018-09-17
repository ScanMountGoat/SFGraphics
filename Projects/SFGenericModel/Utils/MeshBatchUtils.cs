using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SFGenericModel.Utils
{
    /// <summary>
    /// 
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
            Dictionary<PrimitiveType, VertexContainer<T>> vertDataByType = new Dictionary<PrimitiveType, VertexContainer<T>>();

            List<VertexContainer<T>> optimizedContainers = new List<VertexContainer<T>>();

            foreach (var container in containers)
            {
                PrimitiveType primitiveType = container.primitiveType;

                // Combining indices isn't supported for all types currently.
                if (!IsSupportedPrimitiveType(primitiveType))
                {
                    optimizedContainers.Add(container);
                    continue;
                }

                MergeCurrentContainer(vertDataByType, container, primitiveType);
            }

            optimizedContainers.AddRange(vertDataByType.Values);
            return optimizedContainers;
        }

        private static void MergeCurrentContainer<T>(Dictionary<PrimitiveType, VertexContainer<T>> vertDataByType, 
            VertexContainer<T> containerToMerge, PrimitiveType type) where T : struct
        {
            if (vertDataByType.ContainsKey(type))
                vertDataByType[type] = CreateCombinedContainer(vertDataByType[type], containerToMerge, type);
            else
                vertDataByType[type] = containerToMerge;
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
