using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace WinterJam.Screens
{
    public class SettingsScreen : Screen
    {
        private Vector2 framePosition;
        private int frameWidth = 500;
        private int frameHeight = 500;

        private bool isButtonPressed1;
        private bool isButtonPressed2;
        private bool isButtonPressed3 = true;

        public bool isCloseButtonPressed = true;

        private int buttonWidth = 100; // Adjusted button width
        private int buttonHeight = 50; // Adjusted button height

        private MouseState _cMouseState, _pMouseState;
        private SpriteFont _font;

        private Vector2 circlePosition;
        //private float circleSpeed = 5.0f;

        // Slider parameters
        private Vector2 sliderPosition;
        private int sliderWidth = 200;
        private int sliderHeight = 20;
        private float sliderValue = 0.5f; // Initial volume level

        private Keys pressedKey = Keys.None; // Add this field

        private Rectangle closeButtonBounds;
        private int closeButtonSize = 30;

        public SettingsScreen(SpriteFont font)
        {
            _font = font;

            // Calculate frame position to center it on the screen
            framePosition = new Vector2((1920 - frameWidth) / 2, (1080 - frameHeight) / 2);

            // Calculate button position relative to the frame
            Vector2 buttonStartPosition = new Vector2((frameWidth - buttonWidth) / 2, (frameHeight - buttonHeight * 4) / 2); // Adjusted button positioning
            circlePosition = new Vector2(buttonStartPosition.X + buttonWidth / 2, buttonStartPosition.Y + buttonHeight / 2);

            // Calculate slider position relative to the frame
            sliderPosition = new Vector2((frameWidth - sliderWidth) / 2, frameHeight - 150); // Adjusted slider position

            // Calculate close button position relative to the frame
            closeButtonBounds = new Rectangle((int)framePosition.X + frameWidth - closeButtonSize - 10, (int)framePosition.Y + 10, closeButtonSize, closeButtonSize);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateStates();
            CheckCloseButtonPressed();
            CheckButtonPresses();
            UpdateControlKeys();
            UpdateSlider();
        }

        private void CheckCloseButtonPressed()
        {
            if (_cMouseState.LeftButton == ButtonState.Pressed && _pMouseState.LeftButton == ButtonState.Released)
            {
                if (closeButtonBounds.Contains(_cMouseState.Position))
                {
                    isCloseButtonPressed = !isCloseButtonPressed;
                }
            }
        }

        private void UpdateSlider()
        {
            Rectangle sliderHandleRect = new Rectangle((int)framePosition.X + (int)sliderPosition.X + (int)(sliderValue * sliderWidth) - 10, (int)framePosition.Y + (int)sliderPosition.Y - 5, 20, sliderHeight + 10);

            if (UserInput._currentMouseState.LeftButton == ButtonState.Pressed && sliderHandleRect.Contains(Mouse.GetState().Position))
            {
                // If the slider handle is clicked, adjust the slider value based on the mouse position
                sliderValue = MathHelper.Clamp((Mouse.GetState().Position.X - framePosition.X - sliderPosition.X) / sliderWidth, 0f, 1f);
            }
        }

        private void UpdateControlKeys()
        {
            if (isButtonPressed1)
                GameSettings.ControlKeys = (Keys.W, Keys.D, Keys.S, Keys.A);
            else if (isButtonPressed2)
                GameSettings.ControlKeys = (Keys.Z, Keys.D, Keys.S, Keys.Q);
            else if (isButtonPressed3)
                GameSettings.ControlKeys = (Keys.Up, Keys.Right, Keys.Down, Keys.Left);

            pressedKey = GetPressedKey();
        }

        private Keys GetPressedKey()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (Keyboard.GetState().IsKeyDown(key))
                {
                    return key;
                }
            }

            return Keys.None;
        }

        /*private void MoveCircleWithWASD()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                circlePosition.Y -= circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                circlePosition.X -= circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                circlePosition.Y += circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                circlePosition.X += circleSpeed;
        }

        private void MoveCircleWithZQSD()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                circlePosition.Y -= circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                circlePosition.X -= circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                circlePosition.Y += circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                circlePosition.X += circleSpeed;
        }

        private void MoveCircleWithArrows()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                circlePosition.Y -= circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                circlePosition.X -= circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                circlePosition.Y += circleSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                circlePosition.X += circleSpeed;
        }*/

        private void CheckButtonPresses()
        {
            if (CheckButtonPressed(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2), buttonWidth, buttonHeight))
            {
                isButtonPressed1 = true;
                isButtonPressed2 = false;
                isButtonPressed3 = false;
            }
            if (CheckButtonPressed(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + buttonHeight + 10), buttonWidth, buttonHeight))
            {
                isButtonPressed2 = true;
                isButtonPressed1 = false;
                isButtonPressed3 = false;
            }
            if (CheckButtonPressed(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 2 * (buttonHeight + 10)), buttonWidth, buttonHeight))
            {
                isButtonPressed3 = true;
                isButtonPressed1 = false;
                isButtonPressed2 = false;
            }
        }

        private void UpdateStates()
        {
            _pMouseState = _cMouseState;
            _cMouseState = Mouse.GetState();
        }

        private bool CheckButtonPressed(Vector2 position, int width, int height)
        {
            Rectangle newButton = new Rectangle((int)position.X, (int)position.Y, width, height);
            return LeftMouseButtonPressed() && newButton.Contains(Mouse.GetState().Position);
        }

        private bool LeftMouseButtonPressed()
        {
            return _pMouseState.LeftButton == ButtonState.Pressed && _cMouseState.LeftButton == ButtonState.Released;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw frame
            //spriteBatch.DrawRectangle((int)framePosition.X, (int)framePosition.Y, frameWidth, frameHeight, Color.Gray, Color.Black, 2);
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)framePosition.X + (int)sliderPosition.X, (int)framePosition.Y + (int)sliderPosition.Y, sliderWidth, sliderHeight), Color.Gray);

            // Draw buttons
            DrawButton(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2), buttonWidth, buttonHeight, "Qwerty", isButtonPressed1, spriteBatch);
            DrawButton(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + buttonHeight + 10), buttonWidth, buttonHeight, "Azerty", isButtonPressed2, spriteBatch);
            DrawButton(new Vector2(framePosition.X + (frameWidth - buttonWidth) / 2, framePosition.Y + (frameHeight - buttonHeight * 4) / 2 + 2 * (buttonHeight + 10)), buttonWidth, buttonHeight, "Arrows", isButtonPressed3, spriteBatch);

            // Draw the circle
            //spriteBatch.DrawCircle((int)circlePosition.X, (int)circlePosition.Y, 20, Color.Red, Color.Blue, 5);
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)circlePosition.X, (int)circlePosition.Y, 40, 40), Color.Red);


            // Draw the slider
            DrawSlider(spriteBatch);

            // Draw the close button
            DrawCloseButton(spriteBatch);

            // Draw pressed key info
            spriteBatch.DrawString(_font, "Pressed Key: " + pressedKey.ToString(), new Vector2(framePosition.X + 10, framePosition.Y + frameHeight - 50), Color.White);
        }

        private void DrawCloseButton(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangle((int)framePosition.X + frameWidth - closeButtonSize - 10, (int)framePosition.Y + 10, closeButtonSize, closeButtonSize, Color.Red, Color.Red, 1);
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)framePosition.X + frameWidth - closeButtonSize - 10, (int)framePosition.Y + 10, closeButtonSize, closeButtonSize), Color.Red);
            spriteBatch.DrawString(_font, "X", new Vector2(framePosition.X + frameWidth - closeButtonSize, framePosition.Y + 15), Color.White);
        }

        private void DrawSlider(SpriteBatch spriteBatch)
        {
            // Draw slider track
            //spriteBatch.DrawRectangle((int)framePosition.X + (int)sliderPosition.X, (int)framePosition.Y + (int)sliderPosition.Y, sliderWidth, sliderHeight, Color.Gray, Color.Black, 2);
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)framePosition.X + (int)sliderPosition.X, (int)framePosition.Y + (int)sliderPosition.Y, sliderWidth, sliderHeight), Color.Gray);


            // Draw slider handle
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)framePosition.X + (int)(sliderPosition.X + sliderValue * sliderWidth) - 10, (int)framePosition.Y + (int)sliderPosition.Y - 5, 20, sliderHeight + 10), Color.Yellow);
            //spriteBatch.DrawRectangle((int)framePosition.X + (int)(sliderPosition.X + sliderValue * sliderWidth) - 10, (int)framePosition.Y + (int)sliderPosition.Y - 5, 20, sliderHeight + 10, Color.Gold, Color.Black, 2);
        }

        private void DrawButton(Vector2 position, int width, int height, string buttonText, bool buttonPressed, SpriteBatch spriteBatch)
        {
            Vector2 textPosition = new Vector2(position.X + (width - _font.MeasureString(buttonText).X) / 2, position.Y + (height - _font.MeasureString(buttonText).Y) / 2);

            Color fillColor = GetFillColor(buttonPressed);

            //spriteBatch.DrawRectangle((int)position.X, (int)position.Y, width, height, fillColor, Color.Goldenrod, 5);
            spriteBatch.Draw(GameSettings.ScreenTexture, new Rectangle((int)position.X, (int)position.Y, width, height), fillColor);
            spriteBatch.DrawString(_font, buttonText, textPosition, Color.Black);
        }

        private Color GetFillColor(bool buttonPressed)
        {
            if (!buttonPressed)
            {
                return Color.LightGoldenrodYellow;
            }
            else
            {
                return Color.Goldenrod;
            }
        }

        public void ToggleVisibility()
        {
            isCloseButtonPressed = !isCloseButtonPressed;
        }
    }
}
