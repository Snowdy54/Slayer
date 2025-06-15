using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using MyGame.GameContorl;
using MyGame.View;

namespace MyGame.Control
{
    public class GameControl
    {
        private readonly HeroController heroController;
        private MouseState currentMouse;
        private MouseState previousMouse;
        private readonly GameModel model;

        public GameControl(GameModel model)
        {
            this.model = model;
            heroController = new HeroController(model.Hero, model.Mover);
        }

        public void HandleInput()
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            heroController.HandleInput();
            heroController.HandleMouse(currentMouse, previousMouse, model.GetTargets());
        }
    }
}
