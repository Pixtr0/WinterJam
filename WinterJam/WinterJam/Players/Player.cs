using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D11;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam.Players
{
    internal class Player : GameObject
    {
        public UserInput UserInput { get; set; }
        public List<Item> Inventory {get; set;} = new List<Item>();
        public Vector2 CurrentPosition { get; set; }
        public Vector2 NextPosition { get; set; } = Vector2.Zero;
        public float Speed { get; set; } = 4f;
        public Vector2 CurrentTopLeftPosition { get; set; }
        public Vector2 NextTopLeftPosition { get; set; } 
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
            UpdateInventory();
            UpdatePlayerPosition();
            base.Update(gameTime);
        }

        private void UpdateInventory()
        {

        }

        private void UpdatePlayerPosition()
        {
            //GridMovement(); // lags
            Vector2 movement = Vector2.Zero;

            if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left))
                movement += new Vector2(-1, 0);
            if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                movement += new Vector2(1, 0);
            if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                movement += new Vector2(0, -1);
            if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                movement += new Vector2(0, 1);
            
            if (movement != Vector2.Zero)
                movement.Normalize();

            TopLeftPosition += movement * Speed;
            CurrentPosition = GameSettings.Grid.GetGridPosition(TopLeftPosition);
        }

        private void GridMovement()
        {
            if (TopLeftPosition != NextTopLeftPosition && NextTopLeftPosition != Vector2.Zero)
            {
                Vector2 target = NextTopLeftPosition - TopLeftPosition;
                float length = target.Length();
                Vector2 movement = target;
                if (length > 0)
                    movement.Normalize();

                movement *= MathF.Min(Speed, length);

                TopLeftPosition += movement;
            }
            else
            {
                if (UserInput.IsKeyDown)
                {
                    if (NextPosition != Vector2.Zero)
                        CurrentPosition = NextPosition;

                    //cardinal directions
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left))
                    {
                        NextPosition = CurrentPosition + new Vector2(-1, 1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                    {
                        NextPosition = CurrentPosition + new Vector2(1, -1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                    {
                        NextPosition = CurrentPosition + new Vector2(-1, -1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                    {
                        NextPosition = CurrentPosition + new Vector2(1, 1);
                    }

                    //Diagonal Directions
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right) &&
                        UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                    {
                        NextPosition = CurrentPosition + new Vector2(0, -1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left) &&
                        UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                    {
                        NextPosition = CurrentPosition + new Vector2(0, 1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down)
                        && UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                    {
                        NextPosition = CurrentPosition + new Vector2(1, 0);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up)
                        && UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left))
                    {
                        NextPosition = CurrentPosition + new Vector2(-1, 0);
                    }

                    float clampedX = MathHelper.Clamp(NextPosition.X, 0, GameSettings.Grid.Size.X - 1);
                    float clampedY = MathHelper.Clamp(NextPosition.Y, 0, GameSettings.Grid.Size.Y - 1);

                    NextPosition = new Vector2(clampedX, clampedY);

                    NextTopLeftPosition = GameSettings.Grid.GetPlayerPosition(NextPosition);
                }

            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Visualisation.Texture,Visualisation.DestinationRectangle,Color.White);
            //base.Draw(spriteBatch);
        }
    }
}
