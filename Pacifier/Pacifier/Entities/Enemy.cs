using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudColony.Framework;
using Pacifier.Simulation;
using Microsoft.Xna.Framework;
using Pacifier.Entities.Particles;
using ShapeBlaster;
using CloudColony.Framework.Tools;

namespace Pacifier.Entities
{
    public class Enemy : Entity
    {
        public const float SEPARATION_WEIGHT = 2.5f;
        public const float COHESION_WEIGHT = 11f;
        public const float ALIGNMENT_WEIGHT = 2f;
     

        public float Speed { get; set; }
        public float MaxSpeed { get; set; }
        

        public Enemy(World world, TextureRegion region, float x, float y, float width, float height) : base(world, region, x, y, width, height)
        {
            MaxSpeed = MathUtils.Random(1.0f, 1.2f);
            Bounds.Radius =  Bounds.Radius * 0.8f;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            var colliders = World.Collisions.GetPossibleColliders(this, 1.2f, x => x is Enemy).Take(35);    //.ToList();

            // Cap
            // if (colliders.Count > 30)
            //     colliders.RemoveRange(30, colliders.Count - 30);

            var alignment = Vector2.Zero;// Alignment(colliders);
            var cohesion = Vector2.Zero;// Cohesion(colliders);
            var separation = Separation(colliders);

            var flock = (alignment * ALIGNMENT_WEIGHT) + (cohesion * COHESION_WEIGHT) + (separation * SEPARATION_WEIGHT);
            if (flock != Vector2.Zero)
            {
                velocity += flock * delta;
            }

            Scale = new Vector2(1 + 0.2f * (float)Math.Sin(World.Time * 5), 1);

            SeekTarget(delta);

            CapSpeed(delta);

            Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        public override void OnCollide(Entity other)
        {
            base.OnCollide(other);
            if (other is Player)
            {
                other.IsDead = true;
                World.Grid.ApplyExplosiveForce(10, position, 3, Color.Red);
            }
        }

        private void SeekTarget(float delta)
        {
            var desPos = Vector2.Zero;
            Player player = null;
            if (World.PlayerGreen.IsDead)
            {
                player = World.PlayerYellow;
            }
            else if (World.PlayerYellow.IsDead)
            {
                player = World.PlayerGreen;
            }
            else
            {
                player = Vector2.Distance(World.PlayerGreen.Position, position) < Vector2.Distance(World.PlayerYellow.Position, position) ? World.PlayerGreen : World.PlayerYellow;
            }

            desPos = player.Position + player.Velocity * 0.1f;
            
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
            } else           if (position.X > World.WORLD_WIDTH - Size.X / 2f)
            {
                position.X = World.WORLD_WIDTH - Size.X / 2f;
                velocity.X = -Math.Abs(velocity.X);
            }

            if (position.Y < Size.Y / 2f)
            {
                position.Y = Size.Y / 2f;
                velocity.Y = Math.Abs(velocity.Y);
            }             if (position.Y > World.WORLD_HEIGHT - Size.Y / 2f)
            {
                position.Y = World.WORLD_HEIGHT - Size.Y / 2f;
                velocity.Y = -Math.Abs(velocity.Y);
            }
        }

        
        private Vector2 Alignment(Entity[] colliders)
        {
            if (World.Enemies.Count <= 1)
                return Vector2.Zero;

            Vector2 pvj = Vector2.Zero;
           
            foreach (var b in colliders) 
            {
                if (this != b)
                {
                    pvj += b.Velocity;
                }
            }
            pvj /= (World.Enemies.Count - 1);
            return (pvj - velocity) * 0.01f;
        }

        public void Kill()
        {
            IsDead = true;
            for (int i = 0; i < 20; i++)
            {
                World.ParticleManager.CreateParticle(PR.Particle, Position, Color.MediumVioletRed, 50, 1,
                    new ParticleState() { Velocity = Extensions.NextVector2(0, 0.2f), Type = ParticleType.Bullet, LengthMultiplier = 1 });
            }
        }

        private Vector2 Cohesion(Entity[] colliders)
        {
            if (World.Enemies.Count <= 1)
                return Vector2.Zero;

            Vector2 pcj = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in colliders)
            {
                if (this != b && Distance(b.Position, position) <= 2f)
                {
                    pcj += b.Position;
                    neighborCount++;
                }
            }
            pcj /= neighborCount + 1;
            return (pcj - position) * 0.005f; // 0.01f
        }

        private Vector2 Separation(IEnumerable<Entity> colliders)
        {
            if (World.Enemies.Count <= 1)
                return Vector2.Zero;

            Vector2 vec = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in colliders)
            {
                if (this != b)
                {
                    var distance = Distance(position, b.Position);
                    if (distance > 0 && distance < 2.5f)
                    {
                        var deltaVector = position - b.Position;
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
