using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public class TimeManager
    {
        private readonly List<IGameUpdatable> updatables = new();

        public void Register(IGameUpdatable obj) =>
            updatables.Add(obj);

        public void Unregister(IGameUpdatable obj) =>
            updatables.Remove(obj);

        public void Update(GameTime gameTime)
        {
            foreach (var obj in updatables)
                obj.Update(gameTime);

        }
    }
}
