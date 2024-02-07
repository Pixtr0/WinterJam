using Microsoft.Xna.Framework.Input;
using System;

namespace WinterJam
{
    public static class UserInput
    {
        public static MouseState _currentMouseState, _previousMouseState;
        public static KeyboardState _currentKeyboardSate, _previousKeyboardSate;

        public static void Update()
        {
            _previousKeyboardSate = _currentKeyboardSate;
            _previousMouseState = _currentMouseState;

            _currentKeyboardSate = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
        }



    }
}


