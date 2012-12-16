using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare25
{
    class Dude
    {
        private Texture2D Texture;
        public Vector2 Position { get; set; }
        private Random random;
        public double happy { get; set; }
        private double disp; //how fast happy decreases
        public double dispLight { get; set; }
        public double dispWind { get; set; }
        private int frames;
        private int posInc;
        private Cloud Player;
        public bool inCrowd { get; set; }
        public float rotation { get; set; }
        private int jumper;
        
        public Dude(Texture2D texture, Random PassRandom, Cloud player)
        {
            Texture = texture;
            random = PassRandom;
            happy = 100;
            int WindRain = random.Next(3);
            if (WindRain == 0)
            {
                dispLight = (random.NextDouble() + 0.1f) * 2;
                dispWind = 0;
            }
            else
            {
                dispLight = 0;
                dispWind = (random.NextDouble() + 0.1f) * 2;
            }
            disp = (random.NextDouble() + 0.1f) * 2;
            Position = new Vector2(10 + (float)(random.NextDouble() * 200.0f), 460 + (float)random.NextDouble() * 8);
            frames = 0;
            posInc = 200;
            Player = player;
            inCrowd = true;
            rotation = 0;
            jumper = 0;
        }

        public void blowOver()
        {
            rotation = (float)Math.PI * (3.0f/2.0f);
            Position = new Vector2(Position.X, Position.Y + 16);
            happy -= 30;
            if (happy < 0)
            {
                happy = 0;
            }
        }

        public int Update()
        {
            if (frames == 15)
            {
                if (rotation != 0 && random.Next(4) == 1) //get up from wind
                {
                    rotation = 0;
                    Position = new Vector2(Position.X, Position.Y - 16); 
                }

                //jump if doing nowt
                if (jumper != 0 || (happy > 50 && random.Next(4) == 1 && posInc == 0))
                {
                    if (jumper == 0 || jumper == 1)
                    {
                        Position = new Vector2(Position.X, Position.Y - 8);
                        jumper += 1;
                    }
                    else if (jumper == 2)
                    {
                        Position = new Vector2(Position.X, Position.Y + 8);
                        jumper += 1;
                    }
                    else
                    {
                        Position = new Vector2(Position.X, Position.Y + 8);
                        jumper = 0;
                    }
                }
                //reset frame counter
                frames = 0;
                //decrease happiness
                if (happy > 0 && (Player.isRaining || Player.isWind || Player.isLightening) && inCrowd)
                {
                    if (Player.isRaining)
                    {
                        happy -= (disp * 2);
                    }
                    if (Player.isWind)
                    {
                        happy -= (dispWind * 3);
                    }
                    if (Player.isLightening)
                    {
                        happy -= (dispLight * 4);
                    }
                    if (happy < 0)
                    {
                        happy = 0;
                    }
                }
                //or increase when not weather
                else if (happy < 100 && !Player.isRaining && !Player.isWind && !Player.isLightening && inCrowd)
                {
                    happy += 1;
                    if (happy > 100)
                    {
                        happy = 100;
                    }
                }
            }
            else
            {
                frames += 1; //increase framecounter
                if (happy > 0 && posInc > 0 && Position.X > -16 && inCrowd && rotation == 0) //move dude right till in position
                {
                    Position = new Vector2(Position.X + random.Next(2) + 1, Position.Y + (int)random.NextDouble());
                    posInc -= 1;
                }
                else if ((happy < 1 || !inCrowd) && Position.X > -16 && rotation == 0) //if unhappy move off screen
                {
                    Position = new Vector2(Position.X - 1, Position.Y);
                    posInc += random.Next(2);
                }
                else if (happy < 1 && Position.X <= -16 && inCrowd) //if off screen remove from crowd
                {
                    inCrowd = false;
                    happy = 0;
                    return -1;
                }
            }
            return 0;
        }

        public void Draw(SpriteBatch spriteBatch, Color sky)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, sky,
                rotation, origin, 1, SpriteEffects.None, 0f);
        }

    }
}
