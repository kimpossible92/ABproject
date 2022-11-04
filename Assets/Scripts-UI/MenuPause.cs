using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPause : MonoBehaviour {

    public static bool jPause = false;

    public GameObject menuDePausaUI;

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (jPause)
            {
                respawn();
            }
            else
            {
                pause();
            }

        }

	}

    public void respawn()
    {
        menuDePausaUI.SetActive(false);
        Time.timeScale = 1f; //El tiempo se ejecuta de manera normal de nuevo.
        jPause = false;
    }

    private void pause()
    {
        menuDePausaUI.SetActive(true);
        Time.timeScale = 0f; //Pausamos el tiempo del juego.
        jPause = true;
    }
    public void SetNewScene()
    {
        SceneManager.LoadScene("scenenew");
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}

