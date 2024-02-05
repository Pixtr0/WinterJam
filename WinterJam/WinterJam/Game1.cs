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
        private Player _player;

        private List<Enemy> Enemies = new List<Enemy>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = (int)GameSettings.ScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)GameSettings.ScreenSize.Y;
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

            GameSettings.Grid = new Grid(new Vector2(_graphics.PreferredBackBufferWidth / 2, 0)
            , new List<Texture2D>() {
                Content.Load<Texture2D>("Graphics/Blocks/grass_01"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_02"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_03"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_04"),
            }, new List<Texture2D>()
            {
                Content.Load<Texture2D>("Graphics/Blocks/bush_01"),
                Content.Load<Texture2D>("Graphics/Blocks/bush_02"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_01"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_02"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_03"),
                Content.Load<Texture2D>("Graphics/Blocks/stump_01"),
                Content.Load<Texture2D>("Graphics/Blocks/stump_02")
            });
            Vector2 playerStart = new Vector2(4, 4);
            _player = new Player(playerStart,
                new SpriteSheet(
                    Content.Load<Texture2D>("Graphics/Player/Player"),
                    GameSettings.Grid.GetPlayerPosition(playerStart),
                    new Vector2(80, 80),
                    0,
                    1,
                    1,
                    1,
                    false
                    )
                );
            GameSettings.Squirrel_Up = Content.Load<Texture2D>("Graphics/Enemy/up");
            GameSettings.Squirrel_Down = Content.Load<Texture2D>("Graphics/Enemy/down");
            GameSettings.Squirrel_Left = Content.Load<Texture2D>("Graphics/Enemy/left");
            GameSettings.Squirrel_Right = Content.Load<Texture2D>("Graphics/Enemy/right");
            GameSettings.Squirrel_Up_Left = Content.Load<Texture2D>("Graphics/Enemy/up_left");
            GameSettings.Squirrel_Up_Right = Content.Load<Texture2D>("Graphics/Enemy/up_right");
            GameSettings.Squirrel_Down_Left = Content.Load<Texture2D>("Graphics/Enemy/down_left");
            GameSettings.Squirrel_Down_Right = Content.Load<Texture2D>("Graphics/Enemy/down_right");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameSettings.Grid.Update();
            UserInput.Update();
            _player.Update(gameTime);

            if(UserInput._currentKeyboardSate.IsKeyDown(Keys.Enter) && UserInput._previousKeyboardSate.IsKeyUp(Keys.Enter))
            {
                Enemies.Add(Enemy.Spawn());
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            GameSettings.Grid.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}