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
        
        public Dude(Texture2D texture, Random PassRandom, Cloud player)
        {
            Texture = texture;
            random = PassRandom;
            happy = 100;
            disp = random.NextDouble() + 0.1f;
            Position = new Vector2(10 + (float)(random.NextDouble() * 200.0f), 440 + (float)random.NextDouble() * 8);
            frames = 0;
            posInc = 200;
            Player = player;
        }

        public void Update()
        {
            if (frames == 15)
            {
                frames = 0;
                if (happy > 0 && Player.isRaining)
                {
                    happy -= (disp * 2);
                }
                else if (happy < 100 && !Player.isRaining && !Player.isThunder && !Player.isLightening)
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
                if (happy > 0 && posInc > 0) //move dude right till in position
                {
                    Position = new Vector2(Position.X + random.Next(2) + 1, Position.Y + (int)random.NextDouble());
                    posInc -= 1;
                }
                else if (happy < 1 && Position.X > -16) //if unhappy move off screen
                {
                        Position = new Vector2(Position.X - 1, Position.Y);
                }
            }
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
