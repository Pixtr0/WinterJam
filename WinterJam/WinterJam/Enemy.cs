﻿using Isometric_Thingy;
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
    internal class Enemy
    {
        private Vector2 SpawnPosition { get; set; }
        private Vector2 CurrentPosition { get; set; }
        private Vector2 TargetLocation { get; set; } = new Vector2(9, 9);

        private SpriteSheet CurrentSpriteSheet { get; set; }
        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();

        private const float _delay = 0.2f; // seconds
        private float _remainingDelay = _delay;

        public Enemy(List<SpriteSheet> animations)
        {
            Animations = animations;
            CurrentSpriteSheet = Animations[0];
            SpawnPosition = GameSettings.Grid.GetRandomBorderPos();
            CurrentPosition = SpawnPosition;
            CurrentSpriteSheet.CenterPosition = GameSettings.Grid.GetPlayerPosition(CurrentPosition);
        }

        private Vector2 getNextPosition()
        {
            Vector2 newNextPos = CurrentPosition;
            List<Vector2> possibilities = new List<Vector2>();
            possibilities.Add(CurrentPosition + new Vector2(-1, -1));
            possibilities.Add(CurrentPosition + new Vector2(0, -1));
            possibilities.Add(CurrentPosition + new Vector2(1, -1));
            possibilities.Add(CurrentPosition + new Vector2(1, 0));
            possibilities.Add(CurrentPosition + new Vector2(1, 1));
            possibilities.Add(CurrentPosition + new Vector2(0, 1));
            possibilities.Add(CurrentPosition + new Vector2(-1, 1));
            possibilities.Add(CurrentPosition + new Vector2(-1, 0));


            float shortestDistance = 1000;
            int shortestIndex = 0;
            for (int i = 0; i < possibilities.Count; i++)
            {
                if (!GameSettings.Grid.BlockedTiles.Contains(possibilities[i]) && !IsBlockOffMap(possibilities[i]))
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
            CurrentSpriteSheet = Animations[shortestIndex];
            //if (includesBlockedTiles(possibilities))
            //{
            //    float shortestDistance = 1000;
            //    int shortestIndex = 0;
            //    for (int i = 0; i < possibilities.Count; i++)
            //    {
            //        if (!GameSettings.Grid.BlockedTiles.Contains(possibilities[i]) && !IsBlockOffMap(possibilities[i]))
            //        {
            //            float distance = Vector2.Distance(TargetLocation, possibilities[i]);
            //            if (distance < shortestDistance)
            //            {
            //                shortestDistance = distance;
            //                shortestIndex = i;
            //            }
            //        }
            //    }

            //    newNextPos = possibilities[shortestIndex];
            //    CurrentSpriteSheet = Animations[shortestIndex];
            //}
            //else
            //{
            //    if (CurrentPosition.Y > TargetLocation.Y)
            //    {
            //        newNextPos = CurrentPosition - new Vector2(0, 1);
            //        CurrentSpriteSheet = Animations[1];
            //    }
            //    else if (CurrentPosition.Y < TargetLocation.Y)
            //    {
            //        newNextPos = CurrentPosition + new Vector2(0, 1);
            //        CurrentSpriteSheet = Animations[5];
            //    }
            //    else if (CurrentPosition.X > TargetLocation.X)
            //    {
            //        newNextPos = CurrentPosition - new Vector2(1, 0);
            //        CurrentSpriteSheet = Animations[7];
            //    }
            //    else if (CurrentPosition.X < TargetLocation.X)
            //    {
            //        newNextPos = CurrentPosition + new Vector2(1, 0);
            //        CurrentSpriteSheet = Animations[3];
            //    }
            //}

            return newNextPos;
        }

        private bool IsBlockOffMap(Vector2 pos)
        {
            if (pos.X >= 18 || pos.Y >= 18 || pos.X < 0 || pos.Y < 0)
            {
                return true;
            }
            return false;
        }

       
        public void Update(GameTime gt)
        {
            var timer = (float)gt.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                if(CurrentPosition != TargetLocation)
                {
                    CurrentPosition = getNextPosition();
                    CurrentSpriteSheet.CenterPosition = GameSettings.Grid.GetPlayerPosition(CurrentPosition);
                    CurrentSpriteSheet.Update();
                } else
                {
                    SpawnPosition = GameSettings.Grid.GetRandomBorderPos();
                    CurrentPosition = SpawnPosition;
                    CurrentSpriteSheet.CenterPosition = GameSettings.Grid.GetPlayerPosition(CurrentPosition);
                }
                _remainingDelay = _delay;
               
            }
            
            
        }
        public void Draw(SpriteBatch sb)
        {
            CurrentSpriteSheet.Draw(sb);
        }
        public static Enemy Spawn()
        {
            return new Enemy(new List<SpriteSheet>()
            {
                new SpriteSheet(GameSettings.Squirrel_Up,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Up_Right,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Right,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Down_Right,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Down,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Down_Left,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Left,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
                new SpriteSheet(GameSettings.Squirrel_Up_Left,Vector2.Zero,new Vector2(26,13) * GameSettings.Grid.ScaleFactor,0,1,1,0,true),
            });
        }

    }
}
