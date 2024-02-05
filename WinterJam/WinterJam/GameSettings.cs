using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class GameSettings
    {
        public static int Cellsize { get; set; } = 60;
        public static int Columns { get; set; } = 11;
        public static int Rows { get; set; } = 14;
        public static (Keys left, Keys right, Keys up, Keys down) ControlKeys { get; set; }
    }
}
