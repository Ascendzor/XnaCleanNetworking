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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Input input;

        Toon me;
        public static Dictionary<Guid, Toon> them;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            me = new Toon(new Vector2(100, 100));
            them = new Dictionary<Guid, Toon>();

            IsMouseVisible = true;


            Client client = new Client();
            input = new Input(client, me);

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
            input.Update();
            foreach (Guid key in them.Keys)
            {
                them[key].Update();
            }
            me.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (Guid key in them.Keys)
            {
                them[key].Draw(spriteBatch, Content.Load<Texture2D>("tank"));
            }
            me.Draw(spriteBatch, Content.Load<Texture2D>("tank"));
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
