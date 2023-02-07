using System.Collections.Generic;
using SnakeMaze.Interfaces;
using UnityEngine;

namespace SnakeMaze.SO
{
    public abstract class PoolSO<T> : ResseteableSO
    {
        protected readonly Stack<T> Available = new Stack<T>();
        public abstract IFactory<T> Factory { get; set; }
        protected bool HasBeenPrewarmed { get; set; }

        protected virtual T Create()
        {
            return Factory.Create();
        }
        
        /// <summary>
        /// Prewarms the pool with a <paramref name="amount"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="amount"> The amount of members created as part of this pool.</param>
        public virtual void Prewarm(int amount)
        {
            if (HasBeenPrewarmed)
            {
                Debug.LogWarning($"Pool {name} has already been prewarmed.");
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                Available.Push(Create());
            }

            HasBeenPrewarmed = true;
        }

        public virtual T Request()
        {
            return Available.Count > 0 ? Available.Pop() : Create();
        }
        /// <summary>
        /// Request a collection of <paramref name="amount"/> members from this pool.
        /// </summary>
        /// <param name="amount">Amount of members.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Request(int amount = 1)
        {
            List<T> members = new List<T>(amount);
            for (int i = 0; i < amount; i++)
            {
                members.Add(Request());
            }

            return members;
        }
        /// <summary>
        /// Returns a <typeparamref name="T"/> to the pool;
        /// </summary>
        /// <param name="member">The <typeparamref name="T"/> to return.</param>
        public virtual void Return(T member)
        {
            Available.Push(member);
        }
        /// <summary>
        /// Returns a <typeparamref name="T"/> collection to the pool.
        /// </summary>
        /// <param name="members">The <typeparamref name="T"/> collection to return.</param>
        public virtual void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }

        protected virtual void OnDisable()
        {
            Available.Clear();
            HasBeenPrewarmed = false;
        }
    }
}
