using Isometric_Thingy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
