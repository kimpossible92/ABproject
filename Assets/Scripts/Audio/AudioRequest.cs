using System;
using System.Collections;
using SnakeMaze.Enums;
using SnakeMaze.SO;
using SnakeMaze.SO.Audio;
using UnityEngine;

namespace SnakeMaze.Audio
{
    public class AudioRequest : MonoBehaviour
    {
        [SerializeField] private AudioClipType clipType;
        [SerializeField] private bool playOnStart;

        [SerializeField] private BusAudioSO audioBus;
        [SerializeField] private AudioConfigSO audioConfig;

        private void Start()
        {
            if (playOnStart)
                StartCoroutine(PlayDelayed());
        }

        private IEnumerator PlayDelayed()
        {
            yield return new WaitForSeconds(1f);

            if (playOnStart)
                PlayAudio();
        }

        public void PlayAudio()
        {
            audioBus.OnAudioPlay?.Invoke(clipType, audioConfig);
        }
        private void StopAudio()
        {
            audioBus.OnAudioStop?.Invoke();
        }
        private void FinishAudio()
        {
            audioBus.OnAudioFinish?.Invoke();
        }
    }
}