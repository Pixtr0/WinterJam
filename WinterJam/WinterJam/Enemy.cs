//using Isometric_Thingy;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using SpriteSheetClass;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinterJam
//{
//    internal class Enemy
//    {
//        private Vector2 SpawnPosition {  get; set; }
//        private Vector2 CurrentPosition { get; set; }
//        private Vector2 TargetLocation { get; set; }

//        private SpriteSheet  CurrentSpriteSheet { get; set; }
//        public static List<SpriteSheet> Animations { get; set; } = new List<SpriteSheet>();


//        public Enemy(Vector2 Spawn)
//        {

//        }

//        private Vector2 getNextPosition()
//        {
//            Vector2 newNextPos = CurrentPosition;
//            List<Vector2> possibilities = new List<Vector2>();
//            possibilities.Add(CurrentPosition - new Vector2(0, 1));
//            possibilities.Add(CurrentPosition + new Vector2(0, 1));
//            possibilities.Add(CurrentPosition - new Vector2(1, 0));
//            possibilities.Add(CurrentPosition + new Vector2(1, 0));
//            possibilities.Add(CurrentPosition + new Vector2(1, 1));
//            possibilities.Add(CurrentPosition + new Vector2(1, -1));
//            possibilities.Add(CurrentPosition + new Vector2(-1, 1));
//            possibilities.Add(CurrentPosition + new Vector2(-1, -1));

//            if (includesBlockedTiles(possibilities))
//            {
//                float shortestDistance = 1000;
//                int shortestIndex = 0;
//                for (int i = 0; i < possibilities.Count; i++)
//                {
//                    if (!_grid.BlockedTiles.Contains(possibilities[i]) && !IsBlockOffMap(possibilities[i]))
//                    {
//                        float distance = Vector2.Distance(_grid.EndTilePos, possibilities[i]);
//                        if (distance < shortestDistance)
//                        {
//                            shortestDistance = distance;
//                            shortestIndex = i;
//                        }
//                    }
//                }
//                newNextPos = possibilities[shortestIndex];
//            }
//            else
//            {
//                if (CurrentPosition.Y > _grid.EndTilePos.Y)
//                {
//                    newNextPos = CurrentPosition - new Vector2(0, 1);
//                }
//                else if (CurrentPosition.Y < _grid.EndTilePos.Y)
//                {
//                    newNextPos = CurrentPosition + new Vector2(0, 1);
//                }
//                else if (CurrentPosition.X > _grid.EndTilePos.X)
//                {
//                    newNextPos = CurrentPosition - new Vector2(1, 0);
//                }
//                else if (CurrentPosition.X < _grid.EndTilePos.X)
//                {
//                    newNextPos = CurrentPosition + new Vector2(1, 0);
//                }
//            }
//            return newNextPos;
//        }

//        private bool IsBlockOffMap(Vector2 pos)
//        {
//            if (pos.X >= 12 || pos.Y >= 12 || pos.X < 0 || pos.Y < 0)
//            {
//                return true;
//            }
//            return false;
//        }

//        private bool includesBlockedTiles(List<Vector2> list)
//        {
//            for (int i = 0; i < list.Count; i++)
//            {
//                if (_grid.BlockedTiles.Contains(list[i]))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//    }
//}
