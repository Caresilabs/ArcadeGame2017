using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudColony.Framework
{
    public class TextureRegion
    {
        private readonly Texture2D texture;
        public Rectangle region;

        public TextureRegion(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.region = new Rectangle(x, y, width, height);
        }

        public Rectangle GetSource()
        {
            return region;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public static implicit operator Rectangle(TextureRegion obj)
        {
            return obj.region;
        }

        public static implicit operator Texture2D(TextureRegion obj)
        {
            return obj.texture;
        }
    }
}
