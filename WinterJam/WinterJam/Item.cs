using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class Item : GameObject
    {
        public Color Color { get; set; }
        public int MaxDurability { get; set; }
        public int Durability { get; set; }
        public Double ItemAngle { get; set; } = 0f;
        public float ItemOffset { get; set; } = 0f;
        public Vector2 ParentSize { get; set; }

        private const float _delay = 0.2f; // seconds
        private float _remainingDelay = _delay;
        public Item(int durability, Texture2D texture, Vector2 size, GameObject parent)
        {
            Random random = new Random();
            Durability = durability;
            MaxDurability = durability;
            ParentSize = parent.Size;

            Visualisation = new SpriteSheet(texture, parent.anchorPoint - new Vector2(size.X/2, 0), size, 0, 1, 1, 1, false) ;
            Color = new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
        }
        public void Update(GameTime gametime, Vector2 parentAnchorPoint)
        {
            //Durability--;

            if (Durability <= 0)
                IsActive = false;

            var timer = (float)gametime.ElapsedGameTime.TotalSeconds;
            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                ItemAngle++;

                if (ItemAngle >= 360f)
                    ItemAngle = 1f;

                _remainingDelay = _delay;
            }

            TopLeftPosition = parentAnchorPoint + new Vector2(-Size.X / 2, -ParentSize.Y - 10 - Size.Y - ItemOffset * 4);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            spriteBatch.Draw(Visualisation.Texture, Visualisation.DestinationRectangle, Color);
        }
    }
}
