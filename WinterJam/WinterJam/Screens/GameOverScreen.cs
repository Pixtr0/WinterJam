using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;

namespace WinterJam.Screens
{
    public class GameOverScreen : Screen
    {
        private int buttonWidth = 68 * 4;
        private int buttonHeight = 21 * 4;
        private bool playButtonPressed = false;
        private bool settingsButtonPressed = false;
        private bool quitButtonPressed = false;
        private Rectangle UI_Background_Rectangle
        {
            get
            {
                return new Rectangle(Point.Zero, GameSettings.ScreenSize.ToPoint());
            }
        }
        public override void Update(GameTime gameTime)
        {
            UserInput.Update();

            CheckSettingsButtonClick();
            GameSettings.IsPauseScreenDrawn = false;

            if (GameSettings.IsSettingsScreenDrawn)
            {
                GameSettings.SettingsScreen.Update(gameTime);
            }
            if (!GameSettings.IsSettingsScreenDrawn)
            {
                CheckQuitButtonClicked();
                UpdateActiveScreen(gameTime);
            }

            base.Update(gameTime);
        }

        private async void CheckQuitButtonClicked()
        {
            // Check if the settings button is pressed
            Rectangle quitButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y * 4 / 5, buttonWidth, buttonHeight);
            if (!GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                quitButtonRect.Contains(UserInput._currentMouseState.Position))
            {

                GameSettings.SFX_Button.Play();
                // Enable the settings screen
                quitButtonPressed = true;
                await Task.Delay(100);
                quitButtonPressed = false;
                // Exit the program
                Environment.Exit(0);
            }
        }

        private async void CheckSettingsButtonClick()
        {
            // Check if the settings button is pressed
            Rectangle settingsButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, buttonHeight + (int)GameSettings.ScreenSize.Y / 2, buttonWidth, buttonHeight);
            if (!GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                settingsButtonRect.Contains(UserInput._currentMouseState.Position))
            {

                GameSettings.SFX_Button.Play();
                // Enable the settings screen
                settingsButtonPressed = true;
                await Task.Delay(100);
                settingsButtonPressed = false;
                GameSettings.IsSettingsScreenDrawn = true;
            }
        }

        private async void UpdateActiveScreen(GameTime gameTime)
        {
            // Check if the play button is pressed
            Rectangle playButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, buttonHeight + (int)GameSettings.ScreenSize.Y / 3, buttonWidth, buttonHeight);
            if (!playButtonPressed && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                playButtonRect.Contains(UserInput._currentMouseState.Position))
            {

                GameSettings.SFX_Button.Play();
                // Set the play button texture to pressed
                playButtonPressed = true;
                await Task.Delay(200); // Wait for 1 second
                playButtonPressed = false;

                GameSettings.PlayScreen.Reset();
                GameSettings.ActiveScreen = GameSettings.PlayScreen ; // Switch to the play screen
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //GameSettings.Grid.DrawGrass(spriteBatch);

            //spriteBatch.Draw(GameSettings.ScreenTexture, dr, Color.Black);
            

            spriteBatch.Draw(GameSettings.UI_Background_GameOver, UI_Background_Rectangle, Color.White);

            //DestinationRectangle for the GameOver logo
            DrawGameOverText(spriteBatch);

            // Draw Play Button
            DrawPlayButton(spriteBatch);

            // Draw Settings Button
            DrawSettingsButton(spriteBatch);

            //Draw Quit Button
            DrawQuitButton(spriteBatch);

            Score.DrawResults(spriteBatch,new Vector2(GameSettings.ScreenSize.X / 4 * 3 , GameSettings.ScreenSize.Y / 5 * 2));
            DrawCredits(spriteBatch);

            if (GameSettings.IsSettingsScreenDrawn && !GameSettings.IsCloseButtonPressed)
            {
                GameSettings.SettingsScreen.Draw(spriteBatch);
            }
        }

        private void DrawCredits(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameSettings.GameFont, "CREDITS", new Vector2(20
                , GameSettings.ScreenSize.Y - 402), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "CREDITS", new Vector2(20
                , GameSettings.ScreenSize.Y - 398), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "CREDITS", new Vector2(18
                , GameSettings.ScreenSize.Y - 400), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "CREDITS", new Vector2(22
                , GameSettings.ScreenSize.Y - 400), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "CREDITS", new Vector2(20
                , GameSettings.ScreenSize.Y - 400), Color.Black);

            spriteBatch.DrawString(GameSettings.GameFont,
              "Matthias de Vilder\n\n" +
              "Samuel Cutts\n\n" +
              "Matthias Maes\n\n" +
              "Ash",
              new Vector2(20, GameSettings.ScreenSize.Y - 298), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
              "Matthias de Vilder\n\n" +
              "Samuel Cutts\n\n" +
              "Matthias Maes\n\n" +
              "Ash",
              new Vector2(20, GameSettings.ScreenSize.Y - 302), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
              "Matthias de Vilder\n\n" +
              "Samuel Cutts\n\n" +
              "Matthias Maes\n\n" +
              "Ash",
              new Vector2(18, GameSettings.ScreenSize.Y - 300), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
              "Matthias de Vilder\n\n" +
              "Samuel Cutts\n\n" +
              "Matthias Maes\n\n" +
              "Ash",
              new Vector2(22, GameSettings.ScreenSize.Y - 300), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont, 
                "Matthias de Vilder\n\n" +
                "Samuel Cutts\n\n" +
                "Matthias Maes\n\n" +
                "Ash",
                new Vector2(20, GameSettings.ScreenSize.Y - 300), Color.Black, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);


            spriteBatch.DrawString(GameSettings.GameFont,
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Artist",
                new Vector2(400, GameSettings.ScreenSize.Y - 302), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Artist",
                new Vector2(400, GameSettings.ScreenSize.Y - 298), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Artist",
                new Vector2(398, GameSettings.ScreenSize.Y - 300), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Artist",
                new Vector2(402, GameSettings.ScreenSize.Y - 300), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
            spriteBatch.DrawString(GameSettings.GameFont,
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Programmer\n\n" +
                "-  Artist",
                new Vector2(400, GameSettings.ScreenSize.Y - 300), Color.Black, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 1);
        }
        private void DrawGameOverText(SpriteBatch spriteBatch)
        {
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth * 3/2) / 2, (int)GameSettings.ScreenSize.Y / 4 - buttonHeight, buttonWidth * 3/2, buttonHeight);
            spriteBatch.Draw(GameSettings.UI_game_over, dr, Color.White);
        }

        private void DrawQuitButton(SpriteBatch spriteBatch)
        {
            Texture2D quitButtonTexture = quitButtonPressed ? GameSettings.Button_Pressed_Orange : GameSettings.Button_Orange;
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y * 4 / 5, buttonWidth, buttonHeight);
            spriteBatch.Draw(quitButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("QUIT");
            if (!quitButtonPressed)
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 4);
            }
            else
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 16);
            }
            spriteBatch.DrawString(GameSettings.GameFont, "QUIT", textPosition, Color.Black);
        }

        private void DrawSettingsButton(SpriteBatch spriteBatch)
        {
            Texture2D settingsButtonTexture = settingsButtonPressed ? GameSettings.Button_Pressed_Yellow : GameSettings.Button_Yellow;
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, buttonHeight + (int)GameSettings.ScreenSize.Y / 2, buttonWidth, buttonHeight);
            spriteBatch.Draw(settingsButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("SETTINGS");
            if (!settingsButtonPressed)
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 4);
            }
            else
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 16);
            }
            spriteBatch.DrawString(GameSettings.GameFont, "SETTINGS", textPosition, Color.Black);
        }

        private void DrawPlayButton(SpriteBatch spriteBatch)
        {

            Texture2D playButtonTexture = playButtonPressed ? GameSettings.Button_Pressed_Yellow : GameSettings.Button_Yellow;
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, buttonHeight + (int)GameSettings.ScreenSize.Y / 3, buttonWidth, buttonHeight);
            spriteBatch.Draw(playButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("TRY AGAIN");
            if (!playButtonPressed)
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 4);
            }
            else
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 16);
            }
            spriteBatch.DrawString(GameSettings.GameFont, "TRY AGAIN", textPosition, Color.Black);
        }
    }
}
