using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public HitCandy candy;
    public int row;
    public int col;
    public bool QuadBlock(){if (GetComponent<QuadBlock>() != null) { return true; } return false;}
    private List<Block> neighbours = new List<Block>();
    public void AddNeighbour(Block block)
    {
        if (!neighbours.Contains(block)) { neighbours.Add(block); }
    }
    public bool FindNeighbours(Block block) { return neighbours.Contains(block); }
    public List<Block> Neighbours_() {return neighbours; }
    public int types;
    public List<GameObject> block = new List<GameObject>();
    private int ccc;
    public bool emptyes=false;
    [SerializeField] Sprite GetSprite1;
    [HideInInspector] public int modelvlsquare;
    public int addScore;
    private bool isblockset=false;
    public Vector3 oldPosition;
    public bool BlockSet(){return isblockset;}
    public bool BlockIs() { return isblockset&&block.Count>0;}
    [HideInInspector]public Block isFullBlock=null;
	public void setBlock(bool set){this.isblockset=set;}
    [SerializeField] private GameObject BlockLight;
    [HideInInspector] private bool isLite = false;
    [HideInInspector] public bool isLiteLine() { return isLite; }
    [HideInInspector] public void SetLiteLine(bool lite) { isLite = lite; BlockLight.gameObject.SetActive(lite); }
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void DestroyBlocks()
    {

    }
}
public class SquareBlocks
{
    public int blck;
    public void Changeblck(int bl) { blck = bl; }
    public int block() { return blck; }
    public int obstacle;
}
