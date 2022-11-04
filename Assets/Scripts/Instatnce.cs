using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instatnce : MonoBehaviour {

    public GameObject[] pieces;
    private float rastoyanie = 1f;
    public float rast() { return rastoyanie; }
    [SerializeField]
    private UnityEngine.UI.Dropdown dropdown1;
    public void setPlayerData2()
    {
        rastoyanie = 1 + dropdown1.value;
        
    }
    // Use this for initialization
    void Start ()
    {
        instanc();
    }


    public void instanc()
    {
        int aux = Random.Range(0, pieces.Length);
        Instantiate(pieces[aux], new Vector3(rastoyanie, transform.position.y, transform.position.z), Quaternion.identity);
    }
}
