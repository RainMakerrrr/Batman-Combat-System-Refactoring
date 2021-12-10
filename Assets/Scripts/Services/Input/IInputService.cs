using System;
using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        event Action Attack;
        event Action Counter;
        Vector2 Movement { get; }
        Vector2 Look { get; }
    }
}