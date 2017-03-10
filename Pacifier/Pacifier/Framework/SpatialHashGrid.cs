﻿using Microsoft.Xna.Framework;
using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Pacifier.Entities;

namespace CloudColony.Framework
{
    /// <summary>
    /// TODO Explain
    /// Insert O(1) (hashtable)
    /// Remove Average O(1)
    /// Clear O(1)
    /// 
    /// </summary>
    public class SpatialHashGrid
    {
        private List<Entity>[] Buckets;

        private int Rows;
        private int Cols;
        private int SceneWidth;
        private int SceneHeight;
        private float CellSize;

        public void Setup(int scenewidth, int sceneheight, float cellsize)
        {
            Cols = (int)Math.Ceiling(scenewidth / cellsize);
            Rows = (int)Math.Ceiling(sceneheight / cellsize);
            Buckets = new List<Entity>[Cols * Rows];

            for (int i = 0; i < Cols * Rows; i++)
            {
                Buckets[i] = new List<Entity>();
            }

            SceneWidth = scenewidth;
            SceneHeight = sceneheight;
            CellSize = cellsize;
        }


        public void ClearBuckets()
        {
            //Buckets.Clear();
            for (int i = 0; i < Cols * Rows; i++)
            {
                Buckets[i].Clear();
            }
        }

        public void AddObject(Entity obj)
        {
            var cellIds = GetIdForObj(obj.Bounds);
            foreach (var item in cellIds)
            {
                Buckets[(item)].Add(obj);
            }
        }

        public void AddObject(System.Collections.Generic.IEnumerable<Entity> objs)
        {
            foreach (var item in objs)
            {
                AddObject(item);
            }
        }

        private List<int> bucketsObjIsIn = new List<int>();
        private List<int> GetIdForObj(Circle bounds)
        {
            bucketsObjIsIn.Clear();

            float width = Cols;

            float xMax = Math.Min(SceneWidth, bounds.Center.X + bounds.Radius);
            float yMax = Math.Min(SceneHeight, bounds.Center.Y + bounds.Radius);

            for (float x = (int)Math.Max(0, bounds.Center.X - bounds.Radius); x < xMax; x+= 2)
            {
                for (float y = (int)Math.Max(0, bounds.Center.Y - bounds.Radius); y < yMax; y+= 2)
                {
                    AddBucket(x, y, width, bucketsObjIsIn);
                }
            }

            //Vector2 min = new Vector2(
            //   Math.Max(Math.Min(obj.Position.X - (obj.Bounds.Radius), SceneWidth - 1), 1),
            //     Math.Max(Math.Min(obj.Position.Y - (obj.Bounds.Radius), SceneHeight - 1), 1));

            //Vector2 max = new Vector2(
            //    Math.Max(Math.Min(obj.Position.X + (obj.Bounds.Radius), SceneWidth - 1), 1),
            //   Math.Max(Math.Min(obj.Position.Y + (obj.Bounds.Radius), SceneHeight - 1), 1));

            //float width = Cols;

            ////TopLeft
            //AddBucket(min, width, bucketsObjIsIn);

            ////TopRight
            //AddBucket(new Vector2(max.X, min.Y), width, bucketsObjIsIn);

            ////BottomRight
            //AddBucket(new Vector2(max.X, max.Y), width, bucketsObjIsIn);

            ////BottomLeft
            //AddBucket(new Vector2(min.X, max.Y), width, bucketsObjIsIn);

            return bucketsObjIsIn;
        }

        private void AddBucket(Vector2 vector, float width, List<int> buckettoaddto)
        {
            int cellPosition = (int)((Math.Floor(vector.X / CellSize)) + (Math.Floor(vector.Y / CellSize)) * width);

            if (!buckettoaddto.Contains(cellPosition))
                buckettoaddto.Add(cellPosition);

        }

        private void AddBucket(float x, float y, float width, List<int> buckettoaddto)
        {
            int cellPosition = (int)((Math.Floor(x / CellSize)) + (Math.Floor(y / CellSize)) * width);

            if (!buckettoaddto.Contains(cellPosition))
                buckettoaddto.Add(cellPosition);

        }

        private List<Entity> colliders = new List<Entity>();
        public Entity[] GetPossibleColliders(Entity obj)
        {
            colliders.Clear();
            var bucketIds = GetIdForObj(obj.Bounds);
            foreach (var item in bucketIds)
            {
                colliders.AddRange(Buckets[item]);
            }
            return colliders.Distinct().ToArray();
        }

        private Circle tmpCircle = new Circle(Vector2.Zero, 0);
        public Entity[] GetPossibleColliders(Enemy enemy, float r, Func<Entity, bool> cond = null)
        {
            colliders.Clear();
            tmpCircle.Center = enemy.Bounds.Center;
            tmpCircle.Radius = r;
            var bucketIds = GetIdForObj(tmpCircle);
            foreach (var item in bucketIds)
            {
                colliders.AddRange(Buckets[item]);
            }
            if (cond != null)
                return colliders.Distinct().Where(cond).ToArray();
            else
                return colliders.Distinct().ToArray();
        }
    }
}