using System;
using System.Windows;
using SharpGL;
using SharpGlUtil;

namespace VertexBufferSimple
{
    public partial class MainWindow : Window
    {
        private uint _bufferId;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SimpleOpenGlControl_OnInitScene(object sender, SimpleOpenGlEventArgs args)
        {
            var gl = args.OpenGl;

            var vertexData = new[]
            {
                -1.0f, -1.0f, 0.0f,
                -1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, -1.0f, 0.0f
            };

            var bufs = new uint[1];
            gl.GenBuffers(1, bufs);

            _bufferId = bufs[0];
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _bufferId);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertexData, OpenGL.GL_STATIC_DRAW);
            gl.VertexAttribPointer(0, 0, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
        }

        private void SimpleOpenGlControl_OnRenderScene(object sender, SimpleOpenGlEventArgs args)
        {
            var gl = args.OpenGl;

            gl.LookAt(0, 0, -5, 0, 0, 0, 0, 1, 0);

            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _bufferId);
            gl.VertexPointer(3, OpenGL.GL_FLOAT, 0, IntPtr.Zero);
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.DrawArrays(OpenGL.GL_QUADS, 0, 4);
            gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
        }
    }
}
