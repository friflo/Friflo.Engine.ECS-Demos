
# Setup

Friflo.Engine.ECS is added as a nuget package using [NuGetForUnity - GitHub](https://github.com/GlitchEnzo/NuGetForUnity).  
NuGetForUnity **must be installed** in Unity to add Friflo.Engine.ECS as nuget package.

This project was created by executing the following steps.


### Unity Project Setup
- Selected Unity Install: 2022.3.20f1
- Selected template: 3D (URP) Core  
  URP is required to enable using an Matrix4x4[] array with a length  
  of multiple of 100.000 by `Graphics.RenderMeshInstanced()`  


### Install NuGetForUnity

1. Open Package Manager at  
  Menu > Window > Package Manager

2. Click + button on the upper-left of a window, and select "Add package from git URL..."

3. Enter the following URL and click Add button
    ```
    https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
    ```


### Add nuget package **Friflo.Engine.ECS**

1. Open Nuget Manager at  
   Menu > NuGet > Manage Nuget Packages

2. Search the package below and click Install.
    ```
    Friflo.Engine.ECS
    ```
 
  
