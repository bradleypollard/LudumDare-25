using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare25
{
    class Cloud
    {
        public bool isRaining { get; set; }
        public bool isThunder { get; set; }
        public bool isLightening { get; set; }
        private Texture2D rain;
        private Texture2D sun;
        private Texture2D storm;

        public Cloud(List<Texture2D> clouds)
        {
            isRaining = false;
            isThunder = false;
            isLightening = false;

            sun = clouds[0];
            rain = clouds[1];
            storm = clouds[2];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (isLightening || isThunder)
            {
                spriteBatch.Draw(storm, new Vector2(170, 0), Color.White);
            }
            else if (isRaining)
            {
                spriteBatch.Draw(rain, new Vector2(170, 0), Color.White);
            }
            else
            {
                spriteBatch.Draw(sun, new Vector2(170, 0), Color.White);
            }
            spriteBatch.End();
        }

    }
}
