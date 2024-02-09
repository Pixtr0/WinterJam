
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

            GameSettings.StartScreen = new StartScreen();
            GameSettings.ActiveScreen = GameSettings.StartScreen;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Enemy.Textures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Graphics/Enemy/up"),
                Content.Load<Texture2D>("Graphics/Enemy/up_right"),
                Content.Load<Texture2D>("Graphics/Enemy/right"),
                Content.Load<Texture2D>("Graphics/Enemy/down_right"),
                Content.Load<Texture2D>("Graphics/Enemy/down"),
                Content.Load<Texture2D>("Graphics/Enemy/down_left"),
                Content.Load<Texture2D>("Graphics/Enemy/left"),
                Content.Load<Texture2D>("Graphics/Enemy/up_left"),
                Content.Load<Texture2D>("Graphics/Enemy/squirrel_smacked"),
            };
            GameSettings.ScreenTexture = Content.Load<Texture2D>("Graphics/Blocks/placeholder Screen");
            Tree.TreeTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Graphics/Blocks/tree_01"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_02"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_03"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_04"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_05"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_06"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_07"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_08"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_09"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_10"),
                Content.Load<Texture2D>("Graphics/Blocks/tree_11"),
            };
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
                Content.Load<Texture2D>("Graphics/Blocks/flower_04"),
                Content.Load<Texture2D>("Graphics/Blocks/flower_05"),
                Content.Load<Texture2D>("Graphics/Blocks/flower_06"),
            });
            
            GameSettings.GameFont = Content.Load<SpriteFont>("Graphics/Fonts/GameDisplay");
            GameSettings.UiFont = Content.Load<SpriteFont>("Graphics/Fonts/UI");
            PlayScreen.ObstacleTextures = new List<Texture2D>()
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
            Player.Animations.Add(new SpriteSheet(Content.Load<Texture2D>("Graphics/Player/Swing_Down"), Vector2.Zero, new Vector2(42, 48) * GameSettings.Grid.ScaleFactor, 0, 1, 5, 0, false));
            Player.Animations.Add(new SpriteSheet(Content.Load<Texture2D>("Graphics/Player/Swing_Left"), Vector2.Zero, new Vector2(42, 48) * GameSettings.Grid.ScaleFactor, 0, 1, 5, 0, false));
            GameSettings.SwingEffect = Content.Load<Texture2D>("Graphics/Player/ground_hit");
            Vector2 playerStart = new Vector2(3, 4);
            PlayScreen.Player = new Player(playerStart, Player.Animations[3]);
            
            
            PlayScreen.House = new House(Content.Load<Texture2D>("Graphics/Blocks/spritesheet_house"));

            GameSettings.PausedText = Content.Load<Texture2D>("Graphics/Buttons/UI_paused");
            GameSettings.Button_Yellow = Content.Load<Texture2D>("Graphics/Buttons/UI_button_01");
            GameSettings.Button_Pressed_Yellow = Content.Load<Texture2D>("Graphics/Buttons/UI_button_pressed_01");
            GameSettings.Button_Orange = Content.Load<Texture2D>("Graphics/Buttons/UI_button_02");
            GameSettings.Button_Pressed_Orange = Content.Load<Texture2D>("Graphics/Buttons/UI_button_pressed_02");
            GameSettings.UI_Volume = Content.Load<Texture2D>("Graphics/Buttons/UI__volume");
            GameSettings.UI_Volume_slider = Content.Load<Texture2D>("Graphics/Buttons/UI_volume_slider");
            GameSettings.UI_game_over = Content.Load<Texture2D>("Graphics/Buttons/UI_game_over");
            GameSettings.UI_player_Controls = Content.Load<Texture2D>("Graphics/Buttons/UI_player_Controls");
            GameSettings.UI_Controls = Content.Load<Texture2D>("Graphics/Buttons/UI_Controls");
            GameSettings.UI_Settings = Content.Load<Texture2D>("Graphics/Buttons/UI_Settings");
            GameSettings.UI_Background_GameOver = Content.Load<Texture2D>("Graphics/Buttons/UI_Background_GameOver");
            House.HealthBarTexture = Content.Load<Texture2D>("Graphics/Buttons/UI_health_bar");
            House.HealthBarHPTexture = Content.Load<Texture2D>("Graphics/Buttons/UI_health_bar_HP");

            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_donut"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_keys"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_sock"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_toiletPaper"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_toothbrush"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_underwear"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_beanie"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_knife"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_mug"));
            Item.ItemTextures.Add(Content.Load<Texture2D>("Graphics/Items/item_plant"));

            GameSettings.BasketTexture = Content.Load<Texture2D>("Graphics/Blocks/basket");

            //loading screens
            GameSettings.SettingsScreen = new SettingsScreen();
            GameSettings.PauseScreen = new PauseScreen();
            GameSettings.PlayScreen = new PlayScreen();
            GameSettings.ControlsScreen = new ControlsScreen();
            GameSettings.GameOverScreen = new GameOverScreen();

            GameSettings.StartScreenTexture = Content.Load<Texture2D>("Graphics/Buttons/UI_background_title");

            GameSettings.GameMusic = Content.Load<Song>("Sounds/Winterjam_jam");
            GameSettings.SFX_GameOver = Content.Load<Song>("Sounds/GameOver");
            GameSettings.SFX_Smack = Content.Load<SoundEffect>("Sounds/Smack");
            GameSettings.SFX_PlaceItem = Content.Load<SoundEffect>("Sounds/SFX_PlaceItem");
            GameSettings.SFX_Run = Content.Load<SoundEffect>("Sounds/Run");
            GameSettings.SFX_ItemPickup = Content.Load<SoundEffect>("Sounds/item_pickup");
            GameSettings.SFX_Button = Content.Load<SoundEffect>("Sounds/Button");
            GameSettings.SFX_Squirrel_Knockout = Content.Load<SoundEffect>("Sounds/Squirrel_Knockout");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(GameSettings.GameMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Back))
                Exit();
            MediaPlayer.Volume = MathHelper.Clamp(GameSettings.VolumeValue / 2, 0f, 0.5f);
            SoundEffect.MasterVolume = MathHelper.Clamp(GameSettings.VolumeValue * 2f,0f,1f);
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