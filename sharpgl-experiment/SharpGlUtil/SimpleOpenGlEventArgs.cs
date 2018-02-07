using System;
using SharpGL;

namespace SharpGlUtil
{
    public class SimpleOpenGlEventArgs : EventArgs
    {
        public OpenGL OpenGl { get; set; }
    }
}