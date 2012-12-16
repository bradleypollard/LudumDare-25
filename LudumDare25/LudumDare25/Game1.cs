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
        private int directionX;
        private int directionTitle;
        private int directionEnd;
        private int directionY;
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
        private List<SoundEffect> thunder;
        private SoundEffectInstance rain;
        private SoundEffectInstance wind;
        private double score;
        private double timeCounter;
        private bool title;
        private Texture2D TitleScreen;
        private Texture2D titleBG;
        ParticleEngine titleEngine;
        private bool gameOver;
        private Texture2D End;
        private Texture2D EndBG;
        LeafEngine endEngine;
        Song song;
        TimeSpan songLength;
        private Color sky;
        private SpriteFont font2;
        private List<Texture2D> speech;
        private int[] speechIndex = new int[2];
        private bool speechDrawn;
        private double speechCounter;
        private int splash;
        private Texture2D splashScreen;

        public Ludo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "The ''Great'' British Weather";
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
            titleEngine = new ParticleEngine(textures, new Vector2(400, -10));
            textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("leaf"));
            leafEngine = new LeafEngine(textures, new Vector2(860, 200));
            endEngine = new LeafEngine(textures, new Vector2(860, 200));

            //text and other var
            font = Content.Load<SpriteFont>("Font1");
            font2 = Content.Load<SpriteFont>("Font2");
            BG = Content.Load<Texture2D>("BG");
            lightening = 0;
            light = Content.Load<Texture2D>("Light1");
            dudeHit = -1;
            bar0 = Content.Load<Texture2D>("Bar/Bar00");
            timeCounter = 0;
            score = 0;
            directionX = -10;
            directionY = 10;
            directionTitle = -20;
            directionEnd = 20;
            title = true;
            TitleScreen = Content.Load<Texture2D>("TitleScreen2");
            titleBG = Content.Load<Texture2D>("TitleBG");
            splashScreen = Content.Load<Texture2D>("Splash");
            gameOver = false;
            End = Content.Load<Texture2D>("GameOver1");
            EndBG = Content.Load<Texture2D>("GameOverBG");
            sky = Color.White;
            splash = 1;

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

            //audio
            thunder = new List<SoundEffect>();
            thunder.Add(Content.Load<SoundEffect>("Sound/Thunder1"));
            thunder.Add(Content.Load<SoundEffect>("Sound/Thunder2"));
            thunder.Add(Content.Load<SoundEffect>("Sound/Thunder3"));
            rain = Content.Load<SoundEffect>("Sound/Rain").CreateInstance();
            rain.IsLooped = true;
            rain.Play();
            wind = Content.Load<SoundEffect>("Sound/Wind").CreateInstance();
            wind.IsLooped = true;
            wind.Volume = 0.7f;
            song = Content.Load<Song>("Parity_Bit");
            songLength = song.Duration;

            //speech
            speechIndex[0] = -1;
            speechIndex[1] = 0;
            speech = new List<Texture2D>();
            speech.Add(Content.Load<Texture2D>("Speech/SpeechHappy"));
            speech.Add(Content.Load<Texture2D>("Speech/SpeechSad"));
            speech.Add(Content.Load<Texture2D>("Speech/SpeechRain"));
            speech.Add(Content.Load<Texture2D>("Speech/SpeechLight"));
            speech.Add(Content.Load<Texture2D>("Speech/SpeechWind"));
            speechDrawn = false;
            speechCounter = 0;

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
            if (title)
            {
                if (oldState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter))
                {
                    if (splash == 1)
                    {
                        splash = 2;
                    }
                    else
                    {
                        splash = 0;
                        title = false;
                        rain.Stop();
                        MediaPlayer.Play(song);
                    }
                }
            }
            else if (gameOver)
            {
                if (oldState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter))
                {
                    wind.Stop();
                    Reset();
                }
            }
            else
            {
                if (oldState.IsKeyUp(Keys.R) && newState.IsKeyDown(Keys.R))
                {
                    if (player.isRaining)
                    {
                        player.isRaining = false;
                        rain.Stop();
                        if (!player.isLightening)
                        {
                            sky = Color.White;
                        }
                    }
                    else
                    {
                        player.isRaining = true;
                        rain.Play();
                        if (!player.isLightening)
                        {
                            sky = Color.LightGray;
                        }
                    }
                }
                if (oldState.IsKeyUp(Keys.S) && newState.IsKeyDown(Keys.S))
                {
                    if (player.isLightening)
                    {
                        player.isLightening = false;
                        if (!player.isRaining)
                        {
                            sky = Color.White;
                        }
                    }
                    else
                    {
                        player.isLightening = true;
                        sky = Color.Gray;
                    }
                }
                if (oldState.IsKeyUp(Keys.W) && newState.IsKeyDown(Keys.W))
                {
                    if (player.isWind)
                    {
                        player.isWind = false;
                        wind.Stop();
                    }
                    else
                    {
                        player.isWind = true;
                        wind.Play();
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

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

            //check if game has eneded
            if ((crowd.count == 0 || MediaPlayer.PlayPosition.TotalSeconds >= songLength.TotalSeconds - 1) && !gameOver)
            {
                gameOver = true;
                rain.Stop();
                MediaPlayer.Stop();
                player.isRaining = false;
                player.isLightening = false;
                player.isWind = false;
                wind.Play();
            }

            UpdateInput();

            if (gameOver)
            {
                if (endEngine.EmitterLocation.Y > 300)
                {
                    directionEnd = -20;
                }
                else if (endEngine.EmitterLocation.Y < 50)
                {
                    directionEnd = 20;
                }
                endEngine.EmitterLocation = new Vector2(endEngine.EmitterLocation.X, endEngine.EmitterLocation.Y + directionEnd);

                endEngine.Update(true);
            }
            if (title)
            {
                if (titleEngine.EmitterLocation.X > 800)
                {
                    directionTitle = -20;
                }
                else if (titleEngine.EmitterLocation.X < 0)
                {
                    directionTitle = 20;
                }
                titleEngine.EmitterLocation = new Vector2(titleEngine.EmitterLocation.X + directionTitle, titleEngine.EmitterLocation.Y);

                titleEngine.Update(false, true);
            }
            else
            {
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
                    sky = Color.LightGray;
                }

                particleEngine.Update(player.isWind, player.isRaining); //update rain

                if (player.isLightening && dudeHit == -1) //try to attack a dude if one isn't hit
                {
                    dudeHit = crowd.Lightening(thunder);
                    if (dudeHit != -1)
                    {
                        lightening = 1;
                    }
                    sky = Color.Gray;
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

                int[] temp = speechIndex; //handle speech 
                speechIndex = crowd.Update(player); //update crowd
                if (speechDrawn)
                {
                    speechIndex = temp;
                }

                if (!gameOver)
                {
                    if (timeCounter % 60 == 0) //update scorer
                    {
                        score += crowd.happy;
                    }
                    timeCounter += 1;
                }

                if (lightening > 0)
                {
                    sky = Color.White;
                }
            }

            base.Update(gameTime);
        }

        public void Reset()
        {
            lightening = 0;
            dudeHit = -1;
            timeCounter = 0;
            score = 0;
            directionX = -10;
            directionTitle = -20;
            directionEnd = 20;
            directionY = 10;
            wind.Stop();
            rain.Stop();
            player = new Cloud(clouds);
            crowd = new Crowd(50, dudes, player);
            title = true;
            gameOver = false;
            speechIndex[0] = -1;
            speechIndex[1] = 0;
            speechCounter = 0;
            speechDrawn = false;
            rain.Play();
            sky = Color.White;
        }

        public string GetTime(TimeSpan timeSpan)
        {
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;

            if (seconds < 10)
                return minutes + ":0" + seconds;
            else
                return minutes + ":" + seconds;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);

            if (title) //different render for titlescreen
            {
                spriteBatch.Begin();
                spriteBatch.Draw(titleBG, new Vector2(0, 0), Color.White);
                spriteBatch.End();
                titleEngine.Draw(spriteBatch);
                spriteBatch.Begin();
                if (splash == 2)
                {
                    spriteBatch.Draw(splashScreen, new Vector2(0, 0), Color.White);
                    //intro
                    string intro = "You are an every-day villain that any British person will know well: the weather!";
                    spriteBatch.DrawString(font, intro, new Vector2(20, 90), Color.White);
                    intro = "Your objective is to make the crowd of festival goers have the worst time possible - ";
                    spriteBatch.DrawString(font, intro, new Vector2(20, 120), Color.White);
                    intro = "without making them leave before the end of the band's set!";
                    spriteBatch.DrawString(font, intro, new Vector2(20, 150), Color.White);
                    intro = "You can't have any fun if no-ones around, right?";
                    spriteBatch.DrawString(font, intro, new Vector2(20, 180), Color.White);
                    intro = "To achieve this you have the power of rain, wind and thunder storms on your side:";
                    spriteBatch.DrawString(font, intro, new Vector2(20, 210), Color.White);
                    //desc
                    intro = "Everyone dislikes rain. Not";
                    spriteBatch.DrawString(font, intro, new Vector2(5, 320), new Color(122,154,209));
                    intro = "as much as other conditions,";
                    spriteBatch.DrawString(font, intro, new Vector2(5, 350), new Color(122, 154, 209));
                    intro = "but enough to ruin their day.";
                    spriteBatch.DrawString(font, intro, new Vector2(5, 380), new Color(122, 154, 209));
                    intro = "A few people hate storms. Really";
                    spriteBatch.DrawString(font, intro, new Vector2(255, 320), new Color(255, 194, 14));
                    intro = "hate them. And if someone is hit";
                    spriteBatch.DrawString(font, intro, new Vector2(255, 350), new Color(255, 194, 14));
                    intro = "by lightning, they're gone!";
                    spriteBatch.DrawString(font, intro, new Vector2(255, 380), new Color(255, 194, 14));
                    intro = "A lot of people can't stand the";
                    spriteBatch.DrawString(font, intro, new Vector2(540, 320), new Color(157, 187, 97));
                    intro = "wind. In fact, if someone is";
                    spriteBatch.DrawString(font, intro, new Vector2(540, 350), new Color(157, 187, 97));
                    intro = "blown over, they get real mad!";
                    spriteBatch.DrawString(font, intro, new Vector2(540, 380), new Color(157, 187, 97));
                }
                else
                {
                    spriteBatch.Draw(TitleScreen, new Vector2(0, 0), Color.White);
                }
                spriteBatch.End();
            }
            else if (gameOver)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(EndBG, new Vector2(0, 0), Color.White); //draw bg
                spriteBatch.End();
                endEngine.Draw(spriteBatch, Color.White); //draw leaves
                spriteBatch.Begin();
                double finalscore = 1000 * crowd.count * (1 / (score / (timeCounter / 60)));//calc score
                string endLabel = "Number of people left: " + (int)crowd.count;
                spriteBatch.DrawString(font, endLabel, new Vector2(285, 250), Color.Black);
                endLabel = "Average unhappiness: " + (int)(score / (timeCounter / 60));
                spriteBatch.DrawString(font, endLabel, new Vector2(280, 270), Color.Black);
                endLabel = "You scored: " + (int)finalscore;
                spriteBatch.DrawString(font2, endLabel, new Vector2(265, 310), Color.Gold);
                if (crowd.count == 0)
                {
                    endLabel = "You scared everyone away! You can't have any fun like that.";
                    spriteBatch.DrawString(font, endLabel, new Vector2(120, 350), Color.Black);
                }
                else if (finalscore < 1000 && (score / (timeCounter / 60) > 40))
                {
                    endLabel = "The crowd was way too happy! Remember, rain makes everyone unhappy.";
                    spriteBatch.DrawString(font, endLabel, new Vector2(90, 350), Color.Black);
                }
                else if (finalscore < 1000)
                {
                    endLabel = "Your score is almost as bad as the weather! Try to scare less people off.";
                    spriteBatch.DrawString(font, endLabel, new Vector2(70, 350), Color.Black);
                }
                else if (finalscore < 2000)
                {
                    endLabel = "Not totally awful. Note that if wind knocks someone down, they get really unhappy!";
                    spriteBatch.DrawString(font, endLabel, new Vector2(30, 350), Color.Black);
                }
                else if (finalscore < 2500)
                {
                    endLabel = "Better - try to keep as many people at the gig for a higher multiplier!";
                    spriteBatch.DrawString(font, endLabel, new Vector2(90, 350), Color.Black);
                }
                else if (finalscore < 3000)
                {
                    endLabel = "You nearly ruined everyone's day - awesome! Lightning can remove someone instantly.";
                    spriteBatch.DrawString(font, endLabel, new Vector2(30, 350), Color.Black);
                }
                else
                {
                    endLabel = "Now that's what I call British weather! Everyone had a terrible time.";
                    spriteBatch.DrawString(font, endLabel, new Vector2(110, 350), Color.Black);
                }
                endLabel = "Press enter to restart, or escape to quit.";
                spriteBatch.DrawString(font, endLabel, new Vector2(10, 470), Color.IndianRed);
                endLabel = "Bradley Pollard 2012";
                spriteBatch.DrawString(font, endLabel, new Vector2(610, 470), Color.IndianRed);
                spriteBatch.Draw(End, new Vector2(0, 0), Color.White); //draw overlay
                spriteBatch.End();
            }
            else
            {
                //render bg
                spriteBatch.Begin();
                spriteBatch.Draw(BG, new Vector2(0, 0), sky);
                spriteBatch.End();

                //render classes
                particleEngine.Draw(spriteBatch);
                leafEngine.Draw(spriteBatch, sky);
                crowd.Draw(spriteBatch, sky);
                player.Draw(spriteBatch);

                spriteBatch.Begin();

                //fps
                float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
                string fps = "FPS: " + frameRate;
                spriteBatch.DrawString(font, fps, new Vector2(10, 10), Color.Black);

                //happy bar
                string happyLabel = "Happiness: " + (int)crowd.happy + "%";
                spriteBatch.DrawString(font, happyLabel, new Vector2(510, 10), Color.Black);
                if (crowd.happy > 0)
                {
                    spriteBatch.Draw(bars[(int)(crowd.happy - 0.1) / 10], new Vector2(660, 10), Color.White);
                }
                else
                {
                    spriteBatch.Draw(bar0, new Vector2(660, 10), Color.White);
                }

                //people remaining
                string peopleLabel = "People left: " + crowd.count;
                spriteBatch.DrawString(font, peopleLabel, new Vector2(665, 30), Color.Black);

                //time remaining
                string timeleft = "Time left: " + GetTime(songLength.Subtract(MediaPlayer.PlayPosition));
                spriteBatch.DrawString(font, timeleft, new Vector2(667, 50), Color.Black);

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

                //speech
                if (speechIndex[0] != -1 || speechDrawn)
                {
                    if (!speechDrawn)
                    {
                        speechCounter = timeCounter;
                    }
                    spriteBatch.Draw(speech[speechIndex[0]], new Vector2(speechIndex[1], 350), Color.White);
                    speechDrawn = true;
                    if (timeCounter >= speechCounter + 60)
                    {
                        speechDrawn = false;
                        speechCounter = 0;
                    }
                }

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
