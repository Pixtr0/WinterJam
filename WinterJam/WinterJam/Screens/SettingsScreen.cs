using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinterJam.Screens
{
    public class SettingsScreen : Screen
    {
        private Vector2 framePosition;
        private int frameWidth = 500;
        private int frameHeight = 500;

        private bool isQwertyButtonPressed = true;
        private bool isAzertyButtonPressed;
        private bool isArrowsButtonPressed;

        private int buttonWidth = 200; // Adjusted button width
        private int buttonHeight = 50; // Adjusted button height

        private int backButtonWidth = (int)(68 * 6);
        private int backButtonHeight = (int)(21 * 6);
        private bool backButtonPressed = false;

        private SpriteFont _font = GameSettings.GameFont;

        // Slider parameters
        private Vector2 sliderPosition;
        private int sliderWidth = 200;
        private int sliderHeight = 20;
        private float sliderValue = 0.5f; // Initial volume level
        private bool isSliderHeld = false;

        //private Rectangle closeButtonBounds;
        //private int closeButtonSize = 40;

        // Store current and previous mouse states
        private MouseState _currentMouseState, _previousMouseState;

        public SettingsScreen()
        {
            //_font = GameSettings.GameFont;

            // Calculate frame position to center it on the screen
            framePosition = new Vector2((GameSettings.ScreenSize.X - frameWidth) / 2, (GameSettings.ScreenSize.Y - frameHeight) / 2);

            // Calculate slider position relative to the frame
            sliderPosition = new Vector2((frameWidth - sliderWidth) / 2, frameHeight - 150); // Adjusted slider position

            // Calculate close button position relative to the frame
            //closeButtonBounds = new Rectangle((int)framePosition.X + frameWidth - closeButtonSize - 10, (int)framePosition.Y + 10, closeButtonSize, closeButtonSize);
        }

        public override void Update(GameTime gameTime)
        {
            // Update mouse states
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // Check for button presses and slider updates
            CheckCloseButtonPressed();
            CheckButtonPresses();
            UpdateControlKeys();
            UpdateSlider();
        }

        private async void CheckCloseButtonPressed()
        {
            Rectangle backButtonRect = new Rectangle(((int)GameSettings.ScreenSize.X - backButtonWidth) / 2, (int)GameSettings.ScreenSize.Y * 3 / 4, backButtonWidth, backButtonHeight);
            if (!GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                backButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                // Enable the settings screen
                backButtonPressed = true;
                await Task.Delay(100);
                backButtonPressed = false;
            }
            if (LeftMouseButtonPressed() && backButtonRect.Contains(Mouse.GetState().Position))
            {
                GameSettings.IsCloseButtonPressed = true;
                GameSettings.IsSettingsScreenDrawn = false;
                GameSettings.IsCloseButtonPressed = false;
            }
        }

        private void UpdateSlider()
        {
            Rectangle sliderHandleRect = new Rectangle((int)framePosition.X + (int)sliderPosition.X + (int)(sliderValue * sliderWidth) - 10, (int)framePosition.Y + (int)sliderPosition.Y - 5, 20, sliderHeight + 10);

            if (_currentMouseState.LeftButton == ButtonState.Pressed && sliderHandleRect.Contains(_currentMouseState.Position))
            {
                isSliderHeld = true;
            }
            if (isSliderHeld)
            {
                if (_currentMouseState.LeftButton == ButtonState.Released)
                {
                    isSliderHeld = false;
                }
                // If the slider handle is clicked, adjust the slider value based on the mouse position
                sliderValue = MathHelper.Clamp((_currentMouseState.Position.X - framePosition.X - sliderPosition.X) / sliderWidth, 0f, 1f);
            }
        }

        private void UpdateControlKeys()
        {
            if (isQwertyButtonPressed)
                GameSettings.ControlKeys = (Keys.A, Keys.D, Keys.W, Keys.S);
            else if (isAzertyButtonPressed)
                GameSettings.ControlKeys = (Keys.Q, Keys.D, Keys.Z, Keys.S);
            else if (isArrowsButtonPressed)
                GameSettings.ControlKeys = (Keys.Left, Keys.Right, Keys.Up, Keys.Down);
        }

        private void CheckButtonPresses()
        {
            if (CheckButtonPressed(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2), buttonWidth, buttonHeight))
            {
                isQwertyButtonPressed = true;
                isAzertyButtonPressed = false;
                isArrowsButtonPressed = false;
            }
            if (CheckButtonPressed(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + buttonHeight + 10), buttonWidth, buttonHeight))
            {
                isAzertyButtonPressed = true;
                isQwertyButtonPressed = false;
                isArrowsButtonPressed = false;
            }
            if (CheckButtonPressed(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 2 * (buttonHeight + 10)), buttonWidth, buttonHeight))
            {
                isArrowsButtonPressed = true;
                isQwertyButtonPressed = false;
                isAzertyButtonPressed = false;
            }
        }

        private bool CheckButtonPressed(Vector2 position, int width, int height)
        {
            Rectangle newButton = new Rectangle((int)position.X, (int)position.Y, width, height);
            return LeftMouseButtonPressed() && newButton.Contains(_currentMouseState.Position);
        }

        private bool LeftMouseButtonPressed()
        {
            return _previousMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw frame
            //spriteBatch.Draw(GameSettings.Button_Pressed_Orange, new Rectangle((int)framePosition.X, (int)framePosition.Y - frameHeight / 5, frameWidth, frameHeight * 4/3), new Color(128, 128, 128, 128));
            Rectangle dr = new Rectangle(0,0, (int)GameSettings.ScreenSize.X, (int)GameSettings.ScreenSize.Y);
            spriteBatch.Draw(GameSettings.ScreenTexture, dr, new Color(0,0,0,128));

            // Draw buttons
            DrawButton(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2), buttonWidth, buttonHeight, "Qwerty", isQwertyButtonPressed, spriteBatch);
            DrawButton(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + buttonHeight + 10), buttonWidth, buttonHeight, "Azerty", isAzertyButtonPressed, spriteBatch);
            DrawButton(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 2 * (buttonHeight + 10)), buttonWidth, buttonHeight, "Arrows", isArrowsButtonPressed, spriteBatch);

            // Draw the slider
            DrawSlider(spriteBatch);

            // Draw the close button
            DrawCloseButton(spriteBatch);
        }

        private void DrawCloseButton(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(GameSettings.Button_Orange, closeButtonBounds, Color.White);
            //spriteBatch.DrawString(_font, "X", new Vector2(framePosition.X + frameWidth - closeButtonSize, framePosition.Y + 10), Color.White);

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

        private void DrawSlider(SpriteBatch spriteBatch)
        {
            // Draw slider track
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)framePosition.X + (int)sliderPosition.X, (int)framePosition.Y + (int)sliderPosition.Y, sliderWidth, sliderHeight), Color.Gray);

            // Draw slider handle
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)framePosition.X + (int)(sliderPosition.X + sliderValue * sliderWidth) - 10, (int)framePosition.Y + (int)sliderPosition.Y - 5, 20, sliderHeight + 10), Color.Yellow);
        }

        private void DrawButton(Vector2 position, int width, int height, string buttonText, bool buttonPressed, SpriteBatch spriteBatch)
        {
            Vector2 textPosition = new Vector2(position.X + (width - _font.MeasureString(buttonText).X) / 2, position.Y + (height - _font.MeasureString(buttonText).Y) / 2);
            Vector2 pressedButtonTextPosition = new Vector2(textPosition.X, textPosition.Y - 7);

            Texture2D buttonTexture = ButtonTexture(buttonPressed);
            Vector2 buttonTextPosition = GetButtonTextPosition(textPosition, pressedButtonTextPosition, buttonPressed);

            spriteBatch.Draw(buttonTexture, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
            spriteBatch.DrawString(_font, buttonText, buttonTextPosition, Color.Black);
        }

        private Texture2D ButtonTexture(bool buttonPressed)
        {
            if (!buttonPressed)
            {
                return GameSettings.Button_Yellow;
            }
            else
            {
                return GameSettings.Button_Pressed_Yellow;
            }
        }

        private Vector2 GetButtonTextPosition (Vector2 textPosition, Vector2 pressedButtonTextPosition, bool buttonPressed)
        {
            if (buttonPressed)
            {
                return textPosition;
            }
            else
            {
                return pressedButtonTextPosition;
            }
        }
    }
}
