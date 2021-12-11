using Enemies.States;
using Player.Combat;
using UnityEngine;

namespace Enemies
{
    public interface IAttacker : IHitable
    {
        IState State { get; }
        Vector3 Forward { get; }
    }
}