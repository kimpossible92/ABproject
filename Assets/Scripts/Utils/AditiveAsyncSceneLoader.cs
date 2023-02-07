using SnakeMaze.SO.PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SnakeMaze.Utils
{
    public class AditiveAsyncSceneLoader : MonoBehaviour
    {
        [SerializeField] private BusServerCallSO serverCallSo;
        private bool _sceneLoaded;
        private AsyncOperation _loadSceneOperation = new AsyncOperation();
        

        private void LoadWaitForServerResponse()
        {
            if (_sceneLoaded) return;
            SceneManager.LoadScene(Constants.WaitServerScene, LoadSceneMode.Additive);
            _sceneLoaded = true;
        }

        private void AsyncUnloadWaitForServerResponse()
        {
            if (!_sceneLoaded) return;
            _loadSceneOperation = SceneManager.UnloadSceneAsync(Constants.WaitServerScene);
            // _loadSceneOperation.completed += SceneUnloaded;
            _sceneLoaded = false;
            
        }

        private void SceneUnloaded(AsyncOperation operation)
        {
            _sceneLoaded = false;
        }

        private void OnEnable()
        {
            serverCallSo.OnServerCall += LoadWaitForServerResponse;
            serverCallSo.OnServerResponse += AsyncUnloadWaitForServerResponse;
        }
        private void OnDisable()
        {
            serverCallSo.OnServerCall -= LoadWaitForServerResponse;
            serverCallSo.OnServerResponse -= AsyncUnloadWaitForServerResponse;
        }
    }
}
