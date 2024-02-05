using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class Enemy
    {
        private Vector2 SpawnPosition {  get; set; }
        private Vector2 TargetLocation { get; set; }

        private SpriteSheet  CurrentSpriteSheet { get; set; }
        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();


        public Enemy(Vector2 Spawn)
        {

        }
    }
}
