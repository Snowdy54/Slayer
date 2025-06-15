using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Control;
using MyGame.Model;
using MyGame.View;
using System.Collections.Generic;
using System.Linq;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private GameModel model;
        private GameControl control;
        private GameView view;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            model = new GameModel();
            control = new GameControl(model);
            view = new GameView(model, Content, GraphicsDevice);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            control.HandleInput();
            model.Update(gameTime);
            view.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            view.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
