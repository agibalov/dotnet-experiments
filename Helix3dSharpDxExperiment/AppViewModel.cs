using System.Collections.Generic;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX.Core;
using SharpDX;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;

namespace Helix3dSharpDxExperiment
{
    public class AppViewModel
    {
        public Camera Camera { get; set; }
        public MeshGeometry3D Model { get; private set; }
        public Color4 WhiteLight { get; set; }

        public AppViewModel()
        {
            Camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, -5),
                LookDirection = new Vector3D(0, 0, 1),
                UpDirection = new Vector3D(0, 1, 0)
            };

            var positions = new List<Vector3>();
            var indices = new List<int>();
            var colors = new List<Color4>();

            const int n = 1000;

            var index = 0;
            for (var i = -n / 2; i < n / 2; ++i)
            {
                for (var j = -n / 2; j < n / 2; ++j)
                {
                    positions.Add(new Vector3(0.5f + i, 0.5f + j, 0f));
                    positions.Add(new Vector3(0.5f + i, -0.5f + j, 0f));
                    positions.Add(new Vector3(-0.5f + i, -0.5f + j, 0f));

                    indices.Add(index++);
                    indices.Add(index++);
                    indices.Add(index++);

                    Color4 color;
                    if (index % 2 == 0)
                    {
                        color = new Color4(1, 0, 0, 1);
                    }
                    else
                    {
                        color = new Color4(0, 1, 0, 1);
                    }
                    colors.Add(color);
                    colors.Add(color);
                    colors.Add(color);
                }
            }
            
            Model = new MeshGeometry3D
            {
                Positions = new Vector3Collection(positions),
                Indices = new IntCollection(indices),
                Colors = new Color4Collection(colors)
            };

            WhiteLight = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}