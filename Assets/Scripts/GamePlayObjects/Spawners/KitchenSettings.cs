using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayObjects.Fabrics
{
    [System.Serializable]
    public struct KitchenSettings
    {
        [SerializeField] private SpawnerSetting _foodReserveSettings;
        [SerializeField] private SpawnerSetting _cookingSettings;
        [SerializeField] private SpawnerSetting _readyTableSettings;
        
        public SpawnerSetting FoodReserveSettings=>_foodReserveSettings;
      
        public SpawnerSetting CookingSettings=>_cookingSettings;
      
        public SpawnerSetting ReadyTableSettings=>_readyTableSettings;
    }
}