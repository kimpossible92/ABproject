using Gameplay.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cub : ProjectilePool
{
    [SerializeField] private Material material1;
    [SerializeField]
    private Material material2;
    protected override void Move(float speed)
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleIdentity == UnitBattleIdentity.Enemy)
        {
            GetComponent<Renderer>().material = material1;
        }
        if (BattleIdentity == UnitBattleIdentity.Ally)
        {
            GetComponent<Renderer>().material = material2;
        }
    }
}
