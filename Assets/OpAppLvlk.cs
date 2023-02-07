using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class OpAppLvlk : NetworkBehaviour
{
    public static OpAppLvlk THIS;
    [SerializeField] GameObject GetButton;
    public int[] ingTarget = new int[2];
    public int[] collectItems = new int[2];
    private int score1;
    private int score2;
    private int score3;
    public int MaxX;
    public int MaxY;
    public static int Scores;
    public int ltype;
    public int t;
    private int currentlvl;
    public int[] ingCtar;
    [SerializeField] UnityEngine.UI.Image NextImage;
    public Block[] blocksp;
    [SerializeField] public SquareBlocks[] Blocksf = new SquareBlocks[81];
    public int colorLimit;
    public int currentLevel() { return currentlvl; }
    [HideInInspector] public int modeLvl = 0;
    [HideInInspector] public int Limit;
    [SerializeField] MoveLayer2 GetMoveLayer;
    [SerializeField] Button NazadButton;
    [SerializeField] GameObject getBlock;
    [SerializeField] GameObject blockpref;
    [SerializeField] GameObject[] Blockprefs;
    public Vector2 vector2position;
    public Vector2 vector2position2;
    public int printScores;
    [SerializeField] private Sprite doubleBlock;
    [SerializeField] GameObject PrefabBlock4;
    [SerializeField] GameObject PrefabBlock5;
    [SerializeField] GameObject PrefabBlock6;
    [SerializeField] GameObject PrefabBlock7;
    [SerializeField] GameObject PrefabBlock8;
    [SerializeField] GameObject LevelParent;
    [SerializeField] GameObject TargetBlockImage;
    [SerializeField] GameObject IngredientsCountImage;
    [SerializeField] GameObject IngredientsCountImage2;
    [SerializeField] Text GetText;
    [SerializeField] Text GetText2;
    [SerializeField] Sprite[] GetSpritesFromItem;
    [HideInInspector] public int blockscount = 0;
    [HideInInspector]
    public int TargetScore;
    public Vector2 sizeBlocks = new Vector2(3, 3);
    public int TargetScore1;
    public int TargetScore2;
    [SerializeField] Text IngredientsCountUIText;
    [SerializeField]
    Text IngredientsCountUIText2;
    [SerializeField] Text GetTextTimer;
    [SerializeField] OpLvl GetLevels2;
    [SerializeField] bool pause = false;
    [SerializeField] Text Play;
    [Tooltip("New Level Scene")]
    [SerializeField] bool isNewScene = false;
    public bool BNewScene() { return isNewScene; }
    public void RestartJ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scenenew");
    }
    public List<Block> GetBlocks1 = new List<Block>();
    public List<Block> ABPointBlocks = new List<Block>();
    public List<Block> ABPoints2 = new List<Block>();
    private int BlocksType = 0;
    public List<List<Vector2>> positions = new List<List<Vector2>>{
        new List<Vector2>{new Vector2(0,0),new Vector2(0,1),new Vector2(1,0)},
        new List<Vector2>{new Vector2(0,0)},
        new List<Vector2>{new Vector2(0,0),new Vector2(1,0)},
        new List<Vector2>{new Vector2(0,0),new Vector2(1,0),new Vector2(0,1)},
        new List<Vector2>{new Vector2(1,1),new Vector2(1,2),new Vector2(1,0)}
    }; 
    public float blckWH() { return 0.6f; }
    public List<List<Block>> blockBlocks = new List<List<Block>>();
    List<Block> matchesSpecial(float pos2)
    {
        List<Block> match = new List<Block>();
        foreach (var pos1 in positions[UnityEngine.Random.Range(0, positions.Count)])
        {
            Vector2 v2 = new Vector2(pos2, vector2position2.y);
            int bt = BlocksType;
            var m1 = Instantiate(Blockprefs[bt], v2 + new Vector2(pos1.x * 0.6f, pos1.y * 0.6f), Quaternion.identity).GetComponent<Block>();
            m1.row = (int)pos1.y;
            m1.col = (int)pos1.x;
            m1.types = 1;
            match.Add(m1);
            GetBlocks1.Add(m1);
        }
        return match;
    }
    List<Block> matchesOne(float pos2)
    {
        List<Block> match = new List<Block>();
        for (int ir = 0; ir < sizeBlocks.y; ir++)
        {
            for (int ic = 0; ic < sizeBlocks.x; ic++)
            {
                Vector2 v2 = new Vector2(pos2, vector2position2.y);
                int bt = BlocksType;
                if (BlocksType >= Blockprefs.Length) { bt = 0; }
                var m1 = (Instantiate(Blockprefs[bt], v2 + new Vector2(ic * blckWH(), ir * blckWH()), Quaternion.identity).GetComponent<Block>());
                m1.row = ir;
                m1.col = ic;
                m1.types = 1;
                int sblovk = UnityEngine.Random.Range(0, 2);
                if (sblovk == 0)
                {
                    m1.setBlock(true);
                    match.Add(m1);
                }
                else
                {
                    m1.setBlock(false);
                    Destroy(m1.gameObject);
                }

            }
        }
        return match;
    }
    public void NewBlockMatch()
    {
        if (isNewScene)
        {
            blockBlocks.Add(matchesSpecial(vector2position2.x));
            blockBlocks.Add(matchesSpecial(0));
            blockBlocks.Add(matchesSpecial(1.8f));
        }
    }
    public void RandomBlockMatch()
    {
        if (isNewScene)
        {
            blockBlocks.Add(matchesOne(vector2position2.x));
            blockBlocks.Add(matchesOne(0));
            blockBlocks.Add(matchesOne(2.2f));
        }
    }
    void Start()
    {
        //lvl(currentlvl);
        //OnappMatch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lvl(int number)
    {
        TextAsset text = (TextAsset)Resources.Load("" + number);
        if (text == null) text = Resources.Load("" + number) as TextAsset;
        openLeveltxt(text.text);
    }

    private void setBlocksType(int set){this.BlocksType = set;}

    public void OnappMatch()
    {
        {
            //GetMoveLayer.setwinFalse();
            //NazadButton.gameObject.SetActive(false);//
            //GetMoveLayer.gameObject.SetActive(true);
            //GetMoveLayer.loadMove(false);
        }
        if (this.isServer)
        {
            for (int row = 0; row < MaxY; row++)
            {
                for (int col = 0; col < MaxX; col++)
                {
                    Createblock(col, row);
                }
            }
        }
    }
    public void Createblock(int i, int j)//
    {
        var fObstacles = FindObjectsOfType<MObstacle>();
        GameObject vblck = fObstacles[(i + MaxX) + j].gameObject;
        blocksp[j * MaxX + i] = vblck.GetComponent<Block>();
        vblck.GetComponent<Block>().row = j;
        vblck.GetComponent<Block>().col = i;
        vblck.GetComponent<Block>().types = 1; 
        if (Blocksf[j * MaxX + i].block() == 0)
        {
            int ranObstacleSet = UnityEngine.Random.Range(0, 3);
            if (ranObstacleSet==0) { vblck.tag = "wall"; vblck.GetComponent<Renderer>().material.color = Color.red; }
            else if (ranObstacleSet==1) { vblck.tag = "corm"; }
            else if (ranObstacleSet==2) {  }
        }
    }
    public void openLeveltxt(string mapText)
    {
        string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int mapline = 0;
        foreach (string line in lines)
        {
            if (line.StartsWith("MODE"))
            {
                string modeSting = line.Replace("MODE", string.Empty).Trim();
            }
            else if (line.StartsWith("SIZE"))
            {
                string blockString = line.Replace("SIZE", string.Empty).Trim();
                string[] sizes = blockString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (line.StartsWith("LIMIT"))
            {
                string blockString = line.Replace("LIMIT", string.Empty).Trim();
                string[] sizes = blockString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (line.StartsWith("COLOR LIMIT "))
            {
                string blockString = line.Replace("COLOR LIMIT", string.Empty).Trim();
            }
            else if (line.StartsWith("STARS"))
            {
                string starsline = line.Replace("STARS", string.Empty).Trim();
                string[] starsNumbers = starsline.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (line.StartsWith("COLLECT COUNT"))
            {
                string blocksString = line.Replace("COLLECT COUNT", string.Empty).Trim();
                string[] blocksNumbers = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < blocksNumbers.Length; i++)
                {
                }
            }
            else if (line.StartsWith("COLLECT ITEMS"))
            {
                string blocksString = line.Replace("COLLECT ITEMS", string.Empty).Trim();
                string[] blocksNumbers = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < blocksNumbers.Length; i++)
                {
                }
            }
            else
            {
                string[] st = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < st.Length; i++)
                {
                    Blocksf[(mapline * MaxX) + i].blck = int.Parse(st[i][0].ToString());//
                    Blocksf[(mapline * MaxX) + i].obstacle = int.Parse(st[i][1].ToString());//
                }
                mapline++;
            }
        }
    }
}
