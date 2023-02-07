using UnityEngine;
using UnityEngine.Audio;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Scriptables/Audio/AudioConfigSO")]
    public class AudioConfigSO : ScriptableObject
    {
        [SerializeField] private AudioMixerGroup audioMixerGroup;
        [Range(0f, 1f)] [SerializeField] private float volume;
        [Range(-3f, 3f)] [SerializeField] private float pitch;
        [Range(-1f, 1f)] [SerializeField] private float panStereo;
        [Range(0f, 1.1f)] [SerializeField] private float reverbZoneMix;
        [SerializeField] private bool randomPitch;
        [SerializeField] private float minPitch, maxPitch;
        [SerializeField] private bool ignorePause;
        [SerializeField] private float fadeOutTime = 2f;
        [SerializeField] private float fadeInTime = 1f;

        public float Volume
        {
            get => volume;
            set=> volume=value;
        }
        public float Pitch
        {
            get
            {
                if(!randomPitch)
                    return pitch;
                else
                {
                    return Random.Range(minPitch, maxPitch);
                }
            }
            set=> pitch=value;
        }

        public float FadeOutTime
        {
            get => fadeOutTime;
            set => fadeOutTime = value;
        }
        public float FadeInTime
        {
            get => fadeInTime;
            set => fadeInTime = value;
        }

        public void ApplyTo(AudioSource audioSource)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.volume = volume;
            audioSource.pitch = Pitch;
            audioSource.panStereo = panStereo;
            audioSource.reverbZoneMix = reverbZoneMix;
            audioSource.ignoreListenerPause = ignorePause;

        }
    }
}
