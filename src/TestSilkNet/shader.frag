#version 330 core

in vec3 vColor;

out vec4 out_color;

//uniform vec4 uniform_color;

void main()
{
    //out_color = uniform_color;
    //out_color = vec4(1.0, 0.5, 0.2, 1.0);
    out_color = vec4(vColor, 1);
}