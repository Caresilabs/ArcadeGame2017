using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacifier.Utils;
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

        public override void Init()
        {
            this.PlayerGreenReady = false;
            this.PlayerYellowReady = false;
            //this.BothReady = false;

            highscore = HighscoreManager.GetHighscores()[0];
            InitUI();

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
                //PlayDelayTime += delta;

                //if (PlayDelayTime >= 1f)
                //{
                //    if (!BothReady)
                //    {
                //        SetScreen(new GameScreen());
                //        //PR.PlayGameSound.Play();
                //        //PR.PlayGameSound.Play();
                //        // MediaPlayer.Volume = 0.12f;
                //        BothReady = true;
                //    }
                //}
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            Graphics.Clear(Color.Black);

            batch.Begin(SpriteSortMode.BackToFront,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    null);

            //Background.Draw(batch);

            // Draw player green side
            GreenBackground.Draw(batch);
            if (PlayerGreenReady)
            {
                string txt = "Player 1 READY";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.25f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Green, (float)Math.Sin(TotalTime * 10) / 10f, PR.Font.MeasureString(txt) / 2f, 1.45f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 1 - Join";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.25f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Green, 0, PR.Font.MeasureString(txt) / 2f, 1.15f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            // Draw player yellow side
            YellowBackground.Draw(batch);
            if (PlayerYellowReady)
            {
                string txt = "Player 2 READY";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.75f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Yellow, (float)Math.Sin(TotalTime * 10) / 10f, PR.Font.MeasureString(txt) / 2f, 1.45f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 2 - Join";
                batch.DrawString(PR.Font, txt, new Vector2(PR.VIEWPORT_WIDTH * 0.75f, PR.VIEWPORT_HEIGHT * 0.55f),
                    Color.Yellow, 0, PR.Font.MeasureString(txt) / 2f, 1.15f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            // Draw logo
            logo.SetPosition(logo.Position.X, logo.Position.Y + (float)Math.Sin(TotalTime * 2) / 2.5f);
            logo.Draw(batch);

            // Draw insert coin
            {
                CoinBackground.Draw(batch);

                string insert = "Insert coin";
                batch.DrawString(PR.Font, insert, new Vector2(PR.VIEWPORT_WIDTH / 2f, PR.VIEWPORT_HEIGHT * 0.86f),
                     new Color(56, 45, 0) * (float)((Math.Sin(TotalTime * 5.0f) + 1) / 2f), 0, PR.Font.MeasureString(insert) / 2f, 1.9f, SpriteEffects.None, 0);
            }

            // Copy right.
            string copy = highscore +  " Created by Simon Bothen ... github.com/Caresilabs/ArcadeGame2017 ... " + "(c) " + DateTime.Now.Year;
            //batch.DrawString(PR.Font, copy, new Vector2(42, 29f),
            //    Color.WhiteSmoke * (float)((Math.Sin(TotalTime * 2.5f) + 1) / 2f), 0, Vector2.Zero, 1.15f, SpriteEffects.None, 0);
            batch.DrawString(PR.Font, copy, new Vector2(PR.VIEWPORT_WIDTH - (TotalTime * 150) % (PR.VIEWPORT_WIDTH + PR.Font.MeasureString(copy).X * 1.15f), 29f),
               Color.WhiteSmoke, 0, Vector2.Zero, 1.15f, SpriteEffects.None, 0);


            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
