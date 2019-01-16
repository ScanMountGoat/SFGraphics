using OpenTK.Graphics.OpenGL;
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
        /// Creates a new collection of <see cref="IndexedVertexData{T}"/> objects with 
        /// only a single container for types that support merging.
        /// </summary>
        /// <typeparam name="T">The vertex data struct</typeparam>
        /// <param name="containers">The unoptimized vertex containers</param>
        /// <returns></returns>
        public static List<IndexedVertexData<T>> GroupContainersByPrimitiveType<T>(IList<IndexedVertexData<T>> containers)
            where T : struct
        {
            var vertexContainersByType = OrganizeContainersByType(containers);

            // Merge each container list.
            var result = new List<IndexedVertexData<T>>();
            foreach (var unmergedcontainers in vertexContainersByType.Values)
                result.AddRange(GetMergedContainers(unmergedcontainers));

            return result;
        }

        private static Dictionary<PrimitiveType, List<IndexedVertexData<T>>> OrganizeContainersByType<T>(IList<IndexedVertexData<T>> containers) where T : struct
        {
            var vertexContainersByType = new Dictionary<PrimitiveType, List<IndexedVertexData<T>>>();

            // Get all the containers for each type into a single list.
            foreach (var container in containers)
            {
                if (!vertexContainersByType.ContainsKey(container.PrimitiveType))
                    vertexContainersByType[container.PrimitiveType] = new List<IndexedVertexData<T>>();

                vertexContainersByType[container.PrimitiveType].Add(container);
            }

            return vertexContainersByType;
        }

        private static IList<IndexedVertexData<T>> GetMergedContainers<T>(IList<IndexedVertexData<T>> containersToMerge) where T : struct
        {
            // Combining indices isn't supported for all types currently.
            PrimitiveType type = containersToMerge.First().PrimitiveType;
            if (!supportedTypes.Contains(type))
                return containersToMerge;

            // Estimate the correct size to avoid costly array resize/copy operations.
            int indexCount = containersToMerge.First().Indices.Count * containersToMerge.Count;
            int vertexCount = containersToMerge.First().Vertices.Count * containersToMerge.Count;

            List<int> indices = new List<int>(indexCount);
            List<T> vertices = new List<T>(vertexCount);
            foreach (var container in containersToMerge)
            {
                AddIndices(vertices.Count, container.Indices, type, indices);
                vertices.AddRange(container.Vertices);
            }

            var mergedContainer = new IndexedVertexData<T>(vertices, indices, type);
            return new List<IndexedVertexData<T>>() { mergedContainer };
        }

        private static void AddIndices(int offset, IEnumerable<int> indicesToAdd, 
            PrimitiveType primitiveType, List<int> target) 
        {
            if (target.Count == 0)
            {
                target.AddRange(indicesToAdd);
                return;
            }

            if (primitiveType == PrimitiveType.TriangleStrip)
            {
                // Create a degenerate triangle to combine the two lists.
                target.Add(target.Last());
                target.Add(indicesToAdd.First() + offset);
            }

            // Assume no shared vertices between containers.
            foreach (int index in indicesToAdd)
            {
                target.Add(index + offset);
            }
        }
    }
}
