using System;
using SnakeMaze.Food;
using UnityEngine;

namespace SnakeMaze.SO.FoodSO
{
    [CreateAssetMenu(fileName = "BusFood", menuName = "Scriptables/Food/BusFoodSO")]
    public class BusFoodSO : ScriptableObject
    {
        public Action<FoodController> OnEatFood;
        public Action<int> OnEatFoodPoints;
        public Action OnEatFoodNoArg;
    }
}
