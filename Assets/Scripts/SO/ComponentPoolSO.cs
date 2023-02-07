using UnityEngine;

namespace SnakeMaze.SO
{
    public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
    {
        private Transform _poolRoot;
        private Transform _parent;

        private Transform PoolRoot
        {
            get
            {
                if (_poolRoot == null)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                }

                return _poolRoot;
            }
        }

        /// <summary>
        /// Parent the pool root transform to <paramref name="parent"/>
        /// </summary>
        /// <param name="parent"> The transform to which this pool will become a child.</param>
        public void SetParent(Transform parent)
        {
            _parent = parent;
            PoolRoot.SetParent(_parent);
        }

        public override T Request()
        {
            T member = base.Request();
            if (member == null)
                member = Create();
            member.gameObject.SetActive(true);
            return member;
        }

        public override void Return(T member)
        {
            member.transform.SetParent(PoolRoot.transform);
            member.gameObject.SetActive(false);
            base.Return(member);
        }

        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform);
            newMember.gameObject.SetActive(false);
            return newMember;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_poolRoot != null)
            {
                Destroy(_poolRoot.gameObject);
            }
        }
    }
}
