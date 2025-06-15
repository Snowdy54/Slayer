using Microsoft.Xna.Framework.Input;
using MyGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameContorl
{
    public interface IControllable
    {
        void HandleInput();
    }

    public class HeroController : IControllable
    {
        private readonly Move mover;
        private Hero hero;

        public HeroController(Hero hero, Move mover)
        {
            this.hero = hero;
            this.mover = mover;
        }

        public void HandleMouse(MouseState currentState, MouseState previousState, List<Entity> targets)
        {
            if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
            {
                IAttack attack = new HeroAttack(hero);
                attack.Execute(targets);
                //Доделать атаку
            }
        }

        public void HandleInput()
        {
            Vector2 direction = Vector2.Zero;
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.W))
                direction.Y = -1;
            if (keyboard.IsKeyDown(Keys.S))
                direction.Y = 1;
            if (keyboard.IsKeyDown(Keys.D))
                direction.X = 1;
            if (keyboard.IsKeyDown(Keys.A))
                direction.X = -1;

            mover.SetDirection(direction);
        }
    }
}
