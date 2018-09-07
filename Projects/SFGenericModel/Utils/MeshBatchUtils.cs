﻿using System;
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
                PrimitiveType primitiveType = container.primitiveType;

                if (vertDataByType.ContainsKey(primitiveType))
                {
                    // TODO: Create a new container from both containers.
                    List<T> newVertices = new List<T>();
                    newVertices.AddRange(vertDataByType[primitiveType].vertices);
                    newVertices.AddRange(container.vertices);

                    List<int> newIndices = new List<int>();

                    vertDataByType[primitiveType] = new VertexContainer<T>(newVertices, newIndices, primitiveType);
                }
                else
                {
                    vertDataByType.Add(primitiveType, container);
                }
            }

            List<VertexContainer<T>> optimizedContainers = vertDataByType.Values.ToList();
            return optimizedContainers;
        }
    }
}
