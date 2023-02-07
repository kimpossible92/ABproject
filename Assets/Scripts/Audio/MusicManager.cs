using System;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private MusicRequest musicRequest;
        [SerializeField] private BusGameManagerSO gameManagerSo;

        private void PlayMusic()
        {
            musicRequest.PlayMusic();
        }

        private void StopMusic()
        {
            musicRequest.StopMusic();
        }

        private void OnEnable()
        {
            gameManagerSo.StartGame += PlayMusic;
            gameManagerSo.PlayerDeath += StopMusic;
            gameManagerSo.WinGame += StopMusic;
        }

        private void OnDisable()
        {
            gameManagerSo.StartGame -= PlayMusic;
            gameManagerSo.PlayerDeath -= StopMusic;
            gameManagerSo.WinGame -= StopMusic;
        }
    }
}
