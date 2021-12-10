using Data;
using Services.Assets;

namespace Enemies
{
    public class EnemiesStatsFactory : IEnemiesStatsFactory
    {
        private readonly IAssetProvider _assetProvider;

        public EnemiesStatsFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public EnemyStats GetEnemyStats(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.Normal:
                    return _assetProvider.LoadData<EnemyStats>(AssetPath.NormalEnemyStats);
                case EnemyType.Strong:
                    return _assetProvider.LoadData<EnemyStats>(AssetPath.StrongEnemyStats);
            }

            return null;
        }

        public EnemyHealth CreateEnemyHealth(EnemyType enemyType) => new EnemyHealth(GetEnemyStats(enemyType));
    }
}