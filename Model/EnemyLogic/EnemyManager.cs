using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model.EnemyLogic
{
    public class EnemyManager : IGameUpdatable
    {
        private readonly List<Entity> activeEnemies = new();
        private readonly List<Vector2> enemySpawnPoints = new();
        private readonly Hero hero;
        private readonly float activeRadius = 300f;
        private readonly float attackRadius = 50f;
        private readonly AStarPathfinder Pathfinder;

        public EnemyManager(Hero hero, AStarPathfinder pathfinder)
        {
            this.hero = hero;
            Pathfinder = pathfinder;
        }

        public void LoadEnemies(List<Vector2> spawnPoints) => enemySpawnPoints.AddRange(spawnPoints);

        public List<Entity> GetActiveEnemies() => activeEnemies;


        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (hero.IsDead())
                return;

            ActivateEnemiesInRadius();

            foreach (var enemy in activeEnemies)
            {
                float distance = Vector2.Distance(hero.position, enemy.position);

                enemy.UpdateAttackTimer(deltaTime);
                enemy.UpdatePreparation(deltaTime, hero);

                if (enemy.IsStrikingNow || enemy.IsPreparingAttack)
                    continue;

                enemy.Behavior?.Update(deltaTime, hero.position);


                if (distance < attackRadius)
                    TryStartEnemyAttack(enemy);
            }

            activeEnemies.RemoveAll(e => e.IsDead());
        }

        private void ActivateEnemiesInRadius()
        {
            var toActivate = enemySpawnPoints
                .Where(p => Vector2.Distance(p, hero.position) < activeRadius)
                .ToList();

            foreach (var spawnPos in toActivate)
            {
                if (activeEnemies.Any(e => Vector2.Distance(e.position, spawnPos) < 10f))
                    continue;

                var enemy = new Entity(100, 25, 120, spawnPos, new Vector2(96, 96));
                var patrolPoints = new List<Vector2> { spawnPos };

                enemy.Behavior = new EnemyBehavior(
                    () => enemy.position,
                    pos => enemy.position = pos,
                    enemy.MoveSpeed,
                    Pathfinder);

                activeEnemies.Add(enemy);
            }
            enemySpawnPoints.RemoveAll(p => toActivate.Contains(p));
        }

        private Dictionary<Entity, List<Point>> enemyPaths = new();

        private static void TryStartEnemyAttack(Entity enemy)
        {
            if (enemy.CanAttack())
                enemy.StartPreparingAttack();
        }
    }
}
