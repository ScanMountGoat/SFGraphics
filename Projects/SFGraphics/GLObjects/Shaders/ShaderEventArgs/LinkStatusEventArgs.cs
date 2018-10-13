using System;

namespace SFGraphics.GLObjects.Shaders.ShaderEventArgs
{
    /// <summary>
    /// Contains information about a shader program linking.
    /// </summary>
    public class LinkStatusEventArgs : EventArgs
    {
        /// <summary>
        /// <c>true</c> if the shader program linked successfully.
        /// </summary>
        public bool LinkStatus { get; set; }
    }
}
