using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class GameSettings
    {
        public static Vector2 WINDOWSIZE { get; set; } = new Vector2(1260, 840);
        public static int Cellsize { get; set; } = 60;
        public static int Columns { get; set; } = 11;
        public static int Rows { get; set; } = 14;
    }
}
