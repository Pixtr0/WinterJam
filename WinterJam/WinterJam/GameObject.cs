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
            }
        }
        public Vector2 CenterGridPosition
        {
            get
            {
               return GameSettings.grid.GetGridPosition(TopLeftPosition);
            }
            set
            {
                TopLeftPosition = new Vector2(value.X * GameSettings.Cellsize, value.Y * GameSettings.Cellsize);
            }
        }
        public bool IsAtGridPosition(Vector2 position)
        {
            return CenterGridPosition.Equals(position);
        }
        public override string ToString()
        {
            return $"{CenterGridPosition}";
        }
    }
}
