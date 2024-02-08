using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinterJam.Players;
using WinterJam.Screens;

namespace WinterJam.Screens
{
    public class PlayScreen : Screen
    {
        public static List<GameObject> _allObjects = new List<GameObject>();
        public static List<Enemy> _enemies = new List<Enemy>();
        public static House _house;
        public static List<Obstacle> _obstacles = new List<Obstacle>();
        public static List<Texture2D> _obstacleTextures = new List<Texture2D>();
        public static Player _player;

        public int _amountOfObstacles = 15;
        public PlayScreen()
        {
            GenerateRandomTiles();
        }


        public override void Update(GameTime gameTime)
        {
            // Your existing update logic goes here
            GameSettings.Grid.Update();
            UserInput.Update();

            if (UserInput._currentMouseState.RightButton == ButtonState.Pressed && UserInput._previousMouseState.RightButton == ButtonState.Released)
            {
                GameSettings.IsCloseButtonPressed = false;
                GameSettings.IsPauseScreenDrawn = true;
            }

            //if (GameSettings.IsPauseScreenDrawn == true)
            //{
            //    GameSettings.PauseScreen.Update(gameTime);
            //}

            //if (GameSettings.IsSettingsScreenDrawn == true)
            //{
            //    GameSettings.SettingsScreen.Update(gameTime);
            //}

            if (GameSettings.IsPauseScreenDrawn == false)
            {
                if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Enter) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Enter))
                {
                    _enemies.Add(Enemy.Spawn());
                }
                if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Space) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Space))
                {
                    GenerateRandomTiles();
                }
                foreach (Enemy enemy in _enemies)
                {
                    enemy.Update(gameTime);
                }
                foreach (var gameObject in _allObjects)
                {
                    
                    if (gameObject is House)
                    {
                        House house = gameObject as House;
                        house.Update(gameTime);
                    }
                    if (gameObject is Player)
                    {
                        Player player = gameObject as Player;
                        player.Update(gameTime);
                    }
                    if (gameObject is Obstacle)
                    {
                        Obstacle obstacle = gameObject as Obstacle;
                        obstacle.Update(gameTime);
                    }
                }

                _allObjects = SortedObjects();
            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Your existing draw logic goes here

            GameSettings.Grid.DrawGrass(spriteBatch);

            for (int i = 0; i < _allObjects.Count; i++)
            {
                _allObjects[i].Draw(spriteBatch);
            }
            

            if (GameSettings.IsPauseScreenDrawn && !GameSettings.IsSettingsScreenDrawn)
            {
                GameSettings.PauseScreen.Draw(spriteBatch);
            }

            if (GameSettings.IsPauseScreenDrawn && GameSettings.IsSettingsScreenDrawn && GameSettings.IsCloseButtonPressed == false)
            {
                GameSettings.SettingsScreen.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
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

            return SortList(returnList, 0, returnList.Count - 1);
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
        

        public void GenerateRandomTiles()
        {
            int log = 5;
            int bush = 0;
            int rock = 2;

            Texture2D texture;
            _obstacles.Clear();
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
            CheckForLogFormations();
        }

        private void CheckForLogFormations()
        {
            for (int i = 0; i < _obstacles.Count; i++)
            {
                Vector2 topleft = _obstacles[i].indexPosition - new Vector2(1, 0);
                Vector2 topright = _obstacles[i].indexPosition - new Vector2(0, 1);

                Obstacle obstacle = GetObstacleOn(topleft);
                if (obstacle != null)
                {
                    obstacle.Visualisation.Texture = _obstacleTextures[8];
                    obstacle.Visualisation.IsFlipped = true;
                    obstacle.IsLog = true;
                    _obstacles[i].Visualisation.Texture = _obstacleTextures[7];
                    _obstacles[i].Visualisation.IsFlipped = true;
                    _obstacles[i].IsLog = true;
                }
                obstacle = GetObstacleOn(topright);
                if (obstacle != null)
                {
                    obstacle.Visualisation.Texture = _obstacleTextures[8];
                    obstacle.IsLog = true;
                    _obstacles[i].Visualisation.Texture = _obstacleTextures[7];
                    _obstacles[i].IsLog = true;

                }
                //obstacle = GetObstacleOn(topright);
                //if (obstacle != new Obstacle())
                //{
                //    obstacle.IsActive = false;
                //    _obstacles[i].Visualisation.Texture = _obstacleTextures[7];
                //}
            }
        }

        private Obstacle GetObstacleOn(Vector2 pos)
        {
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (!_obstacles[i].IsLog && _obstacles[i].indexPosition == pos) { return _obstacles[i]; }
            }
            return null;
        }

        public Vector2 GetRandomPositionOnGrid()
        {
            Vector2 newPos;
            do
            {
                newPos = new Vector2(Random.Shared.Next(1, (int)GameSettings.Grid.playsize), Random.Shared.Next(1, GameSettings.Grid.playsize));
            } while (ExistsInObjects(newPos) || House.HouseTiles.Contains(newPos) || House.SurroundingTiles.Contains(newPos));
            return newPos;

        }

        private bool ExistsInObjects(Vector2 newPos)
        {
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (_obstacles[i].indexPosition == newPos)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
