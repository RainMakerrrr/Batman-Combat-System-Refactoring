using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Stats / Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        public float MovementSpeed;
        public float Acceleration;
        public float RotationSpeed;
        public float FallSpeed;
        public float LookAtTween = 0.2f;
        public float MovementTween = 0.65f;
    }
}