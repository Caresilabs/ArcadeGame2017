using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudColony.Framework
{
    public class Sprite : IRenderable, IUpdate
    {
        public TextureRegion Region { get; private set; }

        public Vector2 Position { get { return position; } }
        protected Vector2 position;

        public Vector2 Scale { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 DrawOffset { get; set; }

        public Color Color { get; set; }

        public SpriteEffects Effect { get; set; }

        public float ZIndex { get; set; }

        public float Rotation { get; set; }

        private Vector2 SizeScale { get; set; }

        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
            set { size = value; UpdateSizeScale(); }
        }

        protected Animations Animations { get; set; }

        public Sprite(TextureRegion region, float x, float y, float width, float height)
        {
            this.ZIndex = 0;
            this.Region = region;
            this.Color = Color.White;
            this.Scale = new Vector2(1, 1);
            this.Size = new Vector2(width, height);
           // this.DrawOffset = new Vector2();
            this.Effect = SpriteEffects.None;
            this.position = new Vector2(x, y);
            this.Animations = new Animations();
            this.DrawOffset = new Vector2(Size.X / 2, Size.Y / 2);

           // if (region != null)
            //    this.Origin = new Vector2(region.GetSource().Width / 2, region.GetSource().Height / 2);

            UpdateSizeScale();
        }

        public virtual void Update(float delta)
        {
            Animations.Update(delta);
            if (Animations.HasNext())
            {
                SetRegion(Animations.GetRegion());
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (Region != null)
                batch.Draw(Region, Position - DrawOffset + size / 2, Region, Color, Rotation, Origin, Scale * SizeScale, Effect, ZIndex);
        }

        private void UpdateSizeScale()
        {
            if (Region != null)
            {
                this.SizeScale = new Vector2(Size.X / Region.GetSource().Width, Size.Y / Region.GetSource().Height);
                this.Origin = new Vector2(Region.GetSource().Width / 2f, Region.GetSource().Height / 2f);
            }
        }

        // ==== HELPERS ==== //

        public Sprite AddAnimation(string name, Animation anim)
        {
            Animations.AddAnimation(name, anim);
            return this;
        }

        public Sprite SetAnimation(string name)
        {
            Animations.SetAnimation(name);
            SetRegion(Animations.GetRegion());
            return this;
        }

        public Sprite SetScale(float x, float y)
        {
            this.Scale = new Vector2(x, y);
            return this;
        }

        public Sprite SetScale(float v)
        {
            this.Scale = new Vector2(v, v);
            return this;
        }

        public Sprite SetSize(float width, float height)
        {
            this.Size = new Vector2(width, height);
            UpdateSizeScale();
            return this;
        }

        public Sprite SetPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
            return this;
        }

        public Sprite SetPosition(Vector2 position)
        {
            this.position = position;
            return this;
        }

        public Sprite SetRegion(TextureRegion region)
        {
            this.Region = region;
            UpdateSizeScale();
            return this;
        }

        public Vector2 GetRealScale()
        {
            return Scale * SizeScale;
        }

    }
}
