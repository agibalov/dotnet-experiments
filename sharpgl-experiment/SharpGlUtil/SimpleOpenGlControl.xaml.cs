using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpGL;
using SharpGL.RenderContextProviders;
using SharpGL.Version;
using SharpGL.WPF;

namespace SharpGlUtil
{
    public partial class SimpleOpenGlControl : UserControl
    {
        private readonly OpenGL _gl = new OpenGL();

        public SimpleOpenGlControl()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gl.Create(OpenGLVersion.OpenGL2_1, RenderContextType.FBO, 1, 1, 32, null);

            _gl.ShadeModel(OpenGL.GL_SMOOTH);
            _gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            _gl.ClearDepth(1.0f);
            _gl.Enable(OpenGL.GL_DEPTH_TEST);
            _gl.DepthFunc(OpenGL.GL_LEQUAL);
            _gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

            if (InitScene != null)
            {
                InitScene(this, new SimpleOpenGlEventArgs
                {
                    OpenGl = _gl
                });
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            var width = (int)sizeInfo.NewSize.Width;
            var height = (int)sizeInfo.NewSize.Height;
            
            _gl.SetDimensions(width, height);
            _gl.Viewport(0, 0, width, height);

            _gl.MatrixMode(OpenGL.GL_PROJECTION);
            _gl.LoadIdentity();
            _gl.Perspective(45.0f, width / (float)height, 0.1f, 100.0f);
            _gl.MatrixMode(OpenGL.GL_MODELVIEW);
            _gl.LoadIdentity();
            
            Render();
        }

        public void Render()
        {
            _gl.MakeCurrent();
            _gl.ClearColor(0.2f, 0.2f, 0.2f, 0);
            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            _gl.LoadIdentity();

            if (RenderScene != null)
            {
                RenderScene(this, new SimpleOpenGlEventArgs
                {
                    OpenGl = _gl
                });
            }

            _gl.Blit(IntPtr.Zero);

            var fboRenderContextProvider = (FBORenderContextProvider) _gl.RenderContextProvider;
            
            var formatConvertedBitmap = new FormatConvertedBitmap();
            formatConvertedBitmap.BeginInit();
            formatConvertedBitmap.Source = BitmapConversion.HBitmapToBitmapSource(fboRenderContextProvider.InternalDIBSection.HBitmap);
            formatConvertedBitmap.DestinationFormat = PixelFormats.Rgb24;
            formatConvertedBitmap.EndInit();

            OpenGlImage.Source = formatConvertedBitmap;
        }

        public event InitSceneCallback InitScene;
        public event RenderSceneCallback RenderScene;

        public delegate void RenderSceneCallback(object sender, SimpleOpenGlEventArgs args);
        public delegate void InitSceneCallback(object sender, SimpleOpenGlEventArgs args);
    }
}
