# Friflo.Engine.ECS-Demos

This repository contains demos showing integration of [Friflo.Engine.ECS](https://github.com/friflo/Friflo.Json.Fliox/blob/main/Engine/README.md)
in various game engines.

- [Godot](Godot) - Windows, macOS, Linux
- [MonoGame](MonoGame) - Windows
- [Unity](Unity) - Windows, macOS, Linux

All projects are using the same [source code](./Godot/Drones.cs) to animate cubes using multiple
[ArchetypeQuery](https://github.com/friflo/Friflo.Engine-docs/blob/main/api/ArchetypeQuery.md)'s.  
The screenshot below is a MonoGame WebAssembly build demonstrating the animation with a low amount of entities.

<a href="https://sdl-wasm-sample-web.vercel.app/docs/MonoGame/">
<img src="https://raw.githubusercontent.com/friflo/Friflo.Engine-docs/main/docs/images/MonoGame-wasm.png" width="600" height="405"/>
</a>

Interactive Browser Demo showing MonoGame WebAssembly integration. [Try out Demo](https://sdl-wasm-sample-web.vercel.app/docs/MonoGame/).  
*Note:* WebGL rendering performance cannot compete with Desktop build.