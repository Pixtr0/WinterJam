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
    abstract class GameObject
    {
        public SpriteSheet Visualisation { get; set; }
        public bool IsActive { get; set; } = true;
        public Vector2 TopLeftPosition
        {
            get
            {
                return Visualisation.CenterPosition;
            }
            set
            {
                Visualisation.CenterPosition = value;
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
            }
        }
    }
}
