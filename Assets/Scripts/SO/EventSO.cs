using System;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName ="EventSO", menuName = "Scriptables/EventSO")]
    public class EventSO : ScriptableObject
    {
        public Action CurrentAction;
    }
}