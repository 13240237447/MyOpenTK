using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;


namespace MyOpenTK
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Lesson1 lesson1 = new Lesson1(800, 600, "LearnOpenTK"))
            {
                lesson1.Run();
            }
        }
    }
}
