#version 330

in vec3 position;
in int intAttrib;

out vec3 outPosition;

void main() 
{
	gl_Position = vec4(position, 1) * intAttrib;
}