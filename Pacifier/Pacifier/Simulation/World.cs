using CloudColony.Framework;
using CloudColony.Framework.Tools;
using Microsoft.Xna.Framework;
using Pacifier.Entities;
using Pacifier.Entities.Entities;
using Pacifier.Entities.Particles;
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
            READY, RUNNING, GREENWON, YELLOWWON
        }

        public const float WORLD_WIDTH = 17.5f;
        public const float WORLD_HEIGHT = 10f;

        public Grid Grid { get; private set; }

        public ParticleManager<ParticleState> ParticleManager { get; private set; }

        public Camera2D Camera { get; private set; }

        public List<Entity> Entities { get; private set; }
        public List<Entity> DeadEntities { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public Player PlayerGreen { get; private set; }
        public Player PlayerYellow { get; private set; }

        public WorldState State { get; private set; }

        public SpatialHashGrid Collisions { get; private set; }

        public float SlowmoTime { get; private set; }

        public float Time { get; private set; }

        private Spawner spawner;

        public World()
        {
            //this.Grid = new Grid(new Rectangle(-64, -72, PR.VIEWPORT_WIDTH + 64, PR.VIEWPORT_HEIGHT + 72), new Vector2(64, 72));
            this.Collisions = new SpatialHashGrid();
            this.Collisions.Setup((int)(WORLD_WIDTH + 1), (int)(WORLD_HEIGHT + 1), 3.0f);
            this.Grid = new Grid(new Rectangle(-1,-1, (int)WORLD_WIDTH + 2, (int)WORLD_HEIGHT + 1), new Vector2(1, 1));
            this.Camera = new Camera2D(WORLD_WIDTH, WORLD_HEIGHT);
            this.Entities = new List<Entity>();
            this.DeadEntities = new List<Entity>();
            this.Enemies = new List<Enemy>();
            this.spawner = new Spawner(this);
            this.ParticleManager = new ParticleManager<ParticleState>(1024 * 2, ParticleState.UpdateParticle);
            InitWorld();
        }

        private void InitWorld()
        {
            PlayerGreen = new Player(this, PlayerIndex.One, PR.PlayerGreen, WORLD_WIDTH/2f - 2, WORLD_HEIGHT / 2f);
            PlayerYellow = new Player(this, PlayerIndex.Two, PR.PlayerYellow, WORLD_WIDTH /2f + 2, WORLD_HEIGHT / 2f);

            Add(PlayerGreen);
            Add(PlayerYellow);

            spawner.SpawnDumbbell();
        }

        public void Update(float delta)
        {
            Time += delta;

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
                    if (PlayerYellow.IsDead && PlayerGreen.IsDead)
                    {
                        if (PlayerGreen.Score > PlayerYellow.Score)
                        {
                            State = WorldState.GREENWON;
                        }
                        else
                        {
                            State = WorldState.YELLOWWON;
                        }

                        foreach (var item in Enemies)
                            item.Kill();

                        SlowmoTime = 2.5f;
                    }

                    break;
                case WorldState.GREENWON:
                    break;
                case WorldState.YELLOWWON:
                    break;
                default:
                    break;
            }

            // Update our hashgrid
            Collisions.ClearBuckets();
            Collisions.AddObject(Entities);

            Grid.Update();
            ParticleManager.Update();

            UpdateEntities(delta);
            spawner.Update(delta);
            
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
