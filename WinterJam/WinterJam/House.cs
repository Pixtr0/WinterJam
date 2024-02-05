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
        public House(Texture2D texture) {
            Vector2 size = new Vector2(58, 102) * GameSettings.Grid.ScaleFactor;
            Visualisation = new SpriteSheet(texture, GameSettings.Grid.GetPlayerPosition(new Vector2(8, 8)) - new Vector2(12 * GameSettings.Grid.ScaleFactor,
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
