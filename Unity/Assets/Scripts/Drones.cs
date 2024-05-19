// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Numerics;
using Friflo.Engine.ECS;
// Note: Avoid game engine dependencies to enable using code in various engines. E.g.
//  using UnityEngine;
//  using Godot;
//  using Microsoft.Xna.Framework;

// ReSharper disable once CheckNamespace
namespace Example.Systems {

public enum Shape
{
    Plane,
    Cube,
    Ring,
    Rings
}

public struct Start : IComponent
{
    public Vector3 value;
}

public struct Target : IComponent
{
    public Vector3 value;
}

public class Drones
{
    private readonly    EntityStore     store;
    public  readonly    int             maxDroneCount = 64 * 1024;
    private             float           elapsed;
    private             float           duration = 1000; // ms
    
    private readonly    ArchetypeQuery<Start, Position>                     startPositionQuery;
    private readonly    ArchetypeQuery<Target>                              targetQuery;
    private readonly    ArchetypeQuery<Transform, Position, Start, Target>  transPosQuery;
    public  readonly    ArchetypeQuery<Transform>                           transQuery;
    private readonly    ArchetypeQuery                                      allQuery;
    private readonly    CommandBuffer                                       commandBuffer;


    internal Drones() {
        store = new EntityStore(PidType.UsePidAsId);
        startPositionQuery  = store.Query<Start, Position>();
        targetQuery         = store.Query<Target>();
        transPosQuery       = store.Query<Transform, Position, Start, Target>();
        transQuery          = store.Query<Transform>();
        allQuery            = store.Query().WithDisabled();
        commandBuffer       = store.GetCommandBuffer();
        commandBuffer.ReuseBuffer = true;
    }

    public void Initialize()
    {
        for (int n = 0; n < maxDroneCount; n++) {
            store.CreateEntity(new Position(), new Start(), new Target(), new Transform(), Tags.Get<Disabled>());
        }
    }
    
    public void SetEntityCount(int count) {
        int i = 0;
        foreach (var entity in allQuery.Entities) {
            if (i++ < count) {
                commandBuffer.RemoveTag<Disabled>(entity.Id);
            } else {
                commandBuffer.AddTag<Disabled>(entity.Id);
                commandBuffer.AddComponent<Position>(entity.Id);
            }
        }
        commandBuffer.Playback();
    }
    
    internal void SetTargetPlane(int duration, float distance)
    {
        SetStart(duration);
        int  rowCount   = (int)Math.Sqrt(targetQuery.Count);
        var  rowCountF  = (float)rowCount;
        float offset    = distance * rowCountF / 2;
        int x = 0;
        foreach (var (targets, _) in targetQuery.Chunks)
        {
            var targetSpan      = targets.Span;
            for (int n = 0; n < targets.Length; n++) {
                ref var target = ref targetSpan[n];
                target.value.X =  distance * x - offset;
                target.value.Y = -distance;
                target.value.Z =  distance * (n / rowCount) - offset;
                x = (x + 1) % rowCount;
            }
        }
    }
    
    internal void SetTargetCube(float duration, float distance)
    {
        SetStart(duration);
        int edgeCount       = (int)Math.Pow(targetQuery.Count, (1.0 / 3.0));
        int edgeCount2      = edgeCount * edgeCount;
        var offset          = distance * edgeCount / 2;
        int x = 0;
        foreach (var (targets, _) in targetQuery.Chunks)
        {
            var targetSpan    = targets.Span;
            for (int n = 0; n < targets.Length; n++) {
                ref var target = ref targetSpan[n];
                target.value.X =  distance * x - offset;
                target.value.Y =  distance * ((n / edgeCount2)% edgeCount2) - distance - offset;
                target.value.Z =  distance * ((n / edgeCount) % edgeCount) - offset;
                x = (x + 1) % edgeCount;
            }
        }
    }
    
    internal void SetTargetRings(float duration, int radius, float distance, int count)
    {
        SetStart(duration);
        var     entityCount = targetQuery.Count;
        int     ringCount   = Math.Max(1, entityCount / count);
        float   ringCountF  = ringCount;
        foreach (var (targets, _) in targetQuery.Chunks)
        {
            var targetSpan = targets.Span;
            for (int n = 0; n < targets.Length; n++) {
                var pos = (n % ringCount) / ringCountF * Math.PI * 2;
                var rot = Matrix4x4.CreateRotationY((float)pos);
                var y = distance * (n / ringCount);
                var v = new Vector3(radius, y, 0);
                ref var target = ref targetSpan[n];
                target.value = Vector3.Transform(v, rot);
            }
        }
    }
    
    private void SetStart(float duration)
    {
        elapsed         = 0;
        this.duration   = duration;
        foreach (var (starts, positions, _) in startPositionQuery.Chunks)
        {
            var startSpan       = starts.Span;
            var positionSpan    = positions.Span;
            for (int n = 0; n < positions.Length; n++) {
                startSpan[n].value = positionSpan[n].value;
            }
        }
    }
    
    internal void UpdateTransforms(float deltaTime, Matrix4x4 world)
    {
        elapsed += deltaTime;
        var complete = Math.Min(elapsed / duration, 1);
        
        foreach (var (transforms, positions, starts, targets, _) in transPosQuery.Chunks)
        {
            var transformSpan   = transforms.Span;
            var positionSpan    = positions.Span;
            var startSpan       = starts.Span;
            var targetSpan      = targets.Span;
            for (int n = 0; n < positions.Length; n++) {
                // ref var pos = ref positionSpan[n];
                var pos =  Vector3.Lerp(startSpan[n].value, targetSpan[n].value, complete);
                positionSpan[n].value = pos;
                transformSpan[n].value = world + Matrix4x4.CreateTranslation(pos);
            }
        }
    }
}

}