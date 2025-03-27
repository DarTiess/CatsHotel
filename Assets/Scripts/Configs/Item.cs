using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayObjects.Fabrics
{
    [System.Serializable]
    public class Item
    {
        [FormerlySerializedAs("_itemPrefab")]
        [SerializeField] private ItemObject _itemObject;
        [SerializeField] private ItemType _type;
        public ItemObject itemObject=>_itemObject;
        public ItemType Type=>_type;

        public Item(ItemObject obj, ItemType type)
        {
            _itemObject = obj;
            _type = type;
        }
        
    }
}