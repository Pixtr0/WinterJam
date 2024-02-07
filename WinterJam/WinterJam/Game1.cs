﻿using Isometric_Thingy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinterJam.Players;
using WinterJam.Screens;

namespace WinterJam
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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
            GameSettings.ActiveScreen = new StartScreen();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameSettings.ScreenTexture = Content.Load<Texture2D>("Graphics/Blocks/placeholder Screen");

            GameSettings.Grid = new Grid(new Vector2(_graphics.PreferredBackBufferWidth / 2, 0)
            , new List<Texture2D>() {
                Content.Load<Texture2D>("Graphics/Blocks/grass_01"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_02"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_03"),
                Content.Load<Texture2D>("Graphics/Blocks/grass_04"),
            }, new List<Texture2D>() {
                Content.Load<Texture2D>("Graphics/Blocks/flower_01"),
                Content.Load<Texture2D>("Graphics/Blocks/flower_02"),
                Content.Load<Texture2D>("Graphics/Blocks/flower_03"),
            });
            
            GameSettings.GameFont = Content.Load<SpriteFont>("Graphics/Fonts/GameDisplay");
            GameSettings.UiFont = Content.Load<SpriteFont>("Graphics/Fonts/UI");
            PlayScreen._obstacleTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Graphics/Blocks/bush_01"),
                Content.Load<Texture2D>("Graphics/Blocks/bush_02"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_01"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_02"),
                Content.Load<Texture2D>("Graphics/Blocks/rocks_03"),
                Content.Load<Texture2D>("Graphics/Blocks/stump_01"),
                Content.Load<Texture2D>("Graphics/Blocks/stump_02"),
                Content.Load<Texture2D>("Graphics/Blocks/log_01"),
                Content.Load<Texture2D>("Graphics/Blocks/log_02")
            };
            Player.Animations.Add(new SpriteSheet(Content.Load<Texture2D>("Graphics/Player/Up"),Vector2.Zero,new Vector2(36,32) * GameSettings.Grid.ScaleFactor,0,1,4,0, true));
            Player.Animations.Add(new SpriteSheet(Content.Load<Texture2D>("Graphics/Player/Right"), Vector2.Zero, new Vector2(36, 32) * GameSettings.Grid.ScaleFactor, 0, 1, 4, 0, true));
            Player.Animations.Add(new SpriteSheet(Content.Load<Texture2D>("Graphics/Player/Down"), Vector2.Zero, new Vector2(36, 32) * GameSettings.Grid.ScaleFactor, 0, 1, 4, 0, true));
            Player.Animations.Add(new SpriteSheet(Content.Load<Texture2D>("Graphics/Player/Left"), Vector2.Zero, new Vector2(36, 32) * GameSettings.Grid.ScaleFactor, 0, 1, 4, 0, true));
            Vector2 playerStart = new Vector2(8, 9);
            PlayScreen._player = new Player(playerStart, Player.Animations[3]);
            GameSettings.Squirrel_Up = Content.Load<Texture2D>("Graphics/Enemy/up");
            GameSettings.Squirrel_Down = Content.Load<Texture2D>("Graphics/Enemy/down");
            GameSettings.Squirrel_Left = Content.Load<Texture2D>("Graphics/Enemy/left");
            GameSettings.Squirrel_Right = Content.Load<Texture2D>("Graphics/Enemy/right");
            GameSettings.Squirrel_Up_Left = Content.Load<Texture2D>("Graphics/Enemy/up_left");
            GameSettings.Squirrel_Up_Right = Content.Load<Texture2D>("Graphics/Enemy/up_right");
            GameSettings.Squirrel_Down_Left = Content.Load<Texture2D>("Graphics/Enemy/down_left");
            GameSettings.Squirrel_Down_Right = Content.Load<Texture2D>("Graphics/Enemy/down_right");
            PlayScreen._house = new House(Content.Load<Texture2D>("Graphics/Blocks/spritesheet_house"));
            
            PlayScreen._enemies.Add(Enemy.Spawn());

            foreach (var Object in PlayScreen._allObjects)
            {
                Debug.WriteLine(Object.anchorPoint.Y);
            }

            GameSettings.StartScreen = new StartScreen() { Texture = GameSettings.ScreenTexture };
            GameSettings.PlayScreen = new PlayScreen();
            GameSettings.SettingsScreen = new SettingsScreen();
            GameSettings.ActiveScreen = GameSettings.StartScreen;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameSettings.ActiveScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw only the active screen
            GameSettings.ActiveScreen.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}