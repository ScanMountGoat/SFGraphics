﻿using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.Utils;
using System;
using System.Collections.Generic;

namespace SFGenericModel
{
    /// <summary>
    /// A class for drawing indexed vertex data with vertex attributes stored in separate buffers.
    /// </summary>
    public abstract class GenericMeshNonInterleaved : GenericMeshBase
    {
        private static readonly string invalidAccessMessage = "One or more attribute data accesses will not be within the specified buffer's data storage";

        private class VertexAttributeExtended
        {
            public VertexAttribute VertexAttribute { get; }
            public int OffsetInBytes { get; }
            public int StrideInBytes { get; }

            public VertexAttributeExtended(VertexAttribute vertexAttribute, int offsetInBytes, int strideInBytes)
            {
                VertexAttribute = vertexAttribute;
                OffsetInBytes = offsetInBytes;
                StrideInBytes = strideInBytes;
            }
        }

        private readonly BufferObject vertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);

        private readonly Dictionary<string, BufferObject> bufferByName = new Dictionary<string, BufferObject>();
        private readonly Dictionary<BufferObject, List<VertexAttributeExtended>> attributesByBuffer = new Dictionary<BufferObject, List<VertexAttributeExtended>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexIndices">The indices used for drawing</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        /// <param name="vertexCount">The number of vertices for the vertex data buffers. This should be the same for fall buffers.</param>
        public GenericMeshNonInterleaved(uint[] vertexIndices, PrimitiveType primitiveType, int vertexCount) : base(primitiveType, DrawElementsType.UnsignedInt, vertexCount)
        {
            vertexIndexBuffer.SetData(vertexIndices, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Creates and stores a new <see cref="BufferObject"/> called <paramref name="bufferName"/> with data <paramref name="bufferData"/>.
        /// Attributes can be associated with this buffer using <see cref="ConfigureAttribute(VertexAttribute, string, int, int)"/>.
        /// </summary>
        /// <typeparam name="T">The data type of the buffer elements</typeparam>
        /// <param name="bufferName">The name to associate with this buffer</param>
        /// <param name="bufferData">The data used to initialize the buffer</param>
        /// <exception cref="ArgumentException"><paramref name="bufferName"/> has already been added</exception>
        public void AddBuffer<T>(string bufferName, T[] bufferData) where T : struct
        {
            if (bufferByName.ContainsKey(bufferName))
                throw new ArgumentException("A buffer with the given name already exists.", nameof(bufferName));

            var buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(bufferData, BufferUsageHint.StaticDraw);
            bufferByName.Add(bufferName, buffer);
            attributesByBuffer.Add(buffer, new List<VertexAttributeExtended>());
        }

        // TODO:
        // Support a variable number of attributes.
        // Each attribute has it's own buffer using generics (each buffer can be specified in a different type. OpenGL won't care).
        // Validate buffer sizes based on vertex shader attribute information.

        // TODO: 
        // Support the case where buffer can be shared among multiple attributes.
        // This has little advantage for rendering, but it can simplify parsing code.

        // Possible implementation.

        /// <summary>
        /// Configures the data source for a floating point vertex attribute.
        /// An exception is thrown if the specified parameters would result in an invalid buffer access.
        /// </summary>
        /// <param name="vertexAttribute">The vertex attribute information></param>
        /// <param name="bufferName">The name of the buffer used for <see cref="AddBuffer{T}(string, T[])"/></param>
        /// <param name="offsetInBytes">The offset in bytes for the start of the vertex data in the buffer</param>
        /// <param name="strideInBytes">The stride in bytes for the buffer data</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified parameters would result in an invalid buffer access.</exception>
        public void ConfigureAttribute(VertexAttribute vertexAttribute, string bufferName, int offsetInBytes, int strideInBytes)
        {
            if (strideInBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(strideInBytes), invalidAccessMessage);

            if (AttribPointerUtils.GetSizeInBytes(vertexAttribute.Type) > strideInBytes)
                throw new ArgumentOutOfRangeException(nameof(strideInBytes), "The size of the attribute's type must not exceed the stride.");

            if (!bufferByName.ContainsKey(bufferName))
                throw new ArgumentException($"The buffer {bufferName} has not been added.", nameof(bufferName));
            var buffer = bufferByName[bufferName];

            if (!BufferValidation.IsValidAccess(offsetInBytes, strideInBytes, VertexCount, buffer.SizeInBytes))
                throw new ArgumentOutOfRangeException("", invalidAccessMessage);

            // Associate attributes with the appropriate buffer, so the buffer can be bound later.
            attributesByBuffer[buffer].Add(new VertexAttributeExtended(vertexAttribute, offsetInBytes, strideInBytes));
        }

        /// <summary>
        /// Configures the vertex attributes for each buffer.
        /// </summary>
        /// <param name="shader">The shader to query for attribute information</param>
        protected override void ConfigureVertexAttributes(Shader shader)
        {
            // TODO: Check or any active attributes that have no data assigned and throw exception.
            // This will likely cause a crash when callin GL.DrawElements.

            vertexIndexBuffer.Bind();

            shader.EnableVertexAttributes();
            SetVertexAttributes(shader);
        }

        private void SetVertexAttributes(Shader shader)
        {
            foreach (var pair in attributesByBuffer)
            {
                pair.Key.Bind();
                foreach (var attribute in pair.Value)
                {
                    // Invalid names are silently ignored.
                    VertexAttributeUtils.SetVertexAttribute(shader, attribute.VertexAttribute, attribute.OffsetInBytes, attribute.StrideInBytes);
                }
            }
        }
    }
}