using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam.Players
{
    internal class Player : GameObject
    {
        /*
         * Required:
         * Inventory
         * Movement
         * Interactions:
         */

        public List<Item> Inventory {get; set;} = new List<Item>();

        public override void Update(GameTime gameTime)
        {
            UpdatePlayerPosition();
            base.Update(gameTime);
        }

        private void UpdatePlayerPosition()
        {
        }
    }
}
