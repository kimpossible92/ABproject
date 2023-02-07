using SnakeMaze.Food;
using UnityEngine;

namespace SnakeMaze.SO.FoodSO
{
    [CreateAssetMenu(fileName = "FoodFactory", menuName = "Scriptables/Food/FoodFactorySO")]
    public class FoodFactory : FactorySO<FoodController>
    {
        [SerializeField] private FoodController prefab;
        public override FoodController Create()
        {
            return Instantiate(prefab);
        }
    }
}
