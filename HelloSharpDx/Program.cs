using System;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Windows;

namespace HelloSharpDx
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            DrawSingleTriangleWithoutColors();
        }

        private static void DrawSingleTriangleWithoutColors()
        {
            var form = new RenderForm();

            var direct3D = new Direct3D();
            var presentParameters = new PresentParameters(form.ClientSize.Width, form.ClientSize.Height);
            var device = new Device(
                direct3D,
                0,
                DeviceType.Hardware,
                form.Handle,
                CreateFlags.HardwareVertexProcessing,
                presentParameters);

            var vertexBuffer = new VertexBuffer(
                device,
                Utilities.SizeOf<Vector4>() * 3,
                Usage.WriteOnly,
                VertexFormat.Position,
                Pool.Managed);
            var dataStream = vertexBuffer.Lock(0, 0, LockFlags.None);
            dataStream.WriteRange(new[]
            {
                new Vector4(-1/2.0f, -1/2.0f, 0, 1),
                new Vector4(-1/2.0f, 1/2.0f, 0, 1),
                new Vector4(1/2.0f, 1/2.0f, 0, 1) 
            });
            vertexBuffer.Unlock();

            var vertexElements = new[]
            {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                VertexElement.VertexDeclarationEnd
            };
            var vertexDeclaration = new VertexDeclaration(device, vertexElements);
            device.SetStreamSource(0, vertexBuffer, 0, Utilities.SizeOf<Vector4>());
            device.VertexDeclaration = vertexDeclaration;

            RenderLoop.Run(form, () =>
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DimGray, 1f, 0);
                device.BeginScene();

                device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

                device.EndScene();
                device.Present();
            });

            vertexBuffer.Dispose();
            device.Dispose();
            direct3D.Dispose();
        }
    }
}
