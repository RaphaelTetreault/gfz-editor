using gfz_editor;
using System;

namespace GfzEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var window = new GfzRenderWindow();
            window.Temp();
        }
    }
}