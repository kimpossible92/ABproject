using System;
using Gameplay.Weapons;
using UnityEngine;
public enum livetype
{
plus,minus
}
public abstract class ProjectilePool : MonoBehaviour, IDamageDealer
{
    #region Interface
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
    #endregion
    [SerializeField]
    private float _speed;
    [SerializeField]
    livetype live;
    [SerializeField]
    private float _damage=1;
    [SerializeField]
    private UnitBattleIdentity _battleIdentity;

    public UnitBattleIdentity BattleIdentity => _battleIdentity;
    public float Damage => _damage;



    public void Init(UnitBattleIdentity battleIdentity)
    {
        _battleIdentity = battleIdentity;
    }


    private void Update()
    {
        Move(_speed);
    }


    private void OnCollisionEnter(Collision other)
    {
        var damagableObject = other.gameObject.GetComponent<IDamagable>();
        if (damagableObject != null
            && damagableObject.BattleIdentity != BattleIdentity)
        {
            damagableObject.ApplyDamage(this);
            Destroy(gameObject);
        }
    }



    protected abstract void Move(float speed);
}