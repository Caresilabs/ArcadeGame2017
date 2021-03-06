﻿using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Pacifier.Simulation;
using System;

namespace Pacifier.Entities.Entities
{
    public class Entity : Sprite
    {
        public World World { get; private set; }

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        protected Vector2 velocity;

        public Circle Bounds { get; set; }

        public bool IsDead { get; set; }

        public Entity(World world, TextureRegion region, float x, float y, float width, float height) : base(region, x, y, width, height)
        {
            this.World = world;
            this.velocity = new Vector2();
            this.IsDead = false;
            this.Bounds = new Circle(position, Math.Max(width, height) / 2.4f);
            ZIndex = 0.01f;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            position += velocity * delta;
            Bounds.Center = position;
            KeepInside();
        }

        public void KeepInside()
        {
            position.X = MathHelper.Clamp(position.X, Size.X / 2f, World.WORLD_WIDTH - Size.Y / 2f);
            position.Y = MathHelper.Clamp(position.Y, Size.Y / 2f, World.WORLD_HEIGHT - Size.Y / 2f);
        }

        public virtual void OnCollide(Entity other)
        {
        }

        public virtual bool QueryCollision(Entity other)
        {
            if (IsDead)
                return false;

            return Bounds.Intersects(other.Bounds);
        }

    }
}
