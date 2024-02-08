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
        private Vector2 EffectSize { get; set; } = new Vector2(48, 29) * GameSettings.Grid.ScaleFactor;
        private Vector2 EffectPossition{ get; set; }

        private bool IsFlipped { get; set; } = false;
        
        public bool IsSmacking { get; set; } = false;
        private bool ShowSwingEffect = false;
        public int LastAnimationIndex = -1;
        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();
        private const float _playerDelay = 0.04f; // seconds
        private const float _swingDelay = 0.10f; // seconds
        private float _remainingPlayerDelay = _playerDelay;
        private float _remainingSwingDelay = _swingDelay; // seconds
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
            PlaceAnItem();
            if (!IsSmacking && UserInput._currentKeyboardSate.IsKeyDown(Keys.Space) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Space))
            {
                IsSmacking = true;
            }
            //they're still alive PETA i swear they're just sleeping

            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _remainingPlayerDelay -= timer;
            _remainingSwingDelay -= timer;
            

            if (_remainingPlayerDelay <= 0)
            {
                if (IsSmacking)
                {
                    SmackASquirrel();
                    Visualisation.Update();
                    if(Visualisation.CurrentSpriteIndex == 3 || Visualisation.CurrentSpriteIndex == 2)
                    {
                        ShowSwingEffect = true;
                    } else
                    {
                        ShowSwingEffect = false;
                    }
                    if (Visualisation.IsFinished)
                    {
                        Visualisation.IsFlipped = false;
                        Visualisation = Animations[LastAnimationIndex];
                        IsSmacking = false;
                        LastAnimationIndex = -1;
                    }
                }              

                UpdatePlayerPosition();
                if (IsSmacking)
                {
                    Visualisation.TopLeftPosition -= new Vector2(4, 10) * GameSettings.Grid.ScaleFactor;
                    _remainingPlayerDelay = _swingDelay;
                } else
                {
                    _remainingPlayerDelay = _playerDelay;
                }
            }
        }

        private void SmackASquirrel()
        {
            if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Space) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Space))
            {
                List<Vector2> smackedPositions = new List<Vector2>();

            if (this.Visualisation == Animations[0]) // UP
            {
                smackedPositions.Add(CurrentPosition + new Vector2(0, -1));
                smackedPositions.Add(CurrentPosition + new Vector2(0, -2));
                smackedPositions.Add(CurrentPosition + new Vector2(1, -1));
                smackedPositions.Add(CurrentPosition + new Vector2(-1, -1));
                EffectPossition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition + new Vector2(0, -1)) + new Vector2(-2 * GameSettings.Grid.ScaleFactor, -2 * GameSettings.Grid.ScaleFactor + GameSettings.Grid.TileSize.Y - EffectSize.Y) ;
                IsFlipped = true;
                LastAnimationIndex = 0;
                this.Visualisation = Animations[5];
                Visualisation.IsFlipped = true;
                Visualisation.Play();

            }
            if (this.Visualisation == Animations[1]) // RGHT
            {
                smackedPositions.Add(CurrentPosition + new Vector2(1, 0));
                smackedPositions.Add(CurrentPosition + new Vector2(2, 0));
                smackedPositions.Add(CurrentPosition + new Vector2(1, -1));
                smackedPositions.Add(CurrentPosition + new Vector2(1, 1));
                EffectPossition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition + new Vector2(1, 0)) + new Vector2(-4 * GameSettings.Grid.ScaleFactor, 4 * GameSettings.Grid.ScaleFactor + GameSettings.Grid.TileSize.Y - EffectSize.Y);
                IsFlipped = false;
                LastAnimationIndex = 1;
                this.Visualisation = Animations[4];
                Visualisation.IsFlipped = true;
                Visualisation.Play();
            }
            if (this.Visualisation == Animations[2]) // DOWN
            {
                smackedPositions.Add(CurrentPosition + new Vector2(-1, 1));
                smackedPositions.Add(CurrentPosition + new Vector2(0, 1));
                smackedPositions.Add(CurrentPosition + new Vector2(0, 2));
                smackedPositions.Add(CurrentPosition + new Vector2(1, 1));
                EffectPossition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition + new Vector2(0, 2)) + new Vector2(-2 * GameSettings.Grid.ScaleFactor, -2 * GameSettings.Grid.ScaleFactor + GameSettings.Grid.TileSize.Y - EffectSize.Y);
                IsFlipped = true;
                LastAnimationIndex = 2;
                this.Visualisation = Animations[4];
                Visualisation.Play();
            }
            if (this.Visualisation == Animations[3]) // LEFT
            {
                smackedPositions.Add(CurrentPosition + new Vector2(-1, 0));
                smackedPositions.Add(CurrentPosition + new Vector2(-1, -1));
                smackedPositions.Add(CurrentPosition + new Vector2(-2, 0));
                smackedPositions.Add(CurrentPosition + new Vector2(-1, 1));
                EffectPossition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition + new Vector2(-2, 0)) + new Vector2(-4 * GameSettings.Grid.ScaleFactor, 4 * GameSettings.Grid.ScaleFactor + GameSettings.Grid.TileSize.Y - EffectSize.Y);
                IsFlipped = false;
                LastAnimationIndex = 3;
                this.Visualisation = Animations[5];
                Visualisation.Play();
            }

            for (int i = 0; i < PlayScreen.Enemies.Count; i++)
            {
                for (int j = 0; j < smackedPositions.Count; j++)
                {
                    if (PlayScreen.Enemies[i].LastPosition == smackedPositions[j] || PlayScreen.Enemies[i].NextPosition == smackedPositions[j])
                        PlayScreen.Enemies[i].IsSmacked = true;
                }
            }
        }

        //deprecated
        private void PlaceAnItem()
        {
            if (UserInput._currentKeyboardSate.IsKeyDown(Keys.F) && UserInput._previousKeyboardSate.IsKeyUp(Keys.F) && HeldItem != null)
            {
                if (Inventory.Count > 0 && HeldItemIndex >= 0)
                {
                    Vector2 placedPosition = Vector2.Zero;

                    if (this.Visualisation == Animations[0])
                    {
                        placedPosition = CurrentPosition + new Vector2(0, -1);
                    }
                    if (this.Visualisation == Animations[1])
                    {
                        placedPosition = CurrentPosition + new Vector2(1, 0);
                    }
                    if (this.Visualisation == Animations[2])
                    {
                        placedPosition = CurrentPosition + new Vector2(0, 1);
                    }
                    if (this.Visualisation == Animations[3])
                    {
                        placedPosition = CurrentPosition + new Vector2(-1, 0);
                    }

                    if (House.HouseTiles.Contains(placedPosition))
                    {
                        House.currentHp++;

                        if (HeldItemIndex > 0)
                        {
                            Inventory.RemoveAt(HeldItemIndex);
                            HeldItemIndex--;
                            HeldItem = Inventory[HeldItemIndex];
                        }
                        if (HeldItemIndex == 0)
                        {
                            HeldItem.IsActive = false;
                            Inventory.RemoveAt(HeldItemIndex);
                        }
                    }
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
                HeldItem = Inventory[HeldItemIndex];

            if (HeldItem != null)
                HeldItem.Update(gameTime, this);
        }
        private void UpdatePlayerPosition()
        {
            if (!IsSmacking)
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
                    for (int i = 0; i < PlayScreen.Obstacles.Count; i++)
                    {
                        if (PlayScreen.Obstacles[i].indexPosition == NextPosition)
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
            }
            TopLeftPosition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition) + new Vector2(-5, -12f) * GameSettings.Grid.ScaleFactor;

        }
        public void DrawEffect(SpriteBatch spriteBatch)
        {
            if (ShowSwingEffect)
            {
                spriteBatch.Draw(GameSettings.SwingEffect, new Rectangle(EffectPossition.ToPoint(), EffectSize.ToPoint()), null, Color.White, 0, Vector2.Zero, IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            base.Draw(spriteBatch);

            if (HeldItem != null)
            {
                HeldItem.Draw(spriteBatch);
                
            }
            
            if (PlacedItems.Count > 0)
                foreach(Item item in PlacedItems)
                    item.Draw(spriteBatch); 
            
        }
    }
}
