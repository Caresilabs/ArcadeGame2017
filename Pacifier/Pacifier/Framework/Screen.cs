using Microsoft.Xna.Framework.Graphics;
using Pacifier;

namespace CloudColony.Framework
{
    public abstract class Screen
    {
        public GraphicsDeviceArcade Graphics { get; set; }

        public Viewport DefaultViewport { get; set; }

        public Game1 Game { get; set; }

        public abstract void Init();

        public abstract void Update(float delta);

        public abstract void Draw(SpriteBatch batch);

        public abstract void Dispose();

        public void SetScreen(Screen newScreen)
        {
            Game.SetScreen(newScreen);
        }

    }
}
