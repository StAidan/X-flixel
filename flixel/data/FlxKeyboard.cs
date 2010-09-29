/*==============================================
 * 
 * FlxKeyboard class
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
 *============================================*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{

    public class FlxKeyboard
    {
        private PlayerIndex pi;

        public bool ESCAPE
        {
            get { return isKeyDown(Keys.Escape, FlxG.controllingPlayer, out pi); }
        }
        public bool F1
        {
            get { return isKeyDown(Keys.F1, FlxG.controllingPlayer, out pi); }
        }
        public bool F2
        {
            get { return isKeyDown(Keys.F2, FlxG.controllingPlayer, out pi); }
        }
        public bool F3
        {
            get { return isKeyDown(Keys.F3, FlxG.controllingPlayer, out pi); }
        }
        public bool F4
        {
            get { return isKeyDown(Keys.F4, FlxG.controllingPlayer, out pi); }
        }
        public bool F5
        {
            get { return isKeyDown(Keys.F5, FlxG.controllingPlayer, out pi); }
        }
        public bool F6
        {
            get { return isKeyDown(Keys.F6, FlxG.controllingPlayer, out pi); }
        }
        public bool F7
        {
            get { return isKeyDown(Keys.F7, FlxG.controllingPlayer, out pi); }
        }
        public bool F8
        {
            get { return isKeyDown(Keys.F8, FlxG.controllingPlayer, out pi); }
        }
        public bool F9
        {
            get { return isKeyDown(Keys.F9, FlxG.controllingPlayer, out pi); }
        }
        public bool F10
        {
            get { return isKeyDown(Keys.F10, FlxG.controllingPlayer, out pi); }
        }
        public bool F11
        {
            get { return isKeyDown(Keys.F11, FlxG.controllingPlayer, out pi); }
        }
        public bool F12
        {
            get { return isKeyDown(Keys.F12, FlxG.controllingPlayer, out pi); }
        }
        public bool ONE
        {
            get { return isKeyDown(Keys.D1, FlxG.controllingPlayer, out pi); }
        }
        public bool TWO
        {
            get { return isKeyDown(Keys.D2, FlxG.controllingPlayer, out pi); }
        }
        public bool THREE
        {
            get { return isKeyDown(Keys.D3, FlxG.controllingPlayer, out pi); }
        }
        public bool FOUR
        {
            get { return isKeyDown(Keys.D4, FlxG.controllingPlayer, out pi); }
        }
        public bool FIVE
        {
            get { return isKeyDown(Keys.D5, FlxG.controllingPlayer, out pi); }
        }
        public bool SIX
        {
            get { return isKeyDown(Keys.D6, FlxG.controllingPlayer, out pi); }
        }
        public bool SEVEN
        {
            get { return isKeyDown(Keys.D7, FlxG.controllingPlayer, out pi); }
        }
        public bool EIGHT
        {
            get { return isKeyDown(Keys.D8, FlxG.controllingPlayer, out pi); }
        }
        public bool NINE
        {
            get { return isKeyDown(Keys.D9, FlxG.controllingPlayer, out pi); }
        }
        public bool ZERO
        {
            get { return isKeyDown(Keys.D0, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADONE
        {
            get { return isKeyDown(Keys.NumPad1, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADTWO
        {
            get { return isKeyDown(Keys.NumPad2, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADTHREE
        {
            get { return isKeyDown(Keys.NumPad3, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADFOUR
        {
            get { return isKeyDown(Keys.NumPad4, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADFIVE
        {
            get { return isKeyDown(Keys.NumPad5, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADSIX
        {
            get { return isKeyDown(Keys.NumPad6, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADSEVEN
        {
            get { return isKeyDown(Keys.NumPad7, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADEIGHT
        {
            get { return isKeyDown(Keys.NumPad8, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADNINE
        {
            get { return isKeyDown(Keys.NumPad9, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADZERO
        {
            get { return isKeyDown(Keys.NumPad0, FlxG.controllingPlayer, out pi); }
        }
        public bool MINUS
        {
            get { return isKeyDown(Keys.OemMinus, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADMINUS
        {
            get { return isKeyDown(Keys.OemMinus, FlxG.controllingPlayer, out pi); }
        }
        public bool PLUS
        {
            get { return isKeyDown(Keys.OemPlus, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADPLUS
        {
            get { return isKeyDown(Keys.OemPlus, FlxG.controllingPlayer, out pi); }
        }
        public bool DELETE
        {
            get { return isKeyDown(Keys.Delete, FlxG.controllingPlayer, out pi); }
        }
        public bool BACKSPACE
        {
            get { return isKeyDown(Keys.Back, FlxG.controllingPlayer, out pi); }
        }
        public bool Q
        {
            get { return isKeyDown(Keys.Q, FlxG.controllingPlayer, out pi); }
        }
        public bool W
        {
            get { return isKeyDown(Keys.W, FlxG.controllingPlayer, out pi); }
        }
        public bool E
        {
            get { return isKeyDown(Keys.E, FlxG.controllingPlayer, out pi); }
        }
        public bool R
        {
            get { return isKeyDown(Keys.R, FlxG.controllingPlayer, out pi); }
        }
        public bool T
        {
            get { return isKeyDown(Keys.T, FlxG.controllingPlayer, out pi); }
        }
        public bool Y
        {
            get { return isKeyDown(Keys.Y, FlxG.controllingPlayer, out pi); }
        }
        public bool U
        {
            get { return isKeyDown(Keys.U, FlxG.controllingPlayer, out pi); }
        }
        public bool I
        {
            get { return isKeyDown(Keys.I, FlxG.controllingPlayer, out pi); }
        }
        public bool O
        {
            get { return isKeyDown(Keys.O, FlxG.controllingPlayer, out pi); }
        }
        public bool P
        {
            get { return isKeyDown(Keys.P, FlxG.controllingPlayer, out pi); }
        }
        public bool LBRACKET
        {
            get { return isKeyDown(Keys.OemOpenBrackets, FlxG.controllingPlayer, out pi); }
        }
        public bool RBRACKET
        {
            get { return isKeyDown(Keys.OemCloseBrackets, FlxG.controllingPlayer, out pi); }
        }
        public bool BACKSLASH
        {
            get { return isKeyDown(Keys.OemBackslash, FlxG.controllingPlayer, out pi); }
        }
        public bool CAPSLOCK
        {
            get { return isKeyDown(Keys.CapsLock, FlxG.controllingPlayer, out pi); }
        }
        public bool A
        {
            get { return isKeyDown(Keys.A, FlxG.controllingPlayer, out pi); }
        }
        public bool S
        {
            get { return isKeyDown(Keys.S, FlxG.controllingPlayer, out pi); }
        }
        public bool D
        {
            get { return isKeyDown(Keys.D, FlxG.controllingPlayer, out pi); }
        }
        public bool F
        {
            get { return isKeyDown(Keys.F, FlxG.controllingPlayer, out pi); }
        }
        public bool G
        {
            get { return isKeyDown(Keys.G, FlxG.controllingPlayer, out pi); }
        }
        public bool H
        {
            get { return isKeyDown(Keys.H, FlxG.controllingPlayer, out pi); }
        }
        public bool J
        {
            get { return isKeyDown(Keys.J, FlxG.controllingPlayer, out pi); }
        }
        public bool K
        {
            get { return isKeyDown(Keys.K, FlxG.controllingPlayer, out pi); }
        }
        public bool L
        {
            get { return isKeyDown(Keys.L, FlxG.controllingPlayer, out pi); }
        }
        public bool SEMICOLON
        {
            get { return isKeyDown(Keys.OemSemicolon, FlxG.controllingPlayer, out pi); }
        }
        public bool QUOTE
        {
            get { return isKeyDown(Keys.OemQuotes, FlxG.controllingPlayer, out pi); }
        }
        public bool ENTER
        {
            get { return isKeyDown(Keys.Enter, FlxG.controllingPlayer, out pi); }
        }
        public bool SHIFT
        {
            get { return isKeyDown(Keys.LeftShift, FlxG.controllingPlayer, out pi) ||
                isKeyDown(Keys.RightShift, FlxG.controllingPlayer, out pi);
            }
        }
        public bool Z
        {
            get { return isKeyDown(Keys.Z, FlxG.controllingPlayer, out pi); }
        }
        public bool X
        {
            get { return isKeyDown(Keys.X, FlxG.controllingPlayer, out pi); }
        }
        public bool C
        {
            get { return isKeyDown(Keys.C, FlxG.controllingPlayer, out pi); }
        }
        public bool V
        {
            get { return isKeyDown(Keys.V, FlxG.controllingPlayer, out pi); }
        }
        public bool B
        {
            get { return isKeyDown(Keys.B, FlxG.controllingPlayer, out pi); }
        }
        public bool N
        {
            get { return isKeyDown(Keys.N, FlxG.controllingPlayer, out pi); }
        }
        public bool M
        {
            get { return isKeyDown(Keys.M, FlxG.controllingPlayer, out pi); }
        }
        public bool COMMA
        {
            get { return isKeyDown(Keys.OemComma, FlxG.controllingPlayer, out pi); }
        }
        public bool PERIOD
        {
            get { return isKeyDown(Keys.OemPeriod, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADPERIOD
        {
            get { return isKeyDown(Keys.OemPeriod, FlxG.controllingPlayer, out pi); }
        }
        public bool SLASH
        {
            get { return isKeyDown(Keys.Divide, FlxG.controllingPlayer, out pi); }
        }
        public bool NUMPADSLASH
        {
            get { return isKeyDown(Keys.Divide, FlxG.controllingPlayer, out pi); }
        }
        public bool CONTROL
        {
            get { return isKeyDown(Keys.LeftControl, FlxG.controllingPlayer, out pi) ||
                isKeyDown(Keys.RightControl, FlxG.controllingPlayer, out pi);
            }
        }
        public bool ALT
        {
            get { return isKeyDown(Keys.LeftAlt, FlxG.controllingPlayer, out pi) ||
                isKeyDown(Keys.RightAlt, FlxG.controllingPlayer, out pi);
            }
        }
        public bool SPACE
        {
            get { return isKeyDown(Keys.Space, FlxG.controllingPlayer, out pi); }
        }
        public bool UP
        {
            get { return isKeyDown(Keys.Up, FlxG.controllingPlayer, out pi); }
        }
        public bool DOWN
        {
            get { return isKeyDown(Keys.Down, FlxG.controllingPlayer, out pi); }
        }
        public bool LEFT
        {
            get { return isKeyDown(Keys.Left, FlxG.controllingPlayer, out pi); }
        }
        public bool RIGHT
        {
            get { return isKeyDown(Keys.Right, FlxG.controllingPlayer, out pi); }
        }

        public bool justPressed(Keys key)
        {
            return isNewKeyPress(key, FlxG.controllingPlayer, out pi);
        }

        /*==============================================
         * 
         * Properties
         * 
         *============================================*/
        public const int MaxInputs = 4;

        /*==============================================
         * 
         * Private members
         * 
         *============================================*/
        private KeyboardState[] _curKeyboard;
        private KeyboardState[] _lastKeyboard;

        private const float REPEAT_TIMER_LONG = 0.5f;
        private const float REPEAT_TIMER = 0.1f;
        private float _timerCountDown = 0.0f;

        /*==============================================
        * 
        * Properties
        * 
        *============================================*/
        public KeyboardState[] curKeyState
        {
            get { return _curKeyboard; }
        }

        /*==============================================
        * 
        * Initialization
        * 
        *============================================*/
        public FlxKeyboard()
        {
            _curKeyboard = new KeyboardState[MaxInputs];

            _lastKeyboard = new KeyboardState[MaxInputs];
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
                _lastKeyboard[i] = _curKeyboard[i];

                _curKeyboard[i] = Keyboard.GetState((PlayerIndex)i);
            }
        }


        /*==============================================
       * 
       * Keyboard standard input
       * 
       *============================================*/
        public bool isNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                    out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (_curKeyboard[i].IsKeyDown(key) &&
                        _lastKeyboard[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (isNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        isNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        isNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        isNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }

        public bool isNewKeyRelease(Keys key, PlayerIndex? controllingPlayer,
                                    out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (_curKeyboard[i].IsKeyUp(key) &&
                        _lastKeyboard[i].IsKeyDown(key));
            }
            else
            {
                // Accept input from any player.
                return (isNewKeyRelease(key, PlayerIndex.One, out playerIndex) ||
                        isNewKeyRelease(key, PlayerIndex.Two, out playerIndex) ||
                        isNewKeyRelease(key, PlayerIndex.Three, out playerIndex) ||
                        isNewKeyRelease(key, PlayerIndex.Four, out playerIndex));
            }
        }

        public bool isKeyDown(Keys key, PlayerIndex? controllingPlayer,
                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return _curKeyboard[i].IsKeyDown(key);
            }
            else
            {
                // Accept input from any player.
                return (isKeyDown(key, PlayerIndex.One, out playerIndex) ||
                        isKeyDown(key, PlayerIndex.Two, out playerIndex) ||
                        isKeyDown(key, PlayerIndex.Three, out playerIndex) ||
                        isKeyDown(key, PlayerIndex.Four, out playerIndex));
            }
        }

        /*==============================================
        * 
        * Keyboard menu input
        * 
        *============================================*/
        public void reset()
        {
            //called by the menu navigation functions, and by
            //the screen manager during screen
            //transitions.
            for (int i = 0; i < MaxInputs; i++)
            {
                _curKeyboard[i] = _lastKeyboard[i];
            }
        }

        public bool isMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return isNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   isNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex);
        }

        public bool isMenuCancel(PlayerIndex? controllingPlayer,
                         out PlayerIndex playerIndex)
        {
            return isNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex);
        }

        public bool isMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            if (isNewKeyPress(Keys.Up, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }

            KeyboardState repeatKybdState = _curKeyboard[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if (repeatKybdState.IsKeyDown(Keys.Up))
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

            if (isNewKeyPress(Keys.Down, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }

            KeyboardState repeatKybdState = _curKeyboard[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if (repeatKybdState.IsKeyDown(Keys.Down))
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

            if (isNewKeyPress(Keys.Left, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }

            KeyboardState repeatKybdState = _curKeyboard[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if (repeatKybdState.IsKeyDown(Keys.Left))
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

            if (isNewKeyPress(Keys.Right, controllingPlayer, out playerIndex))
            {
                _timerCountDown = REPEAT_TIMER_LONG;
                return true;
            }

            KeyboardState repeatKybdState = _curKeyboard[(int)playerIndex];

            if (_timerCountDown <= 0.0f)
            {
                if (repeatKybdState.IsKeyDown(Keys.Right))
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

            return isNewKeyPress(Keys.P, controllingPlayer, out playerIndex) ||
                isNewKeyPress(Keys.Pause, controllingPlayer, out playerIndex);
        }

    }

}
