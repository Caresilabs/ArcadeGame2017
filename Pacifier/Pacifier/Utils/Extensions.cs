﻿//---------------------------------------------------------------------------------
// Written by Michael Hoffman
// Find the full tutorial at: http://gamedev.tutsplus.com/series/vector-shooter-xna/
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacifier;
using CloudColony.Framework.Tools;

namespace ShapeBlaster
{
	static class Extensions
	{
		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness = 2f)
		{
			Vector2 delta = end - start;
			spriteBatch.Draw(PR.PixelGlow, start, PR.PixelGlow, color, delta.ToAngle(), new Vector2(0, 0.5f), new Vector2(delta.Length() / 32f, thickness / 9f), SpriteEffects.None, 1f);
		}

		public static float ToAngle(this Vector2 vector)
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}

		public static Vector2 ScaleTo(this Vector2 vector, float length)
		{
			return vector * (length / vector.Length());
		}

		public static Point ToPoint(this Vector2 vector)
		{
			return new Point((int)vector.X, (int)vector.Y);
		}

		public static float NextFloat(this Random rand, float minValue, float maxValue)
		{
			return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
		}

		public static Vector2 NextVector2(float minLength, float maxLength)
		{
			double theta = MathUtils.Random() * 2 * Math.PI;
			float length = MathUtils.Random(minLength, maxLength);
			return new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));
		}
	}
}
