using Isometric_Thingy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinterJam.Players;

namespace WinterJam
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;

        private List<Enemy> _enemies = new List<Enemy>();

        private House _house;

        private int _amountOfObstacles = 15;
        internal static List<Obstacle> _obstacles = new List<Obstacle>();
        private List<Texture2D> _obstacleTextures = new List<Texture2D>();

        private List<GameObject> _allObjects = new List<GameObject>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = (int)GameSettings.ScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)GameSettings.ScreenSize.Y;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameSettings.Grid = new Grid(new Vector2(_graphics.PreferredBackBufferWidth / 2, 0)
            , new List<Texture2D>() {
                Content.Load<Texture2D>("Graphics/Blocks/grass_01"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_02"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_03"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_04"),
            });
            Vector2 playerStart = new Vector2(4, 4);
            _player = new Player(playerStart,
            new SpriteSheet(
                Content.Load<Texture2D>("Graphics/Player/Player"),
                GameSettings.Grid.GetPlayerPosition(playerStart),
                new Vector2(80, 80),
                0,
                1,
                1,
                1,
                false
                )
            );
            _obstacleTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Graphics/Blocks/bush_01"),
                Content.Load<Texture2D>("Graphics/Blocks/bush_02"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_01"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_02"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_03"),
                Content.Load<Texture2D>("Graphics/Blocks/stump_01"),
                Content.Load<Texture2D>("Graphics/Blocks/stump_02")
            };
            GameSettings.Squirrel_Up = Content.Load<Texture2D>("Graphics/Enemy/up");
            GameSettings.Squirrel_Down = Content.Load<Texture2D>("Graphics/Enemy/down");
            GameSettings.Squirrel_Left = Content.Load<Texture2D>("Graphics/Enemy/left");
            GameSettings.Squirrel_Right = Content.Load<Texture2D>("Graphics/Enemy/right");
            GameSettings.Squirrel_Up_Left = Content.Load<Texture2D>("Graphics/Enemy/up_left");
            GameSettings.Squirrel_Up_Right = Content.Load<Texture2D>("Graphics/Enemy/up_right");
            GameSettings.Squirrel_Down_Left = Content.Load<Texture2D>("Graphics/Enemy/down_left");
            GameSettings.Squirrel_Down_Right = Content.Load<Texture2D>("Graphics/Enemy/down_right");
            _house = new House(Content.Load<Texture2D>("Graphics/Blocks/spritesheet_house"));

            GenerateRandomTiles();
            _enemies.Add(Enemy.Spawn());

            foreach (var Object in _allObjects)
            {
                Debug.WriteLine(Object.anchorPoint.Y);
            }
        }

        private List<GameObject> SortedObjects()
        {
            List<GameObject> returnList = new List<GameObject>();
            foreach (Obstacle obstacle in _obstacles)
            {
                returnList.Add(obstacle);
            }
            foreach (Enemy enemy in _enemies)
            {
                returnList.Add(enemy);
            }
            returnList.Add(_house);
            returnList.Add(_player);

            return SortList(returnList,0,returnList.Count - 1);
        }
        private List<GameObject> SortList(List<GameObject> list, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = list[leftIndex].anchorPoint.Y;
            while (i <= j)
            {
                while (list[i].anchorPoint.Y < pivot)
                {
                    i++;
                }

                while (list[j].anchorPoint.Y > pivot)
                {
                    j--;
                }
                if (i <= j)
                {
                    var temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                SortList(list, leftIndex, j);
            if (i < rightIndex)
                SortList(list, i, rightIndex);
            return list;
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameSettings.Grid.Update();
            UserInput.Update();
            _player.Update(gameTime, _obstacles);

            if(UserInput._currentKeyboardSate.IsKeyDown(Keys.Enter) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Enter))
            {
                _enemies.Add(Enemy.Spawn());
            }
            if(UserInput._currentKeyboardSate.IsKeyDown(Keys.Space) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Space))
            {
                GenerateRandomTiles();
            }

            foreach (var gameObject in _allObjects)
            {
                if(gameObject is Enemy)
                {
                    Enemy enemy = gameObject as Enemy;
                    enemy.Update(gameTime);
                }
                if (gameObject is House)
                {
                    House house = gameObject as House;
                    house.Update(gameTime);
                }
            }
                    

            _allObjects = SortedObjects();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            GameSettings.Grid.DrawGrass(_spriteBatch);
            for (int i = 0; i < _allObjects.Count; i++)
            {
                _allObjects[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void GenerateRandomTiles()
        {
            int log = 5;
            int bush = 0;
            int rock = 2;

            Texture2D texture;

            for (int i = 0; i < _amountOfObstacles; i++)
            {
                int value = Random.Shared.Next(0, 3);
                if (value == 0)
                {
                    texture = _obstacleTextures[log];
                    log++;
                    if (log > 6)
                        log = 5;
                }
                else if (value == 1)
                {
                    texture = _obstacleTextures[rock];
                    rock++;
                    if (rock > 4)
                        rock = 2;
                }
                else
                {
                    texture = _obstacleTextures[bush];
                    bush++;
                    if (bush > 1)
                        bush = 0;
                }
                _obstacles.Add(new Obstacle(texture, GetRandomPositionOnGrid()));
            }
        }

        private Vector2 GetRandomPositionOnGrid()
        {
            Vector2 newPos;
            do
            {
                newPos = new Vector2(Random.Shared.Next(0, (int)GameSettings.Grid.Size.X), Random.Shared.Next(0, (int)GameSettings.Grid.Size.Y));
            } while (ExistsInObjects(newPos) || House.HouseTiles.Contains(newPos) || House.SurroundingTiles.Contains(newPos));
            return newPos;
            
        }

        private bool ExistsInObjects(Vector2 newPos)
        {
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (_obstacles[i].TopLeftPosition == GameSettings.Grid.GetPlayerPosition(newPos))
                {
                    return true;
                }
            }
            return false;
        }
    }
}