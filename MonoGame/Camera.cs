// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public struct Camera
{
    internal    Vector3		target;
    private 	Vector3		position;
    internal    Matrix		projectionMatrix;	// Convert 3D to 2d to render on screen (camera lens)
    internal	Matrix		viewMatrix;			// camera location & orientation
    private		bool		orbit;
    
    internal void Initialize(GraphicsDevice device)
    {
        target      = new Vector3(0, 0,   0);
        position    = new Vector3(0, 40, -100);
        orbit       = true;
			
        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45),
            device.DisplayMode.AspectRatio, 1, 1000); // view frustum - near plane 1, far plane 1000
			
        viewMatrix	= Matrix.CreateLookAt(position, target, Vector3.Up); // Vector3.Up: (0, 1, 0)
    }
    
    internal void UpdateCamera()
    {
        var keyboard = Keyboard.GetState();
        if (keyboard.IsKeyDown(Keys.Left)) {
            position.X	-= 1;
            target.X    -= 1;
        }
        if (keyboard.IsKeyDown(Keys.Right)) {
            position.X	+= 1;
            target.X	+= 1;
        }
        if (keyboard.IsKeyDown(Keys.Up)) {
            position.Y	-= 1;
            target.Y	-= 1;
        }
        if (keyboard.IsKeyDown(Keys.Down)) {
            position.Y	+= 1;
            target.Y	+= 1;
        }
        if (keyboard.IsKeyDown(Keys.OemPlus)) {
            position.Z	-= 1;
        }
        if (keyboard.IsKeyDown(Keys.OemMinus)) {
            position.Z	+= 1;
        }
        if (keyboard.IsKeyDown(Keys.Space)) {
            orbit = !orbit;
        }
        if (orbit) {
            Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1));
            position = Vector3.Transform(position, rotationMatrix);
        }
        viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up);
    }
}
