using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL.Compatibility;


namespace MyOpenTK
{
    public class Shader : IDisposable
    {
        
        public int Handle {private set; get; }
        
        private bool disposedValue = false;
        
        private readonly Dictionary<string, int> _uniformLocations;
        public Shader(string vertexPath, string fragmentShader)
        {
            string vertexContent = File.ReadAllText($"../../../Shader/{vertexPath}");
            string fragmentContent = File.ReadAllText($"../../../Shader/{fragmentShader}");
            var iVertexHd= GL.CreateShader(ShaderType.VertexShader);
            var iFragmentHd = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(iVertexHd,vertexContent);
            GL.ShaderSource(iFragmentHd,fragmentContent);
            GL.CompileShader(iVertexHd);
            GL.CompileShader(iFragmentHd);
            int state = 0;
            GL.GetShaderi(iVertexHd, ShaderParameterName.CompileStatus, ref state);
            if (state == 0)
            {
                GL.GetShaderInfoLog(iVertexHd, out var info);
                Console.WriteLine($"Compile Vertex Shader error {info}");
                return;
            }
            GL.GetShaderi(iFragmentHd, ShaderParameterName.CompileStatus, ref state);
            if (state == 0)
            {
                GL.GetShaderInfoLog(iVertexHd, out var info);
                Console.WriteLine($"Compile Fragment Shader error {info}");
                return;
            }

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle,iVertexHd);
            GL.AttachShader(Handle,iFragmentHd);
            GL.LinkProgram(Handle);
            GL.GetProgrami(Handle, ProgramProperty.LinkStatus, ref state);
            if (state == 0)
            {
                GL.GetShaderInfoLog(Handle, out var info);
                Console.WriteLine($"Link Shader error {info}");
                return;
            }
            
            GL.DetachShader(Handle,iVertexHd);
            GL.DetachShader(Handle,iFragmentHd);
            GL.DeleteShader(iVertexHd);
            GL.DeleteShader(iFragmentHd);
            int numberOfUniforms = 0;
            GL.GetProgrami(Handle, ProgramProperty.ActiveUniforms, ref numberOfUniforms);

            // Next, allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                int buffSize = 10;
                int lenght = 0;
                int size = 0;
                UniformType uniformType = UniformType.Int;
                var key = GL.GetActiveUniform(Handle, (uint)i,buffSize, ref lenght, ref size,ref uniformType);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }

        public uint GetAttribLocation(string name)
        {
            return (uint)GL.GetAttribLocation(Handle, name);
        }

        public void SetInt(string name, int index)
        {
            GL.Uniform1i(_uniformLocations[name],index);
        }
    }
}
