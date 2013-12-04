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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Toon me;
        Toon him;

        Host host;
        Client client;
        
        enum States { start, playing }
        States state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            me = new Toon(Content.Load<Texture2D>("tank"), new Vector2(100, 100));
            him = new Toon(Content.Load<Texture2D>("bullet"), new Vector2(300, 300));

            state = States.start;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (state == States.start)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                {
                    client = new Client(me, him);
                    state = States.playing;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    host = new Host(me, him);
                    state = States.playing;
                }
            }
            else if (state == States.playing)
            {
                if (client == null)
                {
                    host.sendData();
                }
                else
                {
                    client.sendData();
                }
                me.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (state == States.start)
            {
                //draw texture to say choose host or client
            }
            else
            {
                me.Draw(spriteBatch);
                him.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
