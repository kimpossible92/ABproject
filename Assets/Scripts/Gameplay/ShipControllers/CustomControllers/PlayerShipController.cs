using Gameplay.ShipSystems;
using UnityEngine;

namespace Gameplay.ShipControllers.CustomControllers
{
    public class PlayerShipController : ShipController
    {
        bool MouseHeel=false;
        protected override void ProcessHandling(MovementSystem movementSystem)
        {
            if(
               transform.position.x<GetComponent<CollShip>().limitx||
               transform.position.x>GetComponent<CollShip>().limitx1
               )
            {
                movementSystem.LateralMovement(Input.GetAxis("Horizontal") * Time.deltaTime);
                if (MouseHeel) { /*print((Input.mousePosition.x * 0.1f) - 50);*/ transform.position = new Vector3((Input.mousePosition.x*0.1f)-40, transform.position.y, transform.position.z); }
            }
            if(
               transform.position.x > GetComponent<CollShip>().limitx)
            {
                //print("x");
                transform.position = new Vector3(GetComponent<CollShip>().limitx-2, transform.position.y, transform.position.z);
            }
           
            if(transform.position.x < GetComponent<CollShip>().limitx1
               )
            {
                //print("x1");
                transform.position = new Vector3(GetComponent<CollShip>().limitx1 + 2, transform.position.y, transform.position.z);
            }

        }
      
        protected override void ProcessFire(WeaponSystem fireSystem)
        {
            if (Input.GetKey(KeyCode.Space)|| Input.GetMouseButton(0))
            {
                fireSystem.TriggerFire();
                var source = GetComponent<AudioSource>();
                if(source !=null)source.PlayOneShot(source.clip);
            }
            if (Input.GetKey(KeyCode.M))
            {
                MouseHeel = true;
            }
            if (Input.GetKey(KeyCode.N))
            {
                MouseHeel = false;
            }
        }
       
    }
}
