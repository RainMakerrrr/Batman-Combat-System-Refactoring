using System.Collections.Generic;
using System.Linq;
using Enemies;
using Enemies.States;
using UnityEngine;

namespace Player.Combat
{
    public class AttackerDetector : MonoBehaviour
    {
        private const string EnemyLayerMask = "Enemy";
        private const float Radius = 6f;

        private readonly Collider[] _results = new Collider[10];

        public IAttacker CurrentAttacker { get; private set; }

        private void FixedUpdate() => TryDetectAttackers();

        private void TryDetectAttackers()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, Radius, _results,
                LayerMask.GetMask(EnemyLayerMask));
            if (count == 0)
            {
                CurrentAttacker = null;
                return;
            }

            List<IAttacker> results = _results.Where(result => result != null)
                .Select(result => result.GetComponent<IAttacker>()).Where(result => result is {State: AttackState _})
                .ToList();
            if (results.Count == 0)
            {
                CurrentAttacker = null;
                return;
            }

            TryFindClosestAttacker(results);
        }

        private void TryFindClosestAttacker(IReadOnlyCollection<IAttacker> results)
        {
            IAttacker firstResult = results.FirstOrDefault();
            if (firstResult == null)
            {
                CurrentAttacker = null;
                return;
            }

            float closestDistance = GetDistance(firstResult.Position);
            CurrentAttacker = firstResult;

            foreach (IAttacker result in results.Where(result => GetDistance(result.Position) < closestDistance))
            {
                closestDistance = GetDistance(result.Position);
                CurrentAttacker = result;
            }
        }

        private float GetDistance(Vector3 targetPosition) => Vector3.Distance(transform.position, targetPosition);
    }
}