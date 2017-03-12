using CloudColony.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Pacifier.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using CloudColony.Framework.Tools;
using Microsoft.Xna.Framework.Input;

namespace Pacifier.Screens
{
    public class GameScreen : Screen
    {
        public enum GameState
        {
            RUNNING, GAMEOVER, PAUSED
        }

        public Camera2D UICamera { get; private set; }

        public GameState State { get; private set; }

        public float TotalTime { get; private set; }

        private WorldRenderer renderer;
        private World world;

        public override void Init()
        {
            this.world = new World();
            this.renderer = new WorldRenderer(world);
            this.UICamera = new Camera2D(PR.VIEWPORT_WIDTH, PR.VIEWPORT_HEIGHT);

            world.Bpm = PR.PlayRandomSong();
            world.InitWorld();
        }

        public override void Update(float delta)
        {
            TotalTime += delta;

            switch (State)
            {
                case GameState.RUNNING:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                        InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        State = GameState.PAUSED;
                    }

                    world.Update(delta);

                    if (world.State == World.WorldState.GREENWON)
                    {
                        State = GameState.GAMEOVER;
                        //WinSprite = new Sprite(CC.WinRed, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.45f, 96, 64);
                        MediaPlayer.Volume = 0.21f;
                       // CC.WinSound.Play();
                    }

                    if (world.State == World.WorldState.YELLOWWON)
                    {
                        State = GameState.GAMEOVER;
                      //  WinSprite = new Sprite(CC.WinBlue, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.45f, 96, 64);
                        MediaPlayer.Volume = 0.21f;
                      //  CC.WinSound.Play();
                    }

                    break;
                case GameState.GAMEOVER:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                      InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        SetScreen(new GameScreen());
                    }

                   // WinSprite.SetScale(MathHelper.Lerp(WinSprite.Scale.X, 6f, delta * 4f));

                    world.Update(delta);

                    break;
                case GameState.PAUSED:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Side) == InputState.Released ||
                    InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Side) == InputState.Released)
                    {
                        SetScreen(new GameScreen());
                    }
                    else if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                    InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        State = GameState.RUNNING;
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            Graphics.Clear(Color.Black);
            renderer.Render(batch);

            batch.Begin(SpriteSortMode.BackToFront,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    UICamera.GetMatrix());
            {
                switch (State)
                {
                   
                    case GameState.RUNNING:
                        string scoreText = "Green Score: " + world.PlayerGreen.Score;
                        batch.DrawString(PR.Font, scoreText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.05f),
                            Color.White, 0, PR.Font.MeasureString(scoreText) / 2f, 2.0f, SpriteEffects.None, 0);

                        scoreText = "Yellow Score: " + world.PlayerYellow.Score;
                        batch.DrawString(PR.Font, scoreText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.1f),
                            Color.White, 0, PR.Font.MeasureString(scoreText) / 2f, 2.0f, SpriteEffects.None, 0);


                        scoreText = "Enemies: " + world.Enemies.Count;
                        batch.DrawString(PR.Font, scoreText, new Vector2(2, PR.VIEWPORT_HEIGHT * 0.05f),
                            Color.White, 0, Vector2.Zero, 1.3f, SpriteEffects.None, 0);

                        break;
                    case GameState.GAMEOVER:

                        //WinSprite.Draw(batch);

                        string continueText = "Press start to continue...";
                        batch.DrawString(PR.Font, continueText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.67f),
                            Color.White, 0, PR.Font.MeasureString(continueText) / 2f, 2.2f, SpriteEffects.None, 0);

                        break;
                    case GameState.PAUSED:
                        string txt = "PAUSED";
                        batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.5f),
                            Color.White, 0, PR.Font.MeasureString(txt) / 2f, 3.5f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);

                        txt = "-Side button to exit-";
                        batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.575f),
                            Color.White, 0, PR.Font.MeasureString(txt) / 2f, 1.85f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
                        break;
                    default:
                        break;
                }
            }

            batch.End();
        }

        public override void Dispose()
        {
            MediaPlayer.Stop();
        }
    }
}
