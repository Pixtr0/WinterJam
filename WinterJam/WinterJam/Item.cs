using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterJam.Screens;

namespace WinterJam
{
    public class Item : GameObject
    {
        
        public int MaxDurability { get; set; }
        public Double ItemAngle { get; set; } = 0f;
        public float ItemOffset { get; set; } = 0f;
        public Vector2 ParentSize { get; set; }
        public static List<Texture2D> ItemTextures { get; set; } = new List<Texture2D>();

        private const float _delay = 0.2f; // seconds
        private float _remainingDelay = _delay;
        public Item(GameObject parent)
        {
            ParentSize = parent.Size;
            CurrentPosition = parent.CurrentPosition;

            Vector2 size = new Vector2(18, 19) * GameSettings.Grid.ScaleFactor;
            Texture2D texture = ItemTextures[Random.Shared.Next(0, ItemTextures.Count)];

            Visualisation = new SpriteSheet(texture, parent.anchorPoint - new Vector2(size.X/2, 0), size, 0, 1, 1, 1, false) ;
        }

        public Item()
        {
            CurrentPosition = PlayScreen.GetRandomPositionOnGrid();

            Vector2 size = new Vector2(18, 19) * GameSettings.Grid.ScaleFactor;
            Texture2D texture = ItemTextures[Random.Shared.Next(0, ItemTextures.Count)];

            Visualisation = new SpriteSheet(texture, GameSettings.Grid.GetGridPosition(CurrentPosition) +  new Vector2(-size.X/2, -size.Y), size, 0, 1, 1, 1, false);
        }
        public Item(Vector2 position, SpriteSheet visualisationOld)
        {
            CurrentPosition = position; 
            Visualisation = new SpriteSheet(visualisationOld.Texture, GameSettings.Grid.GetGridPosition(position) + new Vector2(-visualisationOld.Size.X / 2, -visualisationOld.Size.Y), visualisationOld.Size, 0, 1, 1, 1, false);
        }
       
        //Bob above gameobject
        public void Update(GameTime gametime, GameObject parent)
        {
            UpdateDelay(gametime);
            Size = new Vector2(18, 19) * GameSettings.Grid.ScaleFactor;
            TopLeftPosition = parent.anchorPoint + new Vector2(-Size.X / 2, -ParentSize.Y - 10 - Size.Y - ItemOffset * 4);
        }

        private void UpdateDelay(GameTime gametime)
        {
            var timer = (float)gametime.ElapsedGameTime.TotalSeconds;
            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                ItemAngle++;

                if (ItemAngle >= 360f)
                    ItemAngle = 1f;

                ItemOffset = (float)
                    Math.Sin(ItemAngle);

                _remainingDelay = _delay;
            }
        }
        //overload: bobs around currentposition
        public override void Update(GameTime gameTime)
        {
            UpdateDelay(gameTime);

            TopLeftPosition = GameSettings.Grid.GetGridPosition(CurrentPosition)
                             + new Vector2(12, 8) * GameSettings.Grid.ScaleFactor 
                             + new Vector2(-Size.X / 2, -13* GameSettings.Grid.ScaleFactor - ItemOffset * 4 + 10);

            base.Update(gameTime);
        }

       
    }
}
