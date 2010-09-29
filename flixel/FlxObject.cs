using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
	/**
	 * This is the base class for most of the display objects (<code>FlxSprite</code>, <code>FlxText</code>, etc).
	 * It includes some basic attributes about game objects, including retro-style flickering,
	 * basic state information, sizes, scrolling, and basic physics & motion.
	 */
    public class FlxObject
    {
        /**
         * Kind of a global on/off switch for any objects descended from <code>FlxObject</code>.
         */
        public bool exists;
        /**
         * If an object is not alive, the game loop will not automatically call <code>update()</code> on it.
         */
        public bool active;
        /**
         * If an object is not visible, the game loop will not automatically call <code>render()</code> on it.
         */
        public bool visible;
		/**
		 * Internal tracker for whether or not the object collides (see <code>solid</code>).
		 */
		protected bool _solid;
		/**
		 * Internal tracker for whether an object will move/alter position after a collision (see <code>fixed</code>).
		 */
		protected bool _fixed;

		/**
		 * The basic speed of this object.
		 */
		public Vector2 velocity;
		/**
		 * How fast the speed of this object is changing.
		 * Useful for smooth movement and gravity.
		 */
		public Vector2 acceleration;
		/**
		 * This isn't drag exactly, more like deceleration that is only applied
		 * when acceleration is not affecting the sprite.
		 */
		public Vector2 drag;
		/**
		 * If you are using <code>acceleration</code>, you can use <code>maxVelocity</code> with it
		 * to cap the speed automatically (very useful!).
		 */
		public Vector2 maxVelocity;
		/**
		 * Set the angle of a sprite to rotate it.
		 * WARNING: rotating sprites decreases rendering
		 * performance for this sprite by a factor of 10x!
		 */
        public float angle
        {
            get { return _angle; }
            set { _angle = value; _radians = MathHelper.ToRadians(_angle); }
        }
        //@benbaird We keep some private angle-related members in X-flixel due to XNA rotation differences
        protected float _angle = 0f;
        protected float _radians = 0f;

        /**
         * This is how fast you want this sprite to spin.
         */
		public float angularVelocity;
		/**
		 * How fast the spin speed should change.
		 */
		public float angularAcceleration;
		/**
		 * Like <code>drag</code> but for spinning.
		 */
		public float angularDrag;
		/**
		 * Use in conjunction with <code>angularAcceleration</code> for fluid spin speed control.
		 */
		public float maxAngular;
		/**
		 * WARNING: The origin of the sprite will default to its center.
		 * If you change this, the visuals and the collisions will likely be
		 * pretty out-of-sync if you do any rotation.
		 */
        // modified for X-flixel
        protected Vector2 _origin = Vector2.Zero;
        virtual public Vector2 origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
		/**
		 * If you want to do Asteroids style stuff, check out thrust,
		 * instead of directly accessing the object's velocity or acceleration.
		 */
		public float thrust;
		/**
		 * Used to cap <code>thrust</code>, helpful and easy!
		 */
		public float maxThrust;
		/**
		 * A handy "empty point" object
		 */
		static protected Vector2 _pZero = Vector2.Zero;
		
		/**
		 * A point that can store numbers from 0 to 1 (for X and Y independently)
		 * that governs how much this object is affected by the camera subsystem.
		 * 0 means it never moves, like a HUD element or far background graphic.
		 * 1 means it scrolls along a the same speed as the foreground layer.
		 * scrollFactor is initialized as (1,1) by default.
		 */
		public Vector2 scrollFactor;
		/**
		 * Internal helper used for retro-style flickering.
		 */
		protected bool _flicker;
		/**
		 * Internal helper used for retro-style flickering.
		 */
		protected float _flickerTimer;
		/**
		 * Handy for storing health percentage or armor points or whatever.
		 */
		public float health;
		/**
		 * Handy for tracking gameplay or animations.
		 */
        public bool dead;

		/**
		 * This is just a pre-allocated x-y point container to be used however you like
		 */
		protected Vector2 _point;
		/**
		 * This is just a pre-allocated rectangle container to be used however you like
		 */
		protected Rectangle _rect;
		/**
		 * This is a pre-allocated Flash Point object, which is useful for certain Flash graphics API calls
		 */
		protected Vector2 _flashPoint;
		/**
		 * Set this to false if you want to skip the automatic motion/movement stuff (see <code>updateMotion()</code>).
		 * FlxObject and FlxSprite default to true.
		 * FlxText, FlxTileblock, FlxTilemap and FlxSound default to false.
		 */
		public bool moves;
		/**
		 * These store a couple of useful numbers for speeding up collision resolution.
		 */
		public FlxRect colHullX;
		/**
		 * These store a couple of useful numbers for speeding up collision resolution.
		 */
        public FlxRect colHullY;
		/**
		 * These store a couple of useful numbers for speeding up collision resolution.
		 */
		public Vector2 colVector;
		/**
		 * An array of <code>FlxPoint</code> objects.  By default contains a single offset (0,0).
		 */
		public List<Vector2> colOffsets = new List<Vector2>();
		/**
		 * Dedicated internal flag for whether or not this class is a FlxGroup.
		 */
		internal bool _group;
		/**
		 * Flag that indicates whether or not you just hit the floor.
		 * Primarily useful for platformers, this flag is reset during the <code>updateMotion()</code>.
		 */
		public bool onFloor;
		/**
		 * Flag for direction collision resolution.
		 */
		public bool collideLeft;
		/**
		 * Flag for direction collision resolution.
		 */
		public bool collideRight;
		/**
		 * Flag for direction collision resolution.
		 */
		public bool collideTop;
		/**
		 * Flag for direction collision resolution.
		 */
		public bool collideBottom;

        // X-flixel only: Positioning variables to compensate for the fact that in
        // standard flixel, FlxObject inherits from FlxRect.
        public float x;
        public float y;
        public float width;
        public float height;


        /**
         * Creates a new <code>FlxObject</code>.
         * 
         * @param	X		The X-coordinate of the point in space.
         * @param	Y		The Y-coordinate of the point in space.
         * @param	Width	Desired width of the rectangle.
         * @param	Height	Desired height of the rectangle.
         */
        public FlxObject()
        {
            constructor1(0, 0, 0, 0);
        }
        public FlxObject(float X, float Y, float Width, float Height)
        {
            constructor1(X, Y, Width, Height);
        }
        private void constructor1(float X, float Y, float Width, float Height)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;

            exists = true;
            active = true;
            visible = true;
            _solid = true;
            _fixed = false;
            moves = true;

            collideLeft = true;
            collideRight = true;
            collideTop = true;
            collideBottom = true;

            _origin = Vector2.Zero;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            drag = Vector2.Zero;
            maxVelocity = new Vector2(10000, 10000);

            angle = 0;
            angularVelocity = 0;
            angularAcceleration = 0;
            angularDrag = 0;
            maxAngular = 10000;

            thrust = 0;

            scrollFactor = new Vector2(1, 1);
            _flicker = false;
            _flickerTimer = -1;
            health = 1;
            dead = false;
            _point = Vector2.Zero;
            _rect = Rectangle.Empty;
            _flashPoint = Vector2.Zero;

            colHullX = FlxRect.Empty;
            colHullY = FlxRect.Empty;
            colVector = Vector2.Zero;
            colOffsets.Add(Vector2.Zero);
            _group = false;
        }

		/**
		 * Called by <code>FlxGroup</code>, commonly when game states are changed.
		 */
		virtual public void destroy()
		{
			//Nothing to destroy yet
		}

        /**
         * Set <code>solid</code> to true if you want to collide this object.
         */
        public bool solid
        {
            get { return _solid; }
            set { _solid = value; }
        }

        /**
         * Set <code>fixed</code> to true if you want the object to stay in place during collisions.
         * Useful for levels and other environmental objects.
         */
        public bool @fixed
        {
            get { return _fixed; }
            set { _fixed = value; }
        }

		/**
		 * Called by <code>FlxObject.updateMotion()</code> and some constructors to
		 * rebuild the basic collision data for this object.
		 */
        virtual public void refreshHulls()
		{
			colHullX.x = x;
			colHullX.y = y;
			colHullX.width = width;
			colHullX.height = height;
			colHullY.x = x;
			colHullY.y = y;
			colHullY.width = width;
			colHullY.height = height;
		}

		/**
		 * Internal function for updating the position and speed of this object.
		 * Useful for cases when you need to update this but are buried down in too many supers.
		 */
        protected void updateMotion()
        {
            if (!moves)
                return;

            if (_solid)
                refreshHulls();
            onFloor = false;

            // Motion/physics
            angularVelocity = FlxU.computeVelocity(angularVelocity, angularAcceleration, angularDrag, maxAngular);
            angle += angularVelocity * FlxG.elapsed;
            Vector2 thrustComponents;
            if (thrust != 0)
            {
                thrustComponents = FlxU.rotatePoint(-thrust, 0, 0, 0, angle);
                Vector2 maxComponents = FlxU.rotatePoint(-maxThrust, 0, 0, 0, angle);
                float max = Math.Abs(maxComponents.X);
                if (max > Math.Abs(maxComponents.Y))
                    maxComponents.Y = max;
                else
                    max = Math.Abs(maxComponents.Y);
                maxVelocity.X = Math.Abs(max);
                maxVelocity.Y = Math.Abs(max);
            }
            else
            {
                thrustComponents = Vector2.Zero;
            }
            velocity.X = FlxU.computeVelocity(velocity.X, acceleration.X + thrustComponents.X, drag.X, maxVelocity.X);
            velocity.Y = FlxU.computeVelocity(velocity.Y, acceleration.Y + thrustComponents.Y, drag.Y, maxVelocity.Y);
            x += velocity.X * FlxG.elapsed;
            y += velocity.Y * FlxG.elapsed;

            //Update collision data with new movement results
            if (!_solid)
                return;
            colVector.X = velocity.X * FlxG.elapsed;
            colVector.Y = velocity.Y * FlxG.elapsed;
            colHullX.width += ((colVector.X > 0) ? colVector.X : -colVector.X);
            if (colVector.X < 0)
                colHullX.x += colVector.X;
            colHullY.x = x;
            colHullY.height += ((colVector.Y > 0) ? colVector.Y : -colVector.Y);
            if (colVector.Y < 0)
                colHullY.y += colVector.Y;
        }

        /**
         * Just updates the retro-style flickering.
         * Considered update logic rather than rendering because it toggles visibility.
         */
        public virtual void updateFlickering()
        {
            if (flickering())
            {
                if (_flickerTimer > 0)
                {
                    _flickerTimer -= FlxG.elapsed;
                    if (_flickerTimer == 0)
                    {
                        _flickerTimer = -1;
                    }
                }
                if (_flickerTimer < 0) flicker(-1);
                else
                {
                    _flicker = !_flicker;
                    visible = !_flicker;
                }
            }
        }

		/**
		 * Called by the main game loop, handles motion/physics and game logic
		 */
        virtual public void update()
		{
			updateMotion();
			updateFlickering();
		}

        /**
         * Override this function to draw graphics (see <code>FlxSprite</code>).
         */
        virtual public void render(SpriteBatch spriteBatch)
        {
            //Objects don't have any visual logic/display of their own.
        }

		/**
		 * Checks to see if some <code>FlxObject</code> object overlaps this <code>FlxObject</code> object.
		 * 
		 * @param	Object	The object being tested.
		 * 
		 * @return	Whether or not the two objects overlap.
		 */
        virtual public bool overlaps(FlxObject Object)
		{
            _point = getScreenXY();
			float tx = _point.X;
			float ty = _point.Y;
            _point = Object.getScreenXY();
			if((_point.X <= tx-Object.width) || (_point.X >= tx+width) || (_point.Y <= ty-Object.height) || (_point.Y >= ty+height))
				return false;
			return true;
		}

		/**
		 * Checks to see if a point in 2D space overlaps this <code>FlxObject</code> object.
		 * 
		 * @param	X			The X coordinate of the point.
		 * @param	Y			The Y coordinate of the point.
		 * @param	PerPixel	Whether or not to use per pixel collision checking (only available in <code>FlxSprite</code> subclass).
		 * 
		 * @return	Whether or not the point overlaps this object.
		 */
        virtual public bool overlapsPoint(float X, float Y)
        {
            return overlapsPoint(X, Y, false);
        }
        virtual public bool overlapsPoint(float X, float Y, bool PerPixel)
		{
			X = X + FlxU.floor(FlxG.scroll.X);
			Y = Y + FlxU.floor(FlxG.scroll.Y);
            _point = getScreenXY();
			if((X <= _point.X) || (X >= _point.X+width) || (Y <= _point.Y) || (Y >= _point.Y+height))
				return false;
			return true;
		}

		/**
		 * If you don't want to call <code>FlxU.collide()</code> you can use this instead.
		 * Just calls <code>FlxU.collide(this,Object);</code>.  Will collide against itself
		 * if Object==null.
		 * 
		 * @param	Object		The <FlxObject> you want to collide with.
		 */
        virtual public bool collide(FlxObject Object)
		{
			return FlxU.collide(this,((Object==null)?this:Object));
		}

		/**
		 * <code>FlxU.collide()</code> (and thus <code>FlxObject.collide()</code>) call
		 * this function each time two objects are compared to see if they collide.
		 * It doesn't necessarily mean these objects WILL collide, however.
		 * 
		 * @param	Object	The <code>FlxObject</code> you're about to run into.
		 */
        virtual public void preCollide(FlxObject Object)
		{
			//Most objects don't have to do anything here.
		}

		/**
		 * Called when this object's left side collides with another <code>FlxObject</code>'s right.
		 * NOTE: by default this function just calls <code>hitSide()</code>.
		 * 
		 * @param	Contact		The <code>FlxObject</code> you just ran into.
		 * @param	Velocity	The suggested new velocity for this object.
		 */
		virtual public void hitLeft(FlxObject Contact, float Velocity)
		{
			hitSide(Contact,Velocity);
		}
		
		/**
		 * Called when this object's right side collides with another <code>FlxObject</code>'s left.
		 * NOTE: by default this function just calls <code>hitSide()</code>.
		 * 
		 * @param	Contact		The <code>FlxObject</code> you just ran into.
		 * @param	Velocity	The suggested new velocity for this object.
		 */
        virtual public void hitRight(FlxObject Contact, float Velocity)
		{
			hitSide(Contact,Velocity);
		}

		/**
		 * Since most games have identical behavior for running into walls,
		 * you can just override this function instead of overriding both hitLeft and hitRight. 
		 * 
		 * @param	Contact		The <code>FlxObject</code> you just ran into.
		 * @param	Velocity	The suggested new velocity for this object.
		 */
        virtual public void hitSide(FlxObject Contact, float Velocity)
		{
			if(!@fixed || (Contact.@fixed && ((velocity.Y != 0) || (velocity.X != 0))))
				velocity.X = Velocity;
		}

		/**
		 * Called when this object's top collides with the bottom of another <code>FlxObject</code>.
		 * 
		 * @param	Contact		The <code>FlxObject</code> you just ran into.
		 * @param	Velocity	The suggested new velocity for this object.
		 */
        virtual public void hitTop(FlxObject Contact, float Velocity)
		{
			if(!@fixed || (Contact.@fixed && ((velocity.Y != 0) || (velocity.X != 0))))
				velocity.Y = Velocity;
		}

		/**
		 * Called when this object's bottom edge collides with the top of another <code>FlxObject</code>.
		 * 
		 * @param	Contact		The <code>FlxObject</code> you just ran into.
		 * @param	Velocity	The suggested new velocity for this object.
		 */
        virtual public void hitBottom(FlxObject Contact, float Velocity)
		{
			onFloor = true;
			if(!@fixed || (Contact.@fixed && ((velocity.Y != 0) || (velocity.X != 0))))
				velocity.Y = Velocity;
		}

		/**
		 * Call this function to "damage" (or give health bonus) to this sprite.
		 * 
		 * @param	Damage		How much health to take away (use a negative number to give a health bonus).
		 */
		virtual public void hurt(float Damage)
		{
			health = health - Damage;
			if(health <= 0)
				kill();
		}
		
		/**
		 * Call this function to "kill" a sprite so that it no longer 'exists'.
		 */
        virtual public void kill()
		{
			exists = false;
			dead = true;
		}

		/**
		 * Tells this object to flicker, retro-style.
		 * 
		 * @param	Duration	How many seconds to flicker for.
		 */
		public void flicker(float Duration) { _flickerTimer = Duration; if(_flickerTimer < 0) { _flicker = false; visible = true; } }
		
		/**
		 * Check to see if the object is still flickering.
		 * 
		 * @return	Whether the object is flickering or not.
		 */
		public bool flickering() { return _flickerTimer >= 0; }

		/**
		 * Call this function to figure out the on-screen position of the object.
		 * 
		 * @param	P	Takes a <code>Point</code> object and assigns the post-scrolled X and Y values of this object to it.
		 * 
		 * @return	The <code>Point</code> you passed in, or a new <code>Point</code> if you didn't pass one, containing the screen X and Y position of this object.
		 */
        virtual public Vector2 getScreenXY()
		{
            Vector2 Point = Vector2.Zero;
			Point.X = FlxU.floor(x + FlxU.roundingError)+FlxU.floor(FlxG.scroll.X*scrollFactor.X);
			Point.Y = FlxU.floor(y + FlxU.roundingError)+FlxU.floor(FlxG.scroll.Y*scrollFactor.Y);
			return Point;
		}
		
		/**
		 * Check and see if this object is currently on screen.
		 * 
		 * @return	Whether the object is on screen or not.
		 */
        virtual public bool onScreen()
		{
            _point = getScreenXY();
			if((_point.X + width < 0) || (_point.X > FlxG.width) || (_point.Y + height < 0) || (_point.Y > FlxG.height))
				return false;
			return true;
		}

		/**
		 * Handy function for reviving game objects.
		 * Resets their existence flags and position, including LAST position.
		 * 
		 * @param	X	The new X position of this object.
		 * @param	Y	The new Y position of this object.
		 */
        virtual public void reset(float X, float Y)
		{
			x = X;
			y = Y;
			exists = true;
			dead = false;
		}

		/**
		 * Returns the appropriate color for the bounding box depending on object state.
		 */
		public Color getBoundingColor()
		{
			if(solid)
			{
				if(@fixed)
					return new Color(0x00, 0xf2, 0x25, 0x7f);
				else
					return new Color(0xff, 0x00, 0x12, 0x7f);
			}
			else
				return new Color(0x00, 0x90, 0xe9, 0x7f);
		}

    }
}
