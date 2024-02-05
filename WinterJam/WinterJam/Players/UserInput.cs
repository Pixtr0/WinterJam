using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WinterJam
{
    internal class UserInput
    {
        protected MouseState _currentMouseState, _previousMouseState;
        protected KeyboardState _currentKeyboardSate, _previousKeyboardSate;

        public void Update()
        {
            UpdateStates();
        }
        private void UpdateStates()
        {
            _previousKeyboardSate = _currentKeyboardSate;
            _previousMouseState = _currentMouseState;

            _currentKeyboardSate = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
        }
    public  Boolean IsleftMouseClicked
        {
            get
            {
                return _previousMouseState.LeftButton == ButtonState.Pressed &&
                    _currentMouseState.LeftButton == ButtonState.Released;
            }
        }
        public bool IsKeyPressed
        {
            get
            {
                return _currentKeyboardSate.GetPressedKeyCount() == 1 &&
                    _previousKeyboardSate.GetPressedKeyCount() == 0;
            }
        }
        public Keys PressedKey
        {
            get
            {
                return _currentKeyboardSate.GetPressedKeys()[0];
            }
        }
        public Vector2 GetNextPosition(Vector2 currentPosition)
        {
            Vector2 nextPosition = currentPosition;
            if (IsKeyPressed)
            {
                if (PressedKey == Keys.W || PressedKey == Keys.Up)
                    nextPosition -= new Vector2(0, 1);
                if (PressedKey == Keys.A || PressedKey == Keys.Left)
                    nextPosition -= new Vector2(1, 0);
                if (PressedKey == Keys.S || PressedKey == Keys.Down)
                    nextPosition += new Vector2(0, 1);
                if (PressedKey == Keys.D || PressedKey == Keys.Right)
                    nextPosition += new Vector2(1, 0);
            }
            if (nextPosition.X < 0 || nextPosition.Y < 0 || nextPosition.Y > Grid.Rows - 1 || nextPosition.X > Grid.Columns - 1)
            {
                return currentPosition;
            }
            else { return nextPosition; }
        }
    }
}

