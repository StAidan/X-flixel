using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    //@desc		Just a helper structure for the FlxSprite animation system
    public class FlxAnim
    {
		public string name;
		public float delay;
		public int[] frames;
		public bool looped;

		//@desc		Constructor
		//@param	Name		What this animation should be called (e.g. "run")
		//@param	Frames		An array of numbers indicating what frames to play in what order (e.g. 1, 2, 3)
		//@param	FrameRate	The speed in frames per second that the animation should play at (e.g. 40 fps)
		//@param	Looped		Whether or not the animation is looped or just plays once
		public FlxAnim(string Name, int[] Frames, int FrameRate, bool Looped)
		{
			name = Name;
			delay = 1.0f / (float)FrameRate;
			frames = Frames;
			looped = Looped;
		}
        //@desc Constructor overloads
        public FlxAnim(string Name, int[] Frames, int FrameRate)
        {
            name = Name;
            delay = 1.0f / (float)FrameRate;
            frames = Frames;
            looped = true;
        }
        public FlxAnim(string Name, int[] Frames)
        {
            name = Name;
            delay = 0f;
            frames = Frames;
            looped = true;
        }

    }

}
