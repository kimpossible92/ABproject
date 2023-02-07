using System;
using System.Collections;
using DG.Tweening;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.Audio
{
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        public Action<SoundEmitter> OnFinishedPlaying;

        public AudioSource CurrentAudioSource=>_audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
        [ContextMenu("Play")]
        private void PlayMusicTest()
        {
            _audioSource.Play();
        }
        [ContextMenu("Stop")]
        private void StopMusicTest()
        {
            _audioSource.Stop();
        }

        public void PlayAudioClip(AudioClip clip, AudioConfigSO settings, bool loop)
        {
            _audioSource.clip = clip;
            settings.ApplyTo(_audioSource);
            _audioSource.loop = loop;
            _audioSource.time = 0;
            _audioSource.Play();

            if (!loop)
            {
                StartCoroutine(FinishedPlaying(clip.length));
            }
        }
        
        public void FadeMusicIn(AudioClip musicClip, AudioConfigSO settings, float startTime = 0f)
        {
            PlayAudioClip(musicClip, settings, true);

            if (startTime <= _audioSource.clip.length)
                _audioSource.time = startTime;
        }

        public float FadeMusicOut(float duration)
        {
            _audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

            return _audioSource.time;
        }

        private void OnFadeOutComplete()
        {
            NotifyFinished();
        }

        public bool IsLooping() => _audioSource.loop;
        public bool IsPlaying() => _audioSource.isPlaying;
        public AudioClip GetClip() => _audioSource.clip;
        
        
        public void Resume() => _audioSource.Play();
        public void Pause() => _audioSource.Pause();
        public void Stop() => _audioSource.Stop();

        public void StopMusic()
        {
            _audioSource.Stop();
            NotifyFinished();
        }

        public void Finish()
        {
            if (!_audioSource.loop) return;

            _audioSource.loop = false;
            float timeRemaining = _audioSource.clip.length - _audioSource.time;
            StartCoroutine(FinishedPlaying(timeRemaining));
        }

        private IEnumerator FinishedPlaying(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);

            NotifyFinished();
        }

        private void NotifyFinished()
        {
            OnFinishedPlaying.Invoke(this);
        }
    }
}
