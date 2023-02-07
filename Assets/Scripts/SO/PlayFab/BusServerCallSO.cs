using System;
using UnityEngine;

namespace SnakeMaze.SO.PlayFab
{
    [CreateAssetMenu(fileName = "BusServerCall", menuName = "Scriptables/BusSO/BusServerCallSO")]
    public class BusServerCallSO : ScriptableObject
    {
        public Action OnServerCall;
        public Action OnServerResponse;
    }
}
