# SFGraphics
WIP OpenGL object oriented wrapper and utility classes utilizing OpenTK.
Originally based on the rendering implemented for Smash Forge.

# Building
WIP

# Documentation
WIP. Most public methods contain xml comments.
# Features Overview
**ClassName**  
*Interface*
### SFGraphics
The main graphics library.
* *GLObject*
    * Shaders
        * **Shader**
        * **ShaderLog**
    * Textures
        * **Texture2D**
        * **TextureCubeMap**
    * **Framebuffer**
* Tools
    * **ColorTools**
        * RGB -> HSV, HSV -> RGB
        * Color Temperature -> RGB
        * System.Drawing.Color -> OpenTK.Vector4

# Credits
### OpenTK  
Used as the C# wrapper for OpenGL and for useful Vector/Matrix methods.
* [Github](https://github.com/opentk/opentk)
* [License](https://github.com/opentk/opentk/blob/develop/License.txt)

### Smash Forge  
Code adapted from the classes I originally wrote for Forge rendering.
* [Github](https://github.com/jam1garner/Smash-Forge)
* [License](https://github.com/jam1garner/Smash-Forge/blob/master/License.txt)
