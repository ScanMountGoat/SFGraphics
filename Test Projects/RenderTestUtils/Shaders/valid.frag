#version 330

uniform float float1;
uniform float floatArray1[3];

uniform uint uint1;
uniform uint uintArray1[3];
uniform int int1;
uniform int boolInt1;
uniform int intArray1[3];

uniform vec2 vector2a;
uniform vec2[2] vector2Arr;

uniform vec3 vector3a;
uniform vec3[2] vector3Arr;

uniform vec4 vector4a;
uniform vec4[2] vector4Arr;

uniform mat4 matrix4a;
uniform mat4[2] matrix4Arr;

uniform sampler2D tex2D;
uniform samplerCube texCube;

uniform UniformBlockA
{
	float blockAFloat;
	vec4 blockAVec4;
};

out vec4 fragColor;

void main()
{
    // Use all the uniforms, so they aren't optimized out by the compiler.
    fragColor = vec4(1) * float1 * int1;
	if (boolInt1 == 1)
		fragColor *= 0.5;
    fragColor.rg *= vector2a;
	fragColor.rgb *= vector3a;
	fragColor *= vector4a * matrix4a;
	fragColor *= texture(tex2D, vec2(1));
	fragColor *= texture(texCube, vec3(1));
	fragColor.rgb *= blockAFloat;


	// Use a for loop to access multiple elements.
	for (int i = 0; i < 2; i++)
	{
		fragColor *= floatArray1[i];
		fragColor *= intArray1[i];
		fragColor *= uintArray1[i];

		fragColor *= matrix4Arr[i];
		fragColor.rg *= vector2Arr[i];
		fragColor *= vector4Arr[i];
		fragColor.rgb *= vector3Arr[i];
	}

	fragColor.rgb *= blockAVec4.rgb;

    fragColor.rgb *= uint1;
}
