using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteSheetClass;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace WinterJam
{
    public class House : GameObject
    {
        private const float _delay = 0.14f; // seconds
        private float _remainingDelay = _delay;

        private int maxHp { get; set; }
        public static  int currentHp { get; set; }

        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0, 12.5f * GameSettings.Grid.ScaleFactor); } }
        public static Vector2[] SurroundingTiles { get; set; } = new Vector2[] { new Vector2(8, 6), new Vector2(8, 5),new Vector2(6, 6), new Vector2(7, 6), new Vector2(9, 6), new Vector2(6, 9), new Vector2(7, 9), new Vector2(8, 9), new Vector2(9, 9), new Vector2(6, 7), new Vector2(9, 7), new Vector2(6, 8), new Vector2(9, 8) };
        public static Vector2[] HouseTiles { get; set; } = new Vector2[] { new Vector2(7, 7), new Vector2(7, 8), new Vector2(8, 7), new Vector2(8, 8) };
        public static Texture2D HealthBarTexture { get; set; }
        public static Texture2D HealthBarHPTexture { get; set; }
        public House(Texture2D texture)
        {
            Vector2 size = new Vector2(58, 102) * GameSettings.Grid.ScaleFactor;
            Visualisation = new SpriteSheet(texture, GameSettings.Grid.GetGridPosition(HouseTiles[HouseTiles.Length - 1]) - new Vector2(16 * GameSettings.Grid.ScaleFactor,
               size.Y - 24 * GameSettings.Grid.ScaleFactor), size, 0, 10, 10, 0, true);

            currentHp = maxHp = 20;
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Vector2 pos = new Vector2(anchorPoint.X - GameSettings.GameFont.MeasureString(currentHp.ToString()).X / 2, anchorPoint.Y - Size.Y / 3 * 2);
            spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), pos + new Vector2(1, 0) * GameSettings.Grid.ScaleFactor, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), pos + new Vector2(-1, 0) * GameSettings.Grid.ScaleFactor, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), pos + new Vector2(0, 1) * GameSettings.Grid.ScaleFactor, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), pos + new Vector2(0, -1) * GameSettings.Grid.ScaleFactor, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), pos, Color.Black);
        }
    }
}
