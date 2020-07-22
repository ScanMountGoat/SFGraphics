#version 330

out vec4 fragColor;

float FunctionA();

void main() 
{
	fragColor = vec4(FunctionA());
}