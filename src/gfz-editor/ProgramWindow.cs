using ImGuiNET;
using Silk.NET.Core.Native;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace gfz_editor
{
    /// <summary>
    /// Window for inspecting models.
    /// </summary>
    public class ProgramWindow
    {
        // TODO: dynamically load languages?
        public string ShortCutFileOpen = "Ctrl+O";
        public string ShortCutFileSave = "Ctrl+S";

        public delegate void FileAction(string fileName);
        public delegate void SaveAction();

        public event FileAction OnOpen = delegate { };
        public event SaveAction OnSave = delegate { };


        public readonly List<Func<bool>> RunningFunctions = new List<Func<bool>>();


        public void Init()
        {

        }



        public void Display()
        {
            ShowToolbar();
            // TODO: render to main window?
        }

        public void ShowToolbar()
        {
            if (ImGui.BeginMainMenuBar())
            {
                MenuIcon();
                FileMenu();
                EditMenu();
                WindowMenu();
                ImGui.EndMainMenuBar();
            }

            if (!RunningFunctions.Contains(TestDirectoryTreeView))
                RunningFunctions.Add(TestDirectoryTreeView);

            foreach (var x in RunningFunctions)
            {
                x.Invoke();
            }
        }

        public void MenuIcon()
        {
            // TODO: get logo :)
            //ImGui.Image(textureID, new System.Numerics.Vector2(16, 16));
        }

        public void FileMenu()
        {
            if (!ImGui.BeginMenu("File"))
                return;

            FileMenuOpen();
            FileMenuSave();
            ImGui.EndMenu();
        }

        public void WindowMenu()
        {
            if (!ImGui.BeginMenu("Window"))
                return;

            ImGui.EndMenu();
        }



        public void FileMenuOpen()
        {
            bool success = ImGui.MenuItem("Open", ShortCutFileOpen);
            if (!success)
                return;

            //bool something = TestDirectoryTreeView();
            bool contains = RunningFunctions.Contains(TestDirectoryTreeView);
            if (!contains)
            {
                RunningFunctions.Add(TestDirectoryTreeView);
            }

            string fileName = "file.ext";
            OnOpen?.Invoke(fileName);
        }

        public void FileMenuSave()
        {
            OnSave?.Invoke();
        }



        public void EditMenu()
        {
            if (!ImGui.MenuItem("Edit"))
                return;

        }

        // show / update
        private void ShowDir(string name)
        {
            bool isSelected = ImGui.MenuItem(name);
            if (isSelected)
            {
                Directory.SetCurrentDirectory(name);
            }
        } 

        public bool TestDirectoryTreeView()
        {
            // Current Working Directory - CWD; DIRectory - DIR.
            var cwd = Directory.GetCurrentDirectory();
            var dirDirs = Directory.GetDirectories(cwd);
            var dirFiles = Directory.GetFiles(cwd);


            bool windowClosed = false;
            ImGui.Begin("Open File", ref windowClosed, ImGuiWindowFlags.NoCollapse);
            {
                ImGui.Text(cwd);
                ImGui.LabelText("Directories", "");
                ShowDir("..");
                foreach (var dir in dirDirs)
                {
                    string dirName = Path.GetFileName(dir);
                    ShowDir(dirName);
                }

                ImGui.LabelText("Files", "");
                foreach (var file in dirFiles)
                {
                    string fileName = Path.GetFileName(file);
                    bool isSelected = ImGui.MenuItem(fileName);
                    if (isSelected)
                    {
                        return true;
                    }
                }
            }
            ImGui.End();

            if (windowClosed)
                return false;

            return true;
        }


        private void RenderTriangle()
        {

        }

    }
}
