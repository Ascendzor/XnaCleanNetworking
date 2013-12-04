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


namespace NetworkPlay
{
    class Toon
    {
        private Texture2D texture;
        private Vector2 position;
        private float movementSpeed;

        public Toon(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            this.movementSpeed = 10f;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Y -= movementSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= movementSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Y += movementSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += movementSpeed;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
