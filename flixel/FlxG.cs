using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{

    public delegate void FlxAnimationCallback(string Name, uint Frame, int FrameIndex);

    /**
     * This is a global helper class full of useful functions for audio,
     * input, basic info, and the camera system among other things.
     */
    public class FlxG
    {
        //@benbaird Global, XNA-specific stuff that any component should be able
        // to access from anywhere. (As read-only properties, generally.)
        public static Game Game;
        private static ContentManager _content;
        private static Texture2D _xnatiles;
        private static SpriteFont _font;
        private static SpriteBatch _spriteBatch;

		/**
		 * If you build and maintain your own version of flixel,
		 * you can give it your own name here.  Appears in the console.
		 */
		static public string LIBRARY_NAME = "X-flixel";
		/**
		 * Assign a major version to your library.
		 * Appears before the decimal in the console.
		 */
		static public uint LIBRARY_MAJOR_VERSION = 2;
		/**
		 * Assign a minor version to your library.
		 * Appears after the decimal in the console.
		 */
		static public uint LIBRARY_MINOR_VERSION = 43;

        /**
         * Internal tracker for game object (so we can pause & unpause)
         */
        static protected internal FlxGame _game;
		/**
		 * Internal tracker for game pause state.
		 */
		static protected bool _pause;
		/**
		 * Whether you are running in Debug or Release mode.
		 * Set automatically by <code>FlxFactory</code> during startup.
		 */
		static public bool debug;
		/**
		 * Set <code>showBounds</code> to true to display the bounding boxes of the in-game objects.
		 */
		static public bool showBounds;

        /**
         * Represents the amount of time in seconds that passed since last frame.
         */
        public static float elapsed = 0f;
        //@benbaird compatibility with AS3's getTimer()
        public static uint getTimer = 0;
		/**
		 * Essentially locks the framerate to a minimum value - any slower and you'll get slowdown instead of frameskip; default is 1/30th of a second.
		 */
		static public float maxElapsed;
		/**
		 * How fast or slow time should pass in the game; default is 1.0.
		 */
		static public float timeScale;

        //@desc A reference or pointer to the current FlxState object being used by the game
        public static FlxState state
        {
            get
            {
                return _game._state;
            }
            set
            {
                _game.switchState(value);
            }
        }

        /**
         * The width of the screen in game pixels.
         */
        public static int width = 1280;
        /**
         * The height of the screen in game pixels.
         */
        public static int height = 720;

        public static Color backColor = Color.Black;

		/**
		 * Setting this to true will disable/skip stuff that isn't necessary for mobile platforms like Android (or Windows Phone 7). [BETA]
		 */
		static public bool mobile; 

		/**
		 * <code>FlxG.levels</code> and <code>FlxG.scores</code> are generic
		 * global variables that can be used for various cross-state stuff.
		 */
        static public List<int> levels = new List<int>();
        static public int level;
        static public List<int> scores = new List<int>();
        static public int score;
		/**
		 * <code>FlxG.saves</code> is a generic bucket for storing
		 * FlxSaves so you can access them whenever you want.
		 */
#if !WINDOWS_PHONE
        static public List<FlxSave> saves = new List<FlxSave>(); 
        static public int save;
#endif

        //@benbaird X-flixel only. Returns the scale of the screen size in comparison to the actual game size.
        private static float _scale = 0;
        public static float scale
        {
            get { return _scale; }
        }

		/**
		 * A reference to a <code>FlxMouse</code> object.  Important for input!
		 */
		static public FlxMouse mouse = new FlxMouse();
		/**
		 * A reference to a <code>FlxKeyboard</code> object.  Important for input!
		 */
		static public FlxKeyboard keys = new FlxKeyboard();
		/**
		 * An array of <code>FlxGamepad</code> objects.  Important for input!
		 */
		static public FlxGamepad gamepads = new FlxGamepad();

        //@benbaird Used for compatibility with Xbox input standards
        public static PlayerIndex? controllingPlayer
        {
            get;
            set;
        }

		/**
		 * A handy container for a background music object.
		 */
		static public FlxSound music;
		/**
		 * A list of all the sounds being played in the game.
		 */
		static public List<FlxSound> sounds = new List<FlxSound>();
		/**
		 * Internal flag for whether or not the game is muted.
		 */
		static protected bool _mute;
		/**
		 * Internal volume level, used for global sound control.
		 */
		static protected float _volume;

		/**
		 * Tells the camera to follow this <code>FlxCore</code> object around.
		 */
		static public FlxObject followTarget;
		/**
		 * Used to force the camera to look ahead of the <code>followTarget</code>.
		 */
		static public Vector2 followLead;
		/**
		 * Used to smoothly track the camera as it follows.
		 */
		static public float followLerp;
		/**
		 * Stores the top and left edges of the camera area.
		 */
		static public Point followMin;
		/**
		 * Stores the bottom and right edges of the camera area.
		 */
		static public Point followMax;
		/**
		 * Internal, used to assist camera and scrolling.
		 */
		static protected Vector2 _scrollTarget;

        /**
         * Stores the basic parallax scrolling values.
         */
        static public Vector2 scroll;

		/**
		 * Reference to the active graphics buffer.
		 * Can also be referenced via <code>FlxState.screen</code>.
		 */
        //static public var buffer:BitmapData;
		/**
		 * Internal storage system to prevent graphics from being used repeatedly in memory.
		 */
        //static protected var _cache:Object;

		/**
		 * Access to the Kongregate high scores and achievements API.
		 */
        //static public var kong:FlxKong;

		/**
		 * The support panel (twitter, reddit, stumbleupon, paypal, etc) visor thing
		 */
        //static public FlxPanel panel;
		/**
		 * A special effect that shakes the screen.  Usage: FlxG.quake.start();
		 */
		static public FlxQuake quake;
		/**
		 * A special effect that flashes a color on the screen.  Usage: FlxG.flash.start();
		 */
		static public FlxFlash flash;
		/**
		 * A special effect that fades a color onto the screen.  Usage: FlxG.fade.start();
		 */
        static public FlxFade fade;

        /**
         * Log data to the developer console.
         * 
         * @param	Data		Anything you want to log to the console.
         */
        public static void log(string Data) { _game._console.log(Data); }

        /**
         * Set <code>pause</code> to true to pause the game, all sounds, and display the pause popup.
         */
        static public bool pause
        {
            get { return _pause; }
            set
            {
                if (_pause != value)
                {
                    _pause = value;
                    if (_pause)
                    {
                        _game.pauseGame();
                        pauseSounds();
                    }
                    else
                    {
                        _game.unpauseGame();
                        playSounds();
                    }
                }
            }
        }

        //@benbaird Begin XNA-specific public static properties
        public static bool autoHandlePause = false; //whether to automatically handle user pause requests. Typically you'd set this to true only for gameplay states, and set to false for all others (menus, etc.)

        public static ContentManager Content
        {
            get { return _content; }
        }
        public static SpriteFont Font
        {
            get { return _font; }
        }
        public static SpriteBatch spriteBatch
        {
            get { return _spriteBatch; }
        }
        public static Texture2D XnaSheet
        {
            get { return _xnatiles; }
        }

        public static void LoadContent(GraphicsDevice gd)
        {
            _content = Game.Content;

            _spriteBatch = new SpriteBatch(gd);
            _font = _content.Load<SpriteFont>("flixel/deffont");
            _xnatiles = _content.Load<Texture2D>("flixel/xna_tiles");

            _scale = ((float)_game.targetWidth / (float)width);
            FlxG.quake = new FlxQuake((int)_scale);
            FlxG.flash = new FlxFlash();
            FlxG.fade = new FlxFade();
        }
        //@benbaird End XNA-specific public static properties

		/**
		 * Reset the input helper objects (useful when changing screens or states)
		 */
		static public void resetInput()
		{
			keys.reset();
			mouse.reset();
            gamepads.reset();
		}

		/**
		 * Set up and play a looping background soundtrack.
		 * 
		 * @param	Music		The sound file you want to loop in the background.
		 * @param	Volume		How loud the sound should be, from 0 to 1.
		 */
        static public void playMusic(string Music)
        {
            playMusic(Music, 1.0f);
        }
		static public void playMusic(string Music, float Volume)
		{
			if(music == null)
				music = new FlxSound();
			else if(music.active)
				music.stop();
			music.loadEmbedded(Music,true);
			music.volume = Volume;
			music.survive = true;
			music.play();
		}

		/**
		 * Creates a new sound object from an embedded <code>Class</code> object.
		 * 
		 * @param	EmbeddedSound	The sound you want to play.
		 * @param	Volume			How loud to play it (0 to 1).
		 * @param	Looped			Whether or not to loop this sound.
		 * 
		 * @return	A <code>FlxSound</code> object.
		 */
        static public FlxSound play(string EmbeddedSound)
        {
            return play(EmbeddedSound, 1.0f, false);
        }
        static public FlxSound play(string EmbeddedSound, float Volume)
        {
            return play(EmbeddedSound, Volume, false);
        }
        static public FlxSound play(string EmbeddedSound, float Volume, bool Looped)
		{
			int i = 0;
			int sl = sounds.Count;
			while(i < sl)
			{
				if(!(sounds[i] as FlxSound).active)
					break;
				i++;
			}
			if(i >= sl)
				sounds.Add(new FlxSound());
            sounds[i].loadEmbedded(EmbeddedSound, Looped);
            sounds[i].volume = Volume;
            sounds[i].play();
            return sounds[i];
		}

        /**
         * Set <code>mute</code> to true to turn off the sound.
         * 
         * @default false
         */
        public static bool mute
        {
            get { return _mute; }
            set { _mute = value; changeSounds(); }
        }

		/**
		 * Get a number that represents the mute state that we can multiply into a sound transform.
		 * 
		 * @return		An unsigned integer - 0 if muted, 1 if not muted.
		 */
		static public int getMuteValue()
		{
			if(_mute)
				return 0;
			else
				return 1;
		}

        static public float volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                if (_volume < 0)
                    _volume = 0;
                else if (_volume > 1)
                    _volume = 1;
                changeSounds();
            }
        }

		/**
		 * Called by FlxGame on state changes to stop and destroy sounds.
		 * 
		 * @param	ForceDestroy		Kill sounds even if they're flagged <code>survive</code>.
		 */
		static internal void destroySounds(bool ForceDestroy)
		{
			if(sounds == null)
				return;
			if((music != null) && (ForceDestroy || !music.survive))
				music.destroy();
			int i = 0;
			FlxSound s;
			int sl = sounds.Count;
			while(i < sl)
			{
				s = sounds[i++] as FlxSound;
				if((s != null) && (ForceDestroy || !s.survive))
					s.destroy();
			}
		}

		/**
		 * An internal function that adjust the volume levels and the music channel after a change.
		 */
		static protected void changeSounds()
		{
			if((music != null) && music.active)
				music.updateTransform();
			int i = 0;
			FlxSound s;
			int sl = sounds.Count;
			while(i < sl)
			{
				s = sounds[i++] as FlxSound;
				if((s != null) && s.active)
					s.updateTransform();
			}
		}

		/**
		 * Called by the game loop to make sure the sounds get updated each frame.
		 */
		static internal void updateSounds()
		{
			if((music != null) && music.active)
				music.update();
			int i = 0;
			FlxSound s;
			int sl = sounds.Count;
			while(i < sl)
			{
				s = sounds[i++] as FlxSound;
				if((s != null) && s.active)
					s.update();
			}
		}

		/**
		 * Internal helper, pauses all game sounds.
		 */
		static protected void pauseSounds()
		{
			if((music != null) && music.active)
				music.pause();
			int i = 0;
			FlxSound s;
			int sl = sounds.Count;
			while(i < sl)
			{
				s = sounds[i++] as FlxSound;
				if((s != null) && s.active)
					s.pause();
			}
		}

		/**
		 * Internal helper, pauses all game sounds.
		 */
		static protected void playSounds()
		{
			if((music != null) && music.active)
				music.play();
			int i = 0;
            FlxSound s;
			int sl = sounds.Count;
			while(i < sl)
			{
				s = sounds[i++] as FlxSound;
				if((s != null) && s.active)
					s.play();
			}
		}

		//@desc		Tells the camera subsystem what FlxCore object to follow
		//@param	Target		The object to follow
		//@param	Lerp		How much lag the camera should have (can help smooth out the camera movement)
		static public void follow(FlxObject Target, float Lerp)
		{
			followTarget = Target;
			followLerp = Lerp;

            if (Target == null)
                return;

            scroll.X = _scrollTarget.X = (width >> 1) - followTarget.x - ((int)followTarget.width >> 1);
            scroll.Y = _scrollTarget.Y = (height >> 1) - followTarget.y - ((int)followTarget.height >> 1);
		}
		
		//@desc		Specify an additional camera component - the velocity-based "lead", or amount the camera should track in front of a sprite
		//@param	LeadX		Percentage of X velocity to add to the camera's motion
		//@param	LeadY		Percentage of Y velocity to add to the camera's motion
		static public void followAdjust(float LeadX, float LeadY)
		{
			followLead = new Vector2(LeadX, LeadY);
		}

        /**
         * Specify the boundaries of the level or where the camera is allowed to move.
         * 
         * @param	MinX				The smallest X value of your level (usually 0).
         * @param	MinY				The smallest Y value of your level (usually 0).
         * @param	MaxX				The largest X value of your level (usually the level width).
         * @param	MaxY				The largest Y value of your level (usually the level height).
         * @param	UpdateWorldBounds	Whether the quad tree's dimensions should be updated to match.
         */
        static public void followBounds(int MinX, int MinY, int MaxX, int MaxY)
        {
            followBounds(MinX, MinY, MaxX, MaxY, true);
        }
        static public void followBounds(int MinX, int MinY, int MaxX, int MaxY, bool UpdateWorldBounds)
		{
            followMin = new Point(-MinX, -MinY);
            followMax = new Point(-MaxX + width, -MaxY + height);
            if (followMax.X > followMin.X)
                followMax.X = followMin.X;
            if (followMax.Y > followMin.Y)
                followMax.Y = followMin.Y;
            if (UpdateWorldBounds)
                FlxU.setWorldBounds(MinX, MinY, MaxX - MinX, MaxY - MinY);
            doFollow();
        }

        /**
         * Stops and resets the camera.
         */
        internal static void unfollow()
        {
            followTarget = null;
            followLead = Vector2.Zero;
            followLerp = 1;
            followMin = Point.Zero;
            followMax = Point.Zero;
            scroll = new Vector2();
            _scrollTarget = new Vector2();
        }

        /**
         * Called by <code>FlxGame</code> to set up <code>FlxG</code> during <code>FlxGame</code>'s constructor.
         */
        static internal void setGameData(FlxGame Game, int Width, int Height)
		{
            _game = Game;
			width = Width;
			height = Height;

            _mute = false;
            _volume = 0.5f;

			unfollow();

            level = 0;
            score = 0;

            pause = false;
            timeScale = 1.0f;
            maxElapsed = 0.0333f;
            FlxG.elapsed = 0;
            showBounds = false;
#if !WINDOWS_PHONE
            mobile = false;
#else
            mobile = true;
#endif
            FlxU.setWorldBounds(0, 0, FlxG.width, FlxG.height);
        }


        /**
         * Internal function that updates the camera and parallax scrolling.
         */
        internal static void doFollow()
		{
			if(followTarget != null)
			{
				if(followTarget.exists && !followTarget.dead)
				{
                    _scrollTarget.X = (width >> 1) - followTarget.x - ((int)followTarget.width >> 1);
                    _scrollTarget.Y = (height >> 1) - followTarget.y - ((int)followTarget.height >> 1);
					if((followLead != null) && (followTarget is FlxSprite))
					{
                        _scrollTarget.X -= (followTarget as FlxSprite).velocity.X * followLead.X;
                        _scrollTarget.Y -= (followTarget as FlxSprite).velocity.Y * followLead.Y;
					}
				}
                scroll.X += (_scrollTarget.X - scroll.X) * followLerp * FlxG.elapsed;
                scroll.Y += (_scrollTarget.Y - scroll.Y) * followLerp * FlxG.elapsed;

				if(followMin != null)
				{
					if(scroll.X > followMin.X)
						scroll.X = followMin.X;
					if(scroll.Y > followMin.Y)
						scroll.Y = followMin.Y;
				}
				
				if(followMax != null)
				{
					if(scroll.X < followMax.X)
						scroll.X = followMax.X;
					if(scroll.Y < followMax.Y)
                        scroll.Y = followMax.Y;
				}
			}
		}

		
    }
}
