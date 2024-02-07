using Isometric_Thingy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class Enemy : GameObject
    {
        private Vector2 SpawnPosition { get { return GameSettings.Grid.GetRandomBorderPos();} }
        private Vector2 CurrentPosition { get; set; }
        private Vector2 NextPosition { get; set; }
        private Vector2 TargetLocation { get; set; } = new Vector2(9, 7);

        private bool InSideHouse = false;
        
       
        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();

        private float _delay = 0.07f; // seconds
        private float _remainingDelay;

        public Enemy(List<SpriteSheet> animations)
        {
            Animations = animations;
            Visualisation = Animations[0];
            CurrentPosition = SpawnPosition;
            TopLeftPosition = GameSettings.Grid.GetGridPositionNoHeight(CurrentPosition);
            _remainingDelay = _delay;
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
            for (int i = 0;i < Game1._obstacles.Count;i++)
            {
                if (Game1._obstacles[i].indexPosition == pos)
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
            
            _remainingDelay -= timer;
            
            if (_remainingDelay <= 0)
            {
                if (_delay == 2f && InSideHouse)
                    InSideHouse = false;
                if (NextPosition == TargetLocation - new Vector2(0.3f, 0))
                {
                    InSideHouse = true;
                    CurrentPosition = SpawnPosition;
                    NextPosition = CurrentPosition;
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
                    if (CurrentPosition == TargetLocation)
                    {
                        NextPosition = TargetLocation - new Vector2(0.3f, 0);
                        TopLeftPosition = GameSettings.Grid.GetGridPosition(CurrentPosition);
                        Visualisation = Animations[7];
                    } else
                    {
                        NextPosition = getNextPosition();
                    }
                    
                }
               
                
                
                if (InSideHouse)
                {
                    
                    _delay = 2f;
                } else
                {
                    TopLeftPosition = GameSettings.Grid.GetGridPosition(CurrentPosition) + new Vector2(0, 6 * GameSettings.Grid.ScaleFactor);
                    _delay = 0.07f;
                }
                _remainingDelay = _delay;
                
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!InSideHouse)
            {
                base.Draw(spriteBatch);
            }
            
        }
        public static Enemy Spawn()
        {
            return new Enemy(new List<SpriteSheet>()
            {
                new SpriteSheet(GameSettings.Squirrel_Up,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Up_Right,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Right,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Down_Right,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Down,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Down_Left,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Left,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
                new SpriteSheet(GameSettings.Squirrel_Up_Left,Vector2.Zero,new Vector2(21,17) * GameSettings.Grid.ScaleFactor,0,1,2,0,true),
            });
        }

    }
}
