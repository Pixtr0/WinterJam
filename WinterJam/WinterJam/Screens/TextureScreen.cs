using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterJam.Screens;

namespace WinterJam
{
    internal class TextureScreen : Screen
    { 
        public Texture2D Texture { get; set; }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dr = new Rectangle(0, 0, (int)GameSettings.ScreenSize.X, (int)GameSettings.ScreenSize.Y);
            spriteBatch.Draw(Texture, dr, Color.White); 
        }
    }
}
