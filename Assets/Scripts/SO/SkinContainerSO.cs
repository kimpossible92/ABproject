using System;
using System.Collections.Generic;
using SnakeMaze.Enums;
using SnakeMaze.SO.Audio;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "SkinContainer", menuName = "Scriptables/SkinContainerSO")]
    public class SkinContainerSO : InitiableSO
    {
        [SerializeField] private MazeSkinSO defaultMazeSkin;
        [SerializeField] private MazeSkinSO spaceMazeSkin;
        [SerializeField] private MazeSkinSO skin3MazeSkin;
        [SerializeField] private SnakeSkinSO defaultSnakeSkin;
        [SerializeField] private SnakeSkinSO astronautSnakeSkin;
        [SerializeField] private SnakeSkinSO skin3SnakeSkin;
        [SerializeField] private AudioSkinSO defaultAudioSkin;
        [SerializeField] private BusSelectSkinSO busSnakeSelectSkinSo;
        [SerializeField] private BusSelectSkinSO busMazeSelectSkinSo;

        private Dictionary<MazeSkinEnum, MazeSkinSO> _mazeSkinDic;
        private Dictionary<SnakeSkinEnum, SnakeSkinSO> _snakeSkinDic;
        private Dictionary<AudioSkinEnum, AudioSkinSO> _audioSkinDic;

        public MazeSkinSO CurrentMazeSkin { get; set; }
        public SnakeSkinSO CurrentSnakeSkin { get; set; }
        public AudioSkinSO CurrentAudioSkin { get; set; }

        public Dictionary<MazeSkinEnum, MazeSkinSO> MazeSkinDic => _mazeSkinDic;
        public Dictionary<SnakeSkinEnum, SnakeSkinSO> SnakeSkinDic => _snakeSkinDic;
        public Dictionary<AudioSkinEnum, AudioSkinSO> AudioSkinDic => _audioSkinDic;

        public override void InitScriptable()
        {
            InitDics();
            CurrentMazeSkin = _mazeSkinDic[MazeSkinEnum.Default];
            CurrentSnakeSkin = _snakeSkinDic[SnakeSkinEnum.Default];
            CurrentAudioSkin = _audioSkinDic[AudioSkinEnum.Default];
            InitCurrentSkin();
        }
        
        private void InitDics()
        {
            _mazeSkinDic = new Dictionary<MazeSkinEnum, MazeSkinSO>()
            {
                {MazeSkinEnum.Default, defaultMazeSkin},
                {MazeSkinEnum.Space, spaceMazeSkin},
                {MazeSkinEnum.Skin3, skin3MazeSkin}
            };
            _snakeSkinDic = new Dictionary<SnakeSkinEnum, SnakeSkinSO>()
            {
                {SnakeSkinEnum.Default, defaultSnakeSkin},
                {SnakeSkinEnum.Astronaut, astronautSnakeSkin},
                {SnakeSkinEnum.Skin3, skin3SnakeSkin}
            };
            _audioSkinDic = new Dictionary<AudioSkinEnum, AudioSkinSO>()
            {
                {AudioSkinEnum.Default, defaultAudioSkin}
            };
        }

        private void InitCurrentSkin()
        {
            CurrentAudioSkin.InitScriptable();
            CurrentMazeSkin.InitScriptable();
            CurrentSnakeSkin.InitScriptable();
        }
        
        public void ChangeMazeSkin(MazeSkinEnum newSkin)
        {
            CurrentMazeSkin = _mazeSkinDic[newSkin];
            CurrentMazeSkin.InitScriptable();
        }
        public void ChangeSnakeSkin(SnakeSkinEnum newSkin)
        {
            CurrentSnakeSkin = _snakeSkinDic[newSkin];
            CurrentSnakeSkin.InitScriptable();
        }
        public void ChangeAudioSkin(AudioSkinEnum newSkin)
        {
            CurrentAudioSkin = _audioSkinDic[newSkin];
            CurrentAudioSkin.InitScriptable();
        }

        private void OnEnable()
        {
            busSnakeSelectSkinSo.OnSnakeSkinSelect += ChangeSnakeSkin;
            busMazeSelectSkinSo.OnMazeSkinSelect += ChangeMazeSkin;
        }
        private void OnDisable()
        {
            busSnakeSelectSkinSo.OnSnakeSkinSelect -= ChangeSnakeSkin;
            busMazeSelectSkinSo.OnMazeSkinSelect -= ChangeMazeSkin;
        }
    }
}