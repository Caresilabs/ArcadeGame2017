using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CloudColony.Framework
{
    public class FrameAnimation : Animation
    {
        private Texture2D texture;
        private Point origin;
        private Point walker;
        private float stateTime;
        private int width;
        private int height;
        private int frames;
        private float frameDuration;
        private bool reversed;
        private bool loop;

        public FrameAnimation(Texture2D tex, int x, int y, int width, int height, int frames, float frameDuration, Point walker, bool loop = true, bool reversed = false)
        {
            this.origin = new Point(x, y);
            this.walker = walker;
            this.reversed = reversed;
            this.loop = loop;
            this.width = width;
            this.height = height;
            this.stateTime = 0;
            this.frames = frames;
            this.frameDuration = frameDuration;
            this.texture = tex;
        }

        public override bool HasNext()
        {
            return lastFrame != currentFrame;
        }

        public override void Update(float delta)
        {
            lastFrame = currentFrame;
            stateTime += delta;

            if (loop)
            {
                if (reversed)
                    currentFrame = (frames - (int)(stateTime / (float)frameDuration)) % frames;
                else
                    currentFrame = (int)(stateTime / (float)frameDuration) % frames;
            }
            else
            {
                if (reversed)
                    currentFrame = Math.Max(frames - (int)(stateTime / (float)frameDuration) - 1, 0);
                else
                    currentFrame = Math.Min((int)(stateTime / (float)frameDuration), frames - 1);
            }
        }

        public override float GetPercent()
        {
            if (loop)
            {
                return 0;
            }
            else
            {
                if (reversed)
                    return 1.0f - ((currentFrame) / (float)(frames - 1));
                else
                    return currentFrame / (float)(frames - 1);
            }
        }

        public override TextureRegion GetRegion()
        {
            return new TextureRegion(
                texture, origin.X + walker.X * (width * currentFrame), origin.Y + walker.Y * (height * currentFrame), width, height
           );
        }
    }
}
