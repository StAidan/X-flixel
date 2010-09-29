using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /**
     * This class helps contain and track the mouse pointer in your game.
     * Automatically accounts for parallax scrolling, etc.
     */
    public class FlxMouse
    {
        private Texture2D ImgDefaultCursor;

        private MouseState _curMouse;
        private MouseState _lastMouse;
        private EventHandler<FlxMouseEvent> _mouseEvent;

        public void addMouseListener(EventHandler<FlxMouseEvent> MouseEvent)
        {
            _mouseEvent += MouseEvent;
        }
        public void removeMouseListener(EventHandler<FlxMouseEvent> MouseEvent)
        {
            _mouseEvent -= MouseEvent;
        }

        public float x
        {
#if XBOX360
            get { return 0; }
#else
            get
            {
                //if (_curMouse.X >= FlxG._game.targetLeft)
                //{
                    return (float)((_curMouse.X - FlxG._game.targetLeft) / FlxG.scale) - FlxG.scroll.X;
                //}
                //else
                //{
                //    return 0;
                //}
            }
#endif
        }
        public float y
        {
#if XBOX360
            get { return 0; }
#else
            get { return ((float)_curMouse.Y / FlxG.scale) - FlxG.scroll.Y; }
#endif
        }

		/**
		 * Current "delta" value of mouse wheel.  If the wheel was just scrolled up, it will have a positive value.  If it was just scrolled down, it will have a negative value.  If it wasn't just scroll this frame, it will be 0.
		 */
		public int wheel;

		/**
		 * Current X position of the mouse pointer on the screen.
		 */
		public int screenX;
		/**
		 * Current Y position of the mouse pointer on the screen.
		 */
		public int screenY;
		/**
		 * Graphical representation of the mouse pointer.
		 */
		public FlxSprite cursor;


		/**
		 * Constructor.
		 */
        public FlxMouse()
        {
			screenX = 0;
			screenY = 0;
            cursor = new FlxSprite();
            cursor.visible = false;
        }

		/**
		 * Either show an existing cursor or load a new one.
		 * 
		 * @param	Graphic		The image you want to use for the cursor.
		 * @param	XOffset		The number of pixels between the mouse's screen position and the graphic's top left corner.
		 * * @param	YOffset		The number of pixels between the mouse's screen position and the graphic's top left corner. 
		 */
        public void show(Texture2D Graphic)
        {
            show(Graphic, 0, 0);
        }
		public void show(Texture2D Graphic,int XOffset,int YOffset)
		{
			if(Graphic != null)
				load(Graphic,XOffset,YOffset);
			else if(cursor != null)
				cursor.visible = true;
			else
				load(null);
		}
		
		/**
		 * Hides the mouse cursor
		 */
		public void hide()
		{
			if(cursor != null)
			{
				cursor.visible = false;
			}
		}

		/**
		 * Load a new mouse cursor graphic
		 * 
		 * @param	Graphic		The image you want to use for the cursor.
		 * @param	XOffset		The number of pixels between the mouse's screen position and the graphic's top left corner.
		 * * @param	YOffset		The number of pixels between the mouse's screen position and the graphic's top left corner. 
		 */
        public void load(Texture2D Graphic)
        {
            load(Graphic, 0, 0);
        }

        public void load(Texture2D Graphic, int XOffset, int YOffset)
		{
			if(Graphic == null)
				Graphic = ImgDefaultCursor;
			cursor = new FlxSprite(screenX,screenY,Graphic);
			cursor.solid = false;
			cursor.offset.X = XOffset;
			cursor.offset.Y = YOffset;
		}

		/**
		 * Unload the current cursor graphic.  If the current cursor is visible,
		 * then the default system cursor is loaded up to replace the old one.
		 */
        public void unload()
		{
			if(cursor != null)
			{
				if(cursor.visible)
					load(null);
				else
					cursor = null;
			}
		}

        /**
         * Called by the internal game loop to update the mouse pointer's position in the game world.
         * Also updates the just pressed/just released flags.
         */
        public void update()
        {
            _lastMouse = _curMouse;
            _curMouse = Mouse.GetState();
            cursor.x = x;
            cursor.y = y;

            if (_mouseEvent != null)
            {
                if (justPressed())
                {
                    _mouseEvent(this, new FlxMouseEvent(MouseEventType.MouseDown));
                }
                else if (justReleased())
                {
                    _mouseEvent(this, new FlxMouseEvent(MouseEventType.MouseUp));
                }
            }
        }

        public void reset()
        {
            _curMouse = _lastMouse;
            //also get rid of all current event listeners
            _mouseEvent = null;
        }

        public bool pressed()
        {
            return (_curMouse.LeftButton == ButtonState.Pressed ||
                _curMouse.RightButton == ButtonState.Pressed);
        }

        public bool justPressed()
        {
            return ((_curMouse.LeftButton == ButtonState.Pressed && _lastMouse.LeftButton == ButtonState.Released) ||
                (_curMouse.RightButton == ButtonState.Pressed && _lastMouse.RightButton == ButtonState.Released));
        }

        public bool justReleased()
        {
            return ((_curMouse.LeftButton == ButtonState.Released && _lastMouse.LeftButton == ButtonState.Pressed) ||
                (_curMouse.RightButton == ButtonState.Released && _lastMouse.RightButton == ButtonState.Pressed));
        }
    }

}
