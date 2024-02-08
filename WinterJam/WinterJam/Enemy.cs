using Isometric_Thingy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterJam.Screens;

namespace WinterJam
{
    public class Enemy : GameObject
    {
        private Vector2 SpawnPosition { get { return GameSettings.Grid.GetRandomBorderPos(1);} }
        private Vector2 NextPosition { get; set; }
        private Vector2 TargetLocation { get; set; } = new Vector2(9, 7);
        public Item helditem { get; set; }

        public static List<Texture2D> Textures { get; set; }

        public bool IsHoldingItem { get; set; } = false;        
        public bool IsSmacked { get; set; } = false;        

        private bool InSideHouse = false;

        public List<SpriteSheet> Animations { get; set; }

        private float _delay = 0.07f; // seconds
        private float _remainingDelay;

        public Enemy()
        {
            
            Animations = new List<SpriteSheet>()
            {
                new SpriteSheet(Textures[0], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[1], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[2], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[3], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[4], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[5], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[6], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(Textures[7], Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
            };
            Visualisation = Animations[0];
            CurrentPosition = GameSettings.Grid.GetRandomBorderPos(1);
            NextPosition = CurrentPosition;
            TopLeftPosition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition);
            _remainingDelay = _delay;
            Createitem(0);
            IsHoldingItem = false;  

        }

        private void Createitem(int cost)
        {
            House.currentHp -= cost;
            helditem = new Item(this);
            IsHoldingItem = true;

        }
        private Vector2 getNextPosition()
        {
            Vector2 newNextPos = CurrentPosition;
            List<Vector2> possibilities = new List<Vector2>();
            possibilities.Add(CurrentPosition + new Vector2(-1, -1));
            possibilities.Add(CurrentPosition + new Vector2( 0, -1));
            possibilities.Add(CurrentPosition + new Vector2( 1, -1));
            possibilities.Add(CurrentPosition + new Vector2( 1,  0));
            possibilities.Add(CurrentPosition + new Vector2( 1,  1));
            possibilities.Add(CurrentPosition + new Vector2( 0,  1));
            possibilities.Add(CurrentPosition + new Vector2(-1,  1));
            possibilities.Add(CurrentPosition + new Vector2(-1,  0));

            float shortestDistance = 1000;
            int shortestIndex = 0;
            for (int i = 0; i < possibilities.Count; i++)
            {
                if (!House.HouseTiles.Contains(possibilities[i]) && House.SurroundingTiles[0] != possibilities[i] && !IncludesObstacles(possibilities[i]) && !IsBlockOffMap(possibilities[i]))
                {
                    float distance = Vector2.Distance(TargetLocation, possibilities[i]);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        shortestIndex = i;
                    }
                } 
            }
            newNextPos = possibilities[shortestIndex];
            Visualisation = Animations[shortestIndex];
            return newNextPos;
        }
        private bool IncludesObstacles(Vector2 pos)
        {
            for (int i = 0;i < PlayScreen.Obstacles.Count;i++)
            {
                if (PlayScreen.Obstacles[i].indexPosition == pos)
                {
                    return true;
                }
            }
            for (int i = 0; i < House.SurroundingTiles.Length - 10; i++)
            {
                if (House.SurroundingTiles[i] == pos)
                {
                    return true;
                }
            }
            return false;
        }

    private bool IsBlockOffMap(Vector2 pos)
        {
            if (pos.X >= 18 || pos.Y >= 18 || pos.X < 0 || pos.Y < 0)
            {
                return true;
            }
            return false;
        }

       
        public override void Update(GameTime gt)
        {
            var timer = (float)gt.ElapsedGameTime.TotalSeconds;

            if ()
            {
                
            }
            _remainingDelay -= timer;
            if(IsHoldingItem && !IsSmacked)
                helditem.Update(gt,this);
            if (_remainingDelay <= 0)
            {
                //reset insidehouse state
                if (_delay == 2f && InSideHouse)
                {
                    InSideHouse = false;
                    Createitem(1);
                }

                if (NextPosition == TargetLocation - new Vector2(0.3f, 0))
                {
                    InSideHouse = true;
                    TargetLocation = GameSettings.Grid.GetRandomBorderPos(0);
                    NextPosition = CurrentPosition;
                }
                if(IsHoldingItem || IsSmacked)
                {
                    if ( CurrentPosition == TargetLocation)
                    {
                        CurrentPosition = SpawnPosition;
                        NextPosition = CurrentPosition;
                        TargetLocation = new Vector2(9, 7);
                        IsHoldingItem = false;
                    }
                }
                

                int index = Visualisation.CurrentSpriteIndex;
                
                if (CurrentPosition != NextPosition)
                {
                    //+= (NextTopLeftPosition - CurrentTopLeftPosition) / (Visualisation.Cols - index);
                    CurrentPosition += (NextPosition - CurrentPosition) / (Visualisation.Cols - index);
                    base.Update(gt);
                    
                }
                else
                {
                    if (!IsHoldingItem && CurrentPosition == TargetLocation)
                    {
                        if (!InSideHouse)
                        {
                            NextPosition = TargetLocation - new Vector2(0.3f, 0);
                            Visualisation = Animations[7];
                        }
                    }
                    else
                    {
                        NextPosition = getNextPosition();
                    }
                    
                    
                }
                if (InSideHouse)
                {
                    
                    _delay = 2f;
                } else
                {
                    
                    _delay = 0.07f;
                }
                _remainingDelay = _delay;
                TopLeftPosition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition) + new Vector2(0, 6 * GameSettings.Grid.ScaleFactor);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!InSideHouse)
            {
                base.Draw(spriteBatch);
                if(IsHoldingItem)
                    helditem.Draw(spriteBatch);
            }

        }
        public static Enemy Spawn()
        {
            return new Enemy();
        }

    }
}
