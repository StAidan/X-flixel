using System;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    public delegate bool SpriteCollisionEvent(object sender, FlxSpriteCollisionEvent e);

    public enum EffectType
    {
        Rumble = 0,
        FadeOut = 1,
        Flash = 2
    }

    public enum MouseEventType
    {
        MouseDown = 0,
        MouseUp = 1
    }

    public class FlxEffectCompletedEvent : EventArgs
    {
        private EffectType _effectType;

        public FlxEffectCompletedEvent(EffectType Effect)
        {
            this._effectType = Effect;
        }

        public EffectType effect
        {
            get { return _effectType; }
        }
    }

    public class FlxSpriteCollisionEvent : EventArgs
    {
        private FlxObject _s1;
        private FlxObject _s2;

        public FlxSpriteCollisionEvent(FlxObject Attacker, FlxObject Target)
        {
            _s1 = Attacker;
            _s2 = Target;
        }

        public FlxObject Object1
        {
            get { return _s1; }
        }
        public FlxObject Object2
        {
            get { return _s2; }
        }
    }

    public class FlxMouseEvent : EventArgs
    {
        private MouseEventType _mouseType;

        public FlxMouseEvent(MouseEventType MouseEvent)
        {
            this._mouseType = MouseEvent;
        }

        public MouseEventType eventType
        {
            get { return _mouseType; }
        }
    }

}
