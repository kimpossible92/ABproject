using UnityEngine;
using System.Collections;
//using Facebook.Unity;

public class MenuGUI : MonoBehaviour
{
    public void MenuTounamentClick()
    {
        //if (Facebook.Unity.FB.IsLoggedIn)
        //{
        //    if (TournamentLB.THIS != null)// && TounamentLB.THIS.playerNames != null)
        //    {
        //        TournamentLB.THIS.Cleared();
        //    }
        //    PortalNetwork.THIS.GetTournamer();//
        //    PortalNetwork.THIS.GetPlayersTournament();//
        //    //print(Facebook.Unity.FB.IsLoggedIn);
        //    GameObject.Find("CanvasGlobal").transform.Find("TournamentWindow").gameObject.SetActive(true);
        //    Tournament.tournament.kubokButton();
        //    TournamentLB.THIS = Tournament.tournament.TourametWindow.GetComponent<TournamentLB>();
        //    Tournament.tournament.TourametWindow.GetComponent<TournamentLB>().OnSwitch();
        //}
        //else
        {
            GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").gameObject.SetActive(true);
            GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").transform.Find("Image").transform.Find("Join").gameObject.SetActive(true);
            GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").transform.Find("Image").transform.Find("CheckTarget").transform.Find("Check").gameObject.SetActive(true);
            Tournament.tournament.kubokButton();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerPrefs.SetInt("TourQual", (PlayerPrefs.GetInt("TourQual")+10));
        }
    }
}
