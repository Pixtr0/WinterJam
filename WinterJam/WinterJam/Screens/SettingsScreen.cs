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
        private Rectangle SettingsTitleRectangle
        {
            //79 x 15
            get
            {
                return new Rectangle((int)(GameSettings.ScreenSize.X / 2 - 79 * GameSettings.Grid.ScaleFactor / 2 ),
                                     (int)(GameSettings.ScreenSize.Y / 4 - buttonHeight),
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

        private Vector2 offset1 = new Vector2(GameSettings.ScreenSize.X / 6, -50);
        private Vector2 offset2 = new Vector2(GameSettings.ScreenSize.X / 7, -150);

        private int buttonWidth = (int)(68 * 4)  ; // Adjusted button width
        private int buttonHeight = (int)(21 * 4) ; // Adjusted button height

        private int backButtonWidth = (int)(69 * 4);
        private int backButtonHeight = (int)(21 * 4);
        private bool backButtonPressed = false;

        private SpriteFont _font = GameSettings.GameFont;

        // Slider parameters
        private Vector2 sliderPosition;
        private int sliderWidth = (int)(68 * 4);
        private int sliderHeight = (int)(21 * 4);
        private float sliderValue = 0.5f; // Initial volume level
        private bool isSliderHeld = false;

        // Store current and previous mouse states
        private MouseState _currentMouseState, _previousMouseState;

        public SettingsScreen()
        {
            // Calculate frame position to center it on the screen
            framePosition = new Vector2((GameSettings.ScreenSize.X - frameWidth) / 2, (GameSettings.ScreenSize.Y - frameHeight) / 2);

            // Calculate slider position relative to the frame
            sliderPosition = new Vector2((frameWidth - sliderWidth) / 2, frameHeight - 150);
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
            if (GameSettings.IsSettingsScreenDrawn && UserInput._currentMouseState.LeftButton == ButtonState.Pressed && UserInput._previousMouseState.LeftButton == ButtonState.Released &&
                backButtonRect.Contains(UserInput._currentMouseState.Position))
            {
                GameSettings.SFX_Button.Play();
                // Enable the settings screen

                backButtonPressed = true;
                await Task.Delay(100);
                GameSettings.IsCloseButtonPressed = true;
                GameSettings.IsSettingsScreenDrawn = false;
                GameSettings.IsCloseButtonPressed = false;

                backButtonPressed = false;
            }
        }

        private void UpdateSlider()
        {
            Rectangle sliderHandleRect = new Rectangle((int)offset2.X + (int)framePosition.X + (int)sliderPosition.X + (int)(sliderValue * sliderWidth) - 10, (int)offset2.Y + (int)framePosition.Y + (int)sliderPosition.Y - 5, 20, sliderHeight + 10);

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
                sliderValue = MathHelper.Clamp((_currentMouseState.Position.X - framePosition.X - offset2.X - sliderPosition.X) / sliderWidth, 0f, 1f);
            }
            GameSettings.VolumeValue = sliderValue;
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
            if (CheckButtonPressed(-offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 65, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 60), buttonWidth, buttonHeight))
            {
                GameSettings.SFX_Button.Play();
                isQwertyButtonPressed = true;
                isAzertyButtonPressed = false;
                isArrowsButtonPressed = false;
            }
            else if (CheckButtonPressed(-offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 65, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 60 + buttonHeight + 10), buttonWidth, buttonHeight))
            {
                GameSettings.SFX_Button.Play();
                isQwertyButtonPressed = false;
                isAzertyButtonPressed = true;
                isArrowsButtonPressed = false;
            }
            else if (CheckButtonPressed(-offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 70, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 60 + 2 * (buttonHeight + 10)), buttonWidth, buttonHeight))
            {
                GameSettings.SFX_Button.Play();
                isQwertyButtonPressed = false;
                isAzertyButtonPressed = false;
                isArrowsButtonPressed = true;
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
            Rectangle dr = new Rectangle(0,0, (int)GameSettings.ScreenSize.X, (int)GameSettings.ScreenSize.Y);
            spriteBatch.Draw(GameSettings.ScreenTexture, dr, new Color(0,0,0,128));

            spriteBatch.Draw(GameSettings.UI_Settings, SettingsTitleRectangle, Color.White);

            // Draw Buttons Text
            spriteBatch.DrawString(GameSettings.GameFont, "Keybinds", -offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 96, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 130), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Keybinds", -offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 104, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 130), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Keybinds", -offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 100, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 126), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Keybinds", -offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 100, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 134), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Keybinds", -offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 100, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 130), Color.Black);

            //draw slider text
            spriteBatch.DrawString(GameSettings.GameFont, "Audio", new Vector2(offset2.X - 25 +  framePosition.X + (frameWidth - buttonWidth) / 2 + 96, -offset2.Y - 80 + framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 20), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Audio", new Vector2(offset2.X - 25 + framePosition.X + (frameWidth - buttonWidth) / 2 + 104, -offset2.Y - 80 + framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 20), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Audio", new Vector2(offset2.X - 25+ framePosition.X + (frameWidth - buttonWidth) / 2 + 100, -offset2.Y - 80 + framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 16), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Audio", new Vector2(offset2.X - 25 + framePosition.X + (frameWidth - buttonWidth) / 2 + 100, -offset2.Y - 80 + framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 24), Color.White);
            spriteBatch.DrawString(GameSettings.GameFont, "Audio", new Vector2(offset2.X - 25 + framePosition.X + (frameWidth - buttonWidth) / 2 + 100, -offset2.Y - 80 + framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 20), Color.Black);

            // Draw buttons
            DrawButton(-offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 65, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 60), buttonWidth, buttonHeight, "Qwerty", isQwertyButtonPressed, spriteBatch);
            DrawButton(-offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 65, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 60 + buttonHeight + 10), buttonWidth, buttonHeight, "Azerty", isAzertyButtonPressed, spriteBatch);
            DrawButton(-offset1 + new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2 + 65, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 - 60 + 2 * (buttonHeight + 10)), buttonWidth, buttonHeight, "Arrows", isArrowsButtonPressed, spriteBatch);

            // Draw the slider
            DrawSlider(spriteBatch, offset2);

            // Draw the close button
            
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

        private void DrawSlider(SpriteBatch spriteBatch,Vector2 offset)
        {
            // Draw slider track
            spriteBatch.Draw(GameSettings.UI_Volume, new Rectangle((int)offset.X + (int)framePosition.X + (int)sliderPosition.X, (int)offset.Y + (int)framePosition.Y + (int)sliderPosition.Y, sliderWidth, sliderHeight), Color.Gray);
            spriteBatch.Draw(GameSettings.UI_Volume,
                new Rectangle((int)offset.X + (int)(framePosition.X + sliderPosition.X) - 1, (int)offset.Y + (int)(framePosition.Y + sliderPosition.Y), sliderWidth , sliderHeight),
                new Rectangle((int)(GameSettings.UI_Volume.Width * sliderValue - GameSettings.UI_Volume.Width) , 0, GameSettings.UI_Volume.Width, GameSettings.UI_Volume.Height), Color.Orange, 0,
                new Vector2((int)((GameSettings.UI_Volume.Width / 2) - (GameSettings.UI_Volume.Width * sliderValue - GameSettings.UI_Volume.Width / 2)), 0), SpriteEffects.None, 0);

            // Draw slider handle
            spriteBatch.Draw(GameSettings.UI_Volume_slider, new Rectangle(
                (int)offset.X + (int)framePosition.X + (int)(sliderPosition.X + sliderValue * (sliderWidth - (int)(4 * GameSettings.Grid.ScaleFactor))),
                (int)offset.Y + (int)framePosition.Y + (int)sliderPosition.Y + (int)(7 * GameSettings.Grid.ScaleFactor), 
                (int)(4f * GameSettings.Grid.ScaleFactor), 
                (int)(15f * GameSettings.Grid.ScaleFactor))
                , Color.Yellow);
        }

        private void DrawButton(Vector2 position, int width, int height, string buttonText, bool buttonPressed, SpriteBatch spriteBatch)
        {
            Vector2 textPosition = new Vector2(position.X + (width - _font.MeasureString(buttonText).X) / 2, position.Y + (height - _font.MeasureString(buttonText).Y) / 3);
            Vector2 pressedButtonTextPosition = new Vector2(textPosition.X, textPosition.Y - 7);

            Texture2D buttonTexture = KeysButtonTexture(buttonPressed);
            
            spriteBatch.Draw(buttonTexture, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
            spriteBatch.DrawString(_font, buttonText, textPosition, Color.Black);
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
