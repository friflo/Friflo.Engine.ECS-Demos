// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using Example.Systems;
using TMPro;
using UnityEngine;

// ReSharper disable InconsistentNaming
public class MoveDronesECS : MonoBehaviour
{
    [SerializeField] private TMP_Text   count;
    [SerializeField] private TMP_Text   fpsText;
    
    public  Material        material;
    public  Mesh            mesh;
    
    private RenderParams    rp;
    private Matrix4x4[]     instData;
    private int             entityCount;
    private Shape           shape;
    private Drones          drones;
    
    void Start()
    {
        entityCount = 1024;
        drones = new Drones();
        drones.Initialize();
        drones.SetEntityCount(entityCount);
        drones.SetTargetRings(2000, 100, 5f, 10);
        UpdateGuiCount();
        rp          = new RenderParams(material);
        instData    = new Matrix4x4[drones.maxDroneCount];
        transform.Find("Editor Plane").gameObject.SetActive(false);
    }
    
    private void UpdateGuiCount() {
        count.text  = $"Count: {entityCount}";
    }
    
    private void SetShape (Shape shape)
    {
        this.shape = shape;
        switch (shape)
        {
            case Shape.Plane:	drones.SetTargetPlane(500, 5f); 		    break;
            case Shape.Cube:	drones.SetTargetCube (500, 5f);			    break;
            case Shape.Ring:	drones.SetTargetRings(500, 120, 5f, 1);		break;
            case Shape.Rings:	drones.SetTargetRings(500, 100, 5f, 10);	break;
        }
    }
    
    public void SetTargetPlane()    => SetShape(Shape.Plane);
    public void SetTargetCube()     => SetShape(Shape.Cube);
    public void SetTargetRing()     => SetShape(Shape.Ring);
    public void SetTargetRings()    => SetShape(Shape.Rings);
    
    public void IncreaseCount() {
        entityCount = Math.Min(drones.maxDroneCount, entityCount * 2);
        drones.SetEntityCount(entityCount);
        SetShape(shape);
        UpdateGuiCount();
    }
    
    public void DecreaseCount() {
        entityCount = Math.Max(4, entityCount / 2);
        drones.SetEntityCount(entityCount);
        SetShape(shape);
        UpdateGuiCount();
    }

    private const int FPSSampleCount = 30;
    private readonly int[] fpsSamples = new int[FPSSampleCount];
    private int sampleIndex;

    private void UpdateFps()
    {
        var sum = 0;
        for (var i = 0; i < FPSSampleCount; i++)
        {
            sum += fpsSamples[i];
        }
        fpsText.text = $"FPS: {sum / FPSSampleCount}";
    }
    
    void Update()
    {
        fpsSamples[sampleIndex++] = (int)(1.0f / Time.deltaTime);
        if (sampleIndex >= FPSSampleCount) sampleIndex = 0;
        
        UpdateFps();
        
        var deltaTime = Time.deltaTime * 1000;
        drones.UpdateTransforms(deltaTime, default);

        int n = 0;
        // var scale = Matrix4x4.Scale(new Vector3(10, 10, 10));
        foreach (var (transforms, _) in drones.transQuery.Chunks)
        {
            foreach (ref var trans in transforms.Span) {
                instData[n++] = trans.value.AsUnityMatrix4x4();
            }
        }
        Graphics.RenderMeshInstanced(rp, mesh, 0, instData, entityCount);
    }
}
