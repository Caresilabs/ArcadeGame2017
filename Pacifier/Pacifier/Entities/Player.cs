﻿using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudColony.Framework;
using Pacifier.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShapeBlaster;
using Pacifier.Entities.Particles;

namespace Pacifier.Entities
{
    public class Player : Entity
    {
        public const float PLAYER_SPEED = 3.5f;

        public PlayerIndex Index { get; private set; }

        public long Score { get; private set; }
        public object Orientation { get; private set; }

        public Player(World world, PlayerIndex index, TextureRegion region, float x, float y) : base(world, region, x, y, 0.45f, 0.45f)
        {
            this.Index = index;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            const float speed = PLAYER_SPEED;
            var desiredVelocity = speed * GetMovementDirection();

            velocity += (desiredVelocity - velocity) * delta * 12;

            if (velocity != Vector2.Zero)
                Rotation = (float)Math.Atan2(velocity.Y, velocity.X);

            UpdateCollision(delta);

            MakeExhaustFire();
        }

        private void UpdateCollision(float delta)
        {
            var colliders = World.Collisions.GetPossibleColliders(this);
            foreach (var entity in colliders)
            {
                if (entity == this)
                    continue;

                if (entity.QueryCollision(this))
                {
                    entity.OnCollide(this);
                }
            }
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public Vector2 GetMovementDirection()
        {

            Vector2 direction = new Vector2();

            if (ButtonDown(PlayerInput.Left))
                direction.X -= 1;
            if (ButtonDown(PlayerInput.Right))
                direction.X += 1;
            if (ButtonDown(PlayerInput.Up))
                direction.Y -= 1;
            if (ButtonDown(PlayerInput.Down))
                direction.Y += 1;

            // Clamp the length of the vector to a maximum of 1.
            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        private void MakeExhaustFire()
        {
            if (Velocity.LengthSquared() > 0.01f)
            {
                // set up some variables
                Orientation = Velocity.ToAngle();
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, MathHelper.ToDegrees(Rotation));

                double t = World.Time;
                // The primary velocity of the particles is 3 pixels/frame in the direction opposite to which the ship is travelling.
                Vector2 baseVel = Velocity.ScaleTo(-3f);
                // Calculate the sideways velocity for the two side streams. The direction is perpendicular to the ship's velocity and the
                // magnitude varies sinusoidally.
                Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (0.6f * (float)Math.Sin(t * 10));
                Color sideColor = new Color(200, 38, 9);    // deep red
                Color midColor = new Color(255, 187, 30);   // orange-yellow
                Vector2 pos = Position;// + Vector2.Transform(new Vector2(-1, 0), rot);   // position of the ship's exhaust pipe.
                const float alpha = 0.7f;

                // middle particle stream
                Vector2 velMid = baseVel + Extensions.NextVector2(0, 1);
                World.ParticleManager.CreateParticle(PR.Particle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(velMid * 0.01f, ParticleType.Enemy));
                World.ParticleManager.CreateParticle(PR.Particle, pos, midColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(velMid * 0.01f, ParticleType.Enemy));

                // side particle streams
                Vector2 vel1 = baseVel + perpVel + Extensions.NextVector2(0, 0.3f);
                Vector2 vel2 = baseVel - perpVel + Extensions.NextVector2(0, 0.3f);
                World.ParticleManager.CreateParticle(PR.Particle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel1 * 0.01f, ParticleType.Enemy));
                World.ParticleManager.CreateParticle(PR.Particle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel2 * 0.01f, ParticleType.Enemy));

                World.ParticleManager.CreateParticle(PR.Particle, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel1 * 0.01f, ParticleType.Enemy));
                World.ParticleManager.CreateParticle(PR.Particle, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel2 * 0.01f, ParticleType.Enemy));
            }
        }

        private bool ButtonDown(PlayerInput button)
        {
            return InputHandler.GetButtonState(Index, button) == InputState.Down;
        }

        private bool PressedButton(PlayerInput button)
        {
            return InputHandler.GetButtonState(Index, button) == InputState.Released;
        }

    }
}
