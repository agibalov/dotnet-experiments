using System.Windows;
using SharpGL;
using SharpGlUtil;

namespace OldSchoolQuad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SimpleOpenGlControl_OnRenderScene(object sender, SimpleOpenGlEventArgs args)
        {
            var gl = args.OpenGl;

            gl.LookAt(0, 0, -5, 0, 0, 0, 0, 1, 0);

            gl.Begin(OpenGL.GL_QUADS);

            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(-1.0f, -1.0f, 0.0f);
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.Vertex(-1.0f, 1.0f, 0.0f);
            gl.Color(1.0f, 0.0f, 1.0f);
            gl.Vertex(1.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 1.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 0.0f);

            gl.End();
        }
    }
}
