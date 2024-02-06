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

        public Item(int durability, Texture2D texture, Vector2 size, GameObject parent)
        {
            Random random = new Random();
            Durability = durability;
            MaxDurability = durability;

            Visualisation = new SpriteSheet(texture, parent.anchorPoint + new Vector2(-size.X/2 , parent.Size.Y - size.Y), size, 0, 1, 1, 1, false) ;
            Color = new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
        }
        public virtual void Update()
        {
            //Durability--;

            if (Durability <= 0)
                IsActive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            spriteBatch.Draw(Visualisation.Texture, Visualisation.DestinationRectangle, Color);
        }
    }
}
