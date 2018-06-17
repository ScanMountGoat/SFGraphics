# SFGraphics
WIP OpenGL object oriented wrapper and utility classes utilizing OpenTK. This is designed to make OpenTK less complicated to use while still providing full control if needed.
Originally based on the rendering implemented for Smash Forge. **This library is still changing frequently, so don't use this for any official projects until things are more finalized.**

# Building
WIP. Support for platforms other than Windows not planned. There may be issues with the .NET target framework not matching, but this can be changed before building.

# [Documentation](https://scanmountgoat.github.io/SFGraphics/)
Online documentation for all classes, methods, etc. Still a WIP. Add the .xml file from the build directory to the same directory as the .dll if you want the comments to appear in Visual Studio.

# Features Overview
**ClassName**  

## SFGraphics Heirarchy
The main graphics library.
* GLObjects
    * Shaders
        * **Shader**
        * **ShaderLog**
    * Textures
        * **Texture2D**
        * **TextureCubeMap**
    * **Framebuffer**
    * **GLObjectManager**
* Tools
    * **ColorTools**
        * RGB -> HSV, HSV -> RGB
        * Color Temperature -> RGB
        * System.Drawing.Color -> OpenTK.Vector4

## GLObject Usage
### State Management
The ID is always bound to the target specified when the object was created before modifying the object's parameters. This makes modifying and accessing the state of an object much less error prone. IDs are not unbound after operations, so do not rely on the previous ID being bound.

### Memory Management
In order to cleanup memory used by any of the classes in the `SFGraphics.GLObjects` namespace, there should be a call to  `GLObjectManager.DeleteUnusedGLObjects()` while an OpenTK context is current.

GLObjectManager maintains reference counts for all GLObjects (just textures currently) and only calls the corresponding GL delete function when the reference count is 0. Constructors add a reference and finalizers remove a reference. GLObjects won't be flagged for cleaned up immediately (ex: deleting a context) due to the reliance on garbage collection for the container object.

This does guarantee, however, that textures, shaders, etc will not be deleted while there is still exists a GLObject with the ID. Wait for pending finalizers if you need to ensure resources are cleaned up in a timely manner.

# Credits
### OpenTK  
Used as the C# wrapper for OpenGL and for useful Vector/Matrix methods.
* [Github](https://github.com/opentk/opentk)
* [License](https://github.com/opentk/opentk/blob/develop/License.txt)

### Smash Forge  
Code adapted from the classes I originally wrote for Forge rendering.
* [Github](https://github.com/jam1garner/Smash-Forge)
* [License](https://github.com/jam1garner/Smash-Forge/blob/master/License.txt)
