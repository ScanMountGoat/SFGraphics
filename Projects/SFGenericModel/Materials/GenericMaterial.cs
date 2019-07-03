using System.Collections.Generic;
using OpenTK;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;

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
        /// <summary>
        /// Each texture uniform will be assigned to a unique index, starting with this value.
        /// Defaults to <c>0</c>.
        /// </summary>
        public int InitialTextureUnit { get; }

        private Dictionary<string, float> floatUniformsByName = new Dictionary<string, float>();
        private Dictionary<string, int> intUniformsByName = new Dictionary<string, int>();
        private Dictionary<string, Vector2> vec2UniformsByName = new Dictionary<string, Vector2>();
        private Dictionary<string, Vector3> vec3UniformsByName = new Dictionary<string, Vector3>();
        private Dictionary<string, Vector4> vec4UniformsByName = new Dictionary<string, Vector4>();
        private Dictionary<string, Matrix4> mat4UniformsByName = new Dictionary<string, Matrix4>();
        private Dictionary<string, Texture> textureUniformsByName = new Dictionary<string, Texture>();

        /// <summary>
        /// Creates an empty generic material.
        /// </summary>
        public GenericMaterial()
        {

        }

        /// <summary>
        /// Creates an empty generic material.
        /// </summary>
        /// <param name="initialTextureUnit">The starting texture unit for texture uniforms</param>
        public GenericMaterial(int initialTextureUnit)
        {
            InitialTextureUnit = initialTextureUnit;
        }

        /// <summary>
        /// Adds a float uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddFloat(string uniformName, float value)
        {
            floatUniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Adds an int uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddInt(string uniformName, int value)
        {
            intUniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to an int. <c>1</c> is true. <c>0</c> is <c>false</c>.
        /// Adds the resulting int uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddBoolToInt(string uniformName, bool value)
        {
            if (value)
                intUniformsByName.Add(uniformName, 1);
            else
                intUniformsByName.Add(uniformName, 0);
        }

        /// <summary>
        /// Adds a vec2 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddVector2(string uniformName, Vector2 value)
        {
            vec2UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Adds a vec3 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddVector3(string uniformName, Vector3 value)
        {
            vec3UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Adds a vec4 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddVector4(string uniformName, Vector4 value)
        {
            vec4UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Adds a mat4 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddMatrix4(string uniformName, Matrix4 value)
        {
            mat4UniformsByName.Add(uniformName, value);
        }

        /// <summary>
        /// Adds a sampler uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddTexture(string uniformName, Texture value)
        {
            textureUniformsByName.Add(uniformName, value);
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
                shader.SetMatrix4x4(uniform.Key, uniform.Value);
            }

            int textureIndex = InitialTextureUnit;
            foreach (var uniform in textureUniformsByName)
            {
                shader.SetTexture(uniform.Key, uniform.Value, textureIndex);
                textureIndex++;
            }
        }
    }
}
