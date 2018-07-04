#version 330

out vec4 fragColor;

vec3 notDefined();

void main()
{
	fragColor.rgb = notDefined();
}