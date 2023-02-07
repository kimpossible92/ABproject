using System;
using SnakeMaze.Enums;
using SnakeMaze.SO;
using SnakeMaze.SO.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace SnakeMaze.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private SkinContainerSO skinContainer;
        [SerializeField] private BusAudioSO busSfx;
        [SerializeField] private BusAudioSO busMusic;
        [SerializeField] private SoundEmitterPoolSO pool;
        [SerializeField] private AudioMixerGroup musicGorup;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private int initSize = 4;

        private SoundEmitter _musicEmitter;

        private void Awake()
        {
            pool.Prewarm(initSize);
            pool.SetParent(transform);
        }

        private void Start()
        {
            InitMixerGroups();
        }

        private void OnEnable()
        {
            busSfx.OnAudioPlay += PlayAudioClip;

            busMusic.OnAudioPlay += PlayMusic;
            busMusic.OnMusicStop += StopMusic;

            busMusic.MuteAudio += MuteMusic;
            busSfx.MuteAudio += MuteSfx;

        }
        private void OnDestroy()
        {
            busSfx.OnAudioPlay -= PlayAudioClip;

            busMusic.OnAudioPlay -= PlayMusic;
            busMusic.OnMusicStop -= StopMusic;
            
            busMusic.MuteAudio -= MuteMusic;
            busSfx.MuteAudio -= MuteSfx;
        }

        private void InitMixerGroups()
        {
            var musicVolume = PlayerPrefs.GetInt("MuteMusic") == 1 ? -80 : 0;
            var sfxVolume = PlayerPrefs.GetInt("MuteSfx")== 1 ? -80 : 0;
            audioMixer.SetFloat("VolumeMusicGroup", musicVolume);
            audioMixer.SetFloat("VolumeSFXGroup", sfxVolume);
        }

        public void MuteMusic(bool value)
        {
            var muteMusic = 0;
            var musicVolume = 0;
            if (value)
            {
                muteMusic = 1;
                musicVolume = -80;
            }
            PlayerPrefs.SetInt("MuteMusic", muteMusic );
            audioMixer.SetFloat("VolumeMusicGroup", musicVolume);
            PlayerPrefs.Save();
        }
        public void MuteSfx(bool value)
        {
            var muteSfx = 0;
            var sfxVolume = 0;
            if (value)
            {
                muteSfx = 1;
                sfxVolume = -80;
            }
            PlayerPrefs.SetInt("MuteSfx", muteSfx );
            audioMixer.SetFloat("VolumeSFXGroup", sfxVolume);
            PlayerPrefs.Save();
        }

        private void PlayMusic(AudioClipType clipType, AudioConfigSO settings)
        {
            AudioClipSO clip = skinContainer.CurrentAudioSkin.AudioDic[clipType];
            if (_musicEmitter != null && _musicEmitter.IsPlaying())
            {
                if (_musicEmitter.GetClip() == clip.Clip)
                    return;
                _musicEmitter.StopMusic();
            }

            _musicEmitter = pool.Request();
            _musicEmitter.CurrentAudioSource.outputAudioMixerGroup = musicGorup;
            _musicEmitter.PlayAudioClip(clip.Clip, settings, true);
            _musicEmitter.OnFinishedPlaying += StopMusicEmitter;
        }

        private void PlayAudioClip(AudioClipType clipType, AudioConfigSO settings)
        {
            AudioClipSO clip = skinContainer.CurrentAudioSkin.AudioDic[clipType];
            SoundEmitter soundEmitter = pool.Request();
            soundEmitter.CurrentAudioSource.outputAudioMixerGroup = sfxGroup;
            if (soundEmitter != null)
            {
                soundEmitter.PlayAudioClip(clip.Clip, settings, clip.Loop);
                soundEmitter.OnFinishedPlaying += OnSoundEmitterFinished;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        private void StopMusic(AudioConfigSO settings)
        {

            if (_musicEmitter == null || !_musicEmitter.IsPlaying()) return;
            _musicEmitter.OnFinishedPlaying += StopMusicEmitter;
            _musicEmitter.StopMusic();

        }

        private void OnSoundEmitterFinished(SoundEmitter soundEmitter)
        {
            StopAndCleanEmitter(soundEmitter);
        }

        private void StopAndCleanEmitter(SoundEmitter soundEmitter)
        {
            if (!soundEmitter.IsLooping())
                soundEmitter.OnFinishedPlaying -= OnSoundEmitterFinished;
            soundEmitter.Stop();
            pool.Return(soundEmitter);
        }

        private void StopMusicEmitter(SoundEmitter soundEmitter)
        {
            soundEmitter.OnFinishedPlaying -= StopMusicEmitter;
            pool.Return(soundEmitter);
        }
    }
}
