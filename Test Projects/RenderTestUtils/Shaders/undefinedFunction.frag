#version 330

out vec4 fragColor;

uniform vec3 vec3Uniform2;
uniform vec3 vec3Uniform;

vec3 notDefined();

void main()
{
	fragColor.rgb = vec3Uniform2 * vec3Uniform * notDefined();
}