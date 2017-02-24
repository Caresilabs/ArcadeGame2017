using CloudColony.Framework.Tools;
using Microsoft.Xna.Framework;
using ShapeBlaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier.Simulation
{
    public class World
    {
        public Grid Grid { get; private set; }

        public Camera2D Camera { get; private set; }

        public World()
        {
            this.Grid = new Grid(new Rectangle(-32, -72/2, PR.VIEWPORT_WIDTH, PR.VIEWPORT_HEIGHT), new Vector2(64, 72));
            this.Camera = new Camera2D(PR.VIEWPORT_WIDTH, PR.VIEWPORT_HEIGHT);
        }

        public void Update(float delta)
        {
            Grid.Update();
            BloomPostprocess.BloomComponent
            //Grid.ApplyImplosiveForce(70, new Vector2(400, 500), 600);
        }
    }
}
