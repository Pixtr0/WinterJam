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
        private int buttonWidth = 300;
        private int buttonHeight = 200;
        private bool playButtonPressed = false;
        private bool settingsButtonPressed = false;

        public override void Update(GameTime gameTime)
        {
            if (GameSettings.IsSettingsScreenDrawn == false)
            {
                CheckPlayButtonClick();
                CheckSettingsButtonClick();
            }

            if (GameSettings.IsSettingsScreenDrawn)
            {
                GameSettings.SettingsScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void CheckPlayButtonClick()
        {
            // Check if the play button is pressed
            Rectangle playButtonRect = new Rectangle((int)GameSettings.ScreenSize.X / 4 - buttonWidth / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, buttonWidth, buttonHeight);
            if (UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                playButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                // Set the play button texture to pressed
                playButtonPressed = true;
                // Simulate a delay
                Task.Delay(1000).ContinueWith(_ =>
                {
                    playButtonPressed = false;
                    // Perform action after delay, e.g., enable settings screen
                    GameSettings.IsPauseScreenDrawn = false;
                });
            }
        }

        private void CheckSettingsButtonClick()
        {
            // Check if the settings button is pressed
            Rectangle settingsButtonRect = new Rectangle((int)GameSettings.ScreenSize.X * 3 / 4 - buttonWidth / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, buttonWidth, buttonHeight);
            if (!GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                settingsButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                Debug.WriteLine("Settings has been pressed");
                // Set the settings button texture to pressed
                settingsButtonPressed = true;
                // Simulate a delay
                Task.Delay(1000).ContinueWith(_ =>
                {
                    settingsButtonPressed = false;
                    // Perform action after delay, e.g., enable settings screen
                    GameSettings.IsSettingsScreenDrawn = true;
                });
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle dr = new Rectangle(0, 0, (int)GameSettings.ScreenSize.X, (int)GameSettings.ScreenSize.Y);
            spriteBatch.Draw(GameSettings.ScreenTexture, dr, new Color(0, 0, 0, 100));

            // Draw Play Button
            DrawPlayButton(spriteBatch);

            // Draw Settings Button
            DrawSettingsButton(spriteBatch);
        }

        private void DrawSettingsButton(SpriteBatch spriteBatch)
        {
            Texture2D settingsButtonTexture = settingsButtonPressed ? GameSettings.Button_Pressed_Yellow : GameSettings.Button_Yellow;
            Rectangle dr = new Rectangle((int)GameSettings.ScreenSize.X * 3 / 4 - buttonWidth / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, buttonWidth, buttonHeight);
            spriteBatch.Draw(settingsButtonTexture, dr, Color.White);

            Vector2 textSize = GameSettings.GameFont.MeasureString("SETTINGS");
            Vector2 textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2);
            spriteBatch.DrawString(GameSettings.GameFont, "SETTINGS", textPosition, Color.Black);
        }

        private void DrawPlayButton(SpriteBatch spriteBatch)
        {
            Texture2D playButtonTexture = playButtonPressed ? GameSettings.Button_Pressed_Yellow : GameSettings.Button_Yellow;
            Rectangle dr = new Rectangle((int)GameSettings.ScreenSize.X / 4 - buttonWidth / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, buttonWidth, buttonHeight);
            spriteBatch.Draw(playButtonTexture, dr, Color.White);

            Vector2 textSize = GameSettings.GameFont.MeasureString("PLAY");
            Vector2 textPosition = new Vector2(dr.X + (buttonWidth - textSize.X) / 2, dr.Y - textSize.Y + buttonHeight / 2);
            spriteBatch.DrawString(GameSettings.GameFont, "PLAY", textPosition, Color.Black);
        }
    }
}
