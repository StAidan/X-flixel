using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.flixel
{
    public class FlxRect
    {
        public float x;
        public float y;
        public float width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        public float height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        private float _width;
        private float _height;

        public FlxRect(float X, float Y, float Width, float Height)
        {
            x = X;
            y = Y;
            _width = Width;
            _height = Height;
        }

        static public FlxRect Empty
        {
            get { return new FlxRect(0, 0, 0, 0); }
        }
    }
}
