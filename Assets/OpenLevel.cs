using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
public class OpenLevel : NetworkBehaviour
{
    public static OpenLevel THIS;
    public MoveLayer GetMoveLayer;
    public static OpLvl _instance;
    [SerializeField] MapLevel2[] GetLevels;
    public EventHandler<LevelProp> LevelSelect;
    public EventHandler<LevelProp> LevelReach;
    public MapLevel2 currentLevel;
    [SerializeField] OpAppLvlk GetManager;
    [SerializeField] GameObject NextImage;
    [SerializeField] GameObject buttonImage;
    public bool IsEnabled;
    // Start is called before the first frame update
    void Start()
    {
        GetLevels = FindObjectsOfType<MapLevel2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
            if (hit.collider != null)
            {
                currentLevel = hit.collider.gameObject.GetComponent<MapLevel2>();
                if (currentLevel.islock == false)
                {
                    //GetManager.lvl(currentLevel.Number);
                    {
                        //PortalNetwork.THIS.LeaderBoard(currentLevel.Number);
                        //NextImage.gameObject.SetActive(true);
                        //buttonImage.SetActive(true);
                    }
                    //GetManager.setBlocksType(currentLevel.BlockType);//OpenAppLevel.THIS.StripeGameCount = 0;
                    //GetManager.OnappMatch();//GetTargetLoad(currentLevel.Number);
                }
            }
        }
    }
    public List<MapLevel2> GetMaps(MapLevel2[] maps)
    {
        List<MapLevel2> mapLevels = new List<MapLevel2>();
        foreach (MapLevel2 map in maps)
        {
            mapLevels.Add(map);
        }
        return mapLevels;
    }
    public void UpdateLevels()
    {
        List<MapLevel2> mapLevels = GetMaps(GetLevels);
        foreach (MapLevel2 map in mapLevels)
        {
        }
    }
}
