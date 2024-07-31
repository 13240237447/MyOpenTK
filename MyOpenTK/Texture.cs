using System.IO;
using OpenTK.Graphics.OpenGL.Compatibility;
using StbImageSharp;


namespace MyOpenTK
{
    public class Texture
    {
        public readonly int Handle;

        public Texture(string fileName)
        {
            Handle = GL.GenTexture();
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2d, Handle);
    
            // OpenGL has it's texture origin in the lower left corner instead of the top left corner,
            // so we tell StbImageSharp to flip the image when loading.
            StbImage.stbi_set_flip_vertically_on_load(1);
            using(var stream = File.OpenRead($"../../../Resources/Images/{fileName}"))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba,
                    image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                
            }
  

            // your image will fail to render at all (usually resulting in pure black instead).
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(TextureTarget.Texture2d);
            
            GL.BindTexture(TextureTarget.Texture2d, 0);
        }
        
        
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2d, Handle);
        }
    }
}
