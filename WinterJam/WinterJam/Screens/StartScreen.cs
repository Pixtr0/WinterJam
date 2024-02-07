using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam.Screens
{
    internal class StartScreen : TextureScreen
    {
        public override void Update(GameTime gameTime)
        {
            UserInput.Update();
            UpdateActiveScreen(gameTime);
            base.Update(gameTime);
        }

        private void UpdateActiveScreen(GameTime gameTime)
        {
            //UserInput._currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && UserInput._previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed
            if (UserInput._currentMouseState.RightButton == ButtonState.Pressed && 
                UserInput._previousMouseState.RightButton == ButtonState.Released)
            {
                GameSettings.ActiveScreen = GameSettings.PlayScreen;
            }
        }
    }
}
