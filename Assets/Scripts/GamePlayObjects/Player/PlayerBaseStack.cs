using System;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Fabrics;
using UnityEngine;

namespace GamePlayObjects.Player
{
    public abstract class PlayerBaseStack: MonoBehaviour
    {
        protected List<Transform> _itemsPlaces;
        protected int _indexPlace;
        protected int _maxCountItems;
        private Dictionary<ItemType,ItemObject> _itemPrefab;
        protected float _jumpDuration;
        protected float _jumpForce;
        protected ItemType _type;
        protected Dictionary<ItemType,ObjectPoole<ItemObject>> _pooles;
        protected List<Item> _list;
        public event Action<ItemType>EmptyType;

        public event Action Full;
        public event Action Empty;

        public void Init(PlayerStackConfig stackConfig, List<Transform> itemsPlaces)
        {
            _itemPrefab = new Dictionary<ItemType, ItemObject>();
            _maxCountItems = stackConfig.MaxCountItems;
            foreach (var stack in stackConfig.Items)
            {
                _itemPrefab.Add(stack.Type, stack.itemObject);
            }
            _itemsPlaces = itemsPlaces;
            _jumpDuration = stackConfig.JumpDuration;
            _jumpForce = stackConfig.JumpForce;
            _indexPlace = 0;

            _list = new List<Item>();
            CreateItemsList();
        }

        public void AddItem(ItemType type)
        {
           _pooles.TryGetValue(type, out var pool);
           if (pool != null)
           {
               ItemObject obj = pool.GetObject();
               var item = new Item(obj:obj, type);
            
               obj.Init(_jumpDuration,_jumpForce, type);
               obj.PlacedItem(_itemsPlaces[_indexPlace]);
               _list.Add(item);
           }

           _indexPlace++;
           if (_indexPlace >= _maxCountItems)
           {
               Full?.Invoke();
           }
        }

        public abstract void PushItemsToTarget(IAddItem target);

        private void CreateItemsList()
        {
            _pooles = new Dictionary<ItemType, ObjectPoole<ItemObject>>();
            foreach (KeyValuePair<ItemType,ItemObject> pair in _itemPrefab)
            {
                var pool = new ObjectPoole<ItemObject>();
                _pooles.Add(pair.Key,pool);
                pool.CreatePool(pair.Value, _maxCountItems, transform);
            }
        }
        
        protected void MoveItem(ItemType type,IAddItem target, Transform gObject)
        {
            if (type == ItemType.FoodReserve)
            {
                type = ItemType.FoodPackage;
            }

            if (type == ItemType.Food)
            {
                type = ItemType.ReadyFood;
            }

            if (type == ItemType.ClientsCat)
            {
                type = ItemType.Depart;
            }

            if (type == ItemType.Tray)
            {
                type = ItemType.ToiletsPackage;
            }

            if (type == ItemType.HospitalMeds)
            {
                type = ItemType.HospitalPackages;
            }

            if (type == ItemType.Wash)
            {
                type = ItemType.Bath;
            }

            var reverseList = _list;
            reverseList.Reverse();
           
            var item = reverseList.Find(x => x.Type == type);
            if (item == null)
            {
                return;
            }
            item.itemObject.MoveToTarget(gObject, target);
            _list.Remove(item);
            reverseList.Remove(item);
            TryReduceIndex();
           
            if (reverseList.FindAll(x => x.Type == type).Count==0)
            {
                EmptyType?.Invoke(type);
            }
        }

        protected void TryReduceIndex()
        {
            _indexPlace -= 1;
            if (_indexPlace <= 0)
            {
                Empty?.Invoke();
            }

        }
    }
}