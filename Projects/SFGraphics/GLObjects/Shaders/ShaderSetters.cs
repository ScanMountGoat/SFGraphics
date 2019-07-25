using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.Utils;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using SFGraphics.GLObjects.Textures;

namespace SFGraphics.GLObjects.Shaders
{
    public sealed partial class Shader
    {
        /// <summary>
        /// Sets a float uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string name, float value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.Float))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.Float);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value);
        }

        /// <summary>
        /// Sets all values for a float[] uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string name, float[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.Float, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.Float, value.Length);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value.Length, value);
        }

        /// <summary>
        /// Sets an int uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string name, int value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.Int))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.Int);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value);
        }

        /// <summary>
        /// Sets all values for an int[] uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string name, int[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.Int, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.Int, value.Length);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value.Length, value);
        }

        /// <summary>
        /// Sets a uint uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string name, uint value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.UnsignedInt))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.UnsignedInt);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value);
        }

        /// <summary>
        /// Sets all values for a uint[] uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string name, uint[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.UnsignedInt, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.UnsignedInt, value.Length);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value.Length, value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to an int and sets an int uniform. 
        /// <c>true</c> = 1. <c>false</c> = 0. uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetBoolToInt(string name, bool value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.Int))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.Int);
                return;
            }

            GL.Uniform1(activeUniformByName[name].location, value ? 1 : 0);
        }

        /// <summary>
        /// Sets vec2 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector2(string name, Vector2 value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatVec2))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatVec2);
                return;
            }

            GL.Uniform2(activeUniformByName[name].location, value);
        }

        /// <summary>
        /// Sets all values for a vec2[] uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector2(string name, Vector2[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatVec2, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatVec2, value.Length);
                return;
            }

            for (int i = 0; i < value.Length; i++)
            {
                GL.Uniform2(GetUniformLocation(name) + i, value[i]); 
            }
        }

        /// <summary>
        /// Sets vec2 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="x">The value for name.x</param>
        /// <param name="y">The value for name.y</param>
        public void SetVector2(string name, float x, float y)
        {
            SetVector2(name, new Vector2(x, y));
        }

        /// <summary>
        /// Sets vec3 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector3(string name, Vector3 value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatVec3))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatVec3);
                return;
            }

            GL.Uniform3(activeUniformByName[name].location, value);
        }

        /// <summary>
        /// Sets all values for a vec3 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector3(string name, Vector3[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatVec3, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatVec3, value.Length);
                return;
            }

            for (int i = 0; i < value.Length; i++)
            {
                GL.Uniform3(GetUniformLocation(name) + i, value[i]);
            }
        }

        /// <summary>
        /// Sets vec3 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="x">The value for name.x</param>
        /// <param name="y">The value for name.y</param>
        /// <param name="z">The value for name.z</param>
        public void SetVector3(string name, float x, float y, float z)
        {
            SetVector3(name, new Vector3(x, y, z));
        }

        /// <summary>
        /// Sets vec4 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector4(string name, Vector4 value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatVec4))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatVec4);
                return;
            }

            GL.Uniform4(activeUniformByName[name].location, value);
        }

        /// <summary>
        /// Sets vec4 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="x">The value for name.x</param>
        /// <param name="y">The value for name.y</param>
        /// <param name="z">The value for name.z</param>
        /// <param name="w">The value for name.w</param>
        public void SetVector4(string name, float x, float y, float z, float w)
        {
            SetVector4(name, new Vector4(x, y, z, w));
        }

        /// <summary>
        /// Sets all values for a vec4 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector4(string name, Vector4[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatVec4, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatVec4, value.Length);
                return;
            }

            for (int i = 0; i < value.Length; i++)
            {
                GL.Uniform4(GetUniformLocation(name) + i, value[i]);
            }
        }

        /// <summary>
        /// Sets a mat4 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string name, ref Matrix4 value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatMat4))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatMat4);
                return;
            }

            GL.UniformMatrix4(activeUniformByName[name].location, false, ref value);
        }

        /// <summary>
        /// Sets a mat4 uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string name, Matrix4 value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatMat4))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatMat4);
                return;
            }

            GL.UniformMatrix4(activeUniformByName[name].location, false, ref value);
        }

        /// <summary>
        /// Sets all values for a mat4[] uniform variable. Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string name, Matrix4[] value)
        {
            if (!errorLog.IsValidUniform(activeUniformByName, name, ActiveUniformType.FloatMat4, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(name, value, ActiveUniformType.FloatMat4);
                return;
            }

            for (int i = 0; i < value.Length; i++)
            {
                GL.UniformMatrix4(GetUniformLocation(name) + i, false, ref value[i]);
            }
        }

        /// <summary>
        /// <paramref name="texture"/> is bound to <paramref name="textureUnit"/> before 
        /// setting the uniform. 
        /// Invalid values are logged and not set.
        /// </summary>
        /// <param name="name">The uniform variable name</param>
        /// <param name="texture">The integer ID generated by GL.GenTexture()</param>
        /// <param name="textureUnit">The texture unit to which <paramref name="texture"/> will be bound</param>
        public void SetTexture(string name, Texture texture, int textureUnit)
        {
            if (CheckIfTextureSetIsValid(name, texture, textureUnit))
                BindTextureSetUniform(name, texture, textureUnit);
        }

        private void BindTextureSetUniform(string name, Texture texture, int textureUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + textureUnit);
            texture.Bind();
            GL.Uniform1(activeUniformByName[name].location, textureUnit);
        }

        private bool CheckIfTextureSetIsValid(string name, Texture texture, int textureUnit)
        {
            ActiveUniformType uniformType = ShaderTypeConversions.GetUniformType(texture.TextureTarget);
            bool validUniform = errorLog.IsValidUniform(activeUniformByName, name, uniformType);
            if (!validUniform)
                LogInvalidUniformSetRaiseEvent(name, texture, uniformType);

            bool validSamplerType = errorLog.IsValidSamplerType(textureUnit, uniformType);
            if (!validSamplerType)
            {
                var textureSetArgs = new TextureSetEventArgs()
                {
                    Name = name,
                    Type = uniformType,
                    TextureUnit = textureUnit,
                    Value = texture
                };

                OnTextureUnitTypeMismatch?.Invoke(this, textureSetArgs);
            }

            bool validSet = validUniform && validSamplerType;
            return validSet;
        }

        private void LogInvalidUniformSetRaiseEvent(string name, object value, ActiveUniformType type, int length = 1)
        {
            // TODO: This does multiple things and isn't very clear.
            var uniformSetArgs = new UniformSetEventArgs()
            {
                Name = name,
                Type = type,
                Size = length,
                Value = value
            };
            errorLog.LogInvalidUniformSet(uniformSetArgs);
            OnInvalidUniformSet?.Invoke(this, uniformSetArgs);
        }
    }
}
