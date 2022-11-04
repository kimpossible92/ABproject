using Gameplay.ShipSystems;
using Gameplay.Spaceships;
using Gameplay.Weapons;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cubick : MonoBehaviour, ISpaceship, IDamagable
{
    [SerializeField]
    private Text textScore;
    [SerializeField]
    private Sprite[] bonusSprites;
    [SerializeField]
    private PlayerTowerController playerTowerCon;
    [SerializeField]
    private MovementSystem _movementSystem;
    private Vector3 posStart;
    [SerializeField]
    private float lvllive = 0; 
    protected int bonustype = 0;
    [SerializeField]
    private ProjectilePool projectile;
    public int bonusRead => bonustype;
    public MovementSystem MovementSystem => _movementSystem;
    [SerializeField]
    private UnitBattleIdentity _battleIdentity; 
    public static Cubick THIS;
    public string urlOnTournament;
    public UnitBattleIdentity BattleIdentity => _battleIdentity;
    [SerializeField] private List<ProjectilePool> pools = new List<ProjectilePool>();
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;

    // Use this for initialization
    void Start()
    {
        THIS = this;
        //projectile.name = "0";
        //pools.Add(projectile); 
        //pools = pools.OrderBy(x => x.name).ToList();
        //foreach (var p in pools)
        //{
        //    p.transform.localPosition = new Vector3(0, int.Parse(p.name), 0);
        //}
        playerTowerCon.Init(this);
        posStart = transform.position;
        lvllive = 0; playerTowerCon.SetMenu(true);
    }
    public void SaveBombStripePackage()
    {
        //if (OpenAppLevel.THIS.currentlvl > 20 && Facebook.Unity.FB.IsLoggedIn || LevelManager.THIS.currentLevel == 21 && Facebook.Unity.FB.IsLoggedIn)
        //{
        PlayerPrefs.SetInt("TourQual", PlayerPrefs.GetInt("TourQual") + (int)lvllive);
        //}
        //if (LevelManager.THIS.currentLevel > 20 || LevelManager.THIS.currentLevel == 21)
        //{
        string url1 = THIS.urlOnTournament.Replace(Tournament.urlpredicate, string.Empty);
        SaveControl();
        Debug.Log("currentScore:" + lvllive);
        //}
    }
    public void SaveControl()
    {
        //if (Facebook.Unity.FB.IsLoggedIn) PortalNetwork.THIS.SendScoreLevel(0, PlayerPrefs.GetInt("TourQual")*100);
    }
    public void ApplyDamage(IDamageDealer damageDealer)
    {
        if (damageDealer.BattleIdentity == UnitBattleIdentity.Enemy)
        {
            
            if (lvllive == 0) {
                foreach (var pi in pools)
                {
                    Destroy(pi.gameObject);
                }
                pools.Clear();
                transform.position = posStart;
                playerTowerCon.SetMenu(true);
                reload = false;
                /*p.gameObject.SetActive(false); */
            }
            else
            {
                lvllive -= damageDealer.Damage;
                var p = pools.Find(c => c.gameObject.name == lvllive.ToString());
                p.gameObject.transform.SetParent(FindObjectOfType<Road>().transform);
                p.transform.localPosition = transform.localPosition;
                p.GetComponent<Renderer>().material = material2;
                pools.Remove(p);
            }
            
        }
        if (damageDealer.BattleIdentity == UnitBattleIdentity.Ally)
        {
            var cub = Instantiate(projectile, transform);
            cub.transform.localPosition = Vector3.forward;
            pools.Add(cub);
            var pls = pools.Find(c => c.gameObject.name == "0");
            if (pls != null) { pools.Find(c => c.gameObject.name == "0").name = lvllive.ToString(); }
            lvllive += damageDealer.Damage;
            cub.name = "0";
            cub.GetComponent<Renderer>().material = material1;
            pools.OrderBy(x => x.name);
            foreach (var  p in pools)
            {
                p.transform.localPosition = new Vector3(0, int.Parse(p.name),0)+new Vector3(0, 1, 0);
            }
           
        }
        if (lvllive <= 1)
        {
            //int randBonus = UnityEngine.Random.Range(0, 5);
            //if (randBonus == 0) { tag = "bonus"; bonustype = UnityEngine.Random.Range(0, bonusSprites.Length); gameObject.transform.Find("Hull").GetComponent<SpriteRenderer>().sprite = bonusSprites[bonustype]; }
            //else { /*FindObjectOfType<UIPlay>().addScore(UnityEngine.Random.Range(5, 15)); GetComponent<EnemySp>().GetSpawner.lvlplus(); */Destroy(gameObject); }
        }
    }
    [SerializeField] GameObject button1;
    public void SetLive()
    {
        lvllive = 0; playerTowerCon.SetMenu(false);
    }
    bool reload = false;
    // Update is called once per frame
    void Update()
    {
        scrollbar.size = transform.position.z / FindObjectOfType<Road>().Save;
        button1.gameObject.SetActive(playerTowerCon.Menu);
        if (playerTowerCon.Menu && !reload)
        {
            FindObjectOfType<Road>().loadZero();
            FindObjectOfType<Road>().loadRoad();
            reload = true;
        }
        else { playerTowerCon.PerFrameUpdate(); }
        textScore.text = lvllive.ToString();
        if (transform.position.z > FindObjectOfType<Road>().Save)
        {
            SaveBombStripePackage();
            transform.position = posStart;
            playerTowerCon.SetMenu(true);
            reload = false;
            //print("finish");
        }
    }
    bool ingredientFly = false;
    IEnumerator StartAnimateIngredientOther(GameObject item,GameObject OtheringrObject)
    {
        //if (ingrCountTarget[i] > 0)
        //ingrCountTarget[i]--;

        ingredientFly = true;
        GameObject ingr = OtheringrObject.gameObject;//

        //if (targetBlocks > 0)
        //{
        //    ingr = blocksObject.transform.gameObject;
        //}
        AnimationCurve curveX = new AnimationCurve(new Keyframe(0, item.transform.localPosition.x), new Keyframe(0.4f, ingr.transform.position.x));
        AnimationCurve curveY = new AnimationCurve(new Keyframe(0, item.transform.localPosition.y), new Keyframe(0.5f, ingr.transform.position.y));
        curveY.AddKey(0.2f, item.transform.localPosition.y + UnityEngine.Random.Range(-2, 0.5f));
        float startTime = Time.time;
        Vector3 startPos = item.transform.localPosition;
        float speed = UnityEngine.Random.Range(0.2f, 0.3f);
        float distCovered = 0;
        while (distCovered < 0.5f)
        {
            distCovered = (Time.time - startTime) * speed;
            item.transform.localPosition = new Vector3(curveX.Evaluate(distCovered), curveY.Evaluate(distCovered), 0);
            item.transform.Rotate(Vector3.back, Time.deltaTime * 1000);
            yield return new WaitForFixedUpdate();
        }
        //     SoundBase.Instance.audio.PlayOneShot(SoundBase.Instance.getStarIngr);
        //Destroy(item);
        //if (gameStatus == GameState.Playing && !IsIngredientFalling())//1.6.1
        //    CheckWinLose();
        ingredientFly = false;
    }
    private void OnGUI()
    {
        //string strcount = GUI.TextField(new Rect(20, 40, 120, 40), string.Format("Score: {0}", lvllive));
    }
}