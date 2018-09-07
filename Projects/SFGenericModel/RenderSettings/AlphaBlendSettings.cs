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
            /// 
            /// </summary>
            public bool enableAlphaBlending = true;

            /// <summary>
            /// 
            /// </summary>
            public BlendingFactor sourceFactor = BlendingFactor.SrcAlpha;

            /// <summary>
            /// 
            /// </summary>
            public BlendingFactor destinationFactor = BlendingFactor.OneMinusSrcAlpha;

            /// <summary>
            /// 
            /// </summary>
            public BlendEquationMode blendingEquationRgb = BlendEquationMode.FuncAdd;

            /// <summary>
            /// 
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
                       enableAlphaBlending == settings.enableAlphaBlending &&
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
                hashCode = hashCode * -1521134295 + enableAlphaBlending.GetHashCode();
                hashCode = hashCode * -1521134295 + sourceFactor.GetHashCode();
                hashCode = hashCode * -1521134295 + destinationFactor.GetHashCode();
                hashCode = hashCode * -1521134295 + blendingEquationRgb.GetHashCode();
                hashCode = hashCode * -1521134295 + blendingEquationAlpha.GetHashCode();
                return hashCode;
            }
        }
    }
}
