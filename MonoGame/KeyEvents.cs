// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class KeyEvents
{
    private readonly    KeyEvent[]  events  = new KeyEvent[256];
    private readonly    List<Keys>  clicked = new List<Keys>();
    
    
    public IEnumerable<Keys> GetEvents()
    {
        var keyState = Keyboard.GetState();
        clicked.Clear();
        for (int n = 1; n < 256; n++)
        {
            ref var ev = ref events[n];
            var key = (Keys)n;
            var isDown = keyState.IsKeyDown(key);
            if (isDown && !ev.isDown) {
                clicked.Add(key);
            }
            ev.isDown = isDown;
        }
        return clicked;
    }
}

public struct KeyEvent {
    internal bool isDown;
}