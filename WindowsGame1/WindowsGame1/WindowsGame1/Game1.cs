using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    enum GameState
    {
        TITLE_SCREEN,
        PLAYING
    }

    // E_Layer
    public enum E_Layer
    {
        UI = 0,

        // etc ...

        Count,
    };


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state = GameState.TITLE_SCREEN;

        int clickTimes = 0;

        private GraphicsDeviceManager Graphics;
        private UiLayer UiLayer;

        public bool IsRunningSlowly;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            graphics.PreferMultiSampling = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _G.Game = this;

            // add core components
            Components.Add(new GamerServicesComponent(this));

            // add layers
            UiLayer = new UiLayer();
            _G.UI = UiLayer;

            // add other components
            _G.GameInput = new GameInput((int)E_GameButton.Count, (int)E_GameAxis.Count);
            GameControls.Setup(); // initialise mappings

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Guide.NotificationPosition = NotificationPosition.BottomRight;

            // startup ui
            UiLayer.Startup(Content);

            // setup debug menu
#if !RELEASE
            _UI.SetupDebugMenu(null);
#endif

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            UiLayer.Shutdown();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            IsRunningSlowly = gameTime.IsRunningSlowly;

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // update input
            _G.GameInput.Update(frameTime);

#if !RELEASE
            Input input = _G.GameInput.GetInput(0);

            if (input.ButtonJustPressed((int)E_UiButton.Quit))
                this.Exit();
#endif

#if !RELEASE
            // update debug menu
            _UI.DebugMenuActive = _UI.DebugMenu.Update(frameTime);
#endif

            // TODO - other stuff here ...

            // update ui
            UiLayer.Update(frameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO - other stuff here ...

            // render ui
            UiLayer.Render(frameTime);

            #if !RELEASE
                        // render debug menu
                        _UI.DebugMenu.Render();
            #endif

            base.Draw(gameTime);
        }
    }
}


