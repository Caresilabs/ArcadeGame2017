using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier.Utils
{
    public class ColliderUtils
    {
        public static bool PixelCollision(Entity first, Entity other)
        {
            if (other.Region == null || first.Region == null) return false;

            Color[] dataA = new Color[first.Region.GetSource().Width * first.Region.GetSource().Height];
            first.Region.GetTexture().GetData(0, first.Region, dataA, 0, dataA.Length);

            Color[] dataB = new Color[other.Region.GetSource().Width * other.Region.GetSource().Height];
            other.Region.GetTexture().GetData(0, other.Region, dataB, 0, dataB.Length);

            if (first.Effect == SpriteEffects.FlipHorizontally)
                return false;
            else
                return PixelCollision(first, other, dataA, dataB);
        }

        private static bool PixelCollision(Entity first, Entity other, Color[] dataA, Color[] dataB)
        {
            //int top = Math.Max(bounds.Top, other.GetBounds().Top);
            //int bottom = Math.Min(bounds.Bottom, other.GetBounds().Bottom);
            //int left = Math.Max(bounds.Left, other.GetBounds().Left);
            //int right = Math.Min(bounds.Right, other.GetBounds().Right);

            //for (int y = top; y < bottom; y++)
            //{
            //    for (int x = left; x < right; x++)
            //    {
            //        Color colorA = dataA[(int)((x - bounds.Left) / sprite.GetRealScale().X) +
            //        (int)((int)((y - bounds.Top) / sprite.GetRealScale().Y) * (bounds.Width / sprite.GetRealScale().X))];

            //        Color colorB = dataB[(int)((x - other.GetBounds().Left) / other.sprite.GetRealScale().X) +
            //       (int)((int)((y - other.GetBounds().Top) / other.sprite.GetRealScale().Y) * (other.GetBounds().Width / other.sprite.GetRealScale().X))];

            //        if (colorA.A != 0 && colorB.A != 0) // Collision
            //            return true;
            //    }
            //}
            return false;
        }
    }
}
