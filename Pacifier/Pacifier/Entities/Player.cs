using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudColony.Framework;
using Pacifier.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacifier.Entities
{
    public class Player : Entity
    {
        public const float PLAYER_SPEED = 3.5f;

        public PlayerIndex Index { get; private set; }

        public int Score { get; private set; }

        public Player(World world, PlayerIndex index, TextureRegion region, float x, float y) : base(world, region, x, y, 0.45f, 0.45f)
        {
            this.Index = index;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            const float speed = PLAYER_SPEED;
            velocity = speed * GetMovementDirection();
            //Position += Velocity;

            if (velocity != Vector2.Zero)
                Rotation = (float)Math.Atan2(velocity.Y, velocity.X);

            UpdateCollision(delta);
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
