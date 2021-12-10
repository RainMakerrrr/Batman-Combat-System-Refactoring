using Data;

namespace Enemies
{
    public interface IEnemiesStatsFactory
    {
        EnemyStats GetEnemyStats(EnemyType enemyType);
        EnemyHealth CreateEnemyHealth(EnemyType enemyType);
    }
}