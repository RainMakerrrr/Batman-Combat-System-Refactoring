using Data;
using DG.Tweening;
using Enemies;
using Player.Animations;
using Services.Assets;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Player.Combat
{
    public class PlayerCombat : MonoBehaviour, IHitable
    {
        private const float MinAttackDistance = 15f;
        private const float MaxDistanceDelta = 0.95f;

        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private HitableDetector _hitableDetector;
        [SerializeField] private AttackerDetector _attackerDetector;
        [SerializeField] private AnimatorStateReader _stateReader;

        private IInputService _inputService;
        private IAssetProvider _assetProvider;
        private IHitable _currentTarget;
        private IAttacker _currentAttacker;
        private PlayerStats _playerStats;

        private bool CanAttack => _stateReader.CurrentState != AnimatorState.Attack &&
                                  _stateReader.CurrentState != AnimatorState.Hit;

        public Vector3 Position => transform.position;

        [Inject]
        private void Construct(IInputService inputService, IAssetProvider assetProvider,
            EnemiesBattleRouter enemiesBattleRouter)
        {
            _inputService = inputService;
            _assetProvider = assetProvider;
        }

        private void OnEnable()
        {
            _playerStats = _assetProvider.LoadData<PlayerStats>(AssetPath.PlayerStats);

            _inputService.Attack += OnAttack;
            _inputService.Counter += OnCounter;
        }
        
        private void OnAttack()
        {
            if (CanAttack == false) return;

            _currentTarget = _hitableDetector.CurrentHitable;

            if (_currentTarget == null || OutAttackRange())
            {
                _animator.PlayDefaultAttackAnimation();
                return;
            }

            StartAttack(_currentTarget.Position);
        }

        private void OnCounter()
        {
            if (CanAttack == false) return;

            _currentAttacker = _attackerDetector.CurrentAttacker;
             if (_currentAttacker == null) return;

             _currentTarget = _currentAttacker;
             
            _animator.PlayDodgeAnimation();
            transform.DOLookAt(_currentAttacker.Position, _playerStats.LookAtTween);
            transform.DOMove(transform.position + _currentAttacker.Forward, _playerStats.MovementTween);

            DOVirtual.DelayedCall(0.2f, () => StartAttack(_currentAttacker.Position));
        }

        private void StartAttack(Vector3 position)
        {
            _animator.PlayAttackAnimation();

            MoveToTarget(position);
            LerpAcceleration();
        }

        /// <summary>
        /// Attack animation event handler
        /// </summary>
        private void Attack()
        {
            _currentTarget?.TakeHit();
        }

        public void TakeHit()
        {
            _animator.PlayHitAnimation();
        }

        private bool OutAttackRange() =>
            Vector3.Distance(transform.position, _currentTarget.Position) > MinAttackDistance;

        private void MoveToTarget(Vector3 position)
        {
            transform.DOLookAt(position, _playerStats.LookAtTween);
            transform.DOMove(Vector3.MoveTowards(position, transform.position, MaxDistanceDelta),
                _playerStats.MovementTween);
        }

        private void LerpAcceleration()
        {
            _playerStats.Acceleration = 0f;
            DOVirtual.Float(0f, 1f, _playerStats.MovementTween, acceleration => _playerStats.Acceleration = acceleration);
        }

        private void OnDisable()
        {
            _inputService.Attack -= OnAttack;
            _inputService.Counter -= OnCounter;
        }
    }
}