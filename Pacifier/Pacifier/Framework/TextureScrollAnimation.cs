using Microsoft.Xna.Framework.Graphics;

namespace CloudColony.Framework
{
    public class TextureScrollAnimation : Animation
    {
        private TextureRegion region;

        private float scrolled;

        public TextureScrollAnimation(Texture2D tex, int yOffset, int height)
        {
            region = new TextureRegion(tex, 0, yOffset, tex.Width, height);
        }

        public override float GetPercent()
        {
            return 0;
        }

        public override TextureRegion GetRegion()
        {
            return region;
        }

        public override bool HasNext()
        {
            return true;
        }

        public override void Update(float delta)
        {
            scrolled -= (delta * 2);
            region.region.Offset(delta, 0);
          //  region.region.X = (int)scrolled;
        }
    }
}
