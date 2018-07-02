#version 330

uniform float float1;
uniform int int1;
uniform int boolInt1;
uniform vec3 vector3a;
uniform vec4 vector4a;
uniform mat4 matrix4a;

out vec4 fragColor;

void main()
{
    fragColor = vec4(1) * float1 * int1;
	if (boolInt1 == 1)
		fragColor *= 0.5;
	fragColor.rgb *= vector3a;
	fragColor *= vector4a * matrix4a;
}
