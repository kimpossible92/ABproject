using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//En caso de que halla pieces sin cubos, 
//esta clase sera la encargada de borrarlos.
public class RecolectorBase : MonoBehaviour
{
    public float interval;
    float tiempo;
    [SerializeField]
    private UnityEngine.UI.Dropdown dropdown1;
    public void setPlayerData()
    {
        interval = 0;
        interval = (float)dropdown1.value;
    }
   
    void FixedUpdate()
    {
        tiempo += Time.fixedDeltaTime;
        if (tiempo > interval)
        {
            tiempo = 0;
            foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Piece"))
            {
                if (piece.transform.childCount == 0)
                {
                    Destroy(piece);
                }
            }
        }
    }
}
