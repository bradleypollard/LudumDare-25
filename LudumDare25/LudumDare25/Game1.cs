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

namespace LudumDare25
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Ludo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ParticleEngine particleEngine;
        Crowd crowd;
        private int direction = 10;
        private SpriteFont font;
        private Texture2D rainCloud;
        private List<Texture2D> dudes;
        private List<Texture2D> bars;
        private List<Texture2D> clouds;
        Cloud player;

        public Ludo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "The British Weather";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("circle"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 150));
            font = Content.Load<SpriteFont>("Font1");
            rainCloud = Content.Load<Texture2D>("RainCloud");

            //happy bar
            bars = new List<Texture2D>();
            bars.Add(Content.Load<Texture2D>("Bar/Bar10"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar20"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar30"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar40"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar50"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar60"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar70"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar80"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar90"));
            bars.Add(Content.Load<Texture2D>("Bar/Bar100"));

            //cloud textures
            clouds = new List<Texture2D>();
            clouds.Add(Content.Load<Texture2D>("SunCloud"));
            clouds.Add(Content.Load<Texture2D>("RainCloud"));
            clouds.Add(Content.Load<Texture2D>("StormCloud"));
            player = new Cloud(clouds);

            //construct crowd
            dudes = new List<Texture2D>();
            dudes.Add(Content.Load<Texture2D>("Dude/Dude1"));
            dudes.Add(Content.Load<Texture2D>("Dude/Dude2"));
            dudes.Add(Content.Load<Texture2D>("Dude/Dude3"));
            dudes.Add(Content.Load<Texture2D>("Dude/Dude4"));
            dudes.Add(Content.Load<Texture2D>("Dude/Dude5"));
            crowd = new Crowd(50, dudes, player);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (player.isRaining)
            {
                if (particleEngine.EmitterLocation.X > 500)
                {
                    direction = -10;
                }
                else if (particleEngine.EmitterLocation.X < 200)
                {
                    direction = 10;
                }
                particleEngine.EmitterLocation = new Vector2(particleEngine.EmitterLocation.X + direction, particleEngine.EmitterLocation.Y);
                particleEngine.Update();
            }

            crowd.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            if (player.isRaining)
            {
                particleEngine.Draw(spriteBatch);
            }
            crowd.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.Begin();
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            string fps = "FPS: " + crowd.happy;
            spriteBatch.DrawString(font, fps, new Vector2(10, 10), Color.Black);
            

            string happyLabel = "Happiness: ";
            spriteBatch.DrawString(font, happyLabel, new Vector2(550, 10), Color.Black);
            spriteBatch.Draw(bars[(int)(crowd.happy - 0.1) / 10], new Vector2(650, 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
