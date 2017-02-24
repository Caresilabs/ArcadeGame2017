using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier.Simulation
{
    class WorldRenderer
    {
        private World world;

        public WorldRenderer(World world)
        {
            this.world = world;
        }

        public void Render(SpriteBatch batch)
        {
            // Clear Screen
            
            batch.Begin(SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    world.Camera.GetMatrix());

            world.Grid.Draw(batch);

            batch.End();
        }
    }
}
