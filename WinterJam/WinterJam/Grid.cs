using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Isometric_Thingy
{
    internal class Grid
    {
        private Vector2 Size { get; set; } = Vector2.One * 25;
        private int[,] HeightOffsets { get; set; } = new int[25, 25];
        private Vector2 Position { get; set; }
        private List<Texture2D> Tiles { get; set; }
        
        private int TileSize { get; set; } = 30;
        public Vector2 StartTilePos { get; set; }
        public Vector2 EndTilePos { get; set; }

        public Vector2[] BlockedTiles { get; set; } = new Vector2[4];
        public Grid(Vector2 position, List<Texture2D> tiles)
        {
            Position = position;
            Tiles = tiles;
            CreateHeightOffsetsWithNoise();
            SetNewPositions();
            
        }

        private void CreateHeightOffsetsWithNoise()
        {
            float scale = 0.1f;
            for (int i = 0; i < HeightOffsets.GetLength(0); i++)
            {
                for (int j = 0; j < HeightOffsets.GetLength(1); j++)
                {
                    HeightOffsets[i, j] = Random.Shared.Next(-2, 3); 
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Texture2D tileTexture = Tile;
                    //if (StartTilePos.X == x && StartTilePos.Y == y)
                    //    tileTexture = StartTile;
                    //if (EndTilePos.X == x && EndTilePos.Y == y)
                    //    tileTexture = EndTile;
                    //for (int i = 0; i < BlockedTiles.Length; i++)
                    //{
                    //    if (BlockedTiles[i].X == x && BlockedTiles[i].Y == y)
                    //    {
                    //        tileTexture = BlockedTile;
                    //    }
                    //}
                    sb.Draw(tileTexture, new Rectangle((int)(Position.X + x * TileSize / 2 - y * TileSize /2) , (int)(Position.Y + y * (TileSize / 3.8f) + x * (TileSize / 3.8f)) + HeightOffsets[x,y], TileSize, TileSize), Color.White);
                }
            }
        }
        public Vector2 GetPlayerPosition(Vector2 index)
        {
            float x = (int)Position.X + index.X * TileSize / 2 - index.Y * TileSize / 2;
            float y = (int)Position.Y + index.Y * TileSize / 3.8f + index.X * TileSize / 3.8f - TileSize / 2 + HeightOffsets[(int)index.X, (int)index.Y];
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
