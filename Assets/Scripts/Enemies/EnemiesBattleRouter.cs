using System.Collections.Generic;
using Enemies.States;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemiesBattleRouter : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemies;
        [SerializeField] private float _pickingTime;

        public Enemy CurrentEnemy { get; private set; }

        private void Start()
        {
            InvokeRepeating(nameof(PickRandomEnemyToFight), 0f, _pickingTime);

            foreach (Enemy enemy in _enemies)
            {
                enemy.Die += OnEnemyDie;
            }
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void PickRandomEnemyToFight()
        {
            if (CurrentEnemy != null)
                CurrentEnemy.AllowToAttack = false;

            CurrentEnemy = GetRandomEnemy();
            if (CurrentEnemy != null)
                CurrentEnemy.AllowToAttack = true;
        }

        private Enemy GetRandomEnemy() => _enemies.Count == 0 ? null : _enemies[Random.Range(0, _enemies.Count)];

        public bool HasAttackingEnemy() => CurrentEnemy.CurrentState is AttackState;

        private void OnDestroy()
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.Die -= OnEnemyDie;
            }
        }
    }
}