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
using Pacifier.Entities;

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

        private bool playerGreenJoined;
        private bool playerYellowJoined;

        private float gameOverDelay;
        private float lastManStandingTimer;

        public GameScreen(bool playerGreenReady, bool playerYellowReady)
        {
            this.playerGreenJoined = playerGreenReady;
            this.playerYellowJoined = playerYellowReady;
            this.gameOverDelay = 0;
            this.lastManStandingTimer = 10;
        }

        public override void Init()
        {
            this.world = new World();
            this.renderer = new WorldRenderer(world);
            this.UICamera = new Camera2D(PR.VIEWPORT_WIDTH, PR.VIEWPORT_HEIGHT);

            world.Bpm = PR.PlayRandomSong();
            world.InitWorld(playerGreenJoined, playerYellowJoined);
        }

        public override void Update(float delta)
        {
            TotalTime += delta;
            world.bpmTimer += delta;

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
                    else if (world.State == World.WorldState.YELLOWWON)
                    {
                        State = GameState.GAMEOVER;
                      //  WinSprite = new Sprite(CC.WinBlue, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.45f, 96, 64);
                        MediaPlayer.Volume = 0.21f;
                      //  CC.WinSound.Play();
                    }
                    else   // Last man stand
                    if (playerGreenJoined && playerYellowJoined)
                    {
                        if (world.PlayerGreen.IsDead || world.PlayerYellow.IsDead)
                        {
                            lastManStandingTimer -= delta;
                            if (lastManStandingTimer <= 0)
                            {
                                world.PlayerGreen.Kill();
                                world.PlayerYellow.Kill();
                            }
                        }
                    }

                    break;
                case GameState.GAMEOVER:
                    gameOverDelay += delta;

                    if (gameOverDelay > 1.4f)
                    {
                        if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                          InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                        {
                            SetScreen(new MainMenuScreen());
                        }
                        else if (PR.AnyKeyJustClicked(PlayerIndex.One) || PR.AnyKeyJustClicked(PlayerIndex.Two))
                        {
                            SetScreen(new GameScreen(playerGreenJoined, playerYellowJoined));
                        }
                    }

                    // WinSprite.SetScale(MathHelper.Lerp(WinSprite.Scale.X, 6f, delta * 4f));

                    world.Update(delta);

                    break;
                case GameState.PAUSED:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Side) == InputState.Released ||
                    InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Side) == InputState.Released)
                    {
                        //SetScreen(new GameScreen(playerGreenJoined, playerYellowJoined));
                        SetScreen(new MainMenuScreen());
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
                    SamplerState.LinearClamp,
                    null,
                    null,
                    null,
                    UICamera.GetMatrix());
            {

                // Draw debug
                //{
                //    var debugText = string.Format("Debug, FPS: {0}, Enemies: {1}", Math.Round(1f / Game.Delta), world.Enemies.Count);
                //    batch.DrawString(PR.Font, debugText, new Vector2(2, PR.VIEWPORT_HEIGHT * 0.95f),
                //        Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 0);
                //}

                switch (State)
                {

                    case GameState.RUNNING:
                        string scoreText = DrawScore(batch);

                        if (lastManStandingTimer != 10)
                        {
                            string countdownText = (Math.Ceiling(lastManStandingTimer)).ToString();
                            batch.DrawString(PR.Font, countdownText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.47f),
                                Color.White, 0, PR.Font.MeasureString(countdownText) / 2f,
                                    2.4f + 2 * (1 - (lastManStandingTimer - (float)Math.Floor(lastManStandingTimer))),
                                        SpriteEffects.None, 0);
                        }
                        break;
                    case GameState.GAMEOVER:

                        //WinSprite.Draw(batch);
                        DrawScore(batch);

                        if (playerGreenJoined && playerYellowJoined)
                        {
                            // Draw win/Lose
                            string wonText = (world.State == World.WorldState.GREENWON ? "Player 1" : "Player 2") + "  Won!";

                            if (world.PlayerYellow.Score == world.PlayerGreen.Score)
                            {
                                wonText = "Draw!";
                            }

                            batch.DrawString(PR.Font, wonText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.47f),
                                world.State == World.WorldState.GREENWON ? world.PlayerGreen.ShipColor : world.PlayerYellow.ShipColor,
                                    0, PR.Font.MeasureString(wonText) / 2f, 2.8f, SpriteEffects.None, 0);
                        }
                        else
                        {
                            string wonText = "Game Over!";
                            if (world.WasHighscore)
                                wonText = "HIGHSCORE!";

                            batch.DrawString(PR.Font, wonText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.47f),
                                playerGreenJoined ? world.PlayerGreen.ShipColor : world.PlayerYellow.ShipColor,
                                    0, PR.Font.MeasureString(wonText) / 2f, 2.8f, SpriteEffects.None, 0);
                        }

                        string continueText = "Press any key to restart!";
                        batch.DrawString(PR.Font, continueText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.78f),
                            Color.White, 0, PR.Font.MeasureString(continueText) / 2f, 1.8f, SpriteEffects.None, 0);

                        continueText = "Press start to exit...";
                        batch.DrawString(PR.Font, continueText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.86f),
                            Color.White, 0, PR.Font.MeasureString(continueText) / 2f, 1.4f, SpriteEffects.None, 0);

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

        private float greenScoreText;
        private float yellowScoreText;
        private string DrawScore(SpriteBatch batch)
        {
            string scoreText = string.Empty;
            float deltaScore;
            // GREEN
            if (playerGreenJoined)
            {
                scoreText = "Green Score";
                batch.DrawString(PR.Font, scoreText, new Vector2(PR.VIEWPORT_WIDTH * 0.05f, PR.VIEWPORT_HEIGHT * 0.03f),
                    Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

                deltaScore = MathHelper.Lerp(greenScoreText, world.PlayerGreen.Score, 0.07f) - greenScoreText;
                greenScoreText += deltaScore;
                batch.DrawString(PR.Font, (Math.Round(greenScoreText)).ToString(), new Vector2(PR.VIEWPORT_WIDTH * 0.05f, PR.VIEWPORT_HEIGHT * 0.1f),
                    Color.Lerp(Color.White, world.PlayerGreen.ShipColor, deltaScore * 0.002f), 0, Vector2.Zero, 2.0f + Math.Min(1.6f, deltaScore * 0.002f), SpriteEffects.None, 0);
            }


            // YELLOW
            if (playerYellowJoined)
            {
                scoreText = "Yellow Score";
                batch.DrawString(PR.Font, scoreText, new Vector2(PR.VIEWPORT_WIDTH * 0.95f - PR.Font.MeasureString(scoreText).X * 1.5f, PR.VIEWPORT_HEIGHT * 0.03f),
                    Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

                deltaScore = MathHelper.Lerp(yellowScoreText, world.PlayerYellow.Score, 0.07f) - yellowScoreText;
                yellowScoreText += deltaScore;
                scoreText = (Math.Round(yellowScoreText)).ToString();
                var origin = PR.Font.MeasureString(scoreText);
                origin.Y = 0;
                batch.DrawString(PR.Font, scoreText, new Vector2(PR.VIEWPORT_WIDTH * 0.95f, PR.VIEWPORT_HEIGHT * 0.1f), //- PR.Font.MeasureString(scoreText).X * 2
                    Color.Lerp(Color.White, world.PlayerYellow.ShipColor, deltaScore * 0.002f),
                        0, origin, 2.0f + Math.Min(1.6f, deltaScore * 0.002f), SpriteEffects.None, 0);
            }

            return scoreText;
        }

        public override void Dispose()
        {
            MediaPlayer.Stop();
        }
    }
}
