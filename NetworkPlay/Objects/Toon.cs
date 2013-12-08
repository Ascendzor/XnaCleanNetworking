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

namespace Objects
{
    [Serializable()]
    public class Toon
    {
        public Guid id;
        private Vector2 position;
        private float movementSpeed;
        private Vector2 goalPosition;

        public Toon(Vector2 position)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.goalPosition = position;

            this.movementSpeed = 10f;
        }

        public void SubmitEvent(Event leEvent)
        {
            goalPosition = (Vector2)leEvent.value;
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

            Vector2 direction = goalPosition - position;
            if (direction.Length() > 10)
            {
                Console.WriteLine("direction: " + direction);
                direction.Normalize();

                position += direction * movementSpeed;
            }
        }

        public void Draw(SpriteBatch sb, Texture2D textureGiven)
        {
            sb.Draw(textureGiven, position, Color.White);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetGoal(Vector2 goal)
        {
            goalPosition = goal;
        }

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
