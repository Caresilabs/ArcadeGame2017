using System;
using Microsoft.Xna.Framework;

namespace CloudColony.Framework.Tools
{
    /**
     * Simple class for simple math calculations
     */
    public class MathUtils
    {
        public static Random rnd = new Random();

        public static float Random(float max)
        {
            return (float)rnd.NextDouble() * max;
        }

        public static float Random()
        {
            return (float)rnd.NextDouble();
        }

        public static float Random(float min, float max)
        {
            return min + (float)rnd.NextDouble() * (max - min);
        }

        public static int Random(int max)
        {
            return rnd.Next(max);
        }

        public static int Random(int min, int max)
        {
            return min + rnd.Next(0, max - min + 1);
        }

        public static bool RandomBool()
        {
            return rnd.Next(2) == 1 ? true : false;
        }

        public static float Atan(float degree)
        {
            return (float)Math.Atan((double)degree);
        }

        public static float AngleLerp(float nowrap, float wraps, float lerp)
        {
            float c, d;

            if (wraps < nowrap)
            {
                c = wraps + MathHelper.TwoPi;
                d = c - nowrap > nowrap - wraps
                    ? MathHelper.Lerp(nowrap, wraps, lerp)
                    : MathHelper.Lerp(nowrap, c, lerp);

            }
            else if (wraps > nowrap)
            {
                c = wraps - MathHelper.TwoPi;
                d = wraps - nowrap > nowrap - c
                    ? MathHelper.Lerp(nowrap, c, lerp)
                    : MathHelper.Lerp(nowrap, wraps, lerp);

            }
            else
            {
                return nowrap;
            }

            return MathHelper.WrapAngle(d);
        }

        /**
         * Clamps the vector within target range
         */
        public static void Clamp(Vector2 vec, float min, float max)
        {
            if (vec.X < min) vec.X = min;
            else if (vec.X > max) vec.X = max;

            if (vec.Y < min) vec.Y = min;
            else if (vec.Y > max) vec.Y = max;
        }
    }
}
