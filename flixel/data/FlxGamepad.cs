/*==============================================
 * 
 * FlxGamepad class
 * 
 * Comments: Credit where it's due. This class is a
 * modified and enhanced version of the InputState class
 * found in the XNACC GameStateManagement example.
 * Among other things, the menu navigation functions
 * now have a repeat timer to remove the need for
 * multiple dpad/stick presses.
 * 
 * Due to many varied reasons, the FlxKeyboard and FlxGamepad
 * classes bear little resemblance to their official flixel
 * counterparts. FlxKeyboard may receive further alignment
 * later on, but for now I felt that it's simpler to use
 * more XNA-like interfaces for input.
 * 
 * You will probably want to use the functions in this class
 * if you're developing a multiplayer game with X-flixel.
 * The functions directly in FlxG were designed for single-
 * player experiences.
 * 
 *============================================*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    public class FlxGamepad
    {
        private PlayerIndex pi;

        /*==============================================
         * 
         * Properties
         * 
         *============================================*/
        public const int MaxInputs = 4;
        public readonly bool[] GamePadWasConnected;

        /*==============================================
         * 
         * Private members
         * 
         *============================================*/
        private GamePadState[] _curGamepad;
        private GamePadState[] _lastGamepad;

        private const float REPEAT_TIMER_LONG = 0.5f;
        private const float REPEAT_TIMER = 0.1f;
        private float _timerCountDown = 0.0f;

        /*==============================================
         * 
         * Properties
         * 
         *============================================*/
        public GamePadState[] curPadState
        {
            get { return _curGamepad; }
        }

        public FlxGamepad()
        {
            _curGamepad = new GamePadState[MaxInputs];
            _lastGamepad = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }

        /*==============================================
        * 
        * Timing
        * 
        *============================================*/
        public void update()
        {
            if (_timerCountDown > 0.0f)
            {
                _timerCountDown -= FlxG.elapsed;
            }

            for (int i = 0; i < MaxInputs; i++)
            {
                _lastGamepad[i] = _curGamepad[i];

                _curGamepad[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (_curGamepad[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }
        }

        /*==============================================
       * 
       * Keyboard standard input
       * 
       *============================================*/
        public bool isNewButtonPress(Buttons button)
        {
            return isNewButtonPress(button, FlxG.controllingPlayer, out pi);
        }
        public bool isNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                             out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (_curGamepad[i].IsButtonDown(button) &&
                        _lastGamepad[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (isNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        isNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        isNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        isNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }

        public bool isNewButtonRelease(Buttons button)
        {
            return isNewButtonRelease(button, FlxG.controllingPlayer, out pi);
        }
        public bool isNewButtonRelease(Buttons button, PlayerIndex? controllingPlayer,
                                             out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (_curGamepad[i].IsButtonUp(button) &&
                        _lastGamepad[i].IsButtonDown(button));
            }
            else
            {
                // Accept input from any player.
                return (isNewButtonRelease(button, PlayerIndex.One, out playerIndex) ||
                        isNewButtonRelease(button, PlayerIndex.Two, out playerIndex) ||
                        isNewButtonRelease(button, PlayerIndex.Three, out playerIndex) ||
                        isNewButtonRelease(button, PlayerIndex.Four, out playerIndex));
            }
        }

        public bool isButtonDown(Buttons button)
        {
            return isButtonDown(button, FlxG.controllingPlayer, out pi);
        }
        public bool isButtonDown(Buttons button, PlayerIndex? controllingPlayer,
                                             out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;
                if (button == Buttons.LeftThumbstickLeft)
                {
                    return (_curGamepad[i].ThumbSticks.Left.X < -0.25);
                }
                else if (button == Buttons.LeftThumbstickRight)
                {
                    return (_curGamepad[i].ThumbSticks.Left.X > 0.25);
                }
                else if (button == Buttons.LeftThumbstickUp)
                {
                    return (_curGamepad[i].ThumbSticks.Left.Y > 0.25);
                }
                else if (button == Buttons.LeftThumbstickDown)
                {
                    return (_curGamepad[i].ThumbSticks.Left.Y < -0.25);
                }
                else
                {
                    return _curGamepad[i].IsButtonDown(button);
                }
            }
            else
            {
                // Accept input from any player.
                return (isButtonDown(button, PlayerIndex.One, out playerIndex) ||
                        isButtonDown(button, PlayerIndex.Two, out playerIndex) ||
                        isButtonDown(button, PlayerIndex.Three, out playerIndex) ||
                        isButtonDown(button, PlayerIndex.Four, out playerIndex));
            }
        }

        /*==============================================
        * 
        * Gamepad menu input
        * 
        *============================================*/
        public void reset()
        {
            //called by the menu navigation functions, and by
            //the screen manager during screen
            //transitions.
            for (int i = 0; i < MaxInputs; i++)
            {
                _curGamepad[i] = _lastGamepad[i];
            }
        }

        public bool isMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return isNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                   isNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }

        public bool isMenuCancel(PlayerIndex? controllingPlayer,
                         out PlayerIndex playerIndex)
        {
            return isNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                   isNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }

        public bool isMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (isNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }
            else if (isNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex))
            {
                GamePadState gamePadState = _curGamepad[(int)playerIndex];
                if (gamePadState.ThumbSticks.Left.Y > 0.25)
                {
                    _timerCountDown = REPEAT_TIMER_LONG;
                    return true;
                }
                else
                {
                    reset();
                }
            }

            GamePadState repeatPadState = _curGamepad[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if ((repeatPadState.ThumbSticks.Left.Y > 0.25) ||
                    repeatPadState.IsButtonDown(Buttons.DPadUp))
                {
                    _timerCountDown = REPEAT_TIMER;
                    return true;
                }
            }

            return false;
        }

        public bool isMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (isNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }
            else if (isNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex))
            {
                GamePadState gamePadState = _curGamepad[(int)playerIndex];
                if (gamePadState.ThumbSticks.Left.Y < -0.25)
                {
                    _timerCountDown = REPEAT_TIMER_LONG;
                    return true;
                }
                else
                {
                    reset();
                }
            }

            GamePadState repeatPadState = _curGamepad[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if ((repeatPadState.ThumbSticks.Left.Y < -0.25) ||
                    repeatPadState.IsButtonDown(Buttons.DPadDown))
                {
                    _timerCountDown = REPEAT_TIMER;
                    return true;
                }
            }

            return false;
        }

        public bool isMenuLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (isNewButtonPress(Buttons.DPadLeft, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }
            else if (isNewButtonPress(Buttons.LeftThumbstickLeft, controllingPlayer, out playerIndex))
            {
                GamePadState gamePadState = _curGamepad[(int)playerIndex];
                if (gamePadState.ThumbSticks.Left.X < -0.25)
                {
                    _timerCountDown = REPEAT_TIMER_LONG;
                    return true;
                }
                else
                {
                    reset();
                }
            }

            GamePadState repeatPadState = _curGamepad[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if ((repeatPadState.ThumbSticks.Left.X < -0.25) ||
                    repeatPadState.IsButtonDown(Buttons.DPadLeft))
                {
                    _timerCountDown = REPEAT_TIMER;
                    return true;
                }
            }
            return false;
        }

        public bool isMenuRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (isNewButtonPress(Buttons.DPadRight, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }
            else if (isNewButtonPress(Buttons.LeftThumbstickRight, controllingPlayer, out playerIndex))
            {
                GamePadState gamePadState = _curGamepad[(int)playerIndex];
                if (gamePadState.ThumbSticks.Left.X > 0.25)
                {
                    _timerCountDown = REPEAT_TIMER_LONG;
                    return true;
                }
                else
                {
                    reset();
                }
            }

            GamePadState repeatPadState = _curGamepad[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if ((repeatPadState.ThumbSticks.Left.X > 0.25) ||
                    repeatPadState.IsButtonDown(Buttons.DPadRight))
                {
                    _timerCountDown = REPEAT_TIMER;
                    return true;
                }
            }

            return false;
        }


        public bool isPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return isNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        //@benbaird used for flixel compatibility
        public bool isNewThumbstickLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex pi;

            if (isButtonDown(Buttons.LeftThumbstickLeft, controllingPlayer, out pi))
            {
                if ((_curGamepad[(int)pi].ThumbSticks.Left.X < -0.25) &&
                    (_lastGamepad[(int)pi].ThumbSticks.Left.X >= -0.25))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isNewThumbstickRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex pi;

            if (isButtonDown(Buttons.LeftThumbstickRight, controllingPlayer, out pi))
            {
                if ((_curGamepad[(int)pi].ThumbSticks.Left.X > 0.25) &&
                    (_lastGamepad[(int)pi].ThumbSticks.Left.X <= 0.25))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isNewThumbstickUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex pi;

            if (isButtonDown(Buttons.LeftThumbstickUp, controllingPlayer, out pi))
            {
                if ((_curGamepad[(int)pi].ThumbSticks.Left.Y > 0.25) &&
                    (_lastGamepad[(int)pi].ThumbSticks.Left.Y <= 0.25))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isNewThumbstickDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex pi;

            if (isButtonDown(Buttons.LeftThumbstickDown, controllingPlayer, out pi))
            {
                if ((_curGamepad[(int)pi].ThumbSticks.Left.Y < -0.25) &&
                    (_lastGamepad[(int)pi].ThumbSticks.Left.Y >= -0.25))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
