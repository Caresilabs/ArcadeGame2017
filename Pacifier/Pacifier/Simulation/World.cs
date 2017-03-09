using CloudColony.Framework.Tools;
using Microsoft.Xna.Framework;
using Pacifier.Entities;
using Pacifier.Entities.Entities;
using ShapeBlaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier.Simulation
{
    public class World
    {
        public enum WorldState
        {
            READY, RUNNING, REDWON, BLUEWON
        }

        public const float WORLD_WIDTH = 17.5f;
        public const float WORLD_HEIGHT = 10f;

        public Grid Grid { get; private set; }

        public Camera2D Camera { get; private set; }

        public List<Entity> Entities { get; private set; }
        public List<Entity> DeadEntities { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public Player PlayerRed { get; private set; }
        public Player PlayerBlue { get; private set; }

        public WorldState State { get; private set; }

        public float SlowmoTime { get; private set; }

        private Spawner spawner;

        public World()
        {
            //this.Grid = new Grid(new Rectangle(-64, -72, PR.VIEWPORT_WIDTH + 64, PR.VIEWPORT_HEIGHT + 72), new Vector2(64, 72));
            this.Grid = new Grid(new Rectangle(-1,-1, (int)WORLD_WIDTH + 2, (int)WORLD_HEIGHT + 1), new Vector2(1, 1));
            this.Camera = new Camera2D(WORLD_WIDTH, WORLD_HEIGHT);
            this.Entities = new List<Entity>();
            this.DeadEntities = new List<Entity>();
            this.Enemies = new List<Enemy>();
            this.spawner = new Spawner(this);
            InitWorld();
        }

        private void InitWorld()
        {
            PlayerRed = new Player(this, PlayerIndex.One, PR.PlayerRed, 0, 0);
            PlayerBlue = new Player(this, PlayerIndex.Two, PR.PlayerBlue, 0, 0);

            Add(PlayerRed);
            Add(PlayerBlue);

          
        }

        public void Update(float delta)
        {
            if (SlowmoTime > 0)
            {
                SlowmoTime -= delta;
                delta *= 0.2f;
            }

            switch (State)
            {
                case WorldState.READY:
                   // ReadyTime += delta;
                   // if (ReadyTime >= 4)
                        State = WorldState.RUNNING;
                    break;
                case WorldState.RUNNING:
                    if (PlayerBlue.IsDead)
                    {
                        State = WorldState.REDWON;
                        SlowmoTime = 2.5f;
                    }

                    if (PlayerRed.IsDead)
                    {
                        State = WorldState.BLUEWON;
                        SlowmoTime = 2.5f;
                    }

                    break;
                case WorldState.REDWON:
                    break;
                case WorldState.BLUEWON:
                    break;
                default:
                    break;
            }

            Grid.Update();
            UpdateEntities(delta);
            spawner.Update(delta);

            //if (Input)
            // Grid.ApplyImplosiveForce(.1f, new Vector2(3, 3), 3);
        }

        private void UpdateEntities(float delta)
        {
            foreach (var entity in Entities)
            {
                entity.Update(delta);

                if (entity.IsDead)
                    DeadEntities.Add(entity);
            }

            foreach (var dead in DeadEntities)
            {
                Entities.Remove(dead);
                if (dead is Dumbbell)
                {
                    spawner.SpawnDumbbell();
                }
            }
            DeadEntities.Clear();
        }

        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
            Add(enemy);
        }

        public void Add(Entity ent)
        {
            Entities.Add(ent);
        }
    }
}
