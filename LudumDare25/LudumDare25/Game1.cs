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
        private int directionX = -10;
        private int directionY = 10;
        private SpriteFont font;
        private Texture2D BG;
        private List<Texture2D> dudes;
        private List<Texture2D> bars;
        private List<Texture2D> clouds;
        Cloud player;
        KeyboardState oldState;
        private int lightening;
        private float dudeHit;
        private Texture2D light;
        LeafEngine leafEngine;
        private Texture2D bar0;

        public Ludo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "The British Weather";
            graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 500;   // set this value to the desired height of your window
            graphics.ApplyChanges();
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
            oldState = Keyboard.GetState();

            //cloud textures
            clouds = new List<Texture2D>();
            clouds.Add(Content.Load<Texture2D>("SunCloud"));
            clouds.Add(Content.Load<Texture2D>("RainCloud"));
            clouds.Add(Content.Load<Texture2D>("StormCloud"));
            player = new Cloud(clouds);

            //engines
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("circle"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 150));
            textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("leaf"));
            leafEngine = new LeafEngine(textures, new Vector2(860, 200));

            //text and other var
            font = Content.Load<SpriteFont>("Font1");
            BG = Content.Load<Texture2D>("BG");
            lightening = 0;
            light = Content.Load<Texture2D>("Light1");
            dudeHit = -1;
            bar0 = Content.Load<Texture2D>("Bar/Bar00");

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

        private void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();

            if (oldState.IsKeyUp(Keys.R) && newState.IsKeyDown(Keys.R))
            {
                if (player.isRaining)
                {
                    player.isRaining = false;
                }
                else
                {
                    player.isRaining = true;
                }
            }
            if (oldState.IsKeyUp(Keys.S) && newState.IsKeyDown(Keys.S))
            {
                if (player.isLightening)
                {
                    player.isLightening = false;
                }
                else
                {
                    player.isLightening = true;
                }
            }
            if (oldState.IsKeyUp(Keys.W) && newState.IsKeyDown(Keys.W))
            {
                if (player.isWind)
                {
                    player.isWind = false;
                }
                else
                {
                    player.isWind = true;
                }
            }

            oldState = newState;
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

            UpdateInput();

            if (player.isRaining) //calc emmiter pos
            {
                if (particleEngine.EmitterLocation.X > 500)
                {
                    directionX = -10;
                }
                else if (particleEngine.EmitterLocation.X < 200)
                {
                    directionX = 10;
                }
                particleEngine.EmitterLocation = new Vector2(particleEngine.EmitterLocation.X + directionX, particleEngine.EmitterLocation.Y);
                
            }

            particleEngine.Update(player.isWind, player.isRaining); //update rain

            if (player.isLightening && dudeHit == -1) //try to attack a dude if one isn't hit
            {
                dudeHit = crowd.Lightening();
                if (dudeHit != -1)
                {
                    lightening = 1;
                }
            }

            if (player.isWind) // update wind pos
            {
                if (leafEngine.EmitterLocation.Y > 250)
                {
                    directionY = -10;
                }
                else if (leafEngine.EmitterLocation.Y < 150)
                {
                    directionY = 10;
                }
                leafEngine.EmitterLocation = new Vector2(leafEngine.EmitterLocation.X, leafEngine.EmitterLocation.Y + directionY);
                
                crowd.Wind();
            }

            leafEngine.Update(player.isWind); //update wind

            crowd.Update(); //update crowd

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            
            //render bg
            spriteBatch.Begin();
            spriteBatch.Draw(BG, new Vector2(0,0), Color.White);
            spriteBatch.End();

            //render classes
            particleEngine.Draw(spriteBatch);
            leafEngine.Draw(spriteBatch);
            crowd.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.Begin();

            //fps
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            string fps = "FPS: " + frameRate;
            spriteBatch.DrawString(font, fps, new Vector2(10, 10), Color.Black);

            //happy bar
            string happyLabel = "Happiness: ";
            spriteBatch.DrawString(font, happyLabel, new Vector2(550, 10), Color.Black);
            if (crowd.happy > 0)
            {
                spriteBatch.Draw(bars[(int)(crowd.happy - 0.1) / 10], new Vector2(650, 10), Color.White);
            }
            else
            {
                spriteBatch.Draw(bar0, new Vector2(650, 10), Color.White);
            }

            //people remaining
            string peopleLabel = "People left: " + crowd.count;
            spriteBatch.DrawString(font, peopleLabel, new Vector2(550, 30), Color.Black);

            if (lightening > 6) //draw lightening strikes
            {
                lightening = 0;
                dudeHit = -1;
            }
            else if (lightening > 0)
            {
                for (int i = 0; i < lightening; i++)
                {
                    spriteBatch.Draw(light, new Vector2(dudeHit, 200 + 32 * i), Color.White);
                }
                lightening += 1;
            }
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
