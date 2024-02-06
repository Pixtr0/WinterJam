using Microsoft.Xna.Framework.Input;
using System;

namespace WinterJam
{
    public class UserInput
    {
        public static MouseState _currentMouseState, _previousMouseState;
        public static KeyboardState _currentKeyboardSate, _previousKeyboardSate;

        public static void Update()
        {
            UpdateStates();
        }
        private static void UpdateStates()
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
        
        public bool IsKeyDown
        {
            get
            {
                return _currentKeyboardSate.GetPressedKeyCount() >= 1;
            }
        }
        
        }
    }


