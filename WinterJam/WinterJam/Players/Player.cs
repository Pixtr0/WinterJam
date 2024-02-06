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
        public Vector2 NextPosition { get; set; }
        public float Speed { get; set; } = 4f;
        public Vector2 NextTopLeftPosition { get; set; }
        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();

        private const float _delay = 0.2f; // seconds
        private float _remainingDelay = _delay;

        private const float _playerDelay = 0.08f; // seconds
        private float _remainingPlayerDelay = _delay;

        public Player(Vector2 currentPosition, SpriteSheet visualisation)
        {
            UserInput = new UserInput();
            CurrentPosition = currentPosition;
            NextPosition = CurrentPosition;
            Visualisation = visualisation;

        }
        public override void Update(GameTime gameTime)
        {
            //Receives the next grid position based on user input

            UserInput.Update();
            UpdateItems(gameTime);
            AddRemoveItem();

            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _remainingPlayerDelay -= timer;

            if (_remainingPlayerDelay <= 0)
            {
                UpdatePlayerPosition();
                _remainingPlayerDelay = _playerDelay;
            }
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
            
            //Vector2 movement = Vector2.Zero;
            int index = Visualisation.CurrentSpriteIndex;
            if (CurrentPosition != NextPosition)
            {
                CurrentPosition += (NextPosition - CurrentPosition) / (Visualisation.Cols - index);
                Visualisation.Update();

            }
            else
            {
                if (UserInput.IsKeyDown)
                {
                    //cardinal directions
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.left))
                    {
                        NextPosition = CurrentPosition + new Vector2(-1, 0);
                        Visualisation = Animations[3];
                        Visualisation.Play();
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.right))
                    {
                        NextPosition = CurrentPosition + new Vector2(1, 0);
                        Visualisation = Animations[1];
                        Visualisation.Play();
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.up))
                    {
                        NextPosition = CurrentPosition + new Vector2(0, -1);
                        Visualisation = Animations[0];
                        Visualisation.Play();
                    }
                    if (UserInput._currentKeyboardSate.IsKeyDown(GameSettings.ControlKeys.down))
                    {
                        NextPosition = CurrentPosition + new Vector2(0, 1);
                        Visualisation = Animations[2];
                    }
                    for (int i = 0; i < Game1._obstacles.Count; i++)
                    {
                        if (Game1._obstacles[i].indexPosition == NextPosition)
                        {
                            NextPosition = CurrentPosition;
                        }
                    }
                    for (int i = 0; i < House.HouseTiles.Length; i++)
                    {
                        if (House.HouseTiles[i] == NextPosition)
                        {
                            NextPosition = CurrentPosition;
                        }
                    }

                    float clampedX = MathHelper.Clamp(NextPosition.X, 0, GameSettings.Grid.Size.X - 1);
                    float clampedY = MathHelper.Clamp(NextPosition.Y, 0, GameSettings.Grid.Size.Y - 1);
                    NextPosition = new Vector2(clampedX, clampedY);

                }
            }
            TopLeftPosition = GameSettings.Grid.GetPlayerPosition(CurrentPosition) + new Vector2(-6,-10.5f) * GameSettings.Grid.ScaleFactor;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            HeldItem.Draw(spriteBatch);
                
        }
    }
}
