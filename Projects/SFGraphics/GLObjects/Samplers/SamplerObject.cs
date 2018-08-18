using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Samplers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SamplerObject : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.SamplerObject; } }

        /// <summary>
        /// 
        /// </summary>
        public SamplerObject() : base(GL.GenSampler())
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureUnit"></param>
        public void Bind(int textureUnit)
        {
            GL.BindSampler(textureUnit, Id);
        }
    }
}
