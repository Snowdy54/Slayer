using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public interface IAttack
    {
        void Execute(List<Entity> targets);
    }

    public interface IGameUpdatable
    {
        void Update(GameTime gameTime);
    }
}
