using UnityEngine;

namespace Player.Combat
{
    public interface IHitable
    {
        void TakeHit();
        Vector3 Position { get; }
    }
}