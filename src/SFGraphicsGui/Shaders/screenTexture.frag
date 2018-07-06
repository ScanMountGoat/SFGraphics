#version 330

in vec2 texCoord;

uniform sampler2D uvTexture;
a
out vec4 fragColor;

void main()
{
    fragColor.rgb = texture(uvTexture, texCoord).rgb;
    fragColor.a = 1;
}
