using GamePlayObjects.Fabrics;
using UnityEngine;

namespace DefaultNamespace
{
    [System.Serializable]
    public struct SpawnerSetting
    {
        [SerializeField] private StackConfig _configs;
        [SerializeField] private float _deliveryTime;
        [SerializeField] private float _height;
        
        public StackConfig Configs=>_configs;
        public float DeliveryTime=>_deliveryTime;
        public float Height=>_height;
        
    }
}