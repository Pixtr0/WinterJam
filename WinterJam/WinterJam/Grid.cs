using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using WinterJam;

namespace Isometric_Thingy
{
    public class Grid
    {
        private int size = 16;
        public float ScaleFactor {  get; set; }
        public Vector2 Size { get; set; }
        private int[,] HeightOffsets { get; set; }
        private int[,] TileSelected { get; set; }
        private Vector2 Position { get; set; }
        private List<Texture2D> Tiles { get; set; }
        
        private Vector2 TileSize { get; set; } = new Vector2(24, 36);
        public Vector2[] BlockedTiles { get; set; } = new Vector2[8];
        public int[] ObstaclesIndexes { get; set; } = new int[8];
        
        public Grid(Vector2 position, List<Texture2D> tiles)
        {
            Tiles = tiles;
            TileSize.Normalize();
            Size = Vector2.One * size;
            HeightOffsets = new int[size, size];
            TileSelected = new int[size, size];
            ScaleFactor = GameSettings.ScreenSize.Y / (10 * Size.Length());
            //ScaleFactor = 8;
            TileSize *= ScaleFactor;
            Position = position - new Vector2(TileSize.X / 2f, TileSize.Y/6f - 40);

            GenerateRandomTiles();
            CreateHeightOffsets();
        }

        public void GenerateRandomTiles()
        {
            for (int i = 0; i < TileSelected.GetLength(0); i++)
            {
                for (int j = 0; j < TileSelected.GetLength(1); j++)
                {
                    TileSelected[i, j] = Random.Shared.Next(0, Tiles.Count);
                }
            }
        }

        private void CreateHeightOffsets()
        {
            for (int i = 0; i < HeightOffsets.GetLength(0) -1; i++)
            {
                for (int j = 0; j < HeightOffsets.GetLength(1) -1; j++)
                {
                     HeightOffsets[i, j] = Random.Shared.Next(-7, 8);
                }
            }
        }
        public void Update()
        {
            if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Space) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Space))
            {
                GenerateRandomTiles();
                CreateHeightOffsets();
            }
        }
        public void DrawGrass(SpriteBatch sb)
        {

            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Texture2D tileTexture = Tiles[TileSelected[x, y]];

                    sb.Draw(tileTexture, new Rectangle((int)(Position.X + x * TileSize.X / 2 - y * TileSize.X / 2), (int)(Position.Y + y * (TileSize.Y / 6f) + x * (TileSize.Y / 6f)) + HeightOffsets[x, y], (int)TileSize.X, (int)TileSize.Y), Color.White);
                    
                }
            }
        }
        public Vector2 GetPlayerPosition(Vector2 index)
        {
            float x = (int)Position.X + index.X * TileSize.X / 2 - index.Y * TileSize.X / 2;
            float y = (int)Position.Y + index.Y * TileSize.Y / 6f + index.X * TileSize.Y / 6f + HeightOffsets[(int)index.X, (int)index.Y];
            return new Vector2(x, y);
        }
        public Vector2 GetRandomBorderPos()
        {
            Vector2 newPos;
            do
            {
                int x = Random.Shared.Next(0, 2) == 0 ? 0 : size - 1;
                int y = Random.Shared.Next(0, size);
                if (Random.Shared.Next(0, 2) == 0)
                {
                    
                    newPos = new Vector2(x, y);
                } else
                {
                    newPos = new Vector2(y, x);
                }
                
                
            } while (IsIn3x3Range(newPos) || BlockedTiles.Contains(newPos));

            return newPos;
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
