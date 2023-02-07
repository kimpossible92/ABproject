using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road2 : MonoBehaviour
{
    private int pcolor;
    [SerializeField] private Cub Offcorms;
    List<Cub> GetOffcorms = new List<Cub>();
    public int mycolor() { return pcolor; }
    [SerializeField] int Count;
    int saver = 8;
    public int Save => saver;
    public GameObject InstatceOffCorm(float px1, float py1,float pz1, float numScale)
    {
        var worm = Instantiate(Offcorms, new Vector3(px1, py1, pz1), Quaternion.identity);
        worm.transform.localScale = new Vector3(numScale, numScale, numScale);
        worm.Init((Gameplay.Weapons.UnitBattleIdentity)Random.Range(1, 3));
        GetOffcorms.Add(worm.GetComponent<Cub>());
        return worm.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        loadRoad();
    }
    public void loadZero()
    {
        foreach(var p in GetOffcorms)
        {
            if(p!=null)Destroy(p.gameObject);
        }
        GetOffcorms.Clear();
    }
    public void loadRoad()
    {
        saver = 8;
        for (int i = 0; i < Count; i++)
        {
            saver += Random.Range(6, 13);
            InstatceOffCorm(0, -0.378f, saver, 1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
