using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinterJam.Screens
{
    public class ControlsScreen : Screen
    {
        private float _controlsScaleFactor = 18f;

        private Rectangle UI_Controls_Rectangle
        {
            //86 x 15
            get
            {
                return new Rectangle((int)((GameSettings.ScreenSize.X / 2) - (86 * GameSettings.Grid.ScaleFactor)/2),
                                    (int)((GameSettings.ScreenSize.Y / 4) - (21 * GameSettings.Grid.ScaleFactor)/2),
                                    (int)(86 * GameSettings.Grid.ScaleFactor),
                                    (int)(21 * GameSettings.Grid.ScaleFactor));
            }
        }
        private Rectangle UI_Player_Controls_Rectangle
        {
            //1130 x 370
            get
            {
                return new Rectangle((int)((GameSettings.ScreenSize.X /2) - (1130 ) / 2),
                                    (int)((GameSettings.ScreenSize.Y /2) - (370 ) /2),
                                    (int)(1130),
                                    (int)(370));
            }
        }
        private Rectangle SettingsTitleRectangle
        {
            //79 x 15
            get
            {
                return new Rectangle((int)(GameSettings.ScreenSize.X / 2 - 79 * GameSettings.Grid.ScaleFactor / 2 * 1.5f),
                                     (int)(GameSettings.ScreenSize.Y / 4 - buttonHeight * 1.5f),
                                     (int)(79 * 4),
                                     (int)(15 * 4));
            }
        }
        private Vector2 framePosition;
        private int frameWidth = 500;
        private int frameHeight = 500;

        private bool isQwertyButtonPressed = true;
        private bool isAzertyButtonPressed;
        private bool isArrowsButtonPressed;

        private Vector2 offset1 = new Vector2(GameSettings.ScreenSize.X / 6, -90);
        private Vector2 offset2 = new Vector2(GameSettings.ScreenSize.X / 6, -150);

        private int buttonWidth = (int)(68 * 4); // Adjusted button width
        private int buttonHeight = (int)(21 * 4); // Adjusted button height

        private int backButtonWidth = (int)(69 * 4);
        private int backButtonHeight = (int)(21 * 4);
        private bool backButtonPressed = false;

        // Store current and previous mouse states
        private MouseState _currentMouseState, _previousMouseState;

        public override void Update(GameTime gameTime)
        {
            // Update mouse states
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // Check for button presses and slider updates
            CheckCloseButtonPressed();
        }

        private async void CheckCloseButtonPressed()
        {
            Rectangle backButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - backButtonWidth) / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, backButtonWidth, backButtonHeight);
            if (GameSettings.IsControlsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                backButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                // Enable the settings screen

                backButtonPressed = true;
                await Task.Delay(100);
                GameSettings.IsCloseButtonPressed = true;
                GameSettings.IsControlsScreenDrawn = false;
                GameSettings.IsCloseButtonPressed = false;

                backButtonPressed = false;
            }
        }

        private bool LeftMouseButtonPressed()
        {
            return _previousMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw frame
            Rectangle dr = new Rectangle(0, 0, (int)GameSettings.ScreenSize.X, (int)GameSettings.ScreenSize.Y);
            spriteBatch.Draw(GameSettings.ScreenTexture, dr, new Color(0, 0, 0, 128));

            spriteBatch.Draw(GameSettings.UI_Controls, UI_Controls_Rectangle, Color.White);
            spriteBatch.Draw(GameSettings.UI_player_Controls, UI_Player_Controls_Rectangle, Color.White);

            DrawCloseButton(spriteBatch);

        }

        private void DrawCloseButton(SpriteBatch spriteBatch)
        {
            Texture2D backButtonTexture = backButtonPressed ? GameSettings.Button_Pressed_Orange : GameSettings.Button_Orange;
            Rectangle dr = new Rectangle(((int)GameSettings.ScreenSize.X - backButtonWidth) / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, backButtonWidth, backButtonHeight);
            spriteBatch.Draw(backButtonTexture, dr, Color.White);

            Vector2 textPosition = Vector2.One;
            Vector2 textSize = GameSettings.GameFont.MeasureString("BACK");
            if (!backButtonPressed)
            {
                textPosition = new Vector2(dr.X + (backButtonWidth - textSize.X) / 2, dr.Y - textSize.Y + backButtonHeight / 2);
            }
            else
            {
                textPosition = new Vector2(dr.X + (backButtonWidth - textSize.X) / 2, dr.Y - textSize.Y + backButtonHeight / 2 + 16);
            }
            spriteBatch.DrawString(GameSettings.GameFont, "BACK", textPosition, Color.Black);
        }

        private Texture2D KeysButtonTexture(bool buttonPressed)
        {
            if (!buttonPressed)
            {
                return GameSettings.Button_Yellow;
            }
            else
            {
                return GameSettings.Button_Orange;
            }
        }
    }
}
