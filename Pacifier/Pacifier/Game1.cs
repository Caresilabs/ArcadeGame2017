using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacifier.Screens;

namespace Pacifier
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
#if (!ARCADE)
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;
#else
        public override string GameDisplayName { get { return "Cloud Colony"; } }
#endif

        public Screen CurrentScreen { get; private set; }

        private Screen nextScreen;

       // private Sprite frame;

       // private Sprite transitionSprite;
       // private FrameAnimation transitionAnimation;

        public Game1()
        {
#if (!ARCADE)
                graphics = new GraphicsDeviceManager(this);
#endif

        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            base.Initialize();
        }

        protected override void LoadContent()
        {
#if (!ARCADE)
                    // Create a new SpriteBatch, which can be used to draw textures.
                    spriteBatch = new SpriteBatch(GraphicsDevice);
#endif
            // Load Content
            PR.Load(Content);
            

            nextScreen = new GameScreen(true, true);
            SetNextScreen();

        }

        protected override void UnloadContent()
        {
        }

        public float Delta { get; private set; }
        protected override void Update(GameTime gameTime)
        {
#if (!ARCADE)
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif

            // get second between last frame and current frame, used for fair physics manipulation and not based on frames
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // then update the screen
            CurrentScreen.Update(Delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Draw screen
            CurrentScreen.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void SetNextScreen()
        {
            // Dispose old screen
            if (CurrentScreen != null)
                CurrentScreen.Dispose();

            // init new screen
            CurrentScreen = nextScreen;
            nextScreen.Game = this;
            nextScreen.Graphics = GraphicsDevice;
            CurrentScreen.Init();

            nextScreen = null;
        }

        public void SetScreen(Screen newScreen)
        {
            this.nextScreen = newScreen;

            SetNextScreen();
        }
    }
}
