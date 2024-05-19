# Description

Animate (move) the drone cubes using `ArchetypeQuery`'s.  
Render cubes using `ModelMesh.Draw()`.


# MonoGame Project Setup

Platforms: **Windows**, **macOS**,

This MonoGame project was created by executing the instructions below.


### macOS

For macOS ARM the .NET Core CLI need to be set to: `/usr/local/share/dotnet/x64/dotnet`

See: https://community.monogame.net/t/textureimporter-error-mac-os-monterey-12-6/18049/13

In Rider: **Settings | Build, Execution, Deployment | Toolset and Build | .NET CLI executable path** 




### Create MonoGame project

- [Getting Started - MonoGame](https://monogame.net/articles/getting_started/index.html)

    - [Setting up your development environment for Windows](https://monogame.net/articles/getting_started/1_setting_up_your_development_environment_windows.html)

    - [Creating a Project with Visual Studio 2022](https://monogame.net/articles/getting_started/2_creating_a_new_project_vs.html)

    - [.NET CLI - JetBrains Rider or Visual Studio Code](https://monogame.net/articles/getting_started/2_creating_a_new_project_netcore.html)


### Add nuget package: Friflo.Engine.ECS

```
dotnet add package Friflo.Engine.ECS
```