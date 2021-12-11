using System;
using Data;
using Enemies.Animations;
using Enemies.States;
using Player.Animations;
using Player.Combat;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IAttacker
    {
        public event Action<Enemy> Die;
        public event Action<Enemy> AttackEnded;

        private const string PlayerLayerName = "Player";
        private const string DefaultLayerName = "Default";
        private const float Radius = 2f;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private AnimatorStateReader _stateReader;
        [SerializeField] private ParticleSystem _attackPreparingParticle;
        [SerializeField] private EnemyType _enemyType;

        private Transform _playerTransform;
        private AnimatorStateReader _playerStateReader;

        private IEnemyAnimator _enemyAnimator;
        private IEnemiesStatsFactory _statsFactory;
        private EnemyStats _enemyStats;
        private EnemyHealth _enemyHealth;

        private StateMachine _stateMachine;

        private readonly Collider[] _colliders = new Collider[1];

        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;
        public IState State => _stateMachine.CurrentState;

        public bool AllowToAttack { get; set; }

        [Inject]
        private void Construct(StateMachine stateMachine, IEnemyAnimator enemyAnimator, Transform playerTransform,
            IEnemiesStatsFactory statsFactory, AnimatorStateReader playerStateReader)
        {
            _stateMachine = stateMachine;
            _enemyAnimator = enemyAnimator;
            _playerTransform = playerTransform;
            _statsFactory = statsFactory;
            _playerStateReader = playerStateReader;
        }

        private void Start()
        {
            _enemyStats = _statsFactory.GetEnemyStats(_enemyType);
            _enemyHealth = _statsFactory.CreateEnemyHealth(_enemyType);
            InitializeStateMachine();
        }

        public void TakeHit()
        {
            if (_enemyHealth.CurrentLifeCount - 1 > 0)
            {
                _enemyAnimator.PlayHit();
            }
            else
            {
                _enemyAnimator.PlayDeath();
                DisableEnemy();
            }

            AttackEnded?.Invoke(this);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void InitializeStateMachine()
        {
            var idleState = new IdleState(_enemyAnimator, _characterController);
            var chaseState = new ChaseState(_characterController, _enemyAnimator, _playerTransform, _enemyStats);
            var strafeState = new StrafeState(_characterController, _enemyAnimator, _playerTransform, _enemyStats);
            var attackState = new AttackState(_characterController, _enemyAnimator, _playerTransform, _enemyStats,
                _attackPreparingParticle, _stateMachine);
            var hurtState = new HurtState(_enemyHealth, _characterController, _enemyAnimator);

            _stateMachine.AddTransition(idleState, strafeState, CanStrafe);
            _stateMachine.AddTransition(idleState, chaseState, () => AllowToAttack && CanStrafe());
            _stateMachine.AddTransition(strafeState, chaseState, () => AllowToAttack && !CanAttack());
            _stateMachine.AddTransition(strafeState, idleState, () => !AllowToAttack && !CanStrafe());
            _stateMachine.AddTransition(strafeState, attackState, () => AllowToAttack && CanAttack());
            _stateMachine.AddTransition(chaseState, strafeState, () => !AllowToAttack || !CanStrafe());
            _stateMachine.AddTransition(chaseState, attackState, CanAttack);

            _stateMachine.AddTransition(attackState, strafeState, () => !AllowToAttack || !CanAttack());
            _stateMachine.AddTransition(hurtState, strafeState, () => _stateReader.CurrentState == AnimatorState.Move);

            _stateMachine.AddAnyTransition(hurtState,
                () => _stateReader.CurrentState == AnimatorState.Hit ||
                      _stateReader.CurrentState == AnimatorState.Death);
            _stateMachine.AddAnyTransition(idleState, () => _playerStateReader.CurrentState == AnimatorState.Attack);
            _stateMachine.AddAnyTransition(strafeState, () => !AllowToAttack);

            _stateMachine.SetState(idleState);
        }

        /// <summary>
        /// Attack animation event
        /// </summary>
        private void Attack()
        {
            if (State is AttackState == false) return;

            int count = Physics.OverlapSphereNonAlloc(transform.position, Radius, _colliders,
                LayerMask.GetMask(PlayerLayerName));
            if (count == 0) return;

            foreach (Collider coll in _colliders)
            {
                if (coll.TryGetComponent(out IHitable hitable))
                    hitable.TakeHit();
            }

            AttackEnded?.Invoke(this);
        }

        private void DisableEnemy()
        {
            _characterController.enabled = false;
            gameObject.layer = LayerMask.NameToLayer(DefaultLayerName);
            Die?.Invoke(this);

            Destroy(this);
        }

        public bool CanStrafe() =>
            Vector3.Distance(transform.position, _playerTransform.position) <= _enemyStats.StrafeRange;

        private bool CanAttack() =>
            Vector3.Distance(transform.position, _playerTransform.position) <= _enemyStats.AttackRange;
    }
}