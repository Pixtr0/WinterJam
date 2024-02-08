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
        public static int currentHp { get; set; }

        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0, 12.5f * GameSettings.Grid.ScaleFactor); } }
        public static Vector2[] SurroundingTiles { get; set; } = new Vector2[] { new Vector2(8, 6), new Vector2(8, 5), new Vector2(6, 6), new Vector2(7, 6), new Vector2(9, 6), new Vector2(6, 9), new Vector2(7, 9), new Vector2(8, 9), new Vector2(9, 9), new Vector2(6, 7), new Vector2(9, 7), new Vector2(6, 8), new Vector2(9, 8) };
        public static Vector2[] HouseTiles { get; set; } = new Vector2[] { new Vector2(7, 7), new Vector2(7, 8), new Vector2(8, 7), new Vector2(8, 8) };
        private Vector2 HealthPosition { get { return new Vector2(anchorPoint.X - GameSettings.GameFont.MeasureString(currentHp.ToString()).X / 2, anchorPoint.Y - Size.Y / 3 * 2);}}
        public static Texture2D HealthBarTexture { get; set; }
        private Vector2 HealthBarSize { get { return new Vector2(44, 8) * GameSettings.Grid.ScaleFactor; } }
        private Vector2 HPSize { get { return new Vector2(10, 20) * GameSettings.Grid.ScaleFactor; } }
        private Rectangle HealthBarDestinationRectangle
        {
            get
            {
                return new Rectangle((int)(HealthPosition.X - HealthBarSize.X/2), 
                    (int)HealthPosition.Y, 
                    (int)HealthBarSize.X, 
                    (int)HealthBarSize.Y);
            }
        }
        private Rectangle HPDestinationRectangle
        {
            get
            {
                return new Rectangle((int)(HealthPosition.X - HealthBarSize.X / 2 + 2 * GameSettings.Grid.ScaleFactor),
                    (int)(HealthPosition.Y + 2 * GameSettings.Grid.ScaleFactor),
                    (int)((HealthBarSize.X - 15) * (float)currentHp / maxHp),
                    (int)(HealthBarSize.Y - 15));
            }
        }
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
            //HealthPosition = new Vector2(anchorPoint.X - GameSettings.GameFont.MeasureString(currentHp.ToString()).X / 2, anchorPoint.Y - Size.Y / 3 * 2);
            
            //spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), HealthPosition + new Vector2(1, 0) * GameSettings.Grid.ScaleFactor, Color.White);
            //spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), HealthPosition + new Vector2(-1, 0) * GameSettings.Grid.ScaleFactor, Color.White);
            //spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), HealthPosition + new Vector2(0, 1) * GameSettings.Grid.ScaleFactor, Color.White);
            //spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), HealthPosition + new Vector2(0, -1) * GameSettings.Grid.ScaleFactor, Color.White);
            //spriteBatch.DrawString(GameSettings.GameFont, currentHp.ToString(), HealthPosition, Color.Black);

            //Rectangle HPDestinationRectangle = new Rectangle((int)(HealthPosition.X - HealthBarSize.X/2), (int)(HealthPosition.Y + HealthBarDestinationRectangle.Height / 4),                (int)((HealthBarDestinationRectangle.Width - 20)* (float)currentHp / maxHp), HealthBarDestinationRectangle.Height/2);
            spriteBatch.Draw(HealthBarTexture, HealthBarDestinationRectangle, Color.White);
            spriteBatch.Draw(HealthBarHPTexture, HPDestinationRectangle, Color.White);
        }
    }
}
