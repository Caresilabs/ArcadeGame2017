using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier
{
   public class PR
    {
        // Globals
        public const int VIEWPORT_WIDTH = 1920;
        public const int VIEWPORT_HEIGHT = 1080;

        public static readonly Vector2 SCREEN_SIZE = new Vector2(VIEWPORT_WIDTH, VIEWPORT_HEIGHT);

        // Game
        public static Texture2D Atlas { get; private set; }

        public static TextureRegion Pixel { get; private set; }

        public static TextureRegion Dumbbell { get; private set; }
        public static TextureRegion PlayerRed { get; private set; }
        public static TextureRegion PlayerBlue { get; private set; }


        // UI
        public static TextureRegion ScoreBoard { get; private set; }


        public static TextureRegion Button1 { get; private set; }
        public static TextureRegion Button2 { get; private set; }

        public static TextureRegion Logo { get; private set; }

        public static TextureRegion Simon { get; private set; }
        public static TextureRegion Sebastian { get; private set; }

        public static TextureRegion WinBlue { get; private set; }
        public static TextureRegion WinRed { get; private set; }


        // Fonts
        public static SpriteFont Font { get; private set; }

        // Sound
        public static SoundEffect HitSound { get; private set; }

        public static void Load(ContentManager content)
        {
            LoadAssets(content);
        }

        private static void LoadAssets(ContentManager content)
        {
            Atlas = content.Load<Texture2D>("Region");

            //PointerBlue = new TextureRegion(Atlas, 256 + 20 + 32, 0, 32, 32);

            // Font = content.Load<SpriteFont>("Font");

            Pixel = new TextureRegion(Atlas, 0, 0, 1, 1);

            Dumbbell = new TextureRegion(Atlas, 0, 32, 148, 32);

            PlayerRed = new TextureRegion(Atlas, 0, 64, 64, 64);
            PlayerBlue = new TextureRegion(Atlas, 0, 64, 64, 64);


            // Sound
            SoundEffect.MasterVolume = 1.0f;

            MediaPlayer.Volume = 0.0f;

            MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(content.Load<Song>("Sound/JuhaniJunkalaEpicBossBattle"));

           // HitSound = content.Load<SoundEffect>("Sound/Hit");
          
        }

        public static bool AnyKeyPressed(PlayerIndex index)
        {
            bool pressed = false;
            for (int i = 1; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(index, (PlayerInput)i) == InputState.Released)
                    pressed = true;
            }

            return pressed;
        }


        public static bool AnyKeyJustClicked(PlayerIndex index)
        {
            bool pressed = false;
            for (int i = 1; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(index, (PlayerInput)i) == InputState.Pressed)
                    pressed = true;
            }

            return pressed;
        }

        public static bool AnyKeyPressedNoJoystick(PlayerIndex index)
        {
            bool pressed = false;
            for (int i = 6; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(index, (PlayerInput)i) == InputState.Released)
                    pressed = true;
            }

            return pressed;
        }
    }
}

