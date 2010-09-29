using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    //@desc pause overlay used for when the game is inactive or
    // has been manually paused.
    public class FlxPause
    {
        private Rectangle _pauseRect = new Rectangle(0, 0, 180, 100);
        private Color _pauseColor = new Color(0, 0, 0, 0x7F);
        private int _pauseScale = 1;

        private FlxText _pauseText;

        private Texture2D _imgKeyX;
        private string _strKeyX = "A Button";
        private Vector2 _posKeyX = new Vector2(4, 36);
        private Texture2D _imgKeyC;
        private string _strKeyC = "B Button";
        private Vector2 _posKeyC = new Vector2(4, 36 + 14);
        private Texture2D _imgKeyMouse; // not used (yet)
        private string _strKeyMouse = "Mouse";
        private Vector2 _posKeyMouse = new Vector2(4, 36 + 14 + 14);
        private Texture2D _imgKeysArrows;
        private string _strKeysArrows = "Move";
        private Vector2 _posKeysArrows = new Vector2(4, 36 + 14 + 14 + 14);
        private Texture2D _imgKeyMinus;
        private string _strKeyMinus = "Sound Down";
        private Vector2 _posKeyMinus = new Vector2(84, 36);
        private Texture2D _imgKeyPlus;
        private string _strKeyPlus = "Sound Up";
        private Vector2 _posKeyPlus = new Vector2(84, 36 + 14);
        private Texture2D _imgKey0;
        private string _strKey0 = "Mute";
        private Vector2 _posKey0 = new Vector2(84, 36 + 14 + 14);
        private Texture2D _imgKey1;
        private string _strKey1 = "Console";
        private Vector2 _posKey1 = new Vector2(84, 36 + 14 + 14 + 14);

        public string helpX
        {
            get { return _strKeyX; }
            set { _strKeyX = value; }
        }
        public string helpC
        {
            get { return _strKeyC; }
            set { _strKeyC = value; }
        }
        public string helpMouse
        {
            get { return _strKeyMouse; }
            set { _strKeyMouse = value; }
        }
        public string helpArrows
        {
            get { return _strKeysArrows; }
            set { _strKeysArrows = value; }
        }

        public FlxPause()
        {
            //icons for the pause screen
            _imgKeyX = FlxG.Content.Load<Texture2D>("flixel/key_x");
            _imgKeyC = FlxG.Content.Load<Texture2D>("flixel/key_c");
            _imgKeyMouse = FlxG.Content.Load<Texture2D>("flixel/key_mouse");
            _imgKeysArrows = FlxG.Content.Load<Texture2D>("flixel/keys_arrows");
            _imgKeyMinus = FlxG.Content.Load<Texture2D>("flixel/key_minus");
            _imgKeyPlus = FlxG.Content.Load<Texture2D>("flixel/key_plus");
            _imgKey0 = FlxG.Content.Load<Texture2D>("flixel/key_0");
            _imgKey1 = FlxG.Content.Load<Texture2D>("flixel/key_1");

            if (FlxG.height > 240)
                _pauseScale = 2;

            _pauseRect.Width *= _pauseScale;
            _pauseRect.Height *= _pauseScale;
            _pauseRect.X = (FlxG.width - _pauseRect.Width) / 2;
            _pauseRect.Y = (FlxG.height - _pauseRect.Height) / 2;

            _pauseText = new FlxText(_pauseRect.X, _pauseRect.Y + (7 * _pauseScale), _pauseRect.Width, "GAME PAUSED");
            _pauseText.setFormat(null, (2 * _pauseScale) - 1, Color.White, FlxJustification.Center, Color.White);

            _posKeyX *= _pauseScale;
            _posKeyC *= _pauseScale;
            _posKeyMouse *= _pauseScale;
            _posKeysArrows *= _pauseScale;
            _posKeyMinus *= _pauseScale;
            _posKeyPlus *= _pauseScale;
            _posKey0 *= _pauseScale;
            _posKey1 *= _pauseScale;

            _posKeyX.X += _pauseRect.X;
            _posKeyC.X += _pauseRect.X;
            _posKeyMouse.X += _pauseRect.X;
            _posKeysArrows.X += _pauseRect.X;
            _posKeyMinus.X += _pauseRect.X;
            _posKeyPlus.X += _pauseRect.X;
            _posKey0.X += _pauseRect.X;
            _posKey1.X += _pauseRect.X;

            _posKeyX.Y += _pauseRect.Y;
            _posKeyC.Y += _pauseRect.Y;
            _posKeyMouse.Y += _pauseRect.Y;
            _posKeysArrows.Y += _pauseRect.Y;
            _posKeyMinus.Y += _pauseRect.Y;
            _posKeyPlus.Y += _pauseRect.Y;
            _posKey0.Y += _pauseRect.Y;
            _posKey1.Y += _pauseRect.Y;
        }

        public void render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(FlxG.XnaSheet, _pauseRect, new Rectangle(1, 1, 1, 1), _pauseColor);

            _pauseText.render(spriteBatch);

            spriteBatch.Draw(_imgKeyX, _posKeyX, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKeyX, new Vector2(_posKeyX.X + (14 * _pauseScale), _posKeyX.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

            spriteBatch.Draw(_imgKeyC, _posKeyC, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKeyC, new Vector2(_posKeyC.X + (14 * _pauseScale), _posKeyC.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

#if !XBOX360
            spriteBatch.Draw(_imgKeyMouse, _posKeyMouse, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKeyMouse, new Vector2(_posKeyMouse.X + (14 * _pauseScale), _posKeyMouse.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);
#endif

            spriteBatch.Draw(_imgKeysArrows, _posKeysArrows, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKeysArrows, new Vector2(_posKeysArrows.X + (43 * _pauseScale), _posKeysArrows.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

            spriteBatch.Draw(_imgKeyMinus, _posKeyMinus, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKeyMinus, new Vector2(_posKeyMinus.X + (14 * _pauseScale), _posKeyMinus.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

            spriteBatch.Draw(_imgKeyPlus, _posKeyPlus, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKeyPlus, new Vector2(_posKeyPlus.X + (14 * _pauseScale), _posKeyPlus.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

            spriteBatch.Draw(_imgKey0, _posKey0, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKey0, new Vector2(_posKey0.X + (14 * _pauseScale), _posKey0.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

            spriteBatch.Draw(_imgKey1, _posKey1, null, Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(FlxG.Font, _strKey1, new Vector2(_posKey1.X + (14 * _pauseScale), _posKey1.Y),
                Color.White, 0, Vector2.Zero, _pauseScale, SpriteEffects.None, 0);

        }

    }
}
