using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudColony.Framework;
using Pacifier.Simulation;
using Microsoft.Xna.Framework;

namespace Pacifier.Entities
{
    public class Enemy : Entity
    {
        public const float SEPARATION_WEIGHT = 13f;
        public const float COHESION_WEIGHT = 3f;
        public const float ALIGNMENT_WEIGHT = 2f;
     

        public float Speed { get; set; }
        public float MaxSpeed { get; set; }
        

        public Enemy(World world, TextureRegion region, float x, float y, float width, float height) : base(world, region, x, y, width, height)
        {
            MaxSpeed = 1f;
        }

        
        public override void Update(float delta)
        {
            base.Update(delta);
            

            var alignment = Alignment();
            var cohesion = Cohesion();
            var separation = Separation();

            var flock = (alignment * ALIGNMENT_WEIGHT) + (cohesion * COHESION_WEIGHT) + (separation * SEPARATION_WEIGHT);
            if (flock != Vector2.Zero)
            {
                velocity += flock * delta;
            }

            SeekTarget(delta);

            CapSpeed(delta);
            
        }

        public override void OnCollide(Entity other)
        {
            base.OnCollide(other);
            if (other is Player)
            {
                other.IsDead = true;
                World.Grid.ApplyExplosiveForce(10, position, 3);
            }
        }

        private void SeekTarget(float delta)
        {
            var desPos = Vector2.Zero;
            if (World.PlayerRed.IsDead)
            {
                desPos = World.PlayerBlue.Position;
            }
            else if (World.PlayerBlue.IsDead)
            {
                desPos = World.PlayerRed.Position;
            }
            else
            {
                desPos = Vector2.Distance(World.PlayerRed.Position, position) < Vector2.Distance(World.PlayerBlue.Position, position) ? World.PlayerRed.Position : World.PlayerBlue.Position;
            }
            
            var desiredVelocity = (desPos - position);
            desiredVelocity.Normalize();
            desiredVelocity *= MaxSpeed;
            velocity += (desiredVelocity - velocity) * 0.14f;
            // MaxSpeed();
        }

        private void CapSpeed(float delta)
        {
            velocity.Normalize();
            Speed = MathHelper.Lerp(Speed, MaxSpeed, delta * 1f);
            velocity *= Speed;
        }

        private void KeepInside()
        {
            if (position.X < Size.X / 2f)
            {
                position.X = Size.X / 2f;
                velocity.X = Math.Abs(velocity.X);
            }

            if (position.X > World.WORLD_WIDTH - Size.X / 2f)
            {
                position.X = World.WORLD_WIDTH - Size.X / 2f;
                velocity.X = -Math.Abs(velocity.X);
            }

            if (position.Y < Size.Y / 2f)
            {
                position.Y = Size.Y / 2f;
                velocity.Y = Math.Abs(velocity.Y);
            }

            if (position.Y > World.WORLD_HEIGHT - Size.Y / 2f)
            {
                position.Y = World.WORLD_HEIGHT - Size.Y / 2f;
                velocity.Y = -Math.Abs(velocity.Y);
            }
        }

        
        private Vector2 Alignment()
        {
            if (World.Enemies.Count <= 1)
                return Vector2.Zero;

            Vector2 pvj = Vector2.Zero;
            foreach (var b in World.Enemies)
            {
                if (this != b)
                {
                    pvj += b.velocity;
                }
            }
            pvj /= (World.Enemies.Count - 1);
            return (pvj - velocity) * 0.01f;
        }

        private Vector2 Cohesion()
        {
            if (World.Enemies.Count <= 1)
                return Vector2.Zero;

            Vector2 pcj = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in World.Enemies)
            {
                if (this != b && Distance(b.position, position) <= 3.5f)
                {
                    pcj += b.position;
                    neighborCount++;
                }
            }
            pcj /= neighborCount + 1;
            return (pcj - position) * 0.005f; // 0.01f
        }

        private Vector2 Separation()
        {
            if (World.Enemies.Count <= 1)
                return Vector2.Zero;

            Vector2 vec = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in World.Enemies)
            {
                if (this != b)
                {
                    var distance = Distance(position, b.position);
                    if (distance > 0 && distance < 5)
                    {
                        var deltaVector = position - b.position;
                        deltaVector.Normalize();
                        deltaVector /= distance;
                        vec += deltaVector;
                        neighborCount++;
                    }
                }
            }
            Vector2 averageSteeringVector = (neighborCount > 0) ? (vec / neighborCount) : Vector2.Zero;
            return averageSteeringVector;
        }
        
        private float Distance(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).Length();
        }
    }
}
