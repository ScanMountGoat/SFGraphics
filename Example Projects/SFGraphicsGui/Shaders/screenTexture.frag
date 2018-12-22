#version 330

in vec2 texCoord;

uniform sampler2D uvTexture;
uniform sampler3D lutTexture;

out vec4 fragColor;

void main()
{
    fragColor.rgb = texture(uvTexture, vec2(texCoord.x, 1 - texCoord.y)).rgb;
	fragColor.rgb *= texture(lutTexture, fragColor.rgb).rgb;
    fragColor.a = 1;
}
