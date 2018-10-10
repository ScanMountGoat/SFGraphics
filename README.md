# SFGraphics
[![Build status](https://ci.appveyor.com/api/projects/status/2u86186wtxiq77jw/branch/master?svg=true)](https://ci.appveyor.com/project/ScanMountGoat/sfgraphics/branch/master)  

### Overview
SFGraphics is a C# OpenGL utility library using OpenTK. The goal of this library is to simplify and provide a safe, unified interface for the rendering of texture and model data.

**The core functionality is mostly complete, but many features are still subject to change. Avoid
using this for official projects until the first release.**

### Requirements
| Component | Minimum Required Version |
| ---     | ---------------------- |
| .NET | 4.0 |
| OpenGL / GLSL | 3.30 |
| OpenTK | 3.0 |

# Projects
## SFGraphics
Object oriented wrappers that take advantage of C# language features for OpenGL objects such as textures, shaders, buffers, etc. The wrapper classes provide a safer and simpler way of working with OpenGL objects. *Method and class names closely match the OpenGL specification in most cases.*

## SFGraphics.Utils
Contains helpful methods for working with colors and vectors. Provides methods to facilitate working with `Bitmap` and `Color` objects in the `System.Drawing` namespace.

## SFGenericModel
Provides classes for rendering generic vertex data of a specified struct. The `GenericMesh<T>` class handles the management of buffers, vertex attributes, and other OpenGL rendering state internally. This class can be subclassed to allow for simplified rendering of vastly different vertex data.

## SFShapes
Uses the SFGenericModel classes to provide rendering of basic 3d geometric primitives.

# Issues
Please report all bugs or feature requests in the [bug tracker](https://github.com/ScanMountGoat/SFGraphics/issues).

# Building
Open the .sln and build in Visual Studio 2017 with .NET framework 4.0 or later.

# Releases
[Latest Commit](https://github.com/ScanMountGoat/SFGraphics/releases)  
The latest commit to master built by Appveyor. These builds are still being tested.

# Documentation
The online documentation is updated less
frequently than the doc comments. To see doc comments in Visual Studio, simply place `SFGraphics.xml` in the same directory
as the DLL.

[Online Code Documentation](https://scanmountgoat.github.io/SFGraphics/)

TODO: [Wiki]()

# Credits
### OpenTK  
Used as the C# wrapper for OpenGL functions. Provides vector and matrix types.
* [Github](https://github.com/opentk/opentk)
* [License](https://github.com/opentk/opentk/blob/develop/License.txt)

### Smash Forge  
Much of the OpenGL object wrappers and utility code is adapted from code I originally wrote for Smash Forge.
* [Github](https://github.com/jam1garner/Smash-Forge)
* [License](https://github.com/jam1garner/Smash-Forge/blob/master/License.txt)

### Sandcastle
Online documentation for all classes and methods in SFGraphics is generated using Sandcastle.
* [Github](https://github.com/EWSoftware/SHFB)
