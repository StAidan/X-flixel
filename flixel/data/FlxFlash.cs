using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    public class FlxFlash : FlxSprite
    {
		/**
		 * How long the effect should last.
		 */
		protected float _delay;
		/**
		 * Callback for when the effect is finished.
		 */
		protected EventHandler<FlxEffectCompletedEvent> _complete;
		
		/**
		 * Constructor initializes the fade object
		 */
        public FlxFlash()
            : base(0, 0)
		{
            createGraphic(FlxG.width, FlxG.height, Color.Black);
			scrollFactor.X = 0;
			scrollFactor.Y = 0;
			exists = false;
			solid = false;
			@fixed = true;
		}

		/**
		 * Reset and trigger this special effect
		 * 
		 * @param	Color			The color you want to use
		 * @param	Duration		How long it takes for the flash to fade
		 * @param	FlashComplete	A function you want to run when the flash finishes
		 * @param	Force			Force the effect to reset
		 */
        public void start(Color Color)
        {
            start(Color, 1f, null, false);
        }
        public void start(Color Color, float Duration)
        {
            start(Color, Duration, null, false);
        }
        public void start(Color Color, float Duration, EventHandler<FlxEffectCompletedEvent> FlashComplete, bool Force)
		{
			if(!Force && exists) return;
            color = Color;
			_delay = Duration;
			_complete = FlashComplete;
			alpha = 1;
			exists = true;
		}

		/**
		 * Stops and hides this screen effect.
		 */
        public void stop()
		{
			exists = false;
		}

		/**
		 * Updates and/or animates this special effect
		 */
        override public void update()
		{
			alpha -= FlxG.elapsed/_delay;
			if(alpha <= 0)
			{
				exists = false;
				if(_complete != null)
					_complete(this, new FlxEffectCompletedEvent(EffectType.Flash));
			}
		}

    }
}
