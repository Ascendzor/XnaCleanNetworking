using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Objects;

namespace NetworkPlay
{
    class Input
    {
        Client eventTaker;
        Toon me;

        private bool isMouseLeftDown;
        //use a dictionary to map each key to a boolean to elegantly detect new keypresses

        public Input(Client eventTaker, Toon me)
        {
            this.eventTaker = eventTaker;
            this.me = me;
            isMouseLeftDown = false;
        }

        public void Update()
        {
            MouseInput();
        }

        public void MouseInput()
        {
            MouseState leState = Mouse.GetState();
            if (!isMouseLeftDown)
            {
                if (leState.LeftButton == ButtonState.Pressed)
                {
                    isMouseLeftDown = true;
                    Event clickEvent = new Event(me.id, 0, new Vector2(leState.X, leState.Y));
                    eventTaker.SubmitEvent(clickEvent);
                    me.SubmitEvent(clickEvent);
                }
            }
            else
            {
                if (leState.LeftButton != ButtonState.Pressed)
                {
                    isMouseLeftDown = false;
                }
            }
        }
    }
}
