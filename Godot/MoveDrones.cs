// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Numerics;
using Example.Systems;
using Godot;

namespace Example;

public partial class MoveDrones : MultiMeshInstance3D
{
	private readonly	Drones		drones		= new Drones();
	private				int			entityCount	= 1024;
	private				Shape		shape;
	
	public override void _Ready()
	{
		Multimesh.InstanceCount = entityCount;
		for(int i = 0; i < Multimesh.InstanceCount; i++)
		{
			var m = Matrix4x4.CreateTranslation(new System.Numerics.Vector3(i * 2f, 0, 0));
			Multimesh.SetInstanceTransform(i, m.AsTransform3D());
		}
		drones.Initialize();
		drones.SetEntityCount(entityCount);
		drones.SetTargetPlane(2000, 1.2f);
		
		var plane = (Node3D)GetParent().FindChild("Plane");
		plane.Hide();
	}
	
	public void SetTargetPlane()	=> SetShape(Shape.Plane);
	public void SetTargetCube()		=> SetShape(Shape.Cube);
	public void SetTargetRing()		=> SetShape(Shape.Ring);
	public void SetTargetRings()	=> SetShape(Shape.Rings);
	
	private void SetShape (Shape shape)
	{
		this.shape = shape;
		switch (shape)
		{
			case Shape.Plane:	drones.SetTargetPlane(500, 1.2f);			break;
			case Shape.Cube:	drones.SetTargetCube (500, 1.2f);			break;
			case Shape.Ring:	drones.SetTargetRings(500, 24, 1.2f, 1);	break;
			case Shape.Rings:	drones.SetTargetRings(500, 20, 1.2f, 10);	break;
		}
	}
	
	public void IncreaseEntities() {
		entityCount = Math.Min(drones.maxDroneCount, entityCount * 2);
		drones.SetEntityCount(entityCount);
		SetShape(shape);
	}
	
	public void DecreaseEntities() {
		entityCount = Math.Max(4, entityCount / 2);
		drones.SetEntityCount(entityCount);
		SetShape(shape);
	}


	public override void _Process(double delta)
	{
		var deltaTime = (float)delta * 1000;
		drones.UpdateTransforms(deltaTime, default);
		DrawEntities();
	}
	
	private void DrawEntities()
	{
		var mesh = Multimesh;
		mesh.InstanceCount = entityCount;
		foreach (var (transforms, _) in drones.transQuery.Chunks)
		{
			int n = 0;
			foreach (ref var transform in transforms.Span)
			{
				mesh.SetInstanceTransform(n++, transform.value.AsTransform3D());
			}
		}
	}
}
