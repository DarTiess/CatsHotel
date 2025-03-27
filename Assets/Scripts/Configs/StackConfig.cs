using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    [CreateAssetMenu(menuName = "Configs/Create StackConfig", fileName = "StackConfig", order = 0)]
    public class StackConfig : ScriptableObject
    {
      
        [SerializeField] private int _maxCountItems;
        [SerializeField] private ItemObject _itemPrefab;
        
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _jumpForce;
        [SerializeField] private ItemType _type;
        
        public int MaxCountItems=>_maxCountItems;
        public ItemObject ItemPrefab=>_itemPrefab;
        
        public float JumpDuration=>_jumpDuration;
        public float JumpForce=>_jumpForce;
        public ItemType Type=>_type;
    }
}