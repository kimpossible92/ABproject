using System;
using SnakeMaze.BSP;
using SnakeMaze.Maze;
using SnakeMaze.SO.FoodSO;
using UnityEngine;

namespace SnakeMaze.Food
{
    [RequireComponent(typeof(Collider2D))]
    public class FoodController : MonoBehaviour
    {
        [SerializeField] private BusFoodSO busFood;
        [SerializeField] private int points;
        private MazeCell _cell;
        private Room _currentRoom;

        public MazeCell Cell
        {
            get => _cell;
            set => _cell = value;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            busFood.OnEatFood?.Invoke(this);
            busFood.OnEatFoodNoArg?.Invoke();
            busFood.OnEatFoodPoints?.Invoke(points);
        }
    }
}
