using UnityEngine;

namespace Data
{
    
    [CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Stats / Enemy Stats")]
    public class EnemyStats : ScriptableObject
    {
        public float MovementSpeed;
        public float RotationSpeed;
        public float StafeSpeed;
        public float ChaseRange;
        public float StrafeRange;
        public float AttackRange;
        public float AttackRate;
        public int LifeCount;
    }
}