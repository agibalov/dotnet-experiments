using System;
using System.Runtime.InteropServices;
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
            // DrawSingleTriangleWithoutColors();
            // DrawSingleTriangleWithColors();
            // DrawTwoTrianglesWithColors();
            DrawSingleTriangleWithPerspective();
        }

        private static void DrawSingleTriangleWithPerspective()
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
                Utilities.SizeOf<Vertex>() * 3,
                Usage.WriteOnly,
                VertexFormat.None,
                Pool.Managed);
            var dataStream = vertexBuffer.Lock(0, 0, LockFlags.None);
            dataStream.WriteRange(new[]
            {
                new Vertex { Color = Color.Red, Position = new Vector4(-0.5f, -0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.Green, Position = new Vector4(-0.5f, 0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.Blue, Position = new Vector4(0.5f, 0.5f, 0.0f, 1.0f) },
            });
            vertexBuffer.Unlock();

            var vertexElements = new[]
            {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, (short)Utilities.SizeOf<Vector4>(), DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };
            var vertexDeclaration = new VertexDeclaration(device, vertexElements);
            device.SetStreamSource(0, vertexBuffer, 0, Utilities.SizeOf<Vertex>());
            device.VertexDeclaration = vertexDeclaration;

            device.SetRenderState(RenderState.Lighting, false);
            device.SetRenderState(RenderState.CullMode, Cull.None);

            var viewMatrix = Matrix.LookAtLH(new Vector3(0, 0, -1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var projectionMatrix = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(100), form.Width / (float)form.Height, 0.001f, 100f);

            var rotation = 0.0f;
            RenderLoop.Run(form, () =>
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DimGray, 1f, 0);
                device.BeginScene();

                device.SetTransform(TransformState.View, viewMatrix);
                device.SetTransform(TransformState.Projection, projectionMatrix);
                device.SetTransform(TransformState.World, Matrix.RotationY(MathUtil.DegreesToRadians(rotation)));

                device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

                device.EndScene();
                device.Present();

                rotation += 0.01f;
            });

            vertexBuffer.Dispose();
            device.Dispose();
            direct3D.Dispose();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public Vector4 Position;
            public ColorBGRA Color;
        }

        private static void DrawTwoTrianglesWithColors()
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
                Utilities.SizeOf<Vertex>() * 4,
                Usage.WriteOnly,
                VertexFormat.None,
                Pool.Managed);
            var dataStream = vertexBuffer.Lock(0, 0, LockFlags.None);
            dataStream.WriteRange(new[]
            {
                new Vertex { Color = Color.Red, Position = new Vector4(-0.5f, -0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.Green, Position = new Vector4(-0.5f, 0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.Blue, Position = new Vector4(0.5f, 0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.White, Position = new Vector4(0.5f, -0.5f, 0.0f, 1.0f) }
            });
            vertexBuffer.Unlock();

            var vertexElements = new[]
            {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, (short)Utilities.SizeOf<Vector4>(), DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };
            var vertexDeclaration = new VertexDeclaration(device, vertexElements);
            
            var indexBuffer = new IndexBuffer(device, Utilities.SizeOf<short>() * 6, Usage.WriteOnly, Pool.Managed, true);
            var indexBufferDataStream = indexBuffer.Lock(0, 0, LockFlags.None);
            indexBufferDataStream.WriteRange(new short[]
            {
                0, 1, 2, 
                0, 2, 3
            });
            indexBuffer.Unlock();
            
            device.SetStreamSource(0, vertexBuffer, 0, Utilities.SizeOf<Vertex>());
            device.VertexDeclaration = vertexDeclaration;
            device.Indices = indexBuffer;
            device.SetRenderState(RenderState.Lighting, false);

            RenderLoop.Run(form, () =>
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DimGray, 1f, 0);
                device.BeginScene();
                
                device.DrawIndexedPrimitive(PrimitiveType.TriangleList, 0, 0, 6, 0, 2);

                device.EndScene();
                device.Present();
            });

            indexBuffer.Dispose();
            vertexBuffer.Dispose();
            device.Dispose();
            direct3D.Dispose();
        }

        private static void DrawSingleTriangleWithColors()
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
                Utilities.SizeOf<Vertex>() * 3,
                Usage.WriteOnly,
                VertexFormat.None,
                Pool.Managed);
            var dataStream = vertexBuffer.Lock(0, 0, LockFlags.None);
            dataStream.WriteRange(new[]
            {
                new Vertex { Color = Color.Red, Position = new Vector4(-0.5f, -0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.Green, Position = new Vector4(-0.5f, 0.5f, 0.0f, 1.0f) },
                new Vertex { Color = Color.Blue, Position = new Vector4(0.5f, 0.5f, 0.0f, 1.0f) },
            });
            vertexBuffer.Unlock();

            var vertexElements = new[]
            {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, (short)Utilities.SizeOf<Vector4>(), DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };
            var vertexDeclaration = new VertexDeclaration(device, vertexElements);
            device.SetStreamSource(0, vertexBuffer, 0, Utilities.SizeOf<Vertex>());
            device.VertexDeclaration = vertexDeclaration;

            device.SetRenderState(RenderState.Lighting, false);

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
                VertexFormat.None,
                Pool.Managed);
            var dataStream = vertexBuffer.Lock(0, 0, LockFlags.None);
            dataStream.WriteRange(new[]
            {
                new Vector4(-0.5f, -0.5f, 0, 1),
                new Vector4(-0.5f, 0.5f, 0, 1),
                new Vector4(0.5f, 0.5f, 0, 1) 
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
