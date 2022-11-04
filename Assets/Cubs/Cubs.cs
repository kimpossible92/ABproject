using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.ShipSystems;
using Gameplay.Spaceships;

public abstract class Cubs : MonoBehaviour
{
    private ISpaceship _spaceship;
    private bool pause = false;
    public bool Menu => pause;
    public void SetMenu(bool p)
    {
        pause = p;
    }
    public void Init(ISpaceship spaceship)
    {
        _spaceship = spaceship;
        InvokeRepeating("ZVector", 0.1f, 0.1f);
    }
    private void ZVector()
    {
        if (pause==false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.3f);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z - 2.22f);
        }
    }
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void PerFrameUpdate()
    {
        ProcessHandling(_spaceship.MovementSystem);
    }
    protected abstract void ProcessHandling(MovementSystem movementSystem);
    protected abstract void ProcessJump(MovementSystem jumpSystem);
}
