using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudColony.Framework;
using Pacifier.Simulation;
using Pacifier.Utils;
using Microsoft.Xna.Framework;

namespace Pacifier.Entities
{
    public class Dumbbell : Entity
    {
        private Circle leftBell;
        private Circle rightBell;
        private Segment scoreBounds;

        public Dumbbell(World world, TextureRegion region, float x, float y) : base(world, region, x, y, 2f, 0.6f)
        {
            this.leftBell = new Circle(position, 0.2f);
            this.rightBell = new Circle(position, 0.2f);
            this.scoreBounds = new Segment(position, position);
        }

        public override void Update(float delta)
        {
            // Float

            var dir = new Vector2((float)Math.Cos((Rotation)), (float)Math.Sin((Rotation)));
            leftBell.Center = position - dir * Size.X / 2f;
            rightBell.Center = position + dir * Size.X / 2f;

            scoreBounds.start = leftBell.Center;
            scoreBounds.end = rightBell.Center;

            Rotation += delta * 0.4f;

            base.Update(delta);
        }

        public override void OnCollide(Entity other)
        {
            base.OnCollide(other);
            if (other is Player)
            {
                if (scoreBounds.Collide(other.Bounds))
                {
                    (other as Player).AddScore(10);
                }
                else
                {
                    other.IsDead = true;
                }
                World.Grid.ApplyExplosiveForce(10, position, 3);
                IsDead = true;
            }
        }

        public override bool QueryCollision(Entity other)
        {
            if (leftBell.Intersects(other.Bounds) || rightBell.Intersects(other.Bounds))
            {
                return true;
            }
            else if (scoreBounds.Collide(other.Bounds))
            {
                return true;
            }

            return false;
        }
    }
}
