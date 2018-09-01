using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Shaders
{
    public sealed partial class Shader
    {
        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string uniformName, float value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.Float))
                return;

            GL.Uniform1(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloats(string uniformName, float[] value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.Float))
                return;

            GL.Uniform1(activeUniformByName[uniformName].location, value.Length, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string uniformName, int value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.Int))
                return;

            GL.Uniform1(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string uniformName, uint value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.UnsignedInt))
                return;

            GL.Uniform1(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform. True = 1. False = 0.</param>
        public void SetBoolToInt(string uniformName, bool value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.Int))
                return;

            // if/else is faster than the ternary operator. 
            if (value)
                GL.Uniform1(activeUniformByName[uniformName].location, 1);
            else
                GL.Uniform1(activeUniformByName[uniformName].location, 0);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector2(string uniformName, Vector2 value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.FloatVec2))
                return;

            GL.Uniform2(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x"></param>        
        /// <param name="y"></param>
        public void SetVector2(string uniformName, float x, float y)
        {
            SetVector2(uniformName, new Vector2(x, y));
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector3(string uniformName, Vector3 value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.FloatVec3))
                return;

            GL.Uniform3(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x"></param>        
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetVector3(string uniformName, float x, float y, float z)
        {
            SetVector3(uniformName, new Vector3(x, y, z));
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector4(string uniformName, Vector4 value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.FloatVec4))
                return;

            GL.Uniform4(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public void SetVector4(string uniformName, float x, float y, float z, float w)
        {
            SetVector4(uniformName, new Vector4(x, y, z, w));
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string uniformName, ref Matrix4 value)
        {
            if (!ValidUniform(uniformName, ActiveUniformType.FloatMat4))
                return;

            GL.UniformMatrix4(activeUniformByName[uniformName].location, false, ref value);
        }

        /// <summary>
        /// <paramref name="textureId"/> is bound to <paramref name="textureUnit"/> before 
        /// setting the uniform. Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="textureId">The integer ID generated by GL.GenTexture()</param>
        /// <param name="textureTarget">The target to which <paramref name="textureId"/> is bound</param>
        /// <param name="textureUnit">The texture unit to which <paramref name="textureId"/> is bound</param>
        public void SetTexture(string uniformName, int textureId, TextureTarget textureTarget, int textureUnit)
        {
            if (!activeUniformByName.ContainsKey(uniformName))
                return;

            GL.ActiveTexture(TextureUnit.Texture0 + textureUnit);
            GL.BindTexture(textureTarget, textureId);
            GL.Uniform1(activeUniformByName[uniformName].location, textureUnit);
        }

        private bool ValidUniform(string uniformName, ActiveUniformType inputType)
        {
            if (!activeUniformByName.ContainsKey(uniformName))
            {
                if (!invalidUniformByName.ContainsKey(uniformName))
                    invalidUniformByName.Add(uniformName, new ActiveUniformInfo(0, inputType));
                return false;
            }
            else if (activeUniformByName[uniformName].type != inputType)
            {
                if (!invalidUniformByName.ContainsKey(uniformName))
                    invalidUniformByName.Add(uniformName, new ActiveUniformInfo(0, inputType));
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
