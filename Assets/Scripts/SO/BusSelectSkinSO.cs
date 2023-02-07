using System;
using SnakeMaze.Enums;
using SnakeMaze.UI;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "BusSelectSkin", menuName = "Scriptables/BusSO/BusSelectSkinSO")]
    public class BusSelectSkinSO : ScriptableObject
    {
        public Action<string> OnButtonSelect;
        public Action<SnakeSkinEnum> OnSnakeSkinSelect;
        public Action<MazeSkinEnum> OnMazeSkinSelect;
    }
}
