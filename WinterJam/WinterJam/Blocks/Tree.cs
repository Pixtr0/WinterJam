using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteSheetClass;

namespace WinterJam
{
    public class Tree : GameObject
    {
        public override Vector2 anchorPoint { get { return base.anchorPoint - new Vector2(0, 20 * GameSettings.Grid.ScaleFactor); } }
        public static List<Texture2D> TreeTextures { get; set; } = new List<Texture2D>();
        public Tree(Vector2 Position) 
        {
            Texture2D texture = TreeTextures[Random.Shared.Next(0, TreeTextures.Count)];
            Vector2 size = new Vector2((int)(texture.Width / 5), (int)(texture.Height / 5));
            //Vector2 offset = new Vector2((int)((texture.Width / 5) - GameSettings.Grid.TileSize.Y)/2,
            //    GameSettings.Grid.TileSize.Y + (int)(texture.Height / 5)) * GameSettings.Grid.ScaleFactor;
            Vector2 offset = new Vector2(size.X - GameSettings.Grid.TileSize.Y ,
                 size.Y - GameSettings.Grid.TileSize.Y) ;
            Visualisation = new SpriteSheet(texture, GameSettings.Grid.GetGridPositionNoHeight(new Vector2(Position.X - 11f,Position.Y -6)) - offset + Vector2.One * 20 * GameSettings.Grid.ScaleFactor, size * GameSettings.Grid.ScaleFactor, 0,1,1,0,false);
        } 
    }
}
