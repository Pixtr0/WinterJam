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
        public float Speed { get; set; } = 4f;

        public Player(Vector2 currentPosition, SpriteSheet visualisation)
        {
            UserInput = new UserInput();
            CurrentPosition = currentPosition;
            Visualisation = visualisation;

            
        }

        public override void Update(GameTime gameTime)
        {
            //Receives the next grid position based on user input
            UserInput.Update();
            UpdatePlayerPosition();
            TopLeftPosition = GameSettings.Grid.GetPlayerPosition(CurrentPosition); //Sets TopLeftPosition to pixel coordinates of grid position
            base.Update(gameTime);
        }

        private void UpdatePlayerPosition()
        {
            Vector2 movement = Vector2.Zero;
            if (UserInput.IsKeyDown)
            {
                //cardinal directions
                if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left))
                    movement += new Vector2(-1, 0);
                if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                    movement += new Vector2(1, 0);
                if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                    movement += new Vector2(0, -1);
                if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                    movement += new Vector2(0, 1);

                ////diagonal direction
                //if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left)
                //    && UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                //    movement += new Vector2(-1, 1);

                //if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left)
                //    && UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                //    movement += new Vector2(1, -1);

                //if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right)
                //    && UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                //    movement += new Vector2(-1, -1);

                //if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right)
                //    && UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                //    movement += new Vector2(1, 1);

                //if (movement != Vector2.Zero)
                //    movement.Normalize();

                //movement *= Speed;
                CurrentPosition += movement;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Visualisation.Texture, Visualisation.CenterPosition,
                Visualisation.SourceRectangle, Color.White);
            //base.Draw(spriteBatch);
        }
    }
}
