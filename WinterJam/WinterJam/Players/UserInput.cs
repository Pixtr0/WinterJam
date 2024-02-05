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
        
        }
    }


