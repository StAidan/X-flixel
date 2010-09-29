using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    public partial class FlxGame : DrawableGameComponent
    {
        private FlxState _firstScreen;

        private SoundEffect _sndBeep;

        //basic display stuff
        internal FlxState _state;
        private RenderTarget2D backRender;
        internal int targetWidth = 0;
        internal int targetLeft = 0;

		//basic update stuff
		private bool _paused;

        //Pause screen, sound tray, support panel, dev console, and special effects objects
        internal FlxPause _pausePanel;
        internal bool _soundTrayVisible;
        internal Rectangle _soundTrayRect;
        internal float _soundTrayTimer;
        internal FlxSprite[] _soundTrayBars;
        internal FlxText _soundCaption;
        internal FlxConsole _console;

		//pause stuff
        private string[] _helpStrings;

        //effect stuff
        private Point _quakeOffset = Point.Zero;

        //@desc		Constructor
        //@param	GameSizeX		The width of your game in pixels (e.g. 320)
        //@param	GameSizeY		The height of your game in pixels (e.g. 240)
        //@param	InitialState	The class name of the state you want to create and switch to first (e.g. MenuState)
        //@param	BGColor			The color of the app's background
        //@param	FlixelColor		The color of the great big 'f' in the flixel logo
        public void initGame(int GameSizeX, int GameSizeY,
            FlxState InitialState, Color BGColor,
            bool showFlixelLogo, Color logoColor)
        {
            FlxG.backColor = BGColor;
            FlxG.setGameData(this, GameSizeX, GameSizeY);

            _paused = false;

            //activate the first screen.
            if (showFlixelLogo == false)
            {
                _firstScreen = InitialState;
            }
            else
            {
                FlxSplash.setSplashInfo(logoColor, InitialState);
                _firstScreen = new FlxSplash();
            }
        }

		//@desc		Sets up the strings that are displayed on the left side of the pause game popup
		//@param	X		What to display next to the X button
		//@param	C		What to display next to the C button
		//@param	Mouse	What to display next to the mouse icon
		//@param	Arrows	What to display next to the arrows icon
		protected void help(string X, string C, string Mouse, string Arrows)
		{
            _helpStrings = new string[4];

			if(X != null)
				_helpStrings[0] = X;
			if(C != null)
				_helpStrings[1] = C;
			if(Mouse != null)
				_helpStrings[2] = Mouse;
			if(Arrows != null)
				_helpStrings[3] = Arrows;

            if (_pausePanel != null)
            {
                _pausePanel.helpX = _helpStrings[0];
                _pausePanel.helpC = _helpStrings[1];
                _pausePanel.helpMouse = _helpStrings[2];
                _pausePanel.helpArrows = _helpStrings[3];
            }
		}

        public override void Initialize()
        {
            base.Initialize();

            backRender = new RenderTarget2D(GraphicsDevice, FlxG.width, FlxG.height, false, SurfaceFormat.Color,
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        //@benbaird initializes the console, the pause overlay, and the soundbar
        private void initConsole()
        {
            //initialize the debug console
            _console = new FlxConsole(targetLeft, targetWidth);

            _console.log(FlxG.LIBRARY_NAME +
                " v" + FlxG.LIBRARY_MAJOR_VERSION.ToString() + "." + FlxG.LIBRARY_MINOR_VERSION.ToString());
            _console.log("---------------------------------------");

            //Pause screen popup
            _pausePanel = new FlxPause();
            if (_helpStrings != null)
            {
                _pausePanel.helpX = _helpStrings[0];
                _pausePanel.helpC = _helpStrings[1];
                _pausePanel.helpMouse = _helpStrings[2];
                _pausePanel.helpArrows = _helpStrings[3];
            }

			//Sound Tray popup
			_soundTrayRect = new Rectangle((FlxG.width - 80) / 2, -30, 80, 30);
			_soundTrayVisible = false;
			
            _soundCaption = new FlxText((FlxG.width - 80) / 2, -10, 80, "VOLUME");
            _soundCaption.setFormat(null, 1, Color.White, FlxJustification.Center, Color.White).height = 10;

			int bx = 10;
			int by = 14;
            _soundTrayBars = new FlxSprite[10];
			for(int i = 0; i < 10; i++)
			{
                _soundTrayBars[i] = new FlxSprite(_soundTrayRect.X + (bx * 1), -i, null);
                _soundTrayBars[i].width = 4;
                _soundTrayBars[i].height = i + 1;
                _soundTrayBars[i].scrollFactor = Vector2.Zero;
				bx += 6;
				by--;
			}
        }

        protected override void LoadContent()
        {
            //load up graphical content used for the flixel engine
            targetWidth = (int)(GraphicsDevice.Viewport.Height * ((float)FlxG.width / (float)FlxG.height));
            targetLeft = (GraphicsDevice.Viewport.Width - targetWidth) / 2;

            FlxG.LoadContent(GraphicsDevice);
            _sndBeep = FlxG.Content.Load<SoundEffect>("Flixel/beep");

            initConsole();

            if (_firstScreen != null)
            {
                FlxG.state = _firstScreen;
                _firstScreen = null;
            }
        }

        protected override void UnloadContent()
        {
            _sndBeep.Dispose();

            if (FlxG.state != null)
            {
                FlxG.state.destroy();
            }
        }

        //@desc		Switch from one FlxState to another
        //@param	State		The class name of the state you want (e.g. PlayState)
        public void switchState(FlxState newscreen)
        {
            FlxG.unfollow();
            FlxG.keys.reset();
            FlxG.gamepads.reset();
            FlxG.mouse.reset();

            FlxG.flash.stop();
            FlxG.fade.stop();
            FlxG.quake.stop();

            if (_state != null)
            {
                _state.destroy();
            }
            _state = newscreen;
            _state.create();
        }

		/**
		 * Internal function to help with basic pause game functionality.
		 */
        internal void unpauseGame()
		{
			//if(!FlxG.panel.visible) flash.ui.Mouse.hide();
			FlxG.resetInput();
			_paused = false;
			//stage.frameRate = _framerate;
		}

		/**
		 * Internal function to help with basic pause game functionality.
		 */
        internal void pauseGame()
		{
            //if((x != 0) || (y != 0))
            //{
            //    x = 0;
            //    y = 0;
            //}
            //flash.ui.Mouse.show();
			_paused = true;
			//stage.frameRate = _frameratePaused;
		}

        //@desc		This is the main game loop
        public override void Update(GameTime gameTime)
        {
            PlayerIndex pi;

            //Frame timing
            FlxG.getTimer = (uint)gameTime.TotalGameTime.TotalMilliseconds;
            FlxG.elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FlxG.elapsed > FlxG.maxElapsed)
                FlxG.elapsed = FlxG.maxElapsed;
            FlxG.elapsed *= FlxG.timeScale;

            //Animate flixel HUD elements
            _console.update();

			if(_soundTrayTimer > 0)
				_soundTrayTimer -= FlxG.elapsed;
			else if(_soundTrayRect.Y > -_soundTrayRect.Height)
			{
				_soundTrayRect.Y -= (int)(FlxG.elapsed * FlxG.height * 2);
                _soundCaption.y = (_soundTrayRect.Y + 4);
                for (int i = 0; i < _soundTrayBars.Length; i++)
                {
                    _soundTrayBars[i].y = (_soundTrayRect.Y + _soundTrayRect.Height - _soundTrayBars[i].height - 2);
                }
				if(_soundTrayRect.Y < -_soundTrayRect.Height)
					_soundTrayVisible = false;
			}

            //State updating
            FlxG.keys.update();
            FlxG.gamepads.update();
            FlxG.mouse.update();
            FlxG.updateSounds();
            if (FlxG.keys.isNewKeyPress(Keys.D0, null, out pi))
            {
                FlxG.mute = !FlxG.mute;
                showSoundTray();
            }
            else if (FlxG.keys.isNewKeyPress(Keys.OemMinus, null, out pi))
            {
                FlxG.mute = false;
                FlxG.volume -= 0.1f;
                showSoundTray();
            }
            else if (FlxG.keys.isNewKeyPress(Keys.OemPlus, null, out pi))
            {
                FlxG.mute = false;
                FlxG.volume += 0.1f;
                showSoundTray();
            }
            else if (FlxG.keys.isNewKeyPress(Keys.D1, null, out pi) || FlxG.keys.isNewKeyPress(Keys.OemTilde, null, out pi))
            {
                _console.toggle();
            }
            else if (FlxG.autoHandlePause && (FlxG.keys.isPauseGame(FlxG.controllingPlayer) || FlxG.gamepads.isPauseGame(FlxG.controllingPlayer)))
            {
                FlxG.pause = !FlxG.pause;
            }

            if (_paused)
                return;

            if (FlxG.state != null)
            {
                //Update the camera and game state
                FlxG.doFollow();
                FlxG.state.update();

                //Update the various special effects
                if (FlxG.flash.exists)
                    FlxG.flash.update();
                if (FlxG.fade.exists)
                    FlxG.fade.update();
                FlxG.quake.update();
                _quakeOffset.X = FlxG.quake.x;
                _quakeOffset.Y = FlxG.quake.y;
            }
        }


        //Rendering
        public override void Draw(GameTime gameTime)
        {
            //Render the screen to our internal game-sized back buffer.
            GraphicsDevice.SetRenderTarget(backRender);
            if (FlxG.state != null)
            {
                FlxG.state.preProcess(FlxG.spriteBatch);
                FlxG.state.render(FlxG.spriteBatch);

                FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                if (FlxG.flash.exists)
                    FlxG.flash.render(FlxG.spriteBatch);
                if (FlxG.fade.exists)
                    FlxG.fade.render(FlxG.spriteBatch);

                if (FlxG.mouse.cursor.visible)
                    FlxG.mouse.cursor.render(FlxG.spriteBatch);

                FlxG.spriteBatch.End();

                FlxG.state.postProcess(FlxG.spriteBatch);
            }
            //Render sound tray if necessary
            if (_soundTrayVisible || _paused)
            {
                FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                //GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
                //GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
                //GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
                if (_soundTrayVisible)
                {
                    FlxG.spriteBatch.Draw(FlxG.XnaSheet, _soundTrayRect,
                        new Rectangle(1,1,1,1), _console.color);
                    _soundCaption.render(FlxG.spriteBatch);
                    for (int i = 0; i < _soundTrayBars.Length; i++)
                    {
                        _soundTrayBars[i].render(FlxG.spriteBatch);
                    }
                }
                if (_paused)
                {
                    _pausePanel.render(FlxG.spriteBatch);
                }
                FlxG.spriteBatch.End();
            }
            GraphicsDevice.SetRenderTarget(null);

            //Copy the result to the screen, scaled to fit
            GraphicsDevice.Clear(FlxG.backColor);
            FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            //GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            //GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            //GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
            FlxG.spriteBatch.Draw(backRender,
                new Rectangle(targetLeft + _quakeOffset.X, _quakeOffset.Y, targetWidth, GraphicsDevice.Viewport.Height),
                Color.White);
            //Render console if necessary
            if (_console.visible)
            {
                _console.render(FlxG.spriteBatch);
            }
            FlxG.spriteBatch.End();
        }

		//@desc		This function is only used by the FlxGame class to do important internal management stuff
		private void showSoundTray()
		{
            if (!FlxG.mute)
            {
                _sndBeep.Play(FlxG.volume, 0f, 0f);
            }
			_soundTrayTimer = 1;
			_soundTrayRect.Y = 0;
			_soundTrayVisible = true;

            _soundCaption.y = (_soundTrayRect.Y + 4);

			int gv = (int)Math.Round(FlxG.volume * 10);
			if(FlxG.mute)
				gv = 0;
			for (int i = 0; i < _soundTrayBars.Length; i++)
			{
                _soundTrayBars[i].y = (_soundTrayRect.Y + _soundTrayRect.Height - _soundTrayBars[i].height - 2);
				if(i < gv) _soundTrayBars[i].alpha = 1;
				else _soundTrayBars[i].alpha = 0.5f;
			}
		}

    }

}
