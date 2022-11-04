using Gameplay.ShipSystems;
using System.Collections;
using UnityEngine;

namespace Gameplay.Spaceships
{
    public interface ISpaceship
    {
        MovementSystem MovementSystem { get; }
    }
}