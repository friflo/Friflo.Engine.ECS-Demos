// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Button
{
    public readonly string   text;
    public readonly Vector2  position;
    public readonly Keys     key;
    
    public Button(string text, int x, int y, Keys key) {
        this.text = text;
        position = new Vector2(x, y);
        this.key = key;
    }
}


public class Gui
{
    private             SpriteFont	demoFont;
    private             SpriteBatch	spriteBatch;
    private             const int   ButtonWidth = 200;
    private             const int	ButtonHeight = 50;
    private readonly    Color[]	    buttonBackground = new Color[ButtonWidth * ButtonHeight];
    private             Texture2D	buttonRect;
    private             Texture2D	frifloLogo;
    private readonly    Button[]    buttons;
    
    public Gui() {
        for(int i=0; i < buttonBackground.Length; ++i) {
            buttonBackground[i] = Color.Blue;
        }
        buttons = new Button[] {
            new ("1  Plane",  10,  10, Keys.D1),
            new ("2  Cube",   10,  70, Keys.D2),
            new ("3  Ring",   10, 130, Keys.D3),
            new ("4  Plane",  10, 190, Keys.D4),
            new ("5  * 2",    10, 300, Keys.D5),
            new ("6  / 2",    10, 360, Keys.D6),
        };
    }

    public void Initialize(GraphicsDevice graphicsDevice) {
        buttonRect = new Texture2D(graphicsDevice, ButtonWidth, ButtonHeight);
        spriteBatch = new SpriteBatch(graphicsDevice);
    }

    public void LoadContent(ContentManager content) {
        demoFont = content.Load<SpriteFont>("DemoFont");
        frifloLogo = content.Load<Texture2D>("friflo-256x256");
    }

    public void Draw(GraphicsDevice graphicsDevice, int entityCount) {
        spriteBatch.Begin();
        buttonRect.SetData(buttonBackground);

        foreach (var button in buttons) {
            DrawButton(button.text, button.position);
        }
        DrawLabel ($"Entities {entityCount}", new Vector2(300, 10));
        var bounds = graphicsDevice.Viewport.Bounds;
        int x = bounds.Width  - 64;
        int y = bounds.Height - 64;
        spriteBatch.Draw(frifloLogo, new Vector2(x, y), null, Color.White, 0, default, new Vector2(0.2f, 0.2f), SpriteEffects.None, 0);
        spriteBatch.End();
    }
    
    private void DrawButton(string text, Vector2 position)
    {
        spriteBatch.Draw(buttonRect, position, Color.White);
        spriteBatch.DrawString(demoFont, text, position + new Vector2(15, 4),  Color.White);
    }
		
    private void DrawLabel(string text, Vector2 position)
    {
        spriteBatch.DrawString(demoFont, text, position + new Vector2(15, 4),  Color.Black);
    }

    public Keys FindButton(Point position)
    {
        foreach (var button in buttons) {
            if (InRect(position, button.position)) {
                return button.key;
            }
        }
        return 0;
    }
    
    private static bool InRect(Point position, Vector2 pos)
    {
        return pos.X < position.X && position.X < pos.X + ButtonWidth && 
               pos.Y < position.Y && position.Y < pos.Y + ButtonHeight;
    }
}