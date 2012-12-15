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

        public Crowd(int number, List<Texture2D> dudes, Cloud player)
        {
            Number = number;
            Dudes = dudes;
            random = new Random();
            happy = 100;
            randDudes = new List<Dude>();

            for (int i = 0; i < Number; i++)
            {
                randDudes.Add(new Dude(Dudes.ElementAt((int)(random.NextDouble() * Dudes.Count)), random, player));
            }
        }

        public void Update()
        {
            happy = 0;
            for (int i = 0; i < randDudes.Count; i++)
            {
                randDudes[i].Update();
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
