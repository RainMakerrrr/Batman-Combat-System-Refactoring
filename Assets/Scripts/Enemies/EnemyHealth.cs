using Data;

namespace Enemies
{
    public class EnemyHealth
    {
        public bool IsDead => CurrentLifeCount <= 0;

        public int CurrentLifeCount { get; private set; }

        public EnemyHealth(EnemyStats enemyStats)
        {
            CurrentLifeCount = enemyStats.LifeCount;
        }

        public void TakeDamage(int damage = 1)
        {
            if (CurrentLifeCount <= 0) return;

            CurrentLifeCount -= damage;
        }
    }
}