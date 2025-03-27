using System.Collections.Generic;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    [CreateAssetMenu(menuName = "Configs/Create PlayerStackConfig", fileName = "PlayerStackConfig", order = 0)]
    public class PlayerStackConfig : ScriptableObject
    {
      
        [SerializeField] private int _maxCountItems;
        [SerializeField] private List<Item> _items;
        
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _jumpForce;
        
        public int MaxCountItems=>_maxCountItems;
        public List<Item> Items=>_items;
        
        public float JumpDuration=>_jumpDuration;
        public float JumpForce=>_jumpForce;
       
    }
}