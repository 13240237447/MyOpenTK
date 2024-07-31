using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;


namespace MyOpenTK
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "LearnOpenTK"))
            {
                game.Run();
            }
        }
    }
}
