using Isometric_Thingy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSheetClass;
using System.Collections.Generic;
using WinterJam.Players;

namespace WinterJam
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Grid _grid;
        private Player _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _grid = new Grid(new Vector2(_graphics.PreferredBackBufferWidth / 2, 50),
                new List<Texture2D>() {
                    Content.Load<Texture2D>("Graphics/Blocks/grass_01"),
                    Content.Load<Texture2D>("Graphics/Blocks/grass_02"),
                    Content.Load<Texture2D>("Graphics/Blocks/grass_03"),
                    Content.Load<Texture2D>("Graphics/Blocks/grass_04"),
                });

            GameSettings.Grid = _grid;
            Vector2 playerStart = new Vector2(4, 4);
            _player = new Player(playerStart,
                new SpriteSheet(
                    Content.Load<Texture2D>("Graphics/Blocks/Rock_02"),
                    GameSettings.Grid.GetPlayerPosition(playerStart),
                    new Vector2(20, 20),
                    0,
                    1,
                    1,
                    1,
                    false
                    )
                ); 
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _grid.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}