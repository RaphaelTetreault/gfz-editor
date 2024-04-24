#version 330 core

// inputs
in vec3 vclr;

// outputs
out vec4 FragColor;

void main()
{
    FragColor = vec4(vclr, 1.0);
}