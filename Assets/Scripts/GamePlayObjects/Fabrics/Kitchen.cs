using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    [System.Serializable]
    public struct Kitchen
    {
        [SerializeField] private KitchenSpawner _kitchenSpawner;
        [SerializeField] private KitchenSettings _kitchenSettings;

        public KitchenSpawner KitchenSpawner => _kitchenSpawner;
        public KitchenSettings KitchenSettings => _kitchenSettings;
    }
}