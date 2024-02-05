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
        public float Speed { get; set; } = 4f;
        public bool IsMoving { get; set; } = false;
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
            UpdatePlayerPosition();
            base.Update(gameTime);
        }

        private void UpdatePlayerPosition()
        {
            Vector2 nextPosition = CurrentPosition;

            MovePlayer();
            if (!IsMoving) 
            {
                if (UserInput.IsKeyDown)
                {
                    //cardinal directions
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left))
                    {
                        nextPosition = CurrentPosition + new Vector2(-1, 1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                    {
                        nextPosition = CurrentPosition + new Vector2(1, -1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                    {
                        nextPosition = CurrentPosition + new Vector2(-1, -1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                    {
                        nextPosition = CurrentPosition + new Vector2(1, 1);
                    }

                    NextTopLeftPosition = GameSettings.Grid.GetPlayerPosition(nextPosition);
                    
                }
            }
            
        }

        private void MovePlayer()
        {
            if (CurrentTopLeftPosition != NextTopLeftPosition)
            {
                CurrentTopLeftPosition += (NextTopLeftPosition - CurrentTopLeftPosition) 
                    / ((Visualisation.Rows * Visualisation.Cols) - Visualisation.CurrentSpriteIndex);
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Visualisation.Texture,Visualisation.DestinationRectangle,Color.White);
            //base.Draw(spriteBatch);
        }
    }
}
