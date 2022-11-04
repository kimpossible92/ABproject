using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadBlock : MonoBehaviour
{
    public int row() { return GetComponent<Block>().row; }
    public int col() { return GetComponent<Block>().col; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
