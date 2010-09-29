using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    //@desc		Contains all the logic for the developer console
    public class FlxConsole
    {
        private int MAX_CONSOLE_LINES = 256; // Not a const in X-flixel... we'll just base it on max lines visible on the console

        public bool visible = false;

        private Rectangle _srcRect = new Rectangle(1, 1, 1, 1);
        private Rectangle _consoleRect;
        private Color _consoleColor;
        private FlxText _consoleText;
        private FlxText _consoleFPS;
        private int[] _FPS;
        private int _curFPS;
        private List<string> _consoleLines;
        private float _consoleY;
        private float _consoleYT;
        private bool _fpsUpdate;

        public Color color
        {
            get { return _consoleColor; }
        }

        public FlxConsole(int targetLeft, int targetWidth)
        {
            visible = false;

            _FPS = new int[8];

            _consoleRect = new Rectangle(0, 0,
                FlxG.spriteBatch.GraphicsDevice.Viewport.Width, FlxG.spriteBatch.GraphicsDevice.Viewport.Height);
            _consoleColor = new Color(0, 0, 0, 0x7F);

            _consoleText = new FlxText(targetLeft, -800, targetWidth, "").setFormat(null, 1, Color.White, FlxJustification.Left, Color.White);
            _consoleText.height = FlxG.height; //FlxG.spriteBatch.GraphicsDevice.Viewport.Height;

            _consoleFPS = new FlxText(targetLeft + targetWidth - 30, -800, 30, "").setFormat(null, 1, Color.White, FlxJustification.Right, Color.White);

            _consoleLines = new List<string>();

            MAX_CONSOLE_LINES = (FlxG.spriteBatch.GraphicsDevice.Viewport.Height / (int)(_consoleText.font.MeasureString("Qq").Y)) - 1;
        }

        //@desc		Log data to the developer console
        //@param	Data		The data (in string format) that you wanted to write to the console
        public void log(string Data)
        {
            if (Data == null)
                Data = "ERROR: NULL GAME LOG MESSAGE";

            _consoleLines.Add(Data);
            if (_consoleLines.Count > MAX_CONSOLE_LINES)
            {
                _consoleLines.RemoveAt(0);
                string newText = "";
                for (int i = 0; i < _consoleLines.Count; i++)
                    newText += _consoleLines[i] + "\n";
                _consoleText.text = newText;
            }
            else
                _consoleText.text += (Data + "\n");
            //_consoleText.scrollV = _consoleText.height;
        }

        //@desc		Shows/hides the console
        public void toggle()
        {
            if (_consoleYT == FlxG.spriteBatch.GraphicsDevice.Viewport.Height)
                _consoleYT = 0;
            else
            {
                _consoleYT = FlxG.spriteBatch.GraphicsDevice.Viewport.Height;
                visible = true;
            }
        }

        //@desc		Updates and/or animates the dev console
        public void update()
        {
            if (visible)
            {
                //_FPS[_curFPS] = (int)(1f / FlxG.elapsed);
                //if (++_curFPS >= _FPS.Length) _curFPS = 0;
                //_fpsUpdate = !_fpsUpdate;
                //if (_fpsUpdate)
                //{
                //    int fps = 0;
                //    for (int i = 0; i < _FPS.Length; i++)
                //        fps += _FPS[i];
                //    _consoleFPS.text = ((int)Math.Floor((double)(fps / _FPS.Length))).ToString() + " fps";
                //}

                _consoleText.y = (-FlxG.spriteBatch.GraphicsDevice.Viewport.Height + _consoleRect.Height + 8);
                _consoleFPS.y = _consoleText.y;
            }
            if (_consoleY < _consoleYT)
                _consoleY += FlxG.height * 10 * FlxG.elapsed;
            else if (_consoleY > _consoleYT)
                _consoleY -= FlxG.height * 10 * FlxG.elapsed;
            if (_consoleY > FlxG.spriteBatch.GraphicsDevice.Viewport.Height)
                _consoleY = FlxG.spriteBatch.GraphicsDevice.Viewport.Height;
            else if (_consoleY < 0)
            {
                _consoleY = 0;
                visible = false;
            }
            _consoleRect.Height = (int)Math.Floor(_consoleY);
        }

        public void render(SpriteBatch spriteBatch)
        {
            _FPS[_curFPS] = (int)(1f / FlxG.elapsed);
            if (++_curFPS >= _FPS.Length) _curFPS = 0;
            _fpsUpdate = !_fpsUpdate;
            if (_fpsUpdate)
            {
                int fps = 0;
                for (int i = 0; i < _FPS.Length; i++)
                    fps += _FPS[i];
                _consoleFPS.text = ((int)Math.Floor((double)(fps / _FPS.Length))).ToString() + " fps";
            }

            spriteBatch.Draw(FlxG.XnaSheet, _consoleRect,
                _srcRect, _consoleColor);
            _consoleText.render(spriteBatch);
            _consoleFPS.render(spriteBatch);
        }

    }
}
