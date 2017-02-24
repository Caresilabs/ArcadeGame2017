using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacifier;

namespace CloudColony.Framework.Tools
{
    /**
     * Camera makes the live a lot easier by translating a matrix and then tell the spritebatch to translate all objects by the values
     * set in this class
     */
    public class Camera2D
    {
        private Matrix transform;
        private Vector2 position;
        private Vector2 zoom;
        private Vector2 viewportStretch;
        private Vector2 defaultViewPort;

        private float rotation;

        public Camera2D(float width, float height)
        {
            this.defaultViewPort = new Vector2(width, height);
            this.rotation = 0f;
            this.viewportStretch = new Vector2(1, 1);
            this.zoom = new Vector2(1, 1);
            this.position = Vector2.Zero;
            this.GetMatrix();
        }

        private Camera2D Update()
        {
            viewportStretch.X = PR.VIEWPORT_WIDTH / (float)defaultViewPort.X;
            viewportStretch.Y = PR.VIEWPORT_HEIGHT / (float)defaultViewPort.Y;

            return this;
        }

        // Gets the matrix used by the spritebatch
        public Matrix GetMatrix()
        {
            Update();

            transform =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(viewportStretch.X * zoom.X, viewportStretch.Y * zoom.Y, 1));// *
            //Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));

            return transform;
        }

        public Vector2 Unproject(float x, float y)
        {
            return Vector2.Transform(new Vector2(x, y), Matrix.Invert(transform));
        }

        public Vector2 GetZoom()
        {
            return zoom;
        }

        public Vector2 GetViewPortScale()
        {
            return viewportStretch;
        }

        public void SetZoom(float value)
        {
            this.zoom.X = value;
            this.zoom.Y = value;
        }

        public float GetRotation()
        {
            return rotation;
        }

        public void SetRotation(float rot)
        {
            rotation = rot;
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public float GetWidth()
        {
            return defaultViewPort.X;
        }

        public float GetHeight()
        {
            return defaultViewPort.Y;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(float x, float y)
        {
            position.X = x;
            position.Y = y;
        }

        public void Translate(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        public void SetPosition(Vector2 pos)
        {
            position.X = pos.X;
            position.Y = pos.Y;
        }

        public bool IsInside(Rectangle r)
        {
            if (r.Left < position.X) return false;
            if (r.Right > position.X + GetWidth()) return false;
            if (r.Top < position.Y) return false;
            if (r.Bottom > position.Y + GetHeight()) return false;

            return true;
        }
    }
}
