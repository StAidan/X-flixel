using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    public class FlxLogoPixel : FlxSprite
    {
        private List<Color> _layers = new List<Color>();
        private int _curlayer;
        private Rectangle rctSrc = new Rectangle(1, 1, 1, 1);

        public FlxLogoPixel(int xPos, int yPos, int pixelSize, int index, Color finalColor)
            : base(xPos, yPos)
        {
            createGraphic(pixelSize, pixelSize, Color.White);

            Color[] colors = new Color[]
            {
                new Color(255,0,0),
                new Color(0,255,0),
                new Color(0,0,255),
                new Color(255,255,0),
                new Color(0,255,255)
            };

            _layers.Add(finalColor);
            for (int i = 0; i < colors.Length; i++)
            {
                _layers.Add(colors[index]);
                if (++index >= colors.Length) index = 0;
            }
            _curlayer = _layers.Count - 1;

        }

        public override void update()
        {
            if (_curlayer == 0)
                return;

            if (_layers[_curlayer].A >= 25)
            {
                _layers[_curlayer] = new Color(_layers[_curlayer].R, _layers[_curlayer].G, _layers[_curlayer].B, _layers[_curlayer].A - 25);
            }
            else
            {
                _layers[_curlayer] = new Color(_layers[_curlayer].R, _layers[_curlayer].G, _layers[_curlayer].B, 0);
                _curlayer--;
            }
        }

        public override void render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= _curlayer; i++)
            {
                spriteBatch.Draw(_tex, new Rectangle((int)x, (int)y, (int)width, (int)height),
                    rctSrc, _layers[i]);
            }
        }
    }
}
