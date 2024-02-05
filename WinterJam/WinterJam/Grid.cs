using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Isometric_Thingy
{
    public class Grid
    {

        private Vector2 Size { get; set; } = Vector2.One * 15;
        private int[,] HeightOffsets { get; set; } = new int[15, 15];
        private int[,] TileSelected { get; set; } = new int[15, 15];
        private Vector2 Position { get; set; }
        private List<Texture2D> Tiles { get; set; }
        
        private Vector2 TileSize { get; set; } = new Vector2(24, 36);
        public Vector2 StartTilePos { get; set; }
        public Vector2 EndTilePos { get; set; }

        public Vector2[] BlockedTiles { get; set; } = new Vector2[6];
        public int[] ObstaclesIndexes { get; set; } = new int[6];
        private List<Texture2D> Obstacles { get; set; }
        public Grid(Vector2 position, List<Texture2D> tiles, List<Texture2D> obstacles)
        {
            Tiles = tiles;
            Obstacles = obstacles;
            GenerateRandomTiles();
            CreateHeightOffsets();
            SetNewPositions();
            TileSize =  TileSize  * 5;
            Position = position - new Vector2(TileSize.X / 2f, TileSize.Y/6f) ;
        }

        private void GenerateRandomTiles()
        {
            for (int i = 0; i < TileSelected.GetLength(0); i++)
            {
                for (int j = 0; j < TileSelected.GetLength(1); j++)
                {
                    TileSelected[i, j] = Random.Shared.Next(0, Tiles.Count);
                }
            }

            int bush = 0;
            int rock = 2;
            for (int i = 0; i < ObstaclesIndexes.Length; i++)
            {
                int value = Random.Shared.Next(0,3);
                if(value == 0)
                {
                    ObstaclesIndexes[i] = 5;
                } else if(value == 1)
                {
                    ObstaclesIndexes[i] = rock;
                    rock++;
                    if (rock > 4)
                        rock = 2;
                }
                else
                {
                    ObstaclesIndexes[i] = bush;
                    bush++;
                    if (bush > 1)
                        bush = 0;
                }
                
            }
        }

        private void CreateHeightOffsets()
        {
            float scale = 0.1f;
            for (int i = 0; i < HeightOffsets.GetLength(0) -1; i++)
            {
                for (int j = 0; j < HeightOffsets.GetLength(1) -1; j++)
                {
                    
                        HeightOffsets[i, j] = Random.Shared.Next(-7, 8);
                    
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {

            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Texture2D tileTexture = Tiles[TileSelected[x, y]];

                    sb.Draw(tileTexture, new Rectangle((int)(Position.X + x * TileSize.X / 2 - y * TileSize.X / 2), (int)(Position.Y + y * (TileSize.Y / 5f) + x * (TileSize.Y / 5f)) + HeightOffsets[x, y], (int)TileSize.X, (int)TileSize.Y), Color.White);
                    
                }
            }
            
            for (int i = 0; i < BlockedTiles.Length; i++)
            {
                
                sb.Draw(Obstacles[ObstaclesIndexes[i]], new Rectangle((int)(Position.X + BlockedTiles[i].X * TileSize.X / 2 - BlockedTiles[i].Y * TileSize.X / 2), (int)(Position.Y + BlockedTiles[i].Y * (TileSize.Y / 5f) + BlockedTiles[i].X * (TileSize.Y / 5f)) - 4, (int)TileSize.X, (int)TileSize.Y), Color.White);
                
            }
        }
        public Vector2 GetPlayerPosition(Vector2 index)
        {
            float x = (int)Position.X + index.X * TileSize.X / 2 - index.Y * TileSize.X / 2;
            float y = (int)Position.Y + index.Y * TileSize.Y / 5f + index.X * TileSize.Y / 5f - TileSize.Y / 2 + HeightOffsets[(int)index.X, (int)index.Y];
            return new Vector2(x, y);
        }
        public void SetNewPositions()
        {
            StartTilePos = new Vector2(Random.Shared.Next(0, (int)Size.X), Random.Shared.Next(0, (int)Size.Y));
            do
            {
                EndTilePos = new Vector2(Random.Shared.Next(0, (int)Size.X), Random.Shared.Next(0, (int)Size.Y));
            } while (StartTilePos == EndTilePos);
            for (int i = 0; i < BlockedTiles.Length; i++)
            {
                Vector2 newPos;
                do
                {
                    newPos = new Vector2(Random.Shared.Next(0, (int)Size.X), Random.Shared.Next(0, (int)Size.Y));

                } while (IsIn3x3Range(newPos) || BlockedTiles.Contains(newPos) || newPos == EndTilePos || newPos == StartTilePos);
                BlockedTiles[i] = newPos;
            }
        }

        private bool IsIn3x3Range(Vector2 newPos)
        {
            List<Vector2> possibilities = new List<Vector2>();
            possibilities.Add(newPos - new Vector2(0, 1));
            possibilities.Add(newPos + new Vector2(0, 1));
            possibilities.Add(newPos - new Vector2(1, 0));
            possibilities.Add(newPos + new Vector2(1, 0));
            possibilities.Add(newPos + new Vector2(1, 1));
            possibilities.Add(newPos + new Vector2(1, -1));
            possibilities.Add(newPos + new Vector2(-1, 1));
            possibilities.Add(newPos + new Vector2(-1, -1));

            for (int i = 0; i < possibilities.Count; i++)
            {
                if (BlockedTiles.Contains(possibilities[i]))
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
