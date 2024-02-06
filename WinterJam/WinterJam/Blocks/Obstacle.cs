﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using SpriteSheetClass;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WinterJam
{
    internal class Obstacle : GameObject
    {
        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0, 20 * GameSettings.Grid.ScaleFactor); } }

        public Vector2 indexPosition { get; set; }
        public Obstacle() { }
        public Obstacle(Texture2D texture, Vector2 position )
        {
            indexPosition = position;
            Visualisation = new SpriteSheet(texture, GameSettings.Grid.GetPlayerPosition(indexPosition), new Vector2(24, 36) * GameSettings.Grid.ScaleFactor, 0, 1, 1, 0, false);

        }
    }
}