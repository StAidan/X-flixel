using System;
using System.Collections.Generic;

namespace org.flixel
{
    public class FlxQuake
    {
		/**
		 * The game's level of zoom.
		 */
		protected int _zoom;
		/**
		 * The intensity of the quake effect: a percentage of the screen's size.
		 */
		protected float _intensity;
		/**
		 * Set to countdown the quake time.
		 */
		protected float _timer;

		/**
		 * The amount of X distortion to apply to the screen.
		 */
		public int x = 0;
		/**
		 * The amount of Y distortion to apply to the screen.
		 */
		public int y = 0;

		/**
		 * Constructor.
		 */
		public FlxQuake(int Zoom)
		{
			_zoom = Zoom;
			start(0, 0);
		}

		/**
		 * Reset and trigger this special effect.
		 * 
		 * @param	Intensity	Percentage of screen size representing the maximum distance that the screen can move during the 'quake'.
		 * @param	Duration	The length in seconds that the "quake" should last.
		 */
		public void start(float Intensity, float Duration)
		{
			stop();
			_intensity = Intensity;
			_timer = Duration;
		}

		/**
		 * Stops this screen effect.
		 */
        public void stop()
		{
			x = 0;
			y = 0;
			_intensity = 0;
			_timer = 0;
		}

		/**
		 * Updates and/or animates this special effect.
		 */
        public void update()
		{
			if(_timer > 0)
			{
				_timer -= FlxG.elapsed;
				if(_timer <= 0)
				{
					_timer = 0;
					x = 0;
					y = 0;
				}
				else
				{
					x = (int)(FlxU.random()*_intensity*FlxG.width*2-_intensity*FlxG.width)*_zoom;
                    y = (int)(FlxU.random() * _intensity * FlxG.height * 2 - _intensity * FlxG.height) * _zoom;
				}
			}
		}

    }
}
