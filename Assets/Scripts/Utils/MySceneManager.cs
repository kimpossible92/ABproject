using UnityEngine;
using UnityEngine.SceneManagement;

namespace SnakeMaze.Utils
{
    public class MySceneManager : MonoBehaviour
    {

        public void LoadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void LoadLoadingScene()
        {
            SceneManager.LoadScene("LoadingScene");
        }

        public void LoadScene(string sceneToLoad = "LoadingScene")
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        public void LoadScene(int sceneToLoad = 0)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
