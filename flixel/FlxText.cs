using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    //@benbaird justification uses this enum in X-flixel, rather than a string
    public enum FlxJustification
    {
        Left = 0,
        Right = 1,
        Center = 2
    }

    /**
     * Extends <code>FlxSprite</code> to support rendering text.
     * Can tint, fade, rotate and scale just like a sprite.
     * Doesn't really animate though, as far as I know.
     * Also does nice pixel-perfect centering on pixel fonts
     * as long as they are only one liners.
     * 
     * FlxText's internal implementation in X-flixel hasn't
     * changed drastically from its v1.25 debut. The primary
     * modifications are to its public interface in order
     * to align it with AS3 flixel.
     */
    public class FlxText : FlxSprite
    {

        private string _text;
        private SpriteFont _font;
        private Vector2 _fontmeasure = Vector2.Zero;
        private float _scale = 1f;
        //private float _angle = 0f;
        //private float _radians = 0f;

        public int textWidth
        {
            get { return (int)_fontmeasure.X; }
        }
        public int textHeight
        {
            get { return (int)_fontmeasure.Y; }
        }


        public override Vector2 origin
        {
            get
            {
                return _origin * _scale;
            }
            set
            {
                _origin = value / _scale;
            }
        }

        //public float angle
        //{
        //    get { return _angle; }
        //    set { _angle = value; _radians = MathHelper.ToRadians(_angle); }
        //}

        //@benbaird X-flixel only
        public void autoSize()
        {
            width = textWidth;
            height = textHeight;
        }

        /**
         * Creates a new <code>FlxText</code> object at the specified position.
         * 
         * @param	X				The X position of the text.
         * @param	Y				The Y position of the text.
         * @param	Width			The width of the text object (height is determined automatically).
         * @param	Text			The actual text you would like to display initially.
         * @param	EmbeddedFont	Whether this text field uses embedded fonts or nto
         */
        public FlxText(float X, float Y, float Width)
            : base(X, Y)
        {
            constructor(X, Y, Width, 10, "", Color.White, FlxG.Font, 1, FlxJustification.Center, 0);
        }
        public FlxText(float X, float Y, float Width, string Text)
            : base(X, Y)
		{
			if(Text == null)
				Text = "";

            constructor(X, Y, Width, 10, Text, Color.White, FlxG.Font, 1, FlxJustification.Center, 0);
		}

        public void constructor(float X, float Y, float Width, float Height, string sText, Color cColor, SpriteFont fFont, float fScale, FlxJustification fJustification, float fAngle)
        {
            _text = sText;
            color = cColor;
            shadow = Color.Black;

            backColor = new Color(0xFF, 0xFF, 0xFF, 0x0);
            if (fFont == null)
                fFont = FlxG.Font;
            _font = fFont;
            angle = fAngle;
            _scale = fScale;

            alignment = fJustification;

            x = X;
            y = Y;
            width = Width;
            height = Height;

            scrollFactor = Vector2.Zero;

            solid = false;
            moves = false;
            recalcMeasurements();
        }

		/**
		 * You can use this if you have a lot of text parameters
		 * to set instead of the individual properties.
		 * 
		 * @param	Font		The name of the font face for the text display.
		 * @param	Scale		The scale of the font (in AS3 flixel, this is Size)
		 * @param	Color		The color of the text in traditional flash 0xRRGGBB format.
		 * @param	Alignment	A string representing the desired alignment ("left,"right" or "center").
		 * @param	ShadowColor	A uint representing the desired text shadow color in flash 0xRRGGBB format.
		 * 
		 * @return	This FlxText instance (nice for chaining stuff together, if you're into that).
		 */
		public FlxText setFormat(SpriteFont Font, float Scale, Color Color, FlxJustification Alignment, Color ShadowColor)
		{
			if(Font == null)
				Font = FlxG.Font;
			_font = Font;
			_scale = Scale;
			color = Color;
			alignment = Alignment;
			shadow = ShadowColor;
            recalcMeasurements();
			return this;
		}

        /**
         * The text being displayed.
         */
        public string text
        {
            get { return _text; }
            set { _text = value; recalcMeasurements(); }
        }

        /**
         * The size of the text being displayed.
         */
        public new float scale
        {
            get { return _scale; }
            set
            {
                // Preserve origin proportions
                _origin *= _scale;
                _scale = value;
                // Restore origin proportions
                _origin /= _scale;
                recalcMeasurements();
            }
        }

        /**
         * The color of the text being displayed.
         */
        //public Color color;

        /**
         * The font used for this text.
         */
        public SpriteFont font
        {
            get { return _font; }
            set { _font = value; if (_font == null) _font = FlxG.Font; recalcMeasurements(); }
        }

        /**
         * The alignment of the font ("left", "right", or "center").
         */
        public FlxJustification alignment = FlxJustification.Left;

        /**
         * The color of the text's shadow.
         */
        public Color shadow;

        /**
         * The color of the background behind the text (X-flixel only).
         */
        public Color backColor;

        //@benbaird X-flixel only. Used to ensure the textWidth and textHeight properties
        // are always up to date.
        private void recalcMeasurements()
        {
            try
            {
                _fontmeasure = _font.MeasureString(_text) * _scale;
                origin = new Vector2(_fontmeasure.X / 2, _fontmeasure.Y / 2);
            }
            catch
            {
                _fontmeasure = Vector2.Zero;
            }
        }

        //@desc		Called by the game loop automatically, blits the text object to the screen
        public override void render(SpriteBatch spriteBatch)
        {
            if (visible == false || exists == false)
            {
                return;
            }

            Vector2 pos = new Vector2(x, y) + origin;
            pos += (FlxG.scroll * scrollFactor);

            if (backColor.A > 0)
            {
                //Has a background color
                spriteBatch.Draw(FlxG.XnaSheet, new Rectangle((int)x, (int)y, (int)width, (int)height),
                    new Rectangle(1, 1, 1, 1), backColor);
            }

            if (shadow != color)
            {
                pos += new Vector2(1, 1);
                if (alignment == FlxJustification.Left)
                {
                    spriteBatch.DrawString(_font, _text,
                        pos, shadow,
                        _radians, _origin, _scale, SpriteEffects.None, 0f);
                }
                else if (alignment == FlxJustification.Right)
                {
                    spriteBatch.DrawString(_font, _text,
                        new Vector2(pos.X + width - textWidth, pos.Y), shadow,
                        _radians, _origin, _scale, SpriteEffects.None, 0f);
                }
                else if (alignment == FlxJustification.Center)
                {
                    spriteBatch.DrawString(_font, _text,
                        new Vector2(pos.X + ((width - textWidth) / 2), pos.Y), shadow,
                        _radians, _origin, _scale, SpriteEffects.None, 0f);
                }
                pos += new Vector2(-1, -1);
            }

            if (alignment == FlxJustification.Left)
            {
                spriteBatch.DrawString(_font, _text,
                    pos, color,
                    _radians, _origin, _scale, SpriteEffects.None, 0f);
            }
            else if (alignment == FlxJustification.Right)
            {
                spriteBatch.DrawString(_font, _text,
                    new Vector2(pos.X + width - textWidth, pos.Y), color,
                    _radians, _origin, _scale, SpriteEffects.None, 0f);
            }
            else if (alignment == FlxJustification.Center)
            {
                spriteBatch.DrawString(_font, _text,
                    new Vector2(pos.X + ((width - textWidth) / 2), pos.Y), color,
                    _radians, _origin, _scale, SpriteEffects.None, 0f);
            }
        }
    }
}
