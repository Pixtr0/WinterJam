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
        private int size = 45;
        private int playsize = 17;
        public int DownShift = 14;

        public float ScaleFactor { get; set; }
        public Vector2 Size { get; set; }

        bool _isRounded = false;

        public int[,] Mask { get; set; }
        
        private int[,] HeightOffsets { get; set; }
        private int[,] TileSelected { get; set; }
        private int[,] FlowerTiles { get; set; }
        private Vector2 Position { get; set; }
        private List<Texture2D> Tiles { get; set; }
        private List<Texture2D> FlowerTextures { get; set; }
        
        private Vector2 TileSize { get; set; } = new Vector2(24, 36);
        public Vector2[] BlockedTiles { get; set; } = new Vector2[8];
        public int[] ObstaclesIndexes { get; set; } = new int[8];
        
        public Grid(Vector2 position, List<Texture2D> tiles, List<Texture2D> flowers)
        {
            Tiles = tiles;
            FlowerTextures = flowers;
            TileSize.Normalize();
            Size = Vector2.One * size;
            Mask = new int[playsize, playsize];
            HeightOffsets = new int[size, size];
            TileSelected = new int[size, size];
            FlowerTiles = new int[size, size];
            
            GenerateMask();

            //ScaleFactor = GameSettings.ScreenSize.Y / (10 * Size.Length());
            ScaleFactor = 4;
            TileSize *= ScaleFactor;
            Position = position - new Vector2(TileSize.X / 2f, TileSize.Y/6f - 13 * 3 * ScaleFactor) ;

            GenerateRandomTiles();
            CreateHeightOffsets();
        }

        private void GenerateMask()
        {
            for (int x = 0; x < playsize; x++)
            {
                for (int y = 0; y < playsize; y++)
                {
                    if (_isRounded)
                    {
                        if (y == 0 || y == size - 1)
                        {
                            if (x >= 4 && x <= size - 5)
                                Mask[x, y] = 1;
                        }
                        if (y == 1 || y == size - 2)
                        {
                            if (x >= 2 && x <= size - 3)
                                Mask[x, y] = 1;
                        }
                        if (y == 2 || y == 3 || y == size - 3 || y == size - 4)
                        {
                            if (x >= 1 && x <= size - 2)
                                Mask[x, y] = 1;
                        }
                        if (y > 3 && y <= size - 5)
                        {
                            if (x >= 0 && x <= size - 1)
                                Mask[x, y] = 1;
                        }
                    }
                    else
                    {
                        Mask[x, y] = 1;
                    }
                }
            }
        }
        public void GenerateRandomTiles()
        {
            for (int i = 0; i < TileSelected.GetLength(0)- 1; i++)
            {
                for (int j = 0; j < TileSelected.GetLength(1) -1; j++)
                {
                    TileSelected[i, j] = Random.Shared.Next(0, Tiles.Count);
                }
            }
            for (int i = 0; i < FlowerTiles.GetLength(0); i++)
            {
                for (int j = 0; j < FlowerTiles.GetLength(1); j++)
                {
                    if (Random.Shared.Next(0, 9) == 0)
                    {
                        FlowerTiles[i, j] = Random.Shared.Next(0, FlowerTextures.Count);
                    } else { FlowerTiles[i, j] = -1; }
                }
            }
        }

        private void CreateHeightOffsets()
        {
            for (int i = 0; i < HeightOffsets.GetLength(0) -1; i++)
            {
                for (int j = 0; j < HeightOffsets.GetLength(1) -1; j++)
                {
                     HeightOffsets[i, j] = (int)(Random.Shared.Next(-2, 3) * ScaleFactor);
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
            
            for (int x = -DownShift; x < Size.X - DownShift; x++)
            {
                for (int y = -DownShift; y < Size.Y - DownShift; y++)
                {
                    if (x >0 && y > 0 && x < playsize && y < playsize && Mask[x,y] == 1)
                    {
                        Texture2D tileTexture = Tiles[TileSelected[x, y]];

                        sb.Draw(tileTexture, new Rectangle((int)(Position.X + x * TileSize.X / 2 - y * TileSize.X / 2), (int)(Position.Y + y * (TileSize.Y / 6f) + x * (TileSize.Y / 6f)) + HeightOffsets[x, y], (int)TileSize.X, (int)TileSize.Y), Color.White);
                        if (FlowerTiles[x + DownShift, y + DownShift] != -1)
                        {
                            Texture2D flowertexture = FlowerTextures[FlowerTiles[x + DownShift, y + DownShift]];
                            Vector2 flowerSize = new Vector2(11, 10) * ScaleFactor;
                            sb.Draw(flowertexture, new Rectangle((int)(Position.X + x * TileSize.X / 2 - y * TileSize.X / 2 + 6 * ScaleFactor), (int)(Position.Y + y * (TileSize.Y / 6f) + x * (TileSize.Y / 6f) + 7 * ScaleFactor) + HeightOffsets[x + DownShift, y + DownShift], (int)flowerSize.X, (int)flowerSize.Y), Color.White);
                        }
                    } else
                    {
                        Texture2D tileTexture = Tiles[TileSelected[x + DownShift, y + DownShift]];

                        sb.Draw(tileTexture, new Rectangle((int)(Position.X + x * TileSize.X / 2 - y * TileSize.X / 2), (int)(Position.Y + y * (TileSize.Y / 6f) + x * (TileSize.Y / 6f)) + HeightOffsets[x + DownShift, y + DownShift], (int)TileSize.X, (int)TileSize.Y), Color.Red);
                        if (FlowerTiles[x + DownShift, y + DownShift] != -1)
                        {
                            Texture2D flowertexture = FlowerTextures[FlowerTiles[x + DownShift, y + DownShift]];
                            Vector2 flowerSize = new Vector2(11, 10) * ScaleFactor;
                            sb.Draw(flowertexture, new Rectangle((int)(Position.X + x * TileSize.X / 2 - y * TileSize.X / 2 + 6 * ScaleFactor), (int)(Position.Y + y * (TileSize.Y / 6f) + x * (TileSize.Y / 6f) + 7 * ScaleFactor) + HeightOffsets[x + DownShift, y + DownShift], (int)flowerSize.X, (int)flowerSize.Y), Color.White);
                        }
                    }
                        
                        

                }
            }
            
            
        }

        public Vector2 GetGridPosition(Vector2 index)
        {
            float x = (int)Position.X + index.X * (int)TileSize.X / 2 - index.Y * (int)TileSize.X / 2;
            float y = (int)Position.Y + index.Y * (int)TileSize.Y / 6f + index.X * (int)TileSize.Y / 6f + HeightOffsets[(int)index.X, (int)index.Y];
            return new Vector2(x, y);
        }
        public Vector2 GetGridPositionNoHeight(Vector2 index)
        {
            float x = (int)Position.X + index.X * TileSize.X / 2 - index.Y * TileSize.X / 2;
            float y = (int)Position.Y + index.Y * TileSize.Y / 6f + index.X * TileSize.Y / 6f;
            return new Vector2(x, y);
        }
        public Vector2 GetRandomBorderPos()
        {
            Vector2 newPos;
            do
            {
                int x = Random.Shared.Next(0, 2) == 0 ? -1 : size;
                int y = Random.Shared.Next(-1, size + 1);
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
