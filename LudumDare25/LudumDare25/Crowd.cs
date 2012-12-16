using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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

        public float Lightening(List<SoundEffect> thunder)
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
                thunder[random.Next(3)].Play();
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
                } while (!randDudes[hit].inCrowd && randDudes[hit].rotation != 0);
                randDudes[hit].blowOver();
            }
        }

        public int[] Update(Cloud player)
        {
            happy = 0;
            for (int i = 0; i < randDudes.Count; i++)
            {
                count += randDudes[i].Update();
                happy += randDudes[i].happy;
            }
            happy /= randDudes.Count;

            int[] array = new int[2];
            if (random.Next(240) == 120) //speech
            {
                Dude temp = randDudes[random.Next(randDudes.Count)];
                array[1] = (int)temp.Position.X;
                if (!temp.inCrowd)
                {
                    array[0] = -1;
                    return array;
                }
                if (temp.happy > 50)
                {
                    array[0] = 0;
                    return array;
                }
                else if (temp.dispWind == 0 && player.isLightening)
                {
                    array[0] = 3;
                    return array;
                }
                else if (temp.dispLight == 0 && player.isWind)
                {
                    array[0] = 4;
                    return array;
                }
                else if (player.isRaining)
                {
                    array[0] = 2;
                    return array;
                }
                else if (temp.happy < 50)
                {
                    array[0] = 1;
                    return array;
                }
                else
                {
                    array[0] = -1;
                    return array;
                }
            }
            else
            {
                array[0] = -1;
                array[1] = 0;
                return array;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color sky)
        {
            spriteBatch.Begin();
            for (int i = 0; i < randDudes.Count; i++)
            {
                randDudes[i].Draw(spriteBatch, sky);
            }
            spriteBatch.End();
        }
    }
}
