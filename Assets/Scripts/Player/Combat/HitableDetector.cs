using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Player.Combat
{
    public class HitableDetector : MonoBehaviour
    {
        private const float Radius = 3f;
        private const float MaxDistance = 15f;
        private const string EnemyLayerMask = "Enemy";

        private Camera _camera;
        private readonly Collider[] _enemyColliders = new Collider[10];

        public IHitable CurrentHitable { get; private set; }

        [Inject]
        private void Construct(Camera camera)
        {
            _camera = camera;
        }


        private void FixedUpdate() => TryDetectHitable();

        private void TryDetectHitable()
        {
            TryDetectBySphereCast();
            TryDetectByOverlapSphere();
        }

        private void TryDetectBySphereCast()
        {
            if (Physics.SphereCast(transform.position, Radius, _camera.transform.forward, out RaycastHit hit,
                MaxDistance,
                LayerMask.GetMask(EnemyLayerMask)))
            {
                if (hit.transform.TryGetComponent(out IHitable hitable))
                    CurrentHitable = hitable;
            }

            else CurrentHitable = null;
        }

        private void TryDetectByOverlapSphere()
        {
            if (CurrentHitable != null) return;

            int size = Physics.OverlapSphereNonAlloc(transform.position, Radius + 2f, _enemyColliders,
                LayerMask.GetMask(EnemyLayerMask));
            if (size == 0)
            {
                CurrentHitable = null;
                return;
            }

            foreach (Collider enemyCollider in _enemyColliders.Where(c => c != null))
            {
                if (enemyCollider.transform.TryGetComponent(out IHitable hitable))
                    CurrentHitable = hitable;
            }
        }
    }
}