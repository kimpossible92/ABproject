using SnakeMaze.Food;
using SnakeMaze.Interfaces;
using UnityEngine;

namespace SnakeMaze.SO.FoodSO
{
    [CreateAssetMenu(fileName = "FoodPool", menuName = "Scriptables/Food/FoodPoolSO")]
    public class FoodPool : ComponentPoolSO<FoodController>
    {
        [SerializeField] private FoodFactory factory;

        public override IFactory<FoodController> Factory 
        { 
            get=>factory; 
            set=>factory=(FoodFactory)value;
        }
        public override void ResetValues()
        {
            HasBeenPrewarmed = false;
        }
    }
}
