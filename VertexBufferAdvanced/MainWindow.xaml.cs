using System;
using System.Runtime.InteropServices;
using System.Windows;
using SharpGL;
using SharpGlUtil;

namespace VertexBufferAdvanced
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

            var vertices = new[]
            {
                new Vertex { X = -1, Y = -1, Z = 0 },
                new Vertex { X = -1, Y = 1, Z = 0 },
                new Vertex { X = 1, Y = 1, Z = 0 },
                new Vertex { X = 1, Y = -1, Z = 0 }
            };

            var buf = MakeRaw(vertices);

            var bufs = new uint[1];
            gl.GenBuffers(1, bufs);

            _bufferId = bufs[0];
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _bufferId);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices.Length * Marshal.SizeOf(typeof(Vertex)), buf, OpenGL.GL_STATIC_DRAW);
            gl.VertexAttribPointer(0, 0, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);

            Marshal.FreeHGlobal(buf);
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

        private static IntPtr MakeRaw(Vertex[] vertices)
        {
            var buf = Marshal.AllocHGlobal(vertices.Length * Marshal.SizeOf<Vertex>());
            var ptr = buf;
            foreach (var vertex in vertices)
            {
                Marshal.StructureToPtr(vertex, ptr, false);
                ptr += Marshal.SizeOf<Vertex>();
            }

            return buf;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public float X;
            public float Y;
            public float Z;
        }
    }
}
