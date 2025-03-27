using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class KitchenSpawner : MonoBehaviour
    {
        [SerializeField] private FoodReserveTable _foodReserve;
        [SerializeField] private CookingSpawner _cookingSpawner;
        [SerializeField] private FoodReadyTable _readyTable;
        public FoodReserveTable FoodReserve=>_foodReserve;
        public CookingSpawner CookingSpawner=>_cookingSpawner;
        public FoodReadyTable ReadyTable=>_readyTable;

        public void Initialize(KitchenSettings kitchenSettings)
        {
            _foodReserve.Init(kitchenSettings.FoodReserveSettings);
            _cookingSpawner.Init(kitchenSettings.CookingSettings);
            _readyTable.Init(kitchenSettings.ReadyTableSettings);
        }
    }
}