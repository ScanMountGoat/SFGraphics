﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel;
using SFGenericModel.VertexAttributes;
using System.Collections.Generic;
using SFGenericModel.ShaderGenerators;
using System;

namespace SFShapes
{
    /// <summary>
    /// Draws simple geometry given a collection of vertex positions.
    /// </summary>
    public class Mesh3D : GenericMesh<Vector3>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="primitiveType"></param>
        public Mesh3D(List<Vector3> vertices, PrimitiveType primitiveType) : base(vertices, primitiveType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
        public Mesh3D(Tuple<List<Vector3>, PrimitiveType> vertices) : base(vertices.Item1, vertices.Item2)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>()
            {
                new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<VertexAttributeRenderInfo> GetRenderAttributes()
        {
            return new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(GetVertexAttributes()[0], true, true)
            };
        }
    }
}
