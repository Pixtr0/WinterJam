using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    public class GameObject
    {
        public Vector2 CurrentPosition { get; set; }
        public SpriteSheet Visualisation { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Vector2 anchorPoint { get
            {
                return TopLeftPosition + new Vector2(Size.X / 2, Size.Y);            
            }
        }
        public Vector2 TopLeftPosition
        {
            get
            {
                return Visualisation.TopLeftPosition;
            }
            set
            {
                Visualisation.TopLeftPosition = value;
            }
        }
        public Vector2 Size
        {
            get
            { return Visualisation.Size; }
            set { Visualisation.Size = value; }
        }
        public virtual void Update(GameTime gameTime)
        {
            Visualisation.Update();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                Visualisation.Draw(spriteBatch);

               // spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)anchorPoint.X - 3, (int)anchorPoint.Y - 1, 9, 9), Color.Red);
            }
        }
    }
}
