using System;
using SnakeMaze.Enums;
using UnityEngine;

namespace SnakeMaze.SO.Audio
{
    [CreateAssetMenu(fileName = "BusAudio",menuName = "Scriptables/Audio/BusAudioSO")]
    public class BusAudioSO : ScriptableObject
    {
        public Action<AudioClipType, AudioConfigSO> OnAudioPlay;
        public Action OnAudioFinish;
        public Action OnAudioStop;
        public Action<AudioConfigSO> OnMusicStop;
        public Action<bool> MuteAudio;
    }
}
