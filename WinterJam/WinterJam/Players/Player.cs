using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinterJam.Screens;

namespace WinterJam.Players
{
    public class Player : GameObject
    {
        //Prioritise migration to PlayScreen when live
        public List<Item> PlacedItems { get; set; } = new List<Item>();
        public List<Obstacle> PlacedObstacles { get; set; }
        public List<Item> Inventory {get; set;} = new List<Item>();
        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0, 2 * GameSettings.Grid.ScaleFactor); } }
        public Item HeldItem { get; set; }
        public int HeldItemIndex { get; set; } = 0;
        public Vector2 NextPosition { get; set; }
        public float Speed { get; set; } = 4f;
        public Vector2 NextTopLeftPosition { get; set; }
        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();
        private const float _delay = 0.2f; // seconds
        private const float _playerDelay = 0.08f; // seconds
        private float _remainingPlayerDelay = _delay;
        public Player(Vector2 currentPosition, SpriteSheet visualisation)
        {
            CurrentPosition = currentPosition;
            NextPosition = CurrentPosition;
            Visualisation = visualisation;
            TopLeftPosition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition) + new Vector2(-7, -8.5f) * GameSettings.Grid.ScaleFactor;
        }
        public override void Update(GameTime gameTime)
        {
            //Receives the next grid position based on user input

            UpdateItems(gameTime);
            AddRemoveItem();
            PlaceAnItem();

            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _remainingPlayerDelay -= timer;

            if (_remainingPlayerDelay <= 0)
            {
                UpdatePlayerPosition();
                _remainingPlayerDelay = _playerDelay;
            }
        }

        private void PlaceAnItem()
        {
            if (UserInput._currentKeyboardSate.IsKeyDown(Keys.F) && UserInput._previousKeyboardSate.IsKeyUp(Keys.F))
            {
                if (Inventory.Count > 0)
                {
                    Item newPlacedItem = HeldItem;
                    newPlacedItem.Size *= 0.5f;
                    //newPlacedItem.CurrentPosition = CurrentPosition;
                    //newPlacedItem.TopLeftPosition = anchorPoint - newPlacedItem.Size / 2;

                    if (Visualisation == Animations[0])
                    {
                        newPlacedItem.CurrentPosition = CurrentPosition + new Vector2(0, -1);
                    }
                    if (Visualisation == Animations[1])
                    {
                        newPlacedItem.CurrentPosition = CurrentPosition + new Vector2(1, 0);
                    }
                    if (Visualisation == Animations[2])
                    {
                        newPlacedItem.CurrentPosition = CurrentPosition + new Vector2(0, 1);
                    }
                    if (Visualisation == Animations[3])
                    {
                        newPlacedItem.CurrentPosition = CurrentPosition + new Vector2(-1, 0);
                    }

                    newPlacedItem.TopLeftPosition = GameSettings.Grid.GetGridPosition(newPlacedItem.CurrentPosition) 
                        + new Vector2(6, 8)*GameSettings.Grid.ScaleFactor;

                    if (HeldItemIndex > 0)
                        {
                            Inventory.RemoveAt(HeldItemIndex);
                            HeldItemIndex--;
                            HeldItem = Inventory[HeldItemIndex];
                        }
                        else
                        {
                            HeldItem.IsActive = false;
                            Inventory.RemoveAt(0);
                        }
                        
                        PlacedItems.Add(newPlacedItem);

                }
                else
                {
                    Debug.WriteLine("Inventory empty!");
                }
            }
        }

        private void UpdateItems(GameTime gameTime)
        {
            if (Inventory.Count > 0)
            {
                if (UserInput._currentMouseState.ScrollWheelValue > UserInput._previousMouseState.ScrollWheelValue)
                {
                    if (Inventory.Count - 1 > HeldItemIndex)
                    {
                        HeldItem = Inventory[HeldItemIndex + 1];
                        HeldItemIndex++;
                    }
                        
                }
                if (UserInput._currentMouseState.ScrollWheelValue < UserInput._previousMouseState.ScrollWheelValue)
                {
                    if (0 < HeldItemIndex)
                    {
                        HeldItem = Inventory[HeldItemIndex - 1];
                        HeldItemIndex--;
                    }
                }

                HeldItem.Update(gameTime, anchorPoint);
            }

            if (PlacedItems.Count > 0)
            {
                foreach (Item item in PlacedItems)
                    item.Update(gameTime);
            }
        }
        private void AddRemoveItem()
        {
            if (UserInput._currentKeyboardSate.IsKeyDown(Keys.E) && UserInput._previousKeyboardSate.IsKeyUp(Keys.E)) //never true??
            {
                Item item = new Item(20, GameSettings.ScreenTexture, new Vector2(18,19)*GameSettings.Grid.ScaleFactor ,this);
                Inventory.Add(item);

                if (Inventory.Count == 1)
                    HeldItem = Inventory[0];
            }

            //if (UserInput._currentKeyboardSate.IsKeyDown(Keys.F) && UserInput._previousKeyboardSate.IsKeyUp(Keys.F))
            //{
            //    if (Inventory.Count > 0)
            //    {
            //        Inventory.RemoveAt(Inventory.Count - 1);
            //    }
            //    else
            //    {
            //        Debug.WriteLine("Inventory empty!");
            //    }
            //}
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
                    for (int i = 0; i < PlayScreen._obstacles.Count; i++)
                    {
                        if (PlayScreen._obstacles[i].indexPosition == NextPosition)
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

                    float clampedX = MathHelper.Clamp(NextPosition.X, 1, GameSettings.Grid.playsize - 1);
                    float clampedY = MathHelper.Clamp(NextPosition.Y, 1, GameSettings.Grid.playsize - 1);
                    NextPosition = new Vector2(clampedX, clampedY);

            }
            TopLeftPosition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition) + new Vector2(-5, -12f) * GameSettings.Grid.ScaleFactor;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (HeldItem != null)
                HeldItem.Draw(spriteBatch);

            if (PlacedItems.Count > 0)
                foreach(Item item in PlacedItems)
                    item.Draw(spriteBatch); 
            
        }
    }
}
