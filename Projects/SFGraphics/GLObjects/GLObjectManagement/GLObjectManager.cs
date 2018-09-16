using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.GLObjectManagement
{
    /// <summary>
    /// <see cref="GLObject"/> and all subclasses use reference counting 
    /// to determine what OpenGL objects can be safely deleted.
    /// <para></para> <para></para>
    /// Call <see cref="DeleteUnusedGLObjects"/> with a valid OpenGL context current to delete objects with no references.
    /// <para></para><para></para>
    /// The reference count may not be decremented until after the context is destroyed.
    /// Call <see cref="GC.WaitForPendingFinalizers"/> prior to <see cref="DeleteUnusedGLObjects"/> if more 
    /// immediate cleanup is desired.
    /// </summary>
    public static class GLObjectManager
    {
        // Use internal methods/variables so people can't mess this up.
        internal static ConcurrentDictionary<Tuple<GLObject.GLObjectType, int>, int> referenceCountByGLObject = 
            new ConcurrentDictionary<Tuple<GLObject.GLObjectType, int>, int>();

        /// <summary>
        /// The appropriate GL.Delete() function is called for all <see cref="GLObject"/> instances
        /// with no references.
        /// </summary>
        public static void DeleteUnusedGLObjects()
        {
            DeleteUnusedObjects(referenceCountByGLObject);
        }

        private static void DeleteUnusedObjects(ConcurrentDictionary<Tuple<GLObject.GLObjectType, int>, int> referenceCountById)
        {
            HashSet<Tuple<GLObject.GLObjectType, int>> objectsNoReferences = ReferenceCounting.GetObjectsWithNoReferences(referenceCountById);

            // Remove and delete associated data for Ids with no more references.
            foreach (var glObject in objectsNoReferences)
            {
                int value;
                if (referenceCountById.TryRemove(glObject, out value))
                {
                    DeleteIdBasedOnType(glObject);
                }
            }
        }

        private static void DeleteIdBasedOnType(Tuple<GLObject.GLObjectType, int> glObject)
        {
            switch (glObject.Item1)
            {
                default:
                    throw new NotImplementedException($"Memory management not implemented for type {glObject.Item1}");
                case GLObject.GLObjectType.BufferObject:
                    GL.DeleteBuffer(glObject.Item2);
                    break;
                case GLObject.GLObjectType.ShaderProgram:
                    GL.DeleteProgram(glObject.Item2);
                    break;
                case GLObject.GLObjectType.VertexArrayObject:
                    GL.DeleteVertexArray(glObject.Item2);
                    break;
                case GLObject.GLObjectType.FramebufferObject:
                    GL.DeleteFramebuffer(glObject.Item2);
                    break;
                case GLObject.GLObjectType.Texture:
                    GL.DeleteTexture(glObject.Item2);
                    break;
                case GLObject.GLObjectType.RenderbufferObject:
                    GL.DeleteRenderbuffer(glObject.Item2);
                    break;
                case GLObject.GLObjectType.SamplerObject:
                    GL.DeleteSampler(glObject.Item2);
                    break;
            }
        }
    }
}
