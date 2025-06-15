using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public class Move : IGameUpdatable
    {
        private readonly Entity entity;
        private Vector2 direction;

        public Move(Entity entity, Vector2 direction)
        {
            this.entity = entity;
            this.direction = direction;
        }

        public void SetDirection(Vector2 newDirection) =>
            direction = newDirection;

        public void Update(GameTime gameTime)
        {
            if (direction != Vector2.Zero)
                direction.Normalize();

            entity.position += direction * entity.MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    }
}
