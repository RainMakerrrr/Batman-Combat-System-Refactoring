using Enemies;
using Enemies.Animations;
using Player.Animations;
using Services.Assets;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CombatSceneInstaller : MonoInstaller
    {
        [SerializeField] private AnimatorStateReader _playerStateReader;
        [SerializeField] private EnemiesBattleRouter _battleRouter;

        public override void InstallBindings()
        {
            BindPlayerControls();
            BindInputService();
            BindAssetProvider();
            BindCamera();
            BindEnemy();
            BindStateMachine();
            BindPlayerTransform();
            BindEnemiesStatsFactory();
            BindPlayerStateReader();
            BindBattleRouter();
        }


        private void BindPlayerControls() => Container.Bind<PlayerControls>().FromNew().AsSingle();
        private void BindInputService() => Container.Bind<IInputService>().To<InputService>().AsSingle();
        private void BindAssetProvider() => Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        private void BindCamera() => Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        private void BindStateMachine() => Container.Bind<StateMachine>().FromNew().AsTransient();

        private void BindEnemy() =>
            Container.Bind<IEnemyAnimator>().To<EnemyAnimator>().FromComponentSibling().AsTransient();


        private void BindPlayerTransform() =>
            Container.Bind<Transform>().FromInstance(_playerStateReader.transform).AsSingle();

        private void BindEnemiesStatsFactory() =>
            Container.Bind<IEnemiesStatsFactory>().To<EnemiesStatsFactory>().AsSingle();

        private void BindPlayerStateReader() =>
            Container.Bind<AnimatorStateReader>().FromInstance(_playerStateReader).AsSingle();

        private void BindBattleRouter() => Container.Bind<EnemiesBattleRouter>().FromInstance(_battleRouter).AsSingle();
    }
}