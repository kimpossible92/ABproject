using UnityEngine;

#if ENABLE_UNET
namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class NetworkManagerHUD_Side : MonoBehaviour {
        public NetSide manager;
        [SerializeField]
        bool showGUI = true;
        [SerializeField]
        int offsetX;
        [SerializeField]
        int offsetY;
        public int LanHostSide = 0;
        public int LanClientSide = 0;
        [HideInInspector]public MapLevel2 currentLevel;
        [SerializeField] OpAppLvlk opAppLvlk;
        [SerializeField] GameObject currentLvlgo;
        bool isStartLevel=false;
        MapLevel2[] levels_;int current;
        // Use this for initialization
        void Awake() {
            manager = GetComponent<NetSide>();
            levels_ = FindObjectsOfType<MapLevel2>(); 
            current = Random.Range(0, levels_.Length);
             currentLevel = levels_[current];
            currentLvlgo.transform.position = currentLevel.transform.position;
           
        }
        private void Start()
        {
            opAppLvlk.gameObject.SetActive(true);
        }
        void OnGUI()
        {
            if (!showGUI)
                return;
            int xpos = 10 + offsetX;
            int ypos = 40 + offsetX;
            int spacing = 25;
            if(!NetworkClient.active && !NetworkServer.active && manager.matchMaker==null)
            {
                if(GUI.Button(new Rect(xpos,ypos,200,20), "Lan Host"))
                {
                    LanHostSide = 1;
                    manager.StartHost();
                    //opAppLvlk.lvl(current);
                    //opAppLvlk.OnappMatch();
                    isStartLevel = true;
                }
                ypos += spacing;
                if(GUI.Button(new Rect(xpos,ypos,100,20),"Lan Client"))
                {
                    LanClientSide = 2;
                    manager.StartClient();
                }
                manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
                ypos += spacing;
                if(GUI.Button(new Rect(xpos,ypos,200,20), "Lan Server Only"))
                {
                    manager.StartServer();
                }
                ypos += spacing;
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), current.ToString()))
                {
                    current = Random.Range(0, levels_.Length);
                    currentLevel = levels_[current]; //currentLvlgo.transform.position = currentLevel.transform.position;
                }
                ypos += spacing;
            }
            else
            {
                if(NetworkServer.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 250, 20), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }
                if(NetworkClient.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 250, 20),"Client: " + manager.networkAddress);
                    ypos += spacing;
                }
            }
            if (NetworkServer.active || NetworkClient.active)
            {
                
            }
        }
    }
};
# endif
