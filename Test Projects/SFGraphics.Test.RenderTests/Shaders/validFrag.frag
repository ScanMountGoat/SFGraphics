#version 330

uniform float float1;
uniform float floatArray1[3];

uniform int int1;
uniform int boolInt1;
uniform int intArray1[3];

uniform vec3 vector3a;
uniform vec4 vector4a;

uniform mat4 matrix4a;

uniform sampler2D tex2D;
uniform samplerCube texCube;

out vec4 fragColor;

void main()
{
    fragColor = vec4(1) * float1 * int1;
	if (boolInt1 == 1)
		fragColor *= 0.5;
	fragColor.rgb *= vector3a;
	fragColor *= vector4a * matrix4a;
	fragColor *= texture(tex2D, vec2(1));
	fragColor *= texture(texCube, vec3(1));

	for (int i = 0; i < floatArray1.length(); i++)
	{
		fragColor *= floatArray1[i];
	}

	for (int i = 0; i < intArray1.length(); i++)
	{
		fragColor *= intArray1[i];
	}
}