
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterJam.Screens;

namespace WinterJam
{
    public static class GameSettings
    {
        public static Vector2 ScreenSize = new Vector2(1920, 1080);
        public static int Cellsize { get; set; } = 60;
        public static int Columns { get; set; } = 11;
        public static int Rows { get; set; } = 14;
        public static Grid Grid { get; set; }
        public static (Keys left, Keys right, Keys up, Keys down) ControlKeys { get; set; } = (Keys.A, Keys.D, Keys.W, Keys.S);
        public static Screen StartScreen { get; set; }
        public static PlayScreen PlayScreen { get; set; }
        public static Screen GameOverScreen { get; set; }
        public static Screen ActiveScreen { get; set; }
        public static Screen SettingsScreen { get; set; }
        public static Screen PauseScreen { get; set; }

        public static Texture2D SwingEffect { get; set; }
        public static Texture2D SwingEffect2 { get; set; }
        public static SpriteFont GameFont { get; set; }
        public static SpriteFont UiFont { get; set; }
        public static Texture2D ScreenTexture { get; set; }

        public static bool IsCloseButtonPressed { get; set; }
        public static bool IsSettingsScreenDrawn { get; set; }
        public static bool IsPauseScreenDrawn { get; set; }
        public static Texture2D Button_Yellow { get; set; }
        public static Texture2D Button_Orange { get; set; }
        public static Texture2D Button_Pressed_Yellow { get; set; }
        public static Texture2D Button_Pressed_Orange { get; set; }
        public static Texture2D PausedText { get; set; }
        public static Texture2D UI_Volume { get; set; }
        public static Texture2D UI_Volume_slider { get; set; }
        public static Texture2D UI_game_over { get; set; }
        public static Texture2D BasketTexture { get; set; }

    }
}
