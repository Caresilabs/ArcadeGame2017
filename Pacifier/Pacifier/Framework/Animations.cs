using System.Collections.Generic;

namespace CloudColony.Framework
{
    public class Animations
    {
        private Dictionary<string, Animation> animations;

        public Animation CurrentAnimation { get; private set; }

        public Animations()
        {
            this.animations = new Dictionary<string, Animation>();
        }

        public void Update(float delta)
        {
            foreach (var animation in animations)
            {
                animation.Value.Update(delta);
            }
        }

        public TextureRegion GetRegion()
        {
            if (CurrentAnimation != null)
                return CurrentAnimation.GetRegion();
            else return null;
        }

        public void SetAnimation(string name)
        {
            CurrentAnimation = animations[name];
        }

        public void AddAnimation(string name, Animation animation)
        {
            animations[name] = animation;
        }

        public bool HasAnimations()
        {
            return animations.Count != 0;
        }

        public bool HasNext()
        {
            if (CurrentAnimation != null)
                return CurrentAnimation.HasNext();
            else return false;
        }

        public void Clear()
        {
            CurrentAnimation = null;
            animations.Clear();
        }
    }
}
