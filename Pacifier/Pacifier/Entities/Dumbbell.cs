using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudColony.Framework;
using Pacifier.Simulation;
using Pacifier.Utils;
using Microsoft.Xna.Framework;
using CloudColony.Framework.Tools;
using Pacifier.Entities.Particles;
using ShapeBlaster;

namespace Pacifier.Entities
{
    public class Dumbbell : Entity
    {
        private const float KILL_DIST = 2.5f;
        private const float BELL_RADIUS = 0.15f;
        private const float MAX_SPEED = 0.5f;

        private Circle leftBell;
        private Circle rightBell;
        private Segment scoreBounds;
        private float rotationSpeed;

        public Dumbbell(World world, TextureRegion region, float x, float y) : base(world, region, x, y, 2f, 0.6f)
        {
            this.leftBell = new Circle(position, BELL_RADIUS);
            this.rightBell = new Circle(position, BELL_RADIUS);
            this.scoreBounds = new Segment(position, position);
            this.velocity = new Vector2(MathUtils.Random(-MAX_SPEED, MAX_SPEED), MathUtils.Random(-MAX_SPEED, MAX_SPEED));
            this.rotationSpeed = MathUtils.Random(0.3f, 1) * (MathUtils.RandomBool() ? -1 : 1);
        }

        public override void Update(float delta)
        {
            var dir = new Vector2((float)Math.Cos((Rotation)), (float)Math.Sin((Rotation)));
            leftBell.Center = position - (dir * (-BELL_RADIUS * 1.3f + Size.X / 2f));
            rightBell.Center = position + (dir * (-BELL_RADIUS * 1.3f + Size.X / 2f));

            scoreBounds.start = leftBell.Center;
            scoreBounds.end = rightBell.Center;

            UpdateMovement(delta);

            base.Update(delta);
        }

        private void UpdateMovement(float delta)
        {
           // Velocity
            Rotation += delta * 0.5f * rotationSpeed;

            if (Bounds.Center.X - Bounds.Radius < 0)
            {
                velocity.X *= -1;
                rotationSpeed *= -1;
            }
            else if (Bounds.Center.X + Bounds.Radius > World.WORLD_WIDTH)
            {
                velocity.X *= -1;
                rotationSpeed *= -1;
            }
            else if (Bounds.Center.Y + Bounds.Radius > World.WORLD_HEIGHT)
            {
                velocity.Y *= -1;
                rotationSpeed *= -1;
            }
            else if (Bounds.Center.Y - Bounds.Radius < 0)
            {
                velocity.Y *= -1;
                rotationSpeed *= -1;
            }
        }

        public override void OnCollide(Entity other)
        {
            base.OnCollide(other);
            if (other is Player)
            {
                if (scoreBounds.Collide(other.Bounds))
                {
                    (other as Player).AddScore(10);
                    KillAllNearby(other as Player);
                }
                else
                {
                    other.IsDead = true;
                    World.Grid.ApplyExplosiveForce(10, position, KILL_DIST);
                }
               
                IsDead = true;
            }
        }

        private void KillAllNearby(Player player)
        {
            int count = 0;
            foreach (var enemy in World.Enemies)
            {
                if (Vector2.Distance(position, enemy.Position) <= KILL_DIST)
                {
                    // ADD SCORE
                    player.AddScore(5);
                    enemy.Kill();
                    ++count;
                }
            }
            World.Grid.ApplyExplosiveForce(10 + count * 0.01f, position, KILL_DIST + count * 0.1f);

            for (int i = 0; i < 30; i++)
            {
                World.ParticleManager.CreateParticle(PR.Particle, Position, Color.CornflowerBlue, 50, 1,
                    new ParticleState() { Velocity = Extensions.NextVector2(0, 0.2f), Type = ParticleType.Bullet, LengthMultiplier = 0f });
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
