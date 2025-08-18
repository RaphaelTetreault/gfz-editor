using System;

namespace gfz_editor;

internal class Program
{
    static void Main(string[] args)
    {
        //GfzEditorMainWindow gfzEditor = new();
        //gfzEditor.Run();
        //Console.WriteLine("END");

        GfzEditor editor = new();
        editor.Run();
    }
}