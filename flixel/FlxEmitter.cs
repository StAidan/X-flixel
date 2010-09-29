using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /**
     * <code>FlxEmitter</code> is a lightweight particle emitter.
     * It can be used for one-time explosions or for
     * continuous fx like rain and fire.  <code>FlxEmitter</code>
     * is not optimized or anything; all it does is launch
     * <code>FlxSprite</code> objects out at set intervals
     * by setting their positions and velocities accordingly.
     * It is easy to use and relatively efficient, since it
     * automatically redelays its sprites and/or kills
     * them once they've been launched.
     */
    public class FlxEmitter : FlxGroup
    {
		/**
		 * The minimum possible velocity of a particle.
		 * The default value is (-100,-100).
		 */
		public Vector2 minParticleSpeed;
		/**
		 * The maximum possible velocity of a particle.
		 * The default value is (100,100).
		 */
		public Vector2 maxParticleSpeed;
		/**
		 * The X and Y drag component of particles launched from the emitter.
		 */
		public Vector2 particleDrag;
		/**
		 * The minimum possible angular velocity of a particle.  The default value is -360.
		 * NOTE: rotating particles are more expensive to draw than non-rotating ones!
		 */
		public float minRotation;
		/**
		 * The maximum possible angular velocity of a particle.  The default value is 360.
		 * NOTE: rotating particles are more expensive to draw than non-rotating ones!
		 */
		public float maxRotation;
		/**
		 * Sets the <code>acceleration.y</code> member of each particle to this value on launch.
		 */
		public float gravity;
		/**
		 * Determines whether the emitter is currently emitting particles.
		 */
		public bool on;
		/**
		 * This variable has different effects depending on what kind of emission it is.
		 * During an explosion, delay controls the lifespan of the particles.
		 * During normal emission, delay controls the time between particle launches.
		 * NOTE: In older builds, polarity (negative numbers) was used to define emitter behavior.
		 * THIS IS NO LONGER THE CASE!  FlxEmitter.start() controls that now!
		 */
		public float delay;
		/**
		 * The number of particles to launch at a time.
		 */
		public int quantity;
		/**
		 * Checks whether you already fired a particle this frame.
		 */
		public bool justEmitted;
		/**
		 * The style of particle emission (all at once, or one at a time).
		 */
		protected bool _explode;
		/**
		 * Internal helper for deciding when to launch particles or kill them.
		 */
		protected float _timer;
		/**
		 * Internal marker for where we are in <code>_sprites</code>.
		 */
		protected int _particle;
		/**
		 * Internal counter for figuring out how many particles to launch.
		 */
		protected int _counter;

        /**
         * Creates a new <code>FlxEmitter</code> object at a specific position.
         * Does not automatically generate or attach particles!
         * 
         * @param	X			The X position of the emitter.
         * @param	Y			The Y position of the emitter.
         */
        public FlxEmitter()
        {
            constructor(0, 0);
        }
        public FlxEmitter(int X, int Y)
        {
            constructor(X, Y);
        }
        private void constructor(int X, int Y)
        {
            x = X;
            y = Y;
            width = 0;
            height = 0;

            minParticleSpeed = new Vector2(-100, -100);
            maxParticleSpeed = new Vector2(100, 100);
            minRotation = -360;
            maxRotation = 360;
            gravity = 400;
            particleDrag = new Vector2();
            delay = 0;
            quantity = 0;
            _counter = 0;
            _explode = true;
            exists = false;
            on = false;
            justEmitted = false;
        }

        /**
         * This function generates a new array of sprites to attach to the emitter.
         * 
         * @param	Graphics		If you opted to not pre-configure an array of FlxSprite objects, you can simply pass in a particle image or sprite sheet.
         * @param	Quantity		The number of particles to generate when using the "create from image" option.
         * @param	Multiple		Whether the image in the Graphics param is a single particle or a bunch of particles (if it's a bunch, they need to be square!).
         * @param	Collide			Whether the particles should be flagged as not 'dead' (non-colliding particles are higher performance).  0 means no collisions, 0-1 controls scale of particle's bounding box.
         * @param	Bounce			Whether the particles should bounce after colliding with things.  0 means no bounce, 1 means full reflection.
         * 
         * @return	This FlxEmitter instance (nice for chaining stuff together, if you're into that).
         */
        public FlxEmitter createSprites(Texture2D Graphics, int Quantity)
        {
            return createSprites(Graphics, Quantity, true, 0, 0);
        }
        public FlxEmitter createSprites(Texture2D Graphics, int Quantity, bool Multiple)
        {
            return createSprites(Graphics, Quantity, Multiple, 0, 0);
        }
        public FlxEmitter createSprites(Texture2D Graphics, int Quantity, bool Multiple, float Collide, float Bounce)
		{
			members = new List<FlxObject>();
			int  r;
			FlxSprite s;
			int tf = 1;
			float sw;
			float sh;
			if(Multiple)
			{
				s = new FlxSprite();
				s.loadGraphic(Graphics,true);
				tf = s.frames;
			}
			int i = 0;
			while(i < Quantity)
			{
				if((Collide > 0) && (Bounce > 0))
					s = new FlxParticle(Bounce) as FlxSprite;
				else
					s = new FlxSprite();
				if(Multiple)
				{
					r = (int)(FlxU.random()*tf);
                    //if(BakedRotations > 0)
                    //    s.loadRotatedGraphic(Graphics,BakedRotations,r);
                    //else
                    //{
						s.loadGraphic(Graphics,true);
						s.frame = r;
					//}
				}
				else
				{
                    //if(BakedRotations > 0)
                    //    s.loadRotatedGraphic(Graphics,BakedRotations);
                    //else
						s.loadGraphic(Graphics);
				}
				if(Collide > 0)
				{
					sw = s.width;
					sh = s.height;
					s.width = (int)(s.width * Collide);
                    s.height = (int)(s.height * Collide);
					s.offset.X = (int)(sw-s.width)/2;
					s.offset.Y = (int)(sh-s.height)/2;
					s.solid = true;
				}
				else
					s.solid = false;
				s.exists = false;
				s.scrollFactor = scrollFactor;
				add(s);
				i++;
			}
			return this;
		}
		

		/**
		 * A more compact way of setting the width and height of the emitter.
		 * 
		 * @param	Width	The desired width of the emitter (particles are spawned randomly within these dimensions).
		 * @param	Height	The desired height of the emitter.
		 */
		public void setSize(int Width, int Height)
		{
			width = Width;
			height = Height;
		}

		/**
		 * A more compact way of setting the X velocity range of the emitter.
		 * 
		 * @param	Min		The minimum value for this range.
		 * @param	Max		The maximum value for this range.
		 */
        public void setXSpeed()
        {
            setXSpeed(0, 0);
        }
		public void setXSpeed(float Min, float Max)
		{
			minParticleSpeed.X = Min;
			maxParticleSpeed.X = Max;
		}

		/**
		 * A more compact way of setting the Y velocity range of the emitter.
		 * 
		 * @param	Min		The minimum value for this range.
		 * @param	Max		The maximum value for this range.
		 */
        public void setYSpeed()
        {
            setYSpeed(0, 0);
        }
        public void setYSpeed(float Min, float Max)
		{
			minParticleSpeed.Y = Min;
			maxParticleSpeed.Y = Max;
		}

		/**
		 * A more compact way of setting the angular velocity constraints of the emitter.
		 * 
		 * @param	Min		The minimum value for this range.
		 * @param	Max		The maximum value for this range.
		 */
        public void setRotation()
        {
            setRotation(0, 0);
        }
		public void setRotation(float Min, float Max)
		{
			minRotation = Min;
			maxRotation = Max;
		}

		/**
		 * Internal function that actually performs the emitter update (called by update()).
		 */
		protected void updateEmitter()
		{
			if(_explode)
			{
				_timer += FlxG.elapsed;
				if((delay > 0) && (_timer > delay))
				{
					kill();
					return;
				}
				if(on)
				{
					on = false;
					int i = _particle;
					int l = members.Count;
					if(quantity > 0)
						l = quantity;
					l += _particle;
					while(i < l)
					{
						emitParticle();
						i++;
					}
				}
				return;
			}
			if(!on)
				return;
			_timer += FlxG.elapsed;
			while((_timer > delay) && ((quantity <= 0) || (_counter < quantity)))
			{
				_timer -= delay;
				emitParticle();
			}
		}

		/**
		 * Internal function that actually goes through and updates all the group members.
		 * Overridden here to remove the position update code normally used by a FlxGroup.
		 */
		override protected void updateMembers()
		{
			FlxObject o;
			int i = 0;
			int l = members.Count;
			while(i < l)
			{
				o = members[i++] as FlxObject;
				if((o != null) && o.exists && o.active)
					o.update();
			}
		}

		/**
		 * Called automatically by the game loop, decides when to launch particles and when to "die".
		 */
        override public void update()
		{
			justEmitted = false;
			base.update();
			updateEmitter();
		}

		/**
		 * Call this function to start emitting particles.
		 * 
		 * @param	Explode		Whether the particles should all burst out at once.
		 * @param	Delay		You can set the delay (or lifespan) here if you want.
		 * @param	Quantity	How many particles to launch.  Default value is 0, or "all the particles".
		 */
        public void start()
        {
            start(true, 0, 0);
        }
        public void start(bool Explode, float Delay)
        {
            start(Explode, Delay, 0);
        }
        public void start(bool Explode, float Delay, int Quantity)
		{
			if(members.Count <= 0)
			{
				FlxG.log("WARNING: there are no sprites loaded in your emitter.\nAdd some to FlxEmitter.members or use FlxEmitter.createSprites().");
				return;
			}
			_explode = Explode;
			if(!_explode)
				_counter = 0;
			if(!exists)
				_particle = 0;
			exists = true;
			visible = true;
			active = true;
			dead = false;
			on = true;
			_timer = 0;
			if(quantity == 0)
				quantity = Quantity;
			else if(Quantity != 0)
				quantity = Quantity;
			if(Delay != 0)
				delay = Delay;
			if(delay < 0)
				delay = -delay;
			if(delay == 0)
			{
				if(Explode)
					delay = 3;	//default value for particle explosions
				else
					delay = 0.1f;//default value for particle streams
			}
		}


		/**
		 * This function can be used both internally and externally to emit the next particle.
		 */
		public void emitParticle()
		{
			_counter++;
            FlxSprite s = members[_particle] as FlxSprite;
			s.visible = true;
			s.exists = true;
			s.active = true;
            s.x = x - ((int)s.width >> 1) + FlxU.random() * width;
            s.y = y - ((int)s.height >> 1) + FlxU.random() * height;
			s.velocity.X = minParticleSpeed.X;
			if(minParticleSpeed.X != maxParticleSpeed.X) s.velocity.X += FlxU.random()*(maxParticleSpeed.X-minParticleSpeed.X);
			s.velocity.Y = minParticleSpeed.Y;
			if(minParticleSpeed.Y != maxParticleSpeed.Y) s.velocity.Y += FlxU.random()*(maxParticleSpeed.Y-minParticleSpeed.Y);
			s.acceleration.Y = gravity;
			s.angularVelocity = minRotation;
			if(minRotation != maxRotation) s.angularVelocity += FlxU.random()*(maxRotation-minRotation);
			if(s.angularVelocity != 0) s.angle = FlxU.random()*360-180;
			s.drag.X = particleDrag.X;
			s.drag.Y = particleDrag.Y;
			_particle++;
			if(_particle >= members.Count)
				_particle = 0;
			s.onEmit();
			justEmitted = true;
		}

		/**
		 * Call this function to stop the emitter without killing it.
		 * 
		 * @param	Delay	How long to wait before killing all the particles.  Set to 'zero' to never kill them.
		 */
        public void stop()
        {
            stop(3f);
        }
		public void stop(float Delay)
		{
			_explode = true;
			delay = Delay;
			if(delay < 0)
				delay = -Delay;
			on = false;
		}

		/**
		 * Change the emitter's position to the origin of a <code>FlxObject</code>.
		 * 
		 * @param	Object		The <code>FlxObject</code> that needs to spew particles.
		 */
		public void at(FlxObject Object)
		{
			x = Object.x + Object.origin.X;
			y = Object.y + Object.origin.Y;
		}
		
		/**
		 * Call this function to turn off all the particles and the emitter.
		 */
        override public void kill()
		{
			base.kill();
			on = false;
		}

    }
}
