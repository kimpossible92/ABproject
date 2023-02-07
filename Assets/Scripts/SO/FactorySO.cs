using SnakeMaze.Interfaces;
using UnityEngine;

namespace SnakeMaze.SO
{
    public abstract class FactorySO<T> : ScriptableObject, IFactory<T>
    {
        public abstract T Create();
    }
}
