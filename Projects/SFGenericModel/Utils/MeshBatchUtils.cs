using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Get all the containers for each type into a collection.
            var vertexContainersByType = OrganizeContainersByType(containers);

            // Merge each container list.
            var result = new List<VertexContainer<T>>();
            foreach (var unmergedcontainers in vertexContainersByType.Values)
                result.AddRange(GetMergedContainers(unmergedcontainers));

            return result;
        }

        private static Dictionary<PrimitiveType, List<VertexContainer<T>>> OrganizeContainersByType<T>(List<VertexContainer<T>> containers) where T : struct
        {
            var vertexContainersByType = new Dictionary<PrimitiveType, List<VertexContainer<T>>>();

            foreach (var container in containers)
            {
                if (!vertexContainersByType.ContainsKey(container.primitiveType))
                    vertexContainersByType[container.primitiveType] = new List<VertexContainer<T>>();

                vertexContainersByType[container.primitiveType].Add(container);
            }

            return vertexContainersByType;
        }

        private static List<VertexContainer<T>> GetMergedContainers<T>(List<VertexContainer<T>> containersToMerge) where T : struct
        {
            // Combining indices isn't supported for all types currently.
            PrimitiveType type = containersToMerge.First().primitiveType;
            if (!supportedTypes.Contains(type))
                return containersToMerge;

            // Estimate the correct size to avoid costly array resize/copy operations.
            int indexCount = containersToMerge.First().vertexIndices.Count * containersToMerge.Count;
            int vertexCount = containersToMerge.First().vertices.Count * containersToMerge.Count;

            List<int> indices = new List<int>(indexCount);
            List<T> vertices = new List<T>(vertexCount);
            foreach (var container in containersToMerge)
            {
                AddIndices(vertices.Count, container.vertexIndices, type, indices);
                vertices.AddRange(container.vertices);
            }

            VertexContainer<T> mergedContainer = new VertexContainer<T>(vertices, indices, type);
            return new List<VertexContainer<T>>() { mergedContainer };
        }

        private static void AddIndices(int offset, List<int> indicesToAdd, 
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

            // HACK: Assume no shared vertices between containers.
            foreach (int index in indicesToAdd)
            {
                target.Add(index + offset);
            }
        }

        private static List<T> AddVertices<T>(VertexContainer<T> containerA, VertexContainer<T> containerB)
            where T : struct
        {
            return containerA.vertices.Concat(containerB.vertices).ToList();
        }
    }
}
