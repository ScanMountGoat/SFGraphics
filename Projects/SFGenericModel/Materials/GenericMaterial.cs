using System.Collections.Generic;
using OpenTK;
using SFGraphics.GLObjects.Samplers;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;

namespace SFGenericModel.Materials
{
    /// <summary>
    /// Stores and sets shader uniform values, including textures.
    /// For best performance for non texture uniforms, use <see cref="UniformBlock"/>.
    /// </summary>
    public sealed class GenericMaterial
    {
        /// <summary>
        /// Each texture uniform will be assigned to a unique index, starting with this value.
        /// Defaults to <c>0</c>.
        /// </summary>
        public int InitialTextureUnit { get; }

        // TODO: Why are these not dictionaries?
        private readonly List<string> floatUniformNames = new List<string>();
        private readonly List<float> floatValues = new List<float>();

        private readonly List<string> intUniformNames = new List<string>();
        private readonly List<int> intValues = new List<int>();

        private readonly List<string> vec2UniformNames = new List<string>();
        private readonly List<Vector2> vec2Values = new List<Vector2>();

        private readonly List<string> vec3UniformNames = new List<string>();
        private readonly List<Vector3> vec3Values = new List<Vector3>();

        private readonly List<string> vec4UniformNames = new List<string>();
        private readonly List<Vector4> vec4Values = new List<Vector4>();

        private readonly List<string> mat4UniformNames = new List<string>();
        private readonly List<Matrix4> mat4Values = new List<Matrix4>();

        private readonly List<string> textureUniformNames = new List<string>();
        private readonly List<Texture> textureValues = new List<Texture>();
        private readonly List<SamplerObject> samplerValues = new List<SamplerObject>();

        /// <summary>
        /// Creates an empty generic material.
        /// </summary>
        public GenericMaterial()
        {
            InitialTextureUnit = 0;
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
            floatUniformNames.Add(uniformName);
            floatValues.Add(value);
        }

        /// <summary>
        /// Adds an int uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddInt(string uniformName, int value)
        {
            intUniformNames.Add(uniformName);
            intValues.Add(value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to an int. <c>1</c> is true. <c>0</c> is <c>false</c>.
        /// Adds the resulting int uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddBoolToInt(string uniformName, bool value)
        {
            intUniformNames.Add(uniformName);
            intValues.Add(value ? 1 : 0);
        }

        /// <summary>
        /// Adds a vec2 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddVector2(string uniformName, Vector2 value)
        {
            vec2UniformNames.Add(uniformName);
            vec2Values.Add(value);
        }

        /// <summary>
        /// Adds a vec3 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddVector3(string uniformName, Vector3 value)
        {
            vec3UniformNames.Add(uniformName);
            vec3Values.Add(value);
        }

        /// <summary>
        /// Adds a vec4 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddVector4(string uniformName, Vector4 value)
        {
            vec4UniformNames.Add(uniformName);
            vec4Values.Add(value);
        }

        /// <summary>
        /// Adds a mat4 uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddMatrix4(string uniformName, Matrix4 value)
        {
            mat4UniformNames.Add(uniformName);
            mat4Values.Add(value);
        }

        /// <summary>
        /// Adds a sampler uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        public void AddTexture(string uniformName, Texture value)
        {
            textureUniformNames.Add(uniformName);
            textureValues.Add(value);
            samplerValues.Add(null);
        }

        /// <summary>
        /// Adds a sampler uniform to the material.
        /// </summary>
        /// <param name="uniformName">The name of the uniform variable</param>
        /// <param name="value">The value to set for the uniform</param>
        /// <param name="sampler">A sampler for the texture <paramref name="value"/></param>
        public void AddTexture(string uniformName, Texture value, SamplerObject sampler)
        {
            textureUniformNames.Add(uniformName);
            textureValues.Add(value);
            samplerValues.Add(sampler);
        }

        /// <summary>
        /// Sets uniform values for all the added uniform values.
        /// Redundant setting of texture uniform is skipped to improve performance.
        /// </summary>
        /// <param name="shader">The shader whose uniforms will be set</param>
        /// <param name="previousMaterial">The previous material used to set this shader's uniforms</param>
        public void SetShaderUniforms(Shader shader, GenericMaterial previousMaterial = null)
        {
            SetIntUniforms(shader);
            SetFloatUniforms(shader);
            SetVec2Uniforms(shader);
            SetVec3Uniforms(shader);
            SetVec4Uniforms(shader);
            SetMat4Uniforms(shader);
            SetTextureUniforms(shader, previousMaterial);
        }

        private void SetTextureUniforms(Shader shader, GenericMaterial previousMaterial)
        {
            int textureIndex = InitialTextureUnit;
            for (int i = 0; i < textureUniformNames.Count; i++)
            {
                var name = textureUniformNames[i];
                var value = textureValues[i];

                // If the same texture is bound to this texture unit, we don't need to set it again.
                bool shouldSkipTextureSet = previousMaterial != null 
                    && i < previousMaterial.textureUniformNames.Count 
                    && previousMaterial.textureUniformNames[i] == name 
                    && previousMaterial.textureValues[i] == value;

                if (!shouldSkipTextureSet)
                {
                    shader.SetTexture(name, value, textureIndex);
                }

                // Even if the textures are the same, the bound sampler may be different.
                if (samplerValues[i] != null)
                    samplerValues[i].Bind(textureIndex);

                textureIndex++;
            }
        }

        private void SetIntUniforms(Shader shader)
        {
            for (int i = 0; i < intUniformNames.Count; i++)
            {
                shader.SetInt(intUniformNames[i], intValues[i]);
            }
        }

        private void SetFloatUniforms(Shader shader)
        {
            for (int i = 0; i < floatUniformNames.Count; i++)
            {
                shader.SetFloat(floatUniformNames[i], floatValues[i]);
            }
        }

        private void SetVec2Uniforms(Shader shader)
        {
            for (int i = 0; i < vec2UniformNames.Count; i++)
            {
                shader.SetVector2(vec2UniformNames[i], vec2Values[i]);
            }
        }

        private void SetVec3Uniforms(Shader shader)
        {
            for (int i = 0; i < vec3UniformNames.Count; i++)
            {
                shader.SetVector3(vec3UniformNames[i], vec3Values[i]);
            }
        }

        private void SetVec4Uniforms(Shader shader)
        {
            for (int i = 0; i < vec4UniformNames.Count; i++)
            {
                shader.SetVector4(vec4UniformNames[i], vec4Values[i]);
            }
        }

        private void SetMat4Uniforms(Shader shader)
        {
            for (int i = 0; i < mat4UniformNames.Count; i++)
            {
                shader.SetMatrix4x4(mat4UniformNames[i], mat4Values[i]);
            }
        }
    }
}
