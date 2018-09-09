using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.RenderState
{
    partial class RenderSettings
    {
        /// <summary>
        /// The alpha blending state set before drawing a <see cref="GenericMesh{T}"/>.
        /// </summary>
        public class AlphaBlendSettings
        {
            /// <summary>
            /// Enables or disables alpha blending.
            /// </summary>
            public bool enabled = true;

            /// <summary>
            /// The source color is multiplied by <see cref="sourceFactor"/>.
            /// </summary>
            public BlendingFactor sourceFactor = BlendingFactor.SrcAlpha;

            /// <summary>
            /// The destination color is multiplied by <see cref="destinationFactor"/>.
            /// </summary>
            public BlendingFactor destinationFactor = BlendingFactor.OneMinusSrcAlpha;

            /// <summary>
            /// The blending operation used for the RGB components.
            /// </summary>
            public BlendEquationMode blendingEquationRgb = BlendEquationMode.FuncAdd;

            /// <summary>
            /// The blending operation used for only the alpha component.
            /// </summary>
            public BlendEquationMode blendingEquationAlpha = BlendEquationMode.FuncAdd;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                var settings = obj as AlphaBlendSettings;
                return settings != null &&
                       enabled == settings.enabled &&
                       sourceFactor == settings.sourceFactor &&
                       destinationFactor == settings.destinationFactor &&
                       blendingEquationRgb == settings.blendingEquationRgb &&
                       blendingEquationAlpha == settings.blendingEquationAlpha;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                var hashCode = -1265808019;
                hashCode = hashCode * -1521134295 + enabled.GetHashCode();
                hashCode = hashCode * -1521134295 + sourceFactor.GetHashCode();
                hashCode = hashCode * -1521134295 + destinationFactor.GetHashCode();
                hashCode = hashCode * -1521134295 + blendingEquationRgb.GetHashCode();
                hashCode = hashCode * -1521134295 + blendingEquationAlpha.GetHashCode();
                return hashCode;
            }
        }
    }
}
