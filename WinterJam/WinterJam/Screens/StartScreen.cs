using Microsoft.Xna.Framework;
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
        public UserInput UserInput {  get; set; }
        public override void Update(GameTime gameTime)
        {
            UserInput.Update();
            UpdateActiveScreen(gameTime);
            base.Update(gameTime);
        }

        private void UpdateActiveScreen(GameTime gameTime)
        {
            if (UserInput.IsleftMouseClicked)
            {
                GameSettings.PlayScreen = new PlayScreen();
                GameSettings.ActiveScreen = GameSettings.PlayScreen;
            }
        }
    }
}
