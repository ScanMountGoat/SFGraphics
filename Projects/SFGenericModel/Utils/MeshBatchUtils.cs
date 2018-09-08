using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

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

                if (vertDataByType.ContainsKey(primitiveType))
                    CreateCombinedContainer(vertDataByType, container, primitiveType);
                else
                    vertDataByType.Add(primitiveType, container);
            }

            optimizedContainers.AddRange(vertDataByType.Values);
            return optimizedContainers;
        }

        private static bool IsSupportedPrimitiveType(PrimitiveType type)
        {
            return type == PrimitiveType.Lines || type == PrimitiveType.Triangles || type == PrimitiveType.Quads;
        }

        private static void CreateCombinedContainer<T>(Dictionary<PrimitiveType, VertexContainer<T>> vertDataByType, VertexContainer<T> container, PrimitiveType primitiveType) where T : struct
        {
            List<T> newVertices = CombineVertices(vertDataByType, container, primitiveType);

            List<int> newIndices = CombineIndices(vertDataByType, container, primitiveType);

            vertDataByType[primitiveType] = new VertexContainer<T>(newVertices, newIndices, primitiveType);
        }

        private static List<int> CombineIndices<T>(Dictionary<PrimitiveType, VertexContainer<T>> vertDataByType, VertexContainer<T> container, PrimitiveType primitiveType) where T : struct
        {
            List<int> newIndices = new List<int>();
            newIndices.AddRange(vertDataByType[primitiveType].vertexIndices);

            // HACK: Assume no shared vertices between containers.
            int offset = vertDataByType[primitiveType].vertices.Count;
            foreach (int index in container.vertexIndices)
            {
                newIndices.Add(index + offset);
            }

            return newIndices;
        }

        private static List<T> CombineVertices<T>(Dictionary<PrimitiveType, VertexContainer<T>> vertDataByType, VertexContainer<T> container, PrimitiveType primitiveType) where T : struct
        {
            List<T> newVertices = new List<T>();
            newVertices.AddRange(vertDataByType[primitiveType].vertices);
            newVertices.AddRange(container.vertices);
            return newVertices;
        }
    }
}
