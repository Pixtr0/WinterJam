using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam.Players
{
    internal class Player : GameObject
    {
        /*
         * Required:
         * Inventory
         * Movement
         * Interactions:
         */
        public UserInput UserInput { get; set; }
        public List<Item> Inventory {get; set;} = new List<Item>();
        public Vector2 CurrentPosition { get; set; }

        public Player(Vector2 currentPosition, SpriteSheet visualisation)
        {
            CurrentPosition = currentPosition;
        }

        public override void Update(GameTime gameTime)
        {
            UserInput.Update();
            UpdatePlayerPosition();
            base.Update(gameTime);
        }

        private void UpdatePlayerPosition()
        {
            if (UserInput.IsKeyPressed)
            {
                if (UserInput.PressedKey == GameSettings.ControlKeys.left)
                    CurrentPosition += new Vector2(-1, 0);
                if (UserInput.PressedKey == GameSettings.ControlKeys.right)
                    CurrentPosition += new Vector2(1, 0);
                if (UserInput.PressedKey == GameSettings.ControlKeys.up)
                    CurrentPosition += new Vector2(0, -1);
                if (UserInput.PressedKey == GameSettings.ControlKeys.down)
                    CurrentPosition += new Vector2(0, 1);


                if (UserInput.PressedKey == GameSettings.ControlKeys.left
                    && UserInput.PressedKey == GameSettings.ControlKeys.up)
                    CurrentPosition += new Vector2(-1, -1);
                if (UserInput.PressedKey == GameSettings.ControlKeys.left
                    && UserInput.PressedKey == GameSettings.ControlKeys.down)
                    CurrentPosition += new Vector2(-1, 1);
                if (UserInput.PressedKey == GameSettings.ControlKeys.right
                    && UserInput.PressedKey == GameSettings.ControlKeys.up)
                    CurrentPosition += new Vector2(1, -1);
                if (UserInput.PressedKey == GameSettings.ControlKeys.right
                    && UserInput.PressedKey == GameSettings.ControlKeys.down)
                    CurrentPosition += new Vector2(1, 1);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
