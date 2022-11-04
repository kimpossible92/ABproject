using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Vector3Int
{
    public int x, y, z;

    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Int(float x, float y ,float z)
    {
        this.x = Mathf.RoundToInt(x);
        this.y = Mathf.RoundToInt(y);
        this.z = Mathf.RoundToInt(z);
    }

    public Vector3Int(Vector3 v)
    {
        this.x = Mathf.RoundToInt(v.x);
        this.y = Mathf.RoundToInt(v.y);
        this.z = Mathf.RoundToInt(v.z);
    }

    public String toString()
    {
        return "( " + x + " , " + y + " , " + z + " )";
    }

}


public class ControllerNivel : MonoBehaviour {

    public int Puntuation;
    public int puntuationLine;
    public int multiplictionPuntuation;

    public bool pauser;

    public Instatnce generatorPiece;
    public ControllerNivel controller;
    public Vector3Int tamanTable;
    public Transform[,,] table;
    [HideInInspector]private int ty = 0;
    public GameObject gameOverUI;
    [SerializeField]
    private UnityEngine.UI.Dropdown dropdown1;
    public void setPlayerData2()
    {
        //tamanTable.y = ty+dropdown1.value;
    }
    public void Awake()
    {
        
        if (controller != null)
            Destroy(gameObject);
        else
            controller = this;
        
        table = new Transform[tamanTable.x, tamanTable.y, tamanTable.z];
        ty = tamanTable.y;

    }


    public bool isCompleteInX(Vector3Int p)
    {
        for(int i = 0; i< tamanTable.x; i++)
        {
            if(table[i, p.y, p.z] == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool isCompleteInZ(Vector3Int p)
    {

        for (int i = 0; i < tamanTable.z; i++)
        {
            if (table[p.x, p.y, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool isDentTable(Vector3Int p)
    {

        if (p.x >= 0 && p.z >= 0 && p.y >= 0 && p.x < tamanTable.x && p.z < tamanTable.z && p.y < tamanTable.y)
            return true;

        return false;
    }

    public bool isFueraFromAbove(Vector3Int p)
    {
        return p.y >= tamanTable.y;
    }


    public void FilInX(Vector3Int p)
    {
        for(int i = 0; i < tamanTable.x; i++)
        {
          Destroy(table[i, p.y, p.z].gameObject);
        }

        bFilInX(new Vector3Int(p.x, p.y + 1, p.z));
    }

    public void FillInZ(Vector3Int p)
    {
        for (int i = 0; i < tamanTable.z; i++)
        {
            Destroy(table[p.x, p.y, i].gameObject);
        }

        bFIlInZ(new Vector3Int(p.x,p.y+1,p.z));
    }


    public void  bFilInX(Vector3Int v)
    {
        if (v.y >= tamanTable.y)
        {
           // comprFil();
            return;
        }

        for (int i = 0; i< tamanTable.x; i++)
        {
            if(table[i,v.y,v.z] != null)
            {
                table[i, v.y, v.z].position += Vector3.down;

                table[i, v.y - 1, v.z] = table[i, v.y, v.z];
                table[i, v.y, v.z] = null; 
            }
        }

        bFilInX(new Vector3Int(v.x, v.y + 1, v.z));
    }

    public void bFIlInZ(Vector3Int v)
    {
        if (v.y >= tamanTable.y)
        {
           // comprFil();
            return;
        }

        for (int i = 0; i < tamanTable.z; i++)
        {
            if (table[v.x, v.y, i] != null)
            {
                table[v.x, v.y, i].position += Vector3.down;
                table[v.x, v.y-1, i] = table[v.x, v.y, i];
                table[v.x, v.y, i] = null;
            }
        }

        bFIlInZ(new Vector3Int(v.x, v.y + 1, v.z));
    }

    public void comprFil()
    {
        for(int i = 0; i <tamanTable.y; i++)
        {
            for (int j = 0; j < tamanTable.x; j++)
            {
                Vector3Int v1 = new Vector3Int(j,i,0);
                if (isCompleteInZ(v1))
                {
                    FillInZ(v1);

                    Puntuation += puntuationLine * multiplictionPuntuation;
                    GetComponent<ControllerUI>().actualPunctuation(Puntuation);
                    multiplictionPuntuation++;
                    GetComponent<ControllerUI>().actualmultiplicator(multiplictionPuntuation);
                }
            }

            for (int k = 0; k < tamanTable.z; k++)
            {
                Vector3Int v2 = new Vector3Int(0, i, k);
                if (isCompleteInX(v2))
                {
                    FilInX(v2);

                    
                    Puntuation += puntuationLine * multiplictionPuntuation;
                    GetComponent<ControllerUI>().actualPunctuation(Puntuation);
                    
                    multiplictionPuntuation++;
                    GetComponent<ControllerUI>().actualmultiplicator(multiplictionPuntuation);
                }

            }
        }
    }

    public void terminPart()
    {
        gameOverUI.SetActive(true);
    }

}
