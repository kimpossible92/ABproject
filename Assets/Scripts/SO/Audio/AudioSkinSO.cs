using System.Collections.Generic;
using SnakeMaze.Enums;
using UnityEngine;

namespace SnakeMaze.SO.Audio
{
    [CreateAssetMenu(fileName = "AudioSkin",menuName = "Scriptables/Audio/AudioSkin")]
    public class AudioSkinSO : InitiableSO
    {
        [SerializeField] private AudioClipSO gameMusic;

        [SerializeField] private AudioClipSO menuMusic;

        [SerializeField] private AudioClipSO boostIn;

        [SerializeField] private AudioClipSO boostOut;

        [SerializeField] private AudioClipSO death;

        [SerializeField] private AudioClipSO eat;

        [SerializeField] private AudioClipSO tapUi;


        private Dictionary<AudioClipType, AudioClipSO> _audioDic;

        public Dictionary<AudioClipType, AudioClipSO> AudioDic
        {
            get=> _audioDic;
            set=> _audioDic= value;
        }

        private void InitDic()
        {
            _audioDic = new Dictionary<AudioClipType, AudioClipSO>()
            {
                {AudioClipType.GameMusic, gameMusic},
                {AudioClipType.MenuMusic, menuMusic},
                {AudioClipType.BoostIn, boostIn},
                {AudioClipType.BoostOut, boostOut},
                {AudioClipType.Death, death},
                {AudioClipType.Eat, eat},
                {AudioClipType.TapUi, tapUi}
            };
        }

        public override void InitScriptable()
        {
            InitDic();
        }
    }
}
