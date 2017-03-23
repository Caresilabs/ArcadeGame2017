using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacifier.Utils;
using ShapeBlaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier.Screens
{
    public class MainMenuScreen : Screen
    {
        public bool PlayerGreenReady { get; private set; }
        public bool PlayerYellowReady { get; private set; }

        public float TotalTime { get; private set; }

        public float PlayDelayTime { get; private set; }

        //public bool BothReady { get; set; }

        public Sprite Background { get; private set; }

        public Sprite GreenBackground { get; private set; }
        public Sprite YellowBackground { get; private set; }
        public Sprite CoinBackground { get; private set; }

        private Sprite logo;

        private long highscore;

        private Grid grid;

        public float bpm = 1;
        public float bpmTimer;

        public override void Init()
        {
            this.PlayerGreenReady = false;
            this.PlayerYellowReady = false;
            //this.BothReady = false;

            highscore = HighscoreManager.GetHighscores()[0];
            InitUI();

            grid = new Grid(new Rectangle(-128, -108, PR.VIEWPORT_WIDTH + 128, PR.VIEWPORT_HEIGHT + 108), new Vector2(128, 108));
            grid.thickBig = 12;
            grid.thickSmall = 6;
            grid.ApplyExplosiveForce(100, new Vector2(PR.VIEWPORT_WIDTH, PR.VIEWPORT_HEIGHT) /2f, 600, Grid.gridColor);

            bpm = PR.PlayMenuMusic();
            //MediaPlayer.Volume = 0.33f;
        }

        private void InitUI()
        {
            logo = new Sprite(PR.Logo, PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.235f, 358, 64);
            logo.SetScale(2.3f);

            //Background = new Sprite(PR.MenuBackground, PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT / 2f, PR.VIEWPORT_WIDTH, PR.VIEWPORT_HEIGHT);
            //Background.ZIndex = 1f;

            GreenBackground = new Sprite(PR.Button2, PR.VIEWPORT_WIDTH * 0.25f, PR.VIEWPORT_HEIGHT * 0.55f, 600, 180);
            GreenBackground.ZIndex = 0.9f;

            YellowBackground = new Sprite(PR.Button2, PR.VIEWPORT_WIDTH * 0.75f, PR.VIEWPORT_HEIGHT * 0.55f, 600, 180);
            YellowBackground.ZIndex = 0.9f;

            CoinBackground = new Sprite(PR.Button1, PR.VIEWPORT_WIDTH * 0.5f, PR.VIEWPORT_HEIGHT * 0.85f, 500, 140);
            CoinBackground.ZIndex = 0.9f;
        }

        public override void Update(float delta)
        {
            TotalTime += delta;

            grid.Update();

            // Hack so input wont happen after every game
            if (TotalTime > 0.4f)
            {
                //if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Side) == InputState.Released ||
                //       InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Side) == InputState.Released)
                //{
                //    SetScreen(new CreditsScreen());
                //    return;
                //}

                if (PR.AnyKeyJustClicked(PlayerIndex.One))
                    PlayerGreenReady = true;

                if (PR.AnyKeyJustClicked(PlayerIndex.Two))
                    PlayerYellowReady = true;
            }

            if (PlayerGreenReady || PlayerYellowReady)
            {
                if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released 
                    || InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                {
                    SetScreen(new GameScreen(PlayerGreenReady, PlayerYellowReady));
                }
            }

            bpmTimer += delta;
            if (bpmTimer >= bpm)
            {
                bpmTimer -= bpm;
                grid.ApplyExplosiveForce(40, new Vector2(PR.VIEWPORT_WIDTH * 0.25f, PR.VIEWPORT_HEIGHT * 0.55f), 550, Color.Green * 0.7f);
                grid.ApplyExplosiveForce(40, new Vector2(PR.VIEWPORT_WIDTH * 0.75f, PR.VIEWPORT_HEIGHT * 0.55f), 550, Color.Yellow * 0.55f);
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            Graphics.Clear(Color.Black);

            batch.Begin(SpriteSortMode.BackToFront,
                    BlendState.AlphaBlend,
                    SamplerState.LinearClamp,
                    null,
                    null,
                    null,
                    null);

            grid.Draw(batch);
            //Background.Draw(batch);

            // Draw player green side
            GreenBackground.Draw(batch);
            if (PlayerGreenReady)
            {
                string txt = "Player 1 READY";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.25f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Green, (float)Math.Sin(TotalTime * (Math.PI * bpm * 4)) / 10f, PR.Font.MeasureString(txt) / 2f, 1.45f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 1 - Join";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.25f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Green, 0, PR.Font.MeasureString(txt) / 2f, 1.15f + (float)((Math.Cos(TotalTime * Math.PI * bpm * 2)) / 15f), SpriteEffects.None, 0);
            }

            // Draw player yellow side
            YellowBackground.Draw(batch);
            if (PlayerYellowReady)
            {
                string txt = "Player 2 READY";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.75f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Yellow, (float)Math.Sin(TotalTime * Math.PI * bpm * 4) / 10f, PR.Font.MeasureString(txt) / 2f, 1.45f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 2 - Join";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.75f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Yellow, 0, PR.Font.MeasureString(txt) / 2f, 1.15f + (float)(Math.Cos(TotalTime * Math.PI * bpm * 2) / 15f), SpriteEffects.None, 0);
            }

            // Draw logo
            {
                string highscoreText = "PACIFIER";
                batch.DrawString(PR.Font, highscoreText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.18f),
                     Color.White, 0, PR.Font.MeasureString(highscoreText) / 2f, 3.0f, SpriteEffects.None, 0);
            }


            {
                string highscoreText = "Highscore";
                batch.DrawString(PR.Font, highscoreText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.31f),
                     Color.White, 0, PR.Font.MeasureString(highscoreText) / 2f, 1.4f, SpriteEffects.None, 0);

                highscoreText = highscore.ToString();
                batch.DrawString(PR.Font, highscoreText, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.38f),
                     Color.White, 0, PR.Font.MeasureString(highscoreText) / 2f, 1.8f, SpriteEffects.None, 0);
            }

            // Draw insert coin
            if (PlayerGreenReady || PlayerYellowReady)
            {
                CoinBackground.Draw(batch);

                string insert = "Press Start To Play!";
                batch.DrawString(PR.Font, insert, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.83f),
                     Color.White * (float)((Math.Sin(TotalTime * Math.PI * bpm * 2) + 1) / 2f), 0, PR.Font.MeasureString(insert) / 2f, 1.9f, SpriteEffects.None, 0);
                //new Color(56, 45, 0)
            }

            // Copy right.
            string copy = "Created by Simon Bothen ... github.com/Caresilabs/ArcadeGame2017 ... " + "(c) " + DateTime.Now.Year;
            batch.DrawString(PR.Font, copy, new Vector2(PR.VIEWPORT_WIDTH - (TotalTime * 150) % (PR.VIEWPORT_WIDTH + PR.Font.MeasureString(copy).X * 1.15f), 10f),
               Color.WhiteSmoke, 0, Vector2.Zero, 1.15f, SpriteEffects.None, 0);


            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
