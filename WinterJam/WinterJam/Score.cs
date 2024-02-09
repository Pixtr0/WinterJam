using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WinterJam
{
    
    public static class Score
    {
        public static int Scorevalue { get { return (int)(Time + (AmountOfSquirrelsSmacked * 4) + (AmountOfItemsreturned * 10)); } }
        public static int Time { get; set; } = 0;
        private static float CountDuration { get; set; } = 1f; //every  2s.
        private static float CurrentTime { get; set; } = 0f;
        public static float AmountOfSquirrelsSmacked { get; set; } = 0f;
        public static float AmountOfItemsreturned { get; set; } = 0f;

        
        public static void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (CurrentTime >= CountDuration)
            {
                Time++;
                CurrentTime -= CountDuration;
            }
        }
        public static void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 textOffset = new Vector2(GameSettings.GameFont.MeasureString(ToString(0)).X / 2, 0);
            spriteBatch.DrawString(GameSettings.GameFont, ToString(Time), position + new Vector2(1, 0) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, ToString(Time), position + new Vector2(-1, 0) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, ToString(Time), position + new Vector2(0, 1) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, ToString(Time), position + new Vector2(0, -1) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, ToString(Time), position - textOffset, Color.Black);

            textOffset = new Vector2(GameSettings.GameFont.MeasureString("Score: " + 10000).X / 2 - 35, -70);
            spriteBatch.DrawString(GameSettings.GameFont, "Score: " + Scorevalue, position + new Vector2(1, 0) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Score: " + Scorevalue, position + new Vector2(-1, 0) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Score: " + Scorevalue, position + new Vector2(0, 1) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Score: " + Scorevalue, position + new Vector2(0, -1) * GameSettings.Grid.ScaleFactor - textOffset, Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Score: " + Scorevalue, position - textOffset, Color.Black);

        }
        
        public static string ToString(int time)
        {
            return (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
        }

    }

    
}
