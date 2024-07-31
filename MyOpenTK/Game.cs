using System;
using System.Diagnostics;
using System.IO;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;


namespace MyOpenTK
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }

        private int VertexBufferObject { set; get; }
        
        private int VertexArrayObject { set; get; }
        
        private int ElementBufferObject { set; get; }

        private Shader shader;

        private Texture texture;

        private Texture texture2;

        private Stopwatch stopwatch;
        
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, 0,0, //BL
            -0.5f, 0.5f, 0.0f, 0,1, //TL
            0.5f,  0.5f, 0.0f, 1,1,//TR
            0.5f,-0.5f, 0.0f, 1,0//BR
        };

        private int[] indices =
        {
            0, 1, 2,
            0,2,3,
        };
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            // // 启用混合
            // GL.Enable(EnableCap.Blend);
            // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

         
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices,BufferUsage.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer,indices,BufferUsage.StaticDraw);
        
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();

            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation,3,VertexAttribPointerType.Float,false,5 * sizeof(float),0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation,2,VertexAttribPointerType.Float,false,5 * sizeof(float),3 * sizeof(float));


            texture = new Texture("container.jpg");
            texture.Use(TextureUnit.Texture0); 
            texture2 = new Texture("awesomeface.png");
            texture2.Use(TextureUnit.Texture1);

            shader.SetInt("texture0",0);
            shader.SetInt("texture1",1);
            
            stopwatch = Stopwatch.StartNew();
        }
        
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            GL.BindVertexArray(VertexArrayObject);

            texture.Use(TextureUnit.Texture0); 
            texture2.Use(TextureUnit.Texture1);
            shader.Use();
            // float green = (float)Math.Sin(stopwatch.Elapsed.TotalSeconds) * 0.5f + 0.5f;
            // var vertexColorHd =  GL.GetUniformLocation(shader.Handle, "vertexColor");
            // GL.Uniform4f(vertexColorHd,0,green,0,1);
            // GL.DrawArrays(PrimitiveType.Triangles,0,3);
            
            GL.DrawElements(PrimitiveType.Triangles,indices.Length,DrawElementsType.UnsignedInt,0);
            
            SwapBuffers();
          
        }
    }
}
