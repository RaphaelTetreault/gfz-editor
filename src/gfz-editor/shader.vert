#version 330 core

// inputs
in vec3 pos;
in vec3 clr;

// outputs
out vec3 vclr; 

void main()
{
    gl_Position = vec4(pos, 1.0);
    vclr = clr;
}