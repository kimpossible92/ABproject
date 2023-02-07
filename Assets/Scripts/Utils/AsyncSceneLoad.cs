using System;
using System.Collections;
using SnakeMaze.SO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SnakeMaze.Utils
{
    public class AsyncSceneLoad : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private EventSO playFabServerResponse;
        [SerializeField] private bool waitForPlayfab;
        [SerializeField] private int sceneToLoad = 2;

        private bool _playfabFinished;

        private void Start()
        {
            if (waitForPlayfab)
                StartCoroutine(AsyncLoadSceneWithPlayFab(sceneToLoad));
            else
                StartCoroutine(AsyncLoadScene(sceneToLoad));
        }

        private IEnumerator AsyncLoadScene(int sceneToLoad)
        {
            yield return new WaitForSecondsRealtime(1f);
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneToLoad);
            while (!gameLevel.isDone)
            {
                var progress = gameLevel.progress / 0.9f;
                fillImage.fillAmount = progress;
                yield return null;
            }
        }

        private IEnumerator AsyncLoadSceneWithPlayFab(int sceneToLoad)
        {
            yield return new WaitForSecondsRealtime(1f);
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneToLoad);
            gameLevel.allowSceneActivation = false;
            while (!_playfabFinished)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(.5f);

            gameLevel.allowSceneActivation = true;
        }

        public void OnFinishPlayFab()
        {
            _playfabFinished = true;
        }

        private void OnEnable()
        {
            if (playFabServerResponse != null)
                playFabServerResponse.CurrentAction += OnFinishPlayFab;
        }

        private void OnDisable()
        {
            if (playFabServerResponse != null)
                playFabServerResponse.CurrentAction -= OnFinishPlayFab;
        }
    }
}