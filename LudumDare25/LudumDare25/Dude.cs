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
        private int frames;
        private int posInc;
        private Cloud Player;
        public bool inCrowd { get; set; }
        
        public Dude(Texture2D texture, Random PassRandom, Cloud player)
        {
            Texture = texture;
            random = PassRandom;
            happy = 100;
            disp = (random.NextDouble() + 0.1f) * 2;
            Position = new Vector2(10 + (float)(random.NextDouble() * 200.0f), 460 + (float)random.NextDouble() * 8);
            frames = 0;
            posInc = 200;
            Player = player;
            inCrowd = true;
        }

        public int Update()
        {
            if (frames == 15)
            {
                frames = 0;
                if (happy > 0 && (Player.isRaining || Player.isWind || Player.isLightening))
                {
                    if (Player.isRaining)
                    {
                        happy -= (disp * 2);
                    }
                    if (Player.isWind)
                    {
                        happy -= (disp * 3);
                    }
                    if (Player.isLightening)
                    {
                        happy -= (disp * 0);
                    }
                }
                else if (happy < 100 && !Player.isRaining && !Player.isWind && !Player.isLightening)
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
                frames += 1;
                if (happy > 0 && posInc > 0 && Position.X > -4 && inCrowd) //move dude right till in position
                {
                    Position = new Vector2(Position.X + random.Next(2) + 1, Position.Y + (int)random.NextDouble());
                    posInc -= 1;
                }
                else if ((happy < 1 || !inCrowd) && Position.X > -4) //if unhappy move off screen
                {
                    Position = new Vector2(Position.X - 1, Position.Y);
                    posInc += random.Next(2);
                }
                else if (happy < 1 && Position.X <= -4 && inCrowd)
                {
                    inCrowd = false;
                    return -1;
                }
            }
            return 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color.White,
                0, origin, 1, SpriteEffects.None, 0f);
        }

    }
}
