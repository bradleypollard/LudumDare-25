using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare25
{
    class Crowd
    {
        public double happy { get; set; }
        private int Number;
        private Random random;
        private List<Texture2D> Dudes;
        private List<Dude> randDudes;
        public int count { get; set; }

        public Crowd(int number, List<Texture2D> dudes, Cloud player)
        {
            Number = number;
            count = number;
            Dudes = dudes;
            random = new Random();
            happy = 100;
            randDudes = new List<Dude>();

            for (int i = 0; i < Number; i++)
            {
                randDudes.Add(new Dude(Dudes.ElementAt((int)(random.NextDouble() * Dudes.Count)), random, player));
            }
        }

        public float Lightening()
        {
            if (random.Next(120) == 60 && count > 5)
            {
                float pos = 0;
                int hit = 0;
                do
                {
                    hit = random.Next(randDudes.Count);
                    pos = randDudes[hit].Position.X;
                } while (pos < 170 || !randDudes[hit].inCrowd);
                randDudes[hit].happy = 0;
                randDudes[hit].inCrowd = false;
                count -= 1;
                return randDudes[hit].Position.X;
            }
            return -1;
        }

        public void Wind()
        {
            if (random.Next(30) == 15 && count != 0)
            {
                float pos = 0;
                int hit = 0;
                do
                {
                    hit = random.Next(randDudes.Count);
                    pos = randDudes[hit].Position.X;
                } while (!randDudes[hit].inCrowd);
                randDudes[hit].blowOver();
            }
        }

        public void Update()
        {
            happy = 0;
            for (int i = 0; i < randDudes.Count; i++)
            {
                count += randDudes[i].Update();
                happy += randDudes[i].happy;
            }
            happy /= randDudes.Count;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < randDudes.Count; i++)
            {
                randDudes[i].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
