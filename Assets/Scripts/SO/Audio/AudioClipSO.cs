using SnakeMaze.Enums;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "AudioCclip", menuName = "Scriptables/Audio/AudioClipSO")]
    public class AudioClipSO : ScriptableObject
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private bool loop;
        [SerializeField] private bool ignorePause;

        public bool IgnorePuase
        {
            get => ignorePause;
            set => ignorePause = value;
        }

        public bool Loop
        {
            get => loop;
            set => loop = value;
        }
        public AudioClip Clip
        {
            get => clip;
            set => clip = value;
        }
        


    }
}
