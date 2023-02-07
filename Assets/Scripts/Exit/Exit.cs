using System;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.Exit
{
    [RequireComponent(typeof(Collider2D))]
    public class Exit : MonoBehaviour
    {
        [SerializeField] private BusGameManagerSO busGameManager;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            busGameManager.WinGame?.Invoke();

        }
    }
}
