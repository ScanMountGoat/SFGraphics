﻿using System;
using SFGraphics.GLObjects.GLObjectManagement;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Provides a simpler and less error prone way 
    /// to deal with the state and data associated with OpenGL objects. 
    /// </summary>
    public abstract class GLObject
    {
        /// <summary>
        /// The available types of OpenGL objects.
        /// Enables <see cref="GLObjectManager"/> to call the appropriate delete function.
        /// </summary>
        public enum GLObjectType
        {
            /// <summary>
            /// <see cref="GLObjects.Framebuffer"/>
            /// </summary>
            FramebufferObject,

            /// <summary>
            /// <see cref="GLObjects.Renderbuffer"/>
            /// </summary>
            RenderbufferObject,

            /// <summary>
            /// <see cref="Textures.Texture"/>
            /// </summary>
            Texture,

            /// <summary>
            /// <see cref="Shaders.Shader"/>
            /// </summary>
            ShaderProgram,

            /// <summary>
            /// <see cref="GLObjects.BufferObject"/>
            /// </summary>
            BufferObject,

            /// <summary>
            /// <see cref="GLObjects.VertexArrayObject"/>
            /// </summary>
            VertexArrayObject
        }

        /// <summary>
        /// The value generated by GL.GenTexture(), GL.GenProgram(), etc. 
        /// Do not use <see cref="Id"/> after the container object is unreachable.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// 
        /// </summary>
        public abstract GLObjectType ObjectType { get; }

        /// <summary>
        /// Increments the reference count and initializes <see cref="Id"/>.
        /// </summary>
        public GLObject(int id)
        {
            // We need a constructor to set the readonly field and preserve polymorphic behavior.
            Id = id;
            ReferenceCounting.IncrementReference(GLObjectManager.referenceCountByGLObject, new Tuple<GLObjectType, int>(ObjectType, Id));
        }

        /// <summary>
        /// Decrements the reference count for <see cref="Id"/>.
        /// The object can't be deleted here because finalizers are called from a separate thread.
        /// </summary>
        ~GLObject()
        {
            ReferenceCounting.DecrementReference(GLObjectManager.referenceCountByGLObject, new Tuple<GLObjectType, int>(ObjectType, Id));        }

        /// <summary>
        /// Returns the type of OpenGL object and the integer ID generated by OpenGL.
        /// Example: "ShaderProgram ID: 1"
        /// </summary>
        /// <returns>The object's type and ID</returns>
        public override string ToString()
        {
            return $"{ ObjectType } ID: { Id }";
        }
    }
}
