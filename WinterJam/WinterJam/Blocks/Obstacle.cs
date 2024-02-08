using Microsoft.Xna.Framework.Graphics;
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
    public class Obstacle : GameObject
    {
        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0,  18 * GameSettings.Grid.ScaleFactor - OffsetValue); } }
        public float OffsetValue { get; set; } = 0;
        public Vector2 indexPosition { get; set; }
        public bool IsLog { get; set; } = false;
        public Obstacle(Texture2D texture, Vector2 position)
        {
            indexPosition = position;
            Visualisation = new SpriteSheet(texture, IsLog ? GameSettings.Grid.GetGridPosition(indexPosition) : GameSettings.Grid.GetGridPositionNoHeight(indexPosition), new Vector2(24, 36) * GameSettings.Grid.ScaleFactor, 0, 1, 1, 0, false);
        }
        public Obstacle(Texture2D texture, Vector2 position, float offsetValue)
        {
            indexPosition = position;
            OffsetValue = offsetValue * GameSettings.Grid.ScaleFactor;
            Visualisation = new SpriteSheet(texture, IsLog ? GameSettings.Grid.GetGridPosition(indexPosition) : GameSettings.Grid.GetGridPositionNoHeight(indexPosition), new Vector2(24, 36) * GameSettings.Grid.ScaleFactor, 0, 1, 1, 0, false);
        }

    }
}
