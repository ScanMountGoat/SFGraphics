using System.Collections.Generic;
using OpenTK;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Materials
{
    /// <summary>
    /// Used to store uniform data for <see cref="GenericMesh{T}"/>. 
    /// Duplicate uniform names are not allowed, regardless of type differences.
    /// <para></para><para></para>
    /// Custom material data should be converted to a <see cref="GenericMaterial"/> to update shader uniforms.
    /// Avoid updating shader uniforms directly.
    /// </summary>
    public sealed class GenericMaterial
    {
        // Scalar uniforms
        private Dictionary<string, float> floatUniformsByName = new Dictionary<string, float>();
        private Dictionary<string, int> intUniformsByName = new Dictionary<string, int>();

        // Vector uniforms
        private Dictionary<string, Vector2> vec2UniformsByName = new Dictionary<string, Vector2>();
        private Dictionary<string, Vector3> vec3UniformsByName = new Dictionary<string, Vector3>();
        private Dictionary<string, Vector4> vec4UniformsByName = new Dictionary<string, Vector4>();

        // Matrix uniforms.
        private Dictionary<string, Matrix4> mat4UniformsByName = new Dictionary<string, Matrix4>();

        // Texture uniforms
        // TODO: name, texture, texture unit

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <param name="value"></param>
        public void AddFloat(string uniformName, float value)
        {
            floatUniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <param name="value"></param>
        public void AddInt(string uniformName, int value)
        {
            intUniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <param name="value"></param>
        public void AddVector2(string uniformName, Vector2 value)
        {
            vec2UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <param name="value"></param>
        public void AddVector3(string uniformName, Vector3 value)
        {
            vec3UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <param name="value"></param>
        public void AddVector4(string uniformName, Vector4 value)
        {
            vec4UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <param name="value"></param>
        public void AddMatrix4(string uniformName, Matrix4 value)
        {
            mat4UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Sets uniform values for all the added uniform values.
        /// </summary>
        /// <param name="shader">The shader whose uniforms will be set</param>
        public void SetShaderUniforms(Shader shader)
        {
            foreach (var uniform in intUniformsByName)
            {
                shader.SetInt(uniform.Key, uniform.Value);
            }

            foreach (var uniform in floatUniformsByName)
            {
                shader.SetFloat(uniform.Key, uniform.Value);
            }

            foreach (var uniform in vec2UniformsByName)
            {
                shader.SetVector2(uniform.Key, uniform.Value);
            }

            foreach (var uniform in vec3UniformsByName)
            {
                shader.SetVector3(uniform.Key, uniform.Value);
            }

            foreach (var uniform in vec4UniformsByName)
            {
                shader.SetVector4(uniform.Key, uniform.Value);
            }

            foreach (var uniform in mat4UniformsByName)
            {
                Matrix4 value = uniform.Value;
                shader.SetMatrix4x4(uniform.Key, ref value);
            }
        }
    }
}
