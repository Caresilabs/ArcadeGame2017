using CloudColony.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Pacifier.Simulation;
using Microsoft.Xna.Framework;

namespace Pacifier.Screens
{
    public class GameScreen : Screen
    {
        private WorldRenderer renderer;
        private World world;

        public override void Init()
        {
            this.world = new World();
            this.renderer = new WorldRenderer(world);
        }

        public override void Update(float delta)
        {
            world.Update(delta);
        }

        public override void Draw(SpriteBatch batch)
        {
            Graphics.Clear(Color.Black);
            renderer.Render(batch);
        }

        public override void Dispose()
        {
        }
    }
}
