using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemiesBattleRouter : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemies;
        
        private void Start()
        {
            PickRandomEnemyToFight(null);

            foreach (Enemy enemy in _enemies)
            {
                enemy.AttackEnded += PickRandomEnemyToFight;
                enemy.Die += OnEnemyDie;
            }
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void PickRandomEnemyToFight(Enemy enemy)
        {
            if (enemy != null)
            {
                if (enemy.AllowToAttack == false) return;
                
                enemy.AllowToAttack = false;
            }

            Enemy randomEnemy = GetRandomEnemy();

            if (randomEnemy != null)
                randomEnemy.AllowToAttack = true;
        }

        private Enemy GetRandomEnemy()
        {
            List<Enemy> enemies = _enemies.Where(enemy => enemy.CanStrafe()).ToList();
            return enemies.Count == 0 ? null : enemies[Random.Range(0, enemies.Count)];
        }

        private void OnDestroy()
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.AttackEnded -= PickRandomEnemyToFight;
                enemy.Die -= OnEnemyDie;
            }
        }
    }
}