using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpriteSheetClass
{
    public class SpriteSheet
    {
        private Texture2D texture2D;
        private Vector2 vector2;
        private object value;

        public Texture2D Texture { get; set; }
        public Vector2 CenterPosition { get; set; }
        public Vector2 Size { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; } = Color.White;
        public virtual Rectangle DestinationRectangle { get
            {
                return new Rectangle(CenterPosition.ToPoint(), Size.ToPoint());
            } 
        }
        public virtual Rectangle Hitbox { get
            {
                return DestinationRectangle;
            } 
        }
        public Rectangle SourceRectangle { get
            {
                int spriteWidth = Texture.Width/ Cols;
                int spriteHeight = Texture.Height/Rows;

                int spriteCol = CurrentSpriteIndex % Cols;
                int spriteRow = CurrentSpriteIndex / Cols;
                return new Rectangle(spriteWidth * spriteCol, spriteHeight * spriteRow, spriteWidth, spriteHeight);
            }
        }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int CurrentSpriteIndex { get; set; } = 0;
        public bool Loop { get; private set; }
        public bool IsFinished { get; set; } = false;
        public SpriteSheet(Texture2D texture, Vector2 topLeftPos, Vector2 size, float rotation, int rows, int cols, int currentSpriteIndex, bool loop)
        {
            Texture = texture;
            CenterPosition = topLeftPos + Size / 2;
            Size = size;
            Rotation = rotation;
            Rows = rows;
            Cols = cols;
            CurrentSpriteIndex = currentSpriteIndex;
            Loop = loop;
         }

        public SpriteSheet(Texture2D texture2D, Vector2 vector2, object value)
        {
            this.texture2D = texture2D;
            this.vector2 = vector2;
            this.value = value;
        }

        public virtual void Update()
        {
            if (!IsFinished)
            {
                CurrentSpriteIndex++;
                if (CurrentSpriteIndex > (Cols * Rows) - 1 && Loop)
                {
                    CurrentSpriteIndex = 0;
                }
                else if (CurrentSpriteIndex > (Cols * Rows) - 1 && !Loop)
                {
                    CurrentSpriteIndex = (Cols * Rows) - 1;
                    IsFinished = true;
                }
            }
            
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 sourceOrigin = new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);

            Rectangle drMoved = new Rectangle(DestinationRectangle.X + DestinationRectangle.Width / 2, DestinationRectangle.Y + DestinationRectangle.Height / 2, DestinationRectangle.Width, DestinationRectangle.Height);

            spriteBatch.Draw(Texture, drMoved, (Cols > 1 || Rows > 1) ? SourceRectangle : null, Color, Rotation,sourceOrigin, SpriteEffects.None, 0);
        }
        public void Play()
        {
            CurrentSpriteIndex = 0;
            IsFinished = false;
        }
    }
}
