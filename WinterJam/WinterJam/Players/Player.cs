using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D11;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Item HeldItem { get; set; }
        public Double ItemAngle { get; set; } = 0f;
        public float ItemOffset { get; set; } = 0f;
        public Vector2 CurrentPosition { get; set; }
        public Vector2 NextPosition { get; set; } = Vector2.Zero;
        public float Speed { get; set; } = 4f;
        public Vector2 NextTopLeftPosition { get; set; }
        public static List<SpriteSheet> Animations {  get; set; } = new List<SpriteSheet>();

        private const float _delay = 0.2f; // seconds
        private float _remainingDelay = _delay;

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
            UpdateItems(gameTime);
            AddRemoveItem();
            UpdatePlayerPosition();
            base.Update(gameTime);
        }
        private void UpdateItems(GameTime gameTime)
        {
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                ItemAngle++;

                if (ItemAngle >= 360f)
                    ItemAngle = 1f;

                _remainingDelay = _delay;
            }
             


            ItemOffset = (float) Math.Sin(ItemAngle);

            if (Inventory.Count > 0)
            {
                HeldItem = Inventory[Inventory.Count - 1];
                HeldItem.Visualisation.TopLeftPosition = anchorPoint + new Vector2(-HeldItem.Size.X/2, -Size.Y - 10 - HeldItem.Size.Y - ItemOffset * 4);
                HeldItem.Update();

                Debug.WriteLine(HeldItem.Visualisation.TopLeftPosition);
            }
        }
        private void AddRemoveItem()
        {
                if(UserInput._currentKeyboardSate.IsKeyDown(Keys.E) && UserInput._previousKeyboardSate.IsKeyUp(Keys.E))
                {
                    Item shovel = new Item(20, GameSettings.ScreenTexture, this);
                    Inventory.Add(shovel);
                }

                if (UserInput._currentKeyboardSate.IsKeyDown(Keys.F) && UserInput._previousKeyboardSate.IsKeyUp(Keys.F))
                {
                    Inventory.RemoveAt(Inventory.Count);
                }
        }
        private void UpdatePlayerPosition()
        {
            GridMovement(); // lags, but we move, literally
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
                        NextPosition = CurrentPosition + new Vector2(-1, 0);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                    {
                        NextPosition = CurrentPosition + new Vector2(1, 0);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                    {
                        NextPosition = CurrentPosition + new Vector2(0, -1);
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                    {
                        NextPosition = CurrentPosition + new Vector2(0, 1);
                    }

                    
                    float clampedX = MathHelper.Clamp(NextPosition.X, 0, GameSettings.Grid.Size.X - 1);
                    float clampedY = MathHelper.Clamp(NextPosition.Y, 0, GameSettings.Grid.Size.Y - 1);

                    NextPosition = new Vector2(clampedX, clampedY);

                    //Check to see if the player will collide with an obstacle
                    for (int i = 0; i < Game1._obstacles.Count; i++)
                    {
                        if (Game1._obstacles[i].indexPosition ==  NextPosition)
                        {
                            NextPosition = CurrentPosition;
                            return;
                        }
                    }

                    NextTopLeftPosition = GameSettings.Grid.GetPlayerPosition(NextPosition);
                }

            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            HeldItem.Draw(spriteBatch);
                
        }
    }
}
