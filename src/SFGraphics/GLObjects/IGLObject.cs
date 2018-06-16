﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IGLObject
    {
        /// <summary>
        /// The value generated by GL.Gen() for a texture, buffer, etc. Do not attempt to bind this when the object has gone out of scope.
        /// </summary>
        int Id { get; }
    }
}
