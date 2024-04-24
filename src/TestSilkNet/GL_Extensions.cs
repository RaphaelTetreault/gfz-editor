using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestSilkNet
{
    internal static class GL_Extensions
    {
        // Load shader text from file
        private static string LoadShaderText(string path)
        {
            // To get out of debug or releasse folder and in VS dir... not a final solution
            path = "../../../" + path;

            if (!File.Exists(path))
            {
                string msg;
                bool isRelative = Path.IsPathRooted(path);
                if (isRelative)
                {
                    msg = $"No file {path} in {Directory.GetCurrentDirectory()}.";
                }
                else
                {
                    msg = $"File {path} does not exist.";
                }

                throw new FileNotFoundException(msg);
            }

            string text = File.ReadAllText(path);
            return text;
        }

        // Load individual shader
        private static uint CompileShader(this GL gl, ShaderType shaderType, string path)
        {
            uint shader = gl.CreateShader(shaderType);
            string shaderSource = LoadShaderText(path);
            gl.ShaderSource(shader, shaderSource);
            gl.CompileShader(shader);
            gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
            if (status != (int)GLEnum.True)
            {
                string msg = $"{shaderType} failed to compile: {gl.GetShaderInfoLog(shader)}";
                throw new Exception(msg);
            }

            return shader;
        }

        // Load and compile shader program
        public static uint CreateShaderProgram(this GL gl, params string[] shaderPaths)
        {
            // Create GLSL program
            uint program = gl.CreateProgram();

            // Load each part of the shader
            var extensions = new List<ShaderType>(shaderPaths.Length);
            foreach (string shaderPath in shaderPaths)
            {
                // Ensure we are not compiling duplicate shader types
                string shaderExtension = Path.GetExtension(shaderPath)[1..];
                ShaderType shaderType = ExtensionToShaderType(shaderExtension);
                if (extensions.Contains(shaderType))
                {
                    string msg = $"Cannot compile shader program with multiple {shaderType}.";
                    throw new Exception(msg);
                }
                extensions.Add(shaderType);

                // Load the shader
                uint shader = CompileShader(gl, shaderType, shaderPath);
                gl.AttachShader(program, shader);
            }

            // Link shader parts together
            gl.LinkProgram(program);

            // Validate linking
            gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int status);
            if (status != (int)GLEnum.True)
                throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(program));

            // Done!
            return program;
        }

        // Convert a file extension into its appropriate ShaderType
        private static ShaderType ExtensionToShaderType(string extension)
        {
            extension = extension.ToLower();
            ShaderType type = extension switch
            {
                "comp" => ShaderType.ComputeShader,
                "frag" => ShaderType.FragmentShader,
                "geom" => ShaderType.GeometryShader,
                "tesc" => ShaderType.TessControlShader,
                "tese" => ShaderType.TessEvaluationShader,
                "vert" => ShaderType.VertexShader,
                _ => throw new Exception($"Invalid extension \"{extension}\"."),
            };
            return type;
        }
    }
}
