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
        /// </summary>
        /// <typeparam name="T">The struct used to store a single vertex</typeparam>
        /// <param name="containers">The unoptimized vertex containers</param>
        /// <returns></returns>
        public static List<VertexContainer<T>> GroupContainersByPrimitiveType<T>(List<VertexContainer<T>> containers)
            where T : struct
        {
            Dictionary<PrimitiveType, VertexContainer<T>> vertDataByType = new Dictionary<PrimitiveType, VertexContainer<T>>();
            
            foreach (var container in containers)
            {
                if (vertDataByType.ContainsKey(container.primitiveType))
                {
                    // TODO: Create a new container from both containers.
                }
                else
                {
                    vertDataByType.Add(container.primitiveType, container);
                }
            }

            List<VertexContainer<T>> optimizedContainers = vertDataByType.Values.ToList();
            return optimizedContainers;
        }
    }
}
