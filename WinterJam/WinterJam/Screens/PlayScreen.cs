using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
//using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Drawing;
using System.Linq;
using WinterJam.Players;
using WinterJam.Screens;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WinterJam.Screens
{
    public class PlayScreen : Screen
    {
        public static List<GameObject> AllObjects { get; set; } = new List<GameObject>();
        public static List<Vector2> BasketPositions { get; set; } = new List<Vector2>();
        public static List<Obstacle> Baskets { get; set; } = new List<Obstacle>();
        public static List<Obstacle> bgObstacles { get; set; } = new List<Obstacle>();
        public static List<Obstacle> bgObstaclesBack { get; set; } = new List<Obstacle>();
        public static List<GameObject> Backgroundback {  get; set; } = new List<GameObject> { };
        public static List<GameObject> Backgroundfront {  get; set; } = new List<GameObject> { };
        public static List<Enemy> Enemies { get; set; } = new List<Enemy>();
        public static List<Tree> Trees { get; set; } = new List<Tree>();
        public static List<Tree> TreesBack { get; set; } = new List<Tree>();
        public static House House { get; set; }
        public static List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();
        public static List<Texture2D> ObstacleTextures { get; set; } = new List<Texture2D>();
        public static Player Player {  get; set; }
        public static List<Item> DroppedItems { get; set; } = new List<Item>();

        private int _amountOfObstacles = 13;
        private int escToPauseDrawCounter = 360;
        private bool SpawnedSquirrel = false;

        public PlayScreen()
        {
            Reset();
        }
        
        public void Reset()
        {
            GenerateBaskets();
            GenerateRandomTiles();
            GenerateTreesAndSurroundings();
            Enemies.Clear();
            DroppedItems.Clear();
            Score.Time = 0;
            Score.AmountOfItemsreturned = 0;
            Score.AmountOfSquirrelsSmacked = 0;
            Player.CurrentPosition = new Vector2(3, 4);
            Player.NextPosition = Player.CurrentPosition;
            House.currentHp = 20;

        }
        public override void Update(GameTime gameTime)
        {
           
            // Your existing update logic goes here
            GameSettings.Grid.Update();
            UserInput.Update();

            if (UserInput._currentKeyboardSate.IsKeyDown(Keys.Escape) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Escape))
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
            if(GameSettings.IsControlsScreenDrawn == true)
            {
                GameSettings.ControlsScreen.Update(gameTime);
            }

            if (GameSettings.IsPauseScreenDrawn == false)
            {
                Score.Update(gameTime);
                if (!SpawnedSquirrel && Score.Time % 15 == 0)
                {
                    Enemies.Add(Enemy.Spawn());
                    SpawnedSquirrel = true;
                }
                else if(Score.Time % 15 != 0) { SpawnedSquirrel = false; }
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

            UpdateGameState();
            base.Update(gameTime);
        }

        private void UpdateGameState()
        {
            if (House.currentHp <= 0)
            {
                GameSettings.ActiveScreen = GameSettings.GameOverScreen;
            }
        }

        private void GenerateTreesAndSurroundings()
        {
            for (int x = -GameSettings.Grid.DownShift; x < GameSettings.Grid.Size.X - GameSettings.Grid.DownShift; x++)
            {
                for (int y = -GameSettings.Grid.DownShift; y < GameSettings.Grid.Size.Y - GameSettings.Grid.DownShift; y++)
                {
                    Random rnd = new Random();
                    int randomValue = rnd.Next(0, 5);
                    if (randomValue > 2)
                    {
                        if (y < 1)
                        {
                            Trees.Add(new Tree(new Vector2(x, y)));
                        }
                        if (y >= 1 && x < 1)
                        {
                            Trees.Add(new Tree(new Vector2(x, y)));
                        }
                        if (y >= 1 && x > 5 + GameSettings.Grid.playsize)
                        {
                            TreesBack.Add(new Tree(new Vector2(x, y)));
                        }
                        if (y > 5 + GameSettings.Grid.playsize && x >= 1)
                        {
                            TreesBack.Add(new Tree(new Vector2(x, y)));
                        }
                    }

                }
            }

            for (int i = 0; i < Trees.Count; i++)
            {
                Backgroundback.Add(Trees[i]);
            }
            for (int i = 0; i < TreesBack.Count; i++)
            {
                Backgroundfront.Add(TreesBack[i]);
            }

            Backgroundback = SortList(Backgroundback, 0, Backgroundback.Count - 1);
            Backgroundfront = SortList(Backgroundfront, 0, Backgroundfront.Count - 1);

        }

        private void GenerateBaskets()
        {
            //BasketPositions.Add(new Vector2(GameSettings.Grid.playsize - 1, GameSettings.Grid.playsize - 1));
            BasketPositions.Add(new Vector2(1, GameSettings.Grid.playsize - 1));
            BasketPositions.Add(new Vector2(GameSettings.Grid.playsize - 1, 1));
            //BasketPositions.Add(new Vector2(1, 1));
            //for (int i = 0; i < _amountOfBaskets; i++)
            //    BasketPositions.Add(new Vector2(Random.Shared.Next(1, GameSettings.Grid.playsize - 1),
            //        Random.Shared.Next(1, GameSettings.Grid.playsize - 1)));

            for (int i = 0; i < BasketPositions.Count; i++)
                Baskets.Add(new Obstacle(GameSettings.BasketTexture, BasketPositions[i], 5));
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
            for (int i = 0; i < Backgroundback.Count; i++)
            {
                Backgroundback[i].Draw(spriteBatch);
            }
            
            for (int i = 0; i < AllObjects.Count; i++)
            {
                AllObjects[i].Draw(spriteBatch);
            }
            for (int i = 0; i < Backgroundfront.Count; i++)
            {
                Backgroundfront[i].Draw(spriteBatch);
            }

            DrawEscToPause(spriteBatch);
            Score.Draw(spriteBatch, new Vector2((int)(GameSettings.ScreenSize.X / 2), 60));

            if (GameSettings.IsPauseScreenDrawn && GameSettings.IsControlsScreenDrawn && !GameSettings.IsSettingsScreenDrawn && GameSettings.IsCloseButtonPressed == false)
            {
                GameSettings.ControlsScreen.Draw(spriteBatch);
            }
            if (GameSettings.IsPauseScreenDrawn && !GameSettings.IsControlsScreenDrawn && !GameSettings.IsSettingsScreenDrawn)
            {
                GameSettings.PauseScreen.Draw(spriteBatch);
            }

            if (GameSettings.IsPauseScreenDrawn && !GameSettings.IsControlsScreenDrawn && GameSettings.IsSettingsScreenDrawn && GameSettings.IsCloseButtonPressed == false)
            {
                GameSettings.SettingsScreen.Draw(spriteBatch);
            }




            base.Draw(spriteBatch);
        }

        private void DrawEscToPause(SpriteBatch spriteBatch)
        {
            // Get the color for the text

            // Draw the background rectangle
            Vector2 textSize = GameSettings.GameFont.MeasureString("Esc to pause");
            Color buttonColor = Color.White;

            spriteBatch.Draw(GameSettings.Button_Pressed_Orange, new Rectangle((int)(4 * GameSettings.Grid.ScaleFactor), (int)(4 * GameSettings.Grid.ScaleFactor), (int)textSize.X + 50, (int)textSize.Y * 2 + 30), buttonColor);

            // Draw the text with the calculated color
            spriteBatch.DrawString(GameSettings.GameFont, "Esc to pause",new Vector2( (int)(11 * GameSettings.Grid.ScaleFactor), (int)(12 * GameSettings.Grid.ScaleFactor)), Color.Black);
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
            foreach (Obstacle basket in Baskets)
            {
                returnList.Add(basket);
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
            List<GameObject> sortablelist = new List<GameObject>();
            foreach(GameObject obj in Obstacles)
            {
                sortablelist.Add(obj);
            }
            sortablelist = SortList(sortablelist,0, sortablelist.Count - 1);
            Obstacles.Clear();
            foreach (Obstacle obj in sortablelist)
            {
                Obstacles.Add(obj);
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
                } else
                {
                    obstacle = GetObstacleOn(topright);
                    if (obstacle != null)
                    {
                        obstacle.Visualisation.Texture = ObstacleTextures[8];
                        obstacle.IsLog = true;
                        Obstacles[i].Visualisation.Texture = ObstacleTextures[7];
                        Obstacles[i].IsLog = true;

                    }
                }
                
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
            } while (ExistsInObjects(newPos) || House.HouseTiles.Contains(newPos) || House.SurroundingTiles.Contains(newPos) || BasketPositions.Contains(newPos));
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
