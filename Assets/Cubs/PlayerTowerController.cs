using Gameplay.ShipSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerController : Cubs
{
    public bool spaceNot = false;
    protected override void ProcessHandling(MovementSystem movementSystem)
    {
        
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            //if(transform.position.y<=0.1f&&!Menu)transform.Translate(new Vector3(0,4,1));
            if (transform.position.y <= 4.25f && !Menu&&spaceNot==false) { transform.position += new Vector3(0, 0.25f, 0.1f); }
            Invoke("jumpdelay2", 1f);
            Invoke("jumpdelay", 1f);
            Invoke("jumpdelay2", 0.1f);
        }
    }
    private void jumpdelay()
    {
        spaceNot = true;
    }
    private void jumpdelay2()
    {
        spaceNot = false;
    }
    protected override void ProcessJump(MovementSystem jumpSystem)
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
