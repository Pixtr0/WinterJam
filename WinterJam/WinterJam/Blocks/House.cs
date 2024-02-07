using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteSheetClass;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class House : GameObject
    {
        private const float _delay = 0.14f; // seconds
        private float _remainingDelay = _delay;
        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0, 14 * GameSettings.Grid.ScaleFactor); } }
        public static Vector2[] SurroundingTiles { get; set; } = new Vector2[]{ new Vector2(6, 6), new Vector2(7, 6), new Vector2(8, 6), new Vector2(9, 6), new Vector2(6, 9), new Vector2(7, 9), new Vector2(8, 9), new Vector2(9, 9), new Vector2(6, 7), new Vector2(9, 7), new Vector2(6, 8), new Vector2(9, 8) };
        public static Vector2[] HouseTiles { get; set; } = new Vector2[] { new Vector2(7, 7), new Vector2(7, 8), new Vector2(8, 7), new Vector2(8, 8) };

        public static List<Item> Inventory {  get; set; }

        public House(Texture2D texture) {
            Vector2 size = new Vector2(58, 102) * GameSettings.Grid.ScaleFactor;
            Visualisation = new SpriteSheet(texture, GameSettings.Grid.GetGridPosition(HouseTiles[HouseTiles.Length - 1]) - new Vector2(12 * GameSettings.Grid.ScaleFactor,
               size.Y - 24 * GameSettings.Grid.ScaleFactor), size, 0,10,10,0,true);
        }

        public override void Update(GameTime gameTime)
        {
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                base.Update(gameTime);
                _remainingDelay = _delay;

            }
            
        }
    }
}
