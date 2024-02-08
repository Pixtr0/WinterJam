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
        public static List<GameObject> AllObjects { get; set; } = new List<GameObject>();
        public static List<Enemy> Enemies { get; set; } = new List<Enemy>();
        public static House House { get; set; }
        public static List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();
        public static List<Texture2D> ObstacleTextures { get; set; } = new List<Texture2D>();
        public static Player Player {  get; set; }
        public static List<Item> DroppedItems { get; set; } = new List<Item>();

        private int _amountOfObstacles = 15;
        public PlayScreen()
        {
            GenerateRandomTiles();
            GenerateRandomItems();
        }

        private void GenerateRandomItems()
        {
            //while (DroppedItems.Count < 6)
            //{
            //    Item newDroppedItem = new Item();
            //    DroppedItems.Add(newDroppedItem);
            //}
        }

        public override void Update(GameTime gameTime)
        {
            // Your existing update logic goes here
            GameSettings.Grid.Update();
            UserInput.Update();

            if (UserInput._currentMouseState.RightButton == ButtonState.Pressed && UserInput._previousMouseState.RightButton == ButtonState.Released)
            {
                GameSettings.IsCloseButtonPressed = false;
                GameSettings.IsPauseScreenDrawn = !GameSettings.IsPauseScreenDrawn;
            }

            if (GameSettings.IsPauseScreenDrawn == true)
            {
                GameSettings.PauseScreen.Update(gameTime);
            }

            if (GameSettings.IsSettingsScreenDrawn == true)
            {
                GameSettings.SettingsScreen.Update(gameTime);
            }

            if (GameSettings.IsPauseScreenDrawn == false)
            {
                if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Enter) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Enter))
                {
                    Enemies.Add(Enemy.Spawn());
                }
                //if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Space) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Space))
                //{
                //    GenerateRandomTiles();
                //}
                foreach (Enemy enemy in Enemies)
                {
                    enemy.Update(gameTime);
                }
                foreach (var gameObject in AllObjects)
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

                AllObjects = SortedObjects();
            }

            UpdateDroppeditems(gameTime);

            base.Update(gameTime);
        }

        private static void UpdateDroppeditems(GameTime gameTime)
        {

            if (DroppedItems.Count > 0)
            {
                foreach (Item item in DroppedItems)
                {
                    if (item != null)
                        item.Update(gameTime);
                }
                for (int i = 0; i < DroppedItems.Count; i++)
                {
                    if (Player.CurrentPosition == DroppedItems[i].CurrentPosition)
                    {
                        Item newPlayerItem = new Item(Player)
                        {
                            Visualisation = DroppedItems[i].Visualisation,
                            ParentSize = Player.Size
                        };
                        Player.Inventory.Add(newPlayerItem);
                        DroppedItems.Remove(DroppedItems[i]);
                    }
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Your existing draw logic goes here

            GameSettings.Grid.DrawGrass(spriteBatch);
            Player.DrawEffect(spriteBatch);
            for (int i = 0; i < AllObjects.Count; i++)
            {
                AllObjects[i].Draw(spriteBatch);
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
            foreach (Obstacle obstacle in Obstacles)
            {
                returnList.Add(obstacle);
            }
            foreach (Enemy enemy in Enemies)
            {
                returnList.Add(enemy);
            }
            foreach (Item item in DroppedItems)
            {
                returnList.Add(item);
            }
            returnList.Add(House);
            returnList.Add(Player);

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
            Obstacles.Clear();
            for (int i = 0; i < _amountOfObstacles; i++)
            {
                int value = Random.Shared.Next(0, 3);
                if (value == 0)
                {
                    texture = ObstacleTextures[log];
                    log++;
                    if (log > 6)
                        log = 5;
                }
                else if (value == 1)
                {
                    texture = ObstacleTextures[rock];
                    rock++;
                    if (rock > 4)
                        rock = 2;
                }
                else
                {
                    texture = ObstacleTextures[bush];
                    bush++;
                    if (bush > 1)
                        bush = 0;
                }
                Obstacles.Add(new Obstacle(texture, GetRandomPositionOnGrid()));
            }
            CheckForLogFormations();
        }

        private void CheckForLogFormations()
        {
            for (int i = 0; i < Obstacles.Count; i++)
            {
                Vector2 topleft = Obstacles[i].indexPosition - new Vector2(1, 0);
                Vector2 topright = Obstacles[i].indexPosition - new Vector2(0, 1);

                Obstacle obstacle = GetObstacleOn(topleft);
                if (obstacle != null)
                {
                    obstacle.Visualisation.Texture = ObstacleTextures[8];
                    obstacle.Visualisation.IsFlipped = true;
                    obstacle.IsLog = true;
                    Obstacles[i].Visualisation.Texture = ObstacleTextures[7];
                    Obstacles[i].Visualisation.IsFlipped = true;
                    Obstacles[i].IsLog = true;
                }
                obstacle = GetObstacleOn(topright);
                if (obstacle != null)
                {
                    obstacle.Visualisation.Texture = ObstacleTextures[8];
                    obstacle.IsLog = true;
                    Obstacles[i].Visualisation.Texture = ObstacleTextures[7];
                    Obstacles[i].IsLog = true;

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
            for (int i = 0; i < Obstacles.Count; i++)
            {
                if (!Obstacles[i].IsLog && Obstacles[i].indexPosition == pos) { return Obstacles[i]; }
            }
            return null;
        }

        public static Vector2 GetRandomPositionOnGrid()
        {
            Vector2 newPos;
            do
            {
                newPos = new Vector2(Random.Shared.Next(1, (int)GameSettings.Grid.playsize), Random.Shared.Next(1, GameSettings.Grid.playsize));
            } while (ExistsInObjects(newPos) || House.HouseTiles.Contains(newPos) || House.SurroundingTiles.Contains(newPos));
            return newPos;

        }

        private static bool ExistsInObjects(Vector2 newPos)
        {
            for (int i = 0; i < Obstacles.Count; i++)
            {
                if (Obstacles[i].indexPosition == newPos)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
