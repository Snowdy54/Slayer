using Microsoft.Xna.Framework;
using MyGame.GameContorl;
using MyGame.Model.EnemyLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MyGame.Model
{
    public class Entity
    {
        public int HP;
        public int Damage;
        public int MoveSpeed;
        public Vector2 position;
        public Vector2 size;
        public bool isDamaged;
        public float attackCooldown = 1.0f;
        public float attackTimer = 0f;
        public bool IsPreparingAttack;
        private float prepareAttackTimer;
        private float attackWindup = 0.7f;
        private bool readyToStrike = false;
        public bool isStrikingNow = false;
        private float attackRadius = 50f;
        private double timeSinceLastTileEffect = 0;
        private const double tileEffectCooldown = 1;
        public EnemyBehavior Behavior;

        public Entity(int HP, int damage, int moveSpeed, Vector2 position, Vector2 size)
        {
            this.HP = HP;
            Damage = damage;
            MoveSpeed = moveSpeed;
            this.position = position;
            this.size = size;
            isDamaged = false;
        }

        public bool CanApplyTileEffect(double deltaSeconds)
        {
            timeSinceLastTileEffect += deltaSeconds;
            if (timeSinceLastTileEffect >= tileEffectCooldown)
            {
                timeSinceLastTileEffect = 0;
                return true;
            }
            return false;
        }
        public bool IsStrikingNow => isStrikingNow;

        public void StrikeComplete()
        {
            readyToStrike = false;
            isStrikingNow = false;
            ResetAttackCooldown();
        }


        public void StartPreparingAttack()
        {
            IsPreparingAttack = true;
            prepareAttackTimer = attackWindup;
            readyToStrike = false;
        }

        public void CancelStrikePreparation()
        {
            readyToStrike = false;
            IsPreparingAttack = false;
        }

        public void UpdatePreparation(float deltaTime, Hero hero)
        {
            if (IsPreparingAttack)
            {
                prepareAttackTimer -= deltaTime;
                if (prepareAttackTimer <= 0f)
                {
                    IsPreparingAttack = false;
                    isStrikingNow = true;

                    float distanceToHero = Vector2.Distance(hero.position, position);
                    if (distanceToHero < attackRadius)
                        hero.TakeDamage(Damage);
                                                 

                    StrikeComplete();
                }
            }
        }


        public bool IsReadyToStrike() => readyToStrike;

        public bool IsDead() => HP <= 0;

        public bool CanAttack() => attackTimer <= 0f && !IsPreparingAttack && !IsReadyToStrike();

        public void ResetAttackCooldown() => attackTimer = attackCooldown;

        public void UpdateAttackTimer(float deltaTime)
        {
            if (attackTimer > 0f)
                attackTimer -= deltaTime;
        }

        public void TakeDamage(int amount)
        {
            HP -= amount;
            isDamaged = true;
        }
    }

    public class Hero : Entity
    {
        public Hero(Vector2 startPosition) : base(100, 10, 100, startPosition, new Vector2(96, 96))
        {
        }
    }

    public class HeroAttack : IAttack
    {
        private readonly Hero hero;
        private readonly float attackRadius = 50f;

        public HeroAttack(Hero hero)
        {
            this.hero = hero;
        }

        public void Execute(List<Entity> targets)
        {
            foreach (var target in targets)
            {
                var distance = Vector2.Distance(hero.position, target.position);

                if (distance <= attackRadius)
                    target.TakeDamage(hero.Damage);
            }
        }
    }
}
