using Data;
using DG.Tweening;
using Enemies.Animations;
using UnityEngine;

namespace Enemies.States
{
    public class AttackState : IState
    {
        private readonly CharacterController _characterController;
        private readonly IEnemyAnimator _enemyAnimator;
        private readonly Transform _playerTransform;
        private readonly EnemyStats _enemyStats;
        private readonly ParticleSystem _attackPreparingParticle;

        private float _nextAttackTime;
        private float _attackRate;

        public AttackState(CharacterController characterController, IEnemyAnimator enemyAnimator,
            Transform playerTransform, EnemyStats enemyStats, ParticleSystem attackPreparingParticle)
        {
            _characterController = characterController;
            _enemyAnimator = enemyAnimator;
            _playerTransform = playerTransform;
            _enemyStats = enemyStats;
            _attackPreparingParticle = attackPreparingParticle;
        }

        public void Enter()
        {
            _enemyAnimator.EnableStrafe();
            _attackRate = _enemyStats.AttackRate;
        }

        public void Tick()
        {
            LookAtPlayer();

            if (Vector3.Distance(_characterController.transform.position, _playerTransform.position) <=
                _enemyStats.AttackRange)
            {
                Attack();
            }
            else
            {
                UpdateStrafeAnimation();
                Strafe();
            }
        }

        public void Exit()
        {
            _enemyAnimator.DisableStrafe();
            _attackPreparingParticle.Clear();
            _attackPreparingParticle.Stop();
        }

        private void UpdateStrafeAnimation()
        {
            Vector3 direction = (_playerTransform.position - _characterController.transform.position).normalized;
            _enemyAnimator.UpdateStrafeAnimation(direction.normalized.x);
        }

        private void LookAtPlayer() => _characterController.transform.LookAt(_playerTransform);

        private void Strafe()
        {
            Vector3 direction = (_playerTransform.position - _characterController.transform.position).normalized;
            _characterController.Move(direction * (_enemyStats.StafeSpeed * Time.deltaTime));
        }

        private void Attack()
        {
            if (Time.time >= _nextAttackTime)
            {
                _attackPreparingParticle.Play();

                _nextAttackTime = Time.time + 1f / _attackRate;
                DOVirtual.DelayedCall(_attackPreparingParticle.main.duration, () =>
                {
                    _enemyAnimator.PlayAttack();
                    _attackPreparingParticle.Clear();
                    _attackPreparingParticle.Stop();
                });
            }
        }
    }
}