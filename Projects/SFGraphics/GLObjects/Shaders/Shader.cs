using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using SFGraphics.GLObjects.Shaders.Utils;
using System;
using System.Collections.Generic;

namespace SFGraphics.GLObjects.Shaders
{
    /// <summary>
    /// Encapsulates a shader program and attached shaders. 
    /// Errors are stored to an internal log, which can be exported with <see cref="GetErrorLog"/>.
    /// <para></para> <para></para>
    /// Ensure that <see cref="LinkStatusIsOk"/> returns <c>true</c> before rendering to avoid crashes.
    /// </summary>
    public sealed partial class Shader : GLObject
    {
        internal override GLObjectType ObjectType => GLObjectType.ShaderProgram;

        private Dictionary<string, ActiveUniformInfo> activeUniformByName = new Dictionary<string, ActiveUniformInfo>();
        private Dictionary<string, ActiveAttribInfo> activeAttribByName = new Dictionary<string, ActiveAttribInfo>();
        private Dictionary<string, int> activeUniformBlockIndexByName = new Dictionary<string, int>();

        // Keeping another reference will make shader objects eligible for cleanup later, 
        // but it reduces the number of GL calls required for some methods.
        private readonly List<ShaderObject> attachedShaders = new List<ShaderObject>();

        /// <summary>
        /// The number of uniforms used by the shader. 
        /// Uniforms optimized out by the compiler are considered unused.
        /// </summary>
        public int ActiveUniformCount { get; private set; }

        /// <summary>
        /// The number of uniform blocks used by the shader.
        /// Uniforms optimized out by the compiler are considered unused.
        /// </summary>
        public int ActiveUniformBlockCount { get; private set; }

        /// <summary>
        /// The number of vertex attributes used by the shader. 
        /// Attributes optimized out by the compiler are considered unused.
        /// </summary>
        public int ActiveAttributeCount { get; private set; }

        private readonly ShaderLog errorLog = new ShaderLog();

        /// <summary>
        /// If <c>false</c>, rendering with this shader will most likely throw an <see cref="AccessViolationException"/>.
        /// <para></para><para></para>
        /// Does not reflect changes made using <see cref="GLObject.Id"/> directly.
        /// </summary>
        public bool LinkStatusIsOk
        {
            get => linkStatusIsOk; 

            private set
            {
                if (linkStatusIsOk != value)
                {
                    var linkStatusArgs = new LinkStatusEventArgs() { LinkStatus = value };
                    LinkStatusChanged?.Invoke(this, linkStatusArgs);
                }
                linkStatusIsOk = value;
            }
        }
        private bool linkStatusIsOk;

        /// <summary>
        /// <c>true</c> when only one sampler type is used for each texture unit
        /// and the number of active samplers does not exceed the allowed maximum.
        /// <para></para><para></para>
        /// This should be checked at runtime and only for debugging purposes.
        /// </summary>
        public bool ValidateStatusIsOk => ShaderValidation.GetProgramValidateStatus(Id);

        /// <summary>
        /// Occurs when arguments for setting a uniform don't match the shader.
        /// </summary>
        public event EventHandler<UniformSetEventArgs> InvalidUniformSet;

        /// <summary>
        /// Occurs when a call to <see cref="SetTexture(string, Textures.Texture, int)"/>
        /// is made to a previously used texture unit but with a different sampler type.
        /// </summary>
        public event EventHandler<TextureSetEventArgs> TextureUnitTypeMismatched;

        /// <summary>
        /// Occurs when the value of <see cref="LinkStatusIsOk"/> changes.
        /// </summary>
        public event EventHandler<LinkStatusEventArgs> LinkStatusChanged;

        // This isn't in OpenTK's enums for some reason.
        // https://www.khronos.org/registry/OpenGL/api/GL/glcorearb.h
        private static readonly int GL_PROGRAM_BINARY_MAX_LENGTH = 0x8741;

        /// <summary>
        /// Creates an uninitialized shader program. Load shaders before using the shader program.
        /// </summary>
        public Shader() : base(GL.CreateProgram())
        {

        }

        /// <summary>
        /// Use <see cref="GLObject.Id"/> as the current program for drawing.
        /// </summary>
        public void UseProgram()
        {
            if (linkStatusIsOk)
                GL.UseProgram(Id);
        }

        /// <summary>
        /// Enables the vertex attribute arrays for all active attributes.
        /// Use a <see cref="VertexArrays.VertexArrayObject"/> for modern OpenGL.
        /// </summary>
        public void EnableVertexAttributes()
        {
            // Only enable the necessary vertex attributes.
            foreach (var attribute in activeAttribByName)
            {
                GL.EnableVertexAttribArray(attribute.Value.location);
            }
        }

        /// <summary>
        /// Disables the vertex attribute arrays for all active attributes.
        /// Use a <see cref="VertexArrays.VertexArrayObject"/> for modern OpenGL.
        /// </summary>
        public void DisableVertexAttributes()
        {
            // Enabling extra vertex attributes can cause crashes on some drivers.
            foreach (var attribute in activeAttribByName)
            {
                GL.DisableVertexAttribArray(attribute.Value.location);
            }
        }

        /// <summary>
        /// Gets the uniform's location or the location of the first element for arrays.
        /// </summary>
        /// <param name="name">The name of the uniform variable</param>
        /// <returns>The location of <paramref name="name"/></returns>
        public int GetUniformLocation(string name)
        {
            if (string.IsNullOrEmpty(name))
                return -1;

            string nameNoBrackets = GetNameNoArrayBrackets(name);
            if (!activeUniformByName.ContainsKey(nameNoBrackets))
                return -1;
            else
                return activeUniformByName[nameNoBrackets].location;
        }

        /// <summary>
        /// Gets the attribute's location from locations stored after linking.
        /// </summary>
        /// <param name="name">The name of the vertex attribute variable</param>
        /// <returns>The location of <paramref name="name"/></returns>
        public int GetAttribLocation(string name)
        {
            if (string.IsNullOrEmpty(name) || !activeAttribByName.ContainsKey(name))
                return -1;
            else
                return activeAttribByName[name].location;
        }

        /// <summary>
        /// Gets the block index the uniform block <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the uniform block in the shader</param>
        /// <returns>The index of <paramref name="name"/> or <c>-1</c> if not found</returns>
        public int GetUniformBlockIndex(string name)
        {
            if (string.IsNullOrEmpty(name) || !activeUniformBlockIndexByName.ContainsKey(name))
                return -1;
            else
                return activeUniformBlockIndexByName[name];
        }

        /// <summary>
        /// Sets the binding point for a uniform block.
        /// Invalid names are ignored. 
        /// </summary>
        /// <param name="name">The name of the uniform block</param>
        /// <param name="bindingPoint">The binding point for the uniform block</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bindingPoint"/> is negative</exception>
        public void UniformBlockBinding(string name, int bindingPoint)
        {
            if (bindingPoint < 0)
                throw new ArgumentOutOfRangeException(nameof(bindingPoint), "Binding points must be non negative.");

            // Don't use invalid indices to prevent errors. 
            int index = GetUniformBlockIndex(name);
            if (index != -1)
                GL.UniformBlockBinding(Id, index, bindingPoint);
        }

        /// <summary>
        /// Gets the error log containing hardware info, version number, compilation/linker errors, 
        /// and attempts to initialize invalid uniform or vertex attribute names.
        /// </summary>
        /// <returns>A String of all detected errors</returns>
        public string GetErrorLog()
        {
            // Don't append program errors until all the shaders are attached and compiled.
            errorLog.AppendProgramInfoLog(Id);

            return errorLog.ToString();
        }
    }
}

