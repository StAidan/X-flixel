using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    public class FlxParticle : FlxSprite
    {
		protected float _bounce;
		
		public FlxParticle(float Bounce)
		{
			_bounce = Bounce;
		}
		
		override public void hitSide(FlxObject Contact, float Velocity)
		{
			velocity.X = -velocity.X * _bounce;
			if(angularVelocity != 0)
				angularVelocity = -angularVelocity * _bounce;
		}
		
		override public void hitBottom(FlxObject Contact, float Velocity)
		{
			onFloor = true;
			if(((velocity.Y > 0)?velocity.Y:-velocity.Y) > _bounce*100)
			{
				velocity.Y = -velocity.Y * _bounce;
				if(angularVelocity != 0)
					angularVelocity *= -_bounce;
			}
			else
			{
				angularVelocity = 0;
				base.hitBottom(Contact,Velocity);
			}
			velocity.X *= _bounce;
		}
    }
}
