using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinterJam.Screens
{
    internal class PauseScreen : Screen
    {
        private int buttonWidth = (int)(68 * 6);
        private int buttonHeight = (int)(21 * 6);
        private bool playButtonPressed = false;
        private bool settingsButtonPressed = false;
        private bool quitButtonPressed = false;

        public override void Update(GameTime gameTime)
        {
            if (GameSettings.IsSettingsScreenDrawn == false)
            {
                CheckPlayButtonClick();
                CheckQuitButtonClicked();
                CheckSettingsButtonClick();
            }

            if (GameSettings.IsSettingsScreenDrawn)
            {
                GameSettings.SettingsScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private async void CheckPlayButtonClick()
        {
            // Check if the play button is pressed
            Rectangle playButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y / 3, buttonWidth, buttonHeight);
            if (UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                playButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                // Set the play button texture to pressed
                playButtonPressed = true;
                // Simulate a delay
                await Task.Delay(100);
                playButtonPressed = false;
                GameSettings.IsPauseScreenDrawn = false;
            }
        }

        private async void CheckQuitButtonClicked()
        {
            // Check if the settings button is pressed
            Rectangle quitButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, buttonWidth, buttonHeight);
            if (!GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                quitButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                // Enable the settings screen
                quitButtonPressed = true;
                await Task.Delay(100);
                quitButtonPressed = false;
                // Exit the program
                //Environment.Exit(0);
                GameSettings.ActiveScreen = GameSettings.GameOverScreen;
            }
        }

        private async void CheckSettingsButtonClick()
        {
            // Check if the settings button is pressed
            Rectangle settingsButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y / 2, buttonWidth, buttonHeight);
            if (!GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                settingsButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                // Enable the settings screen
                settingsButtonPressed = true;
                await Task.Delay(100);
                settingsButtonPressed = false;
                GameSettings.IsSettingsScreenDrawn = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dr = new Rectangle(0, 0, (int)GameSettings.ScreenSize.X, (int)GameSettings.ScreenSize.Y);
            spriteBatch.Draw(GameSettings.ScreenTexture, dr, new Color(0, 0, 0, 100));

            DrawPausedText(spriteBatch);

            // Draw Play Button
            DrawPlayButton(spriteBatch);

            // Draw Settings Button
            DrawSettingsButton(spriteBatch);

            //Draw Quit Button
            DrawQuitButton(spriteBatch);
        }

        private void DrawPausedText(SpriteBatch spriteBatch)
        {
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y / 4 - buttonHeight, buttonWidth, buttonHeight);
            spriteBatch.Draw(GameSettings.PausedText, dr, Color.White);
        }

        private void DrawQuitButton(SpriteBatch spriteBatch)
        {
            Texture2D quitButtonTexture = quitButtonPressed ? GameSettings.Button_Pressed_Orange : GameSettings.Button_Orange;
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, buttonWidth, buttonHeight);
            spriteBatch.Draw(quitButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("QUIT");
            if (!quitButtonPressed)
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2);
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
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y / 2, buttonWidth, buttonHeight);
            spriteBatch.Draw(settingsButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("SETTINGS");
            if (!settingsButtonPressed)
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2);
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
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - buttonWidth) / 2, (int)GameSettings.ScreenSize.Y / 3, buttonWidth, buttonHeight);
            spriteBatch.Draw(playButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("PLAY");
            if (!playButtonPressed)
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2);
            }
            else
            {
                textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2 + 16);
            }
            spriteBatch.DrawString(GameSettings.GameFont, "PLAY", textPosition, Color.Black);
        }
    }
}
