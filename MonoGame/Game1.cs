// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using Example.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
	public class Game1 : Game
	{
		private readonly GraphicsDeviceManager graphics;
		
		// [Gamefromscratch - MonoGame Tutorial Part Five: 3D Programming - YouTube] https://www.youtube.com/watch?v=OWrBLS7HO0A&list=PLS9MbmO_ssyB_F9AhtJulWkHBCg4Q4tTE&index=8
		internal			Matrix		worldMatrix;		// project entity into world
		private				Camera		camera;
		private				int			entityCount = 1024;
		private				Shape		shape;
		
		private readonly	Triangle	triangle		= new Triangle();
		private				Model		cube;
		private readonly	Drones		drones			= new Drones();
		private readonly	KeyEvents	keyEvents		= new KeyEvents();
		private readonly	Gui			gui				= new Gui();
		private				MouseState  lastMouseState;

		
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}
		
		private void OnResize(Object sender, EventArgs e)
		{
			if (graphics.PreferredBackBufferWidth  == graphics.GraphicsDevice.Viewport.Width &&
			    graphics.PreferredBackBufferHeight == graphics.GraphicsDevice.Viewport.Height) {
				return;
			}
			graphics.PreferredBackBufferWidth	= graphics.GraphicsDevice.Viewport.Width;
			graphics.PreferredBackBufferHeight	= graphics.GraphicsDevice.Viewport.Height;
			graphics.ApplyChanges();
		}

		protected override void Initialize()
		{
			graphics.PreferredBackBufferWidth  = 2400;
			graphics.PreferredBackBufferHeight = 1350;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnResize;
			Console.WriteLine($"------------ ClientBounds: {Window.ClientBounds}");
			
			
			graphics.ApplyChanges();
			gui.Initialize(graphics.GraphicsDevice);
			base.Initialize();
			
			camera.Initialize(GraphicsDevice);
			worldMatrix = Matrix.CreateWorld(camera.target, Vector3.Forward, Vector3.Up);
			
			triangle.Initialize(this);
			drones.Initialize();
			drones.SetEntityCount(entityCount);
			drones.SetTargetPlane(2000, 5f);
		}

		protected override void LoadContent()
		{
			cube = Content.Load<Model>("cube");
			gui.LoadContent(Content);
		}
		
		private void SetShape (Shape shape)
		{
			this.shape = shape;
			switch (shape)
			{
				case Shape.Plane:	drones.SetTargetPlane(500, 5f);   			break;
				case Shape.Cube:	drones.SetTargetCube (500, 5f);   			break;
				case Shape.Ring:	drones.SetTargetRings(500, 120, 5f, 1);		break;
				case Shape.Rings:	drones.SetTargetRings(500, 100, 5f, 10);	break;
			}
		}
		
		private void HandleAction(Keys key)
		{
			if (key == Keys.None) {
				return;
			}
			Console.WriteLine($"key: {key}");
			switch (key) {
				case Keys.D1: SetShape(Shape.Plane);	break;
				case Keys.D2: SetShape(Shape.Cube);		break;
				case Keys.D3: SetShape(Shape.Ring);		break;
				case Keys.D4: SetShape(Shape.Rings);	break;
				case Keys.D5:
					entityCount = Math.Min(drones.maxDroneCount, entityCount * 2);
					drones.SetEntityCount(entityCount);
					SetShape(shape);
					break;
				case Keys.D6:
					entityCount = Math.Max(4, entityCount / 2);
					drones.SetEntityCount(entityCount);
					SetShape(shape);
					break;
			}
		}
		
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			var mouseState = Mouse.GetState();
			// if (mouseState != lastMouseState) { Console.WriteLine($"mouseState.Position: {mouseState.Position}"); }
			
			if (mouseState.LeftButton	  == ButtonState.Pressed &&
			    lastMouseState.LeftButton == ButtonState.Released) {
				var position = Mouse.GetState().Position;
				var key = gui.FindButton(position);
				HandleAction(key);
			}
			lastMouseState = mouseState;
			var events = keyEvents.GetEvents();
			foreach (var ev in events) {
				HandleAction(ev);
			}
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			var world = worldMatrix.AsMatrix4x4();
			drones.UpdateTransforms(deltaTime, world);
			camera.UpdateCamera();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);
			
			// --- 3D scene
			// reset states changed by spriteBatch.Begin();
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			
			triangle.Draw(this, camera);

			// DrawModel(cube, worldMatrix, camera);
			// var world = worldMatrix + Matrix.CreateTranslation(new Vector3(5,0,0));
			// DrawModel(cube, world, camera);
			DrawEntities(cube, camera);
			
			gui.Draw(GraphicsDevice, entityCount);
			base.Draw(gameTime);
		}

		private static void DrawModel(Model model, in Matrix world, in Camera camera)
		{
			foreach (ModelMesh mesh in model.Meshes) {
				foreach (BasicEffect effect in mesh.Effects) {
					effect.EnableDefaultLighting();
					effect.View			= camera.viewMatrix;
					effect.World		= world;
					effect.Projection	= camera.projectionMatrix;
					mesh.Draw();
				}
			}
		}
		
		private void DrawEntities(Model model, Camera camera)
		{
			foreach (var (transforms, _) in drones.transQuery.Chunks)
			{
				foreach (ref var transform in transforms.Span)
				{
					foreach (ModelMesh mesh in model.Meshes)
					{
						foreach (BasicEffect effect in mesh.Effects) {
							effect.EnableDefaultLighting();
							effect.View			= camera.viewMatrix;
							effect.World		= transform.value;
							effect.Projection	= camera.projectionMatrix;
							mesh.Draw();
						}
					}
				}
			}
		}
	}
}
