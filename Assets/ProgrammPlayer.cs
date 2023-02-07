using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammPlayer : MonoBehaviour
{
    [HideInInspector] public int newHead = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SnakeMaze.Player.PlayerController>().SetDvijenie();
    }

    // Update is called once per frame
    void Update()
    {
        var Xmove = Input.GetAxis("Horizontal");
        var Ymove = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            newHead = 0;
        }
        if (Xmove > 0.0001f)
        {
            newHead = 1;
        }
        else if (Xmove < -0.0001f)
        {
            newHead = 2;
        }
        if (Ymove > 0.0001f)
        {
            newHead = 3;
        }
        else if (Ymove < -0.0001f)
        {
            newHead = 4;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newHead = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newHead = 2;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newHead = 3;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newHead = 4;
        }
        if (newHead == 1)
        {
            GetComponent<SnakeMaze.Player.PlayerController>().IsHeadSet(SnakeMaze.Enums.Directions.Right);
        }
        else if (newHead == 2)
        {
            GetComponent<SnakeMaze.Player.PlayerController>().IsHeadSet(SnakeMaze.Enums.Directions.Left);
        }
        else if (newHead == 3)
        {
            GetComponent<SnakeMaze.Player.PlayerController>().IsHeadSet(SnakeMaze.Enums.Directions.Up);
        }
        else if (newHead == 4)
        {
            GetComponent<SnakeMaze.Player.PlayerController>().IsHeadSet(SnakeMaze.Enums.Directions.Down);
        }
        else if (newHead == 0)
        {
            
        }
    }
}
