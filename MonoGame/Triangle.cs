// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Triangle
{
    private		BasicEffect				basicEffect;
    // geometric info
    private		VertexPositionColor[]	triangleVertices;	// position + color[]
    private		VertexBuffer			vertexBuffer;
    
    internal void Initialize(Game1 game)
    {
	    // set basicEffect
	    var device = game.GraphicsDevice;
	    basicEffect = new BasicEffect(device);
	    basicEffect.Alpha = 1; // full opaque
	    basicEffect.VertexColorEnabled = true; // see colored vertices
	    basicEffect.LightingEnabled = false;
				
	    // create triangle
	    triangleVertices = new VertexPositionColor[3];
	    triangleVertices[0] = new VertexPositionColor(new Vector3( 0,  2, 0), Color.Red);
	    triangleVertices[1] = new VertexPositionColor(new Vector3(-2, -2, 0), Color.Green);
	    triangleVertices[2] = new VertexPositionColor(new Vector3( 2, -2, 0), Color.Blue);
				
	    vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
	    vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
    }

    public void Draw(Game1 game, Camera camera)
    {
	    var device = game.GraphicsDevice;
	    basicEffect.Projection	= camera.projectionMatrix;
	    basicEffect.View		= camera.viewMatrix;
	    basicEffect.World		= game.worldMatrix;
	    device.SetVertexBuffer(vertexBuffer);
	    
	    // Turn off back face culling
	    var rasterizerState = new RasterizerState();
	    rasterizerState.CullMode = CullMode.None;
	    device.RasterizerState = rasterizerState;

	    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
		    pass.Apply();
		    device.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
	    }
    }
}