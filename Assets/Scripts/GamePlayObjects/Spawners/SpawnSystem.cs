using System;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
      public abstract class SpawnSystem: MonoBehaviour
    {
        [SerializeField] protected List<Transform> _itemsPlaces; 
        protected StackConfig _stackConfig;
        protected float _itemDeliveryTime;
        protected float _itemHeight;
        protected float _yAxis;
        protected ObjectPoole<ItemObject> _poole;
        protected Stack<ItemObject> _queue;

        protected int _indexPlace=0;
        protected bool _isPushing;
        protected bool _stopPushing;
        protected float _timer=0;

        public bool IsPushing => _isPushing;
        public bool StopPushing => _stopPushing;
        public Transform ItemPlace => _itemsPlaces[0];
        public ItemType Type { get; private set; }


        public virtual void Init(SpawnerSetting settings)
        {
            _stackConfig = settings.Configs;
            _itemDeliveryTime = settings.DeliveryTime;
            _itemHeight = settings.Height;
            Type = _stackConfig.Type;
            _queue = new Stack<ItemObject>();
            CreateItemsList();
        }

        private void Update()
        {
            if(_stopPushing)
                return;
            if (!_isPushing)
                return;

            _timer += Time.deltaTime;
            
            if (_timer <=_itemDeliveryTime)
                return;

            _isPushing = false;
            SpawnItem();
        }

        private void CreateItemsList()
        {
            _poole = new ObjectPoole<ItemObject>();
            _poole.CreatePool(_stackConfig.ItemPrefab, _stackConfig.MaxCountItems, transform);
            _timer = _itemDeliveryTime;
            _isPushing = true;
        }

        protected abstract void SpawnItem();
        public virtual void PushItemToPlayer(PlayerView player)
        {
           _isPushing = true;
           _timer = 0;

           if (_queue.Count > 0)
           {
               _queue.Pop().MoveToPlayer(player);
               ChangeFreePlace();
           }
        }

        protected void ChangeFreePlace()
        {
            if (_indexPlace > 0)
            {
                _indexPlace -= 1;
            }
            else
            {
                _indexPlace = _itemsPlaces.Count - 1;
                TryReduceRow();
            }
        }

        private void TryReduceRow()
        {
            if (_yAxis > 0)
            {
                _yAxis -= _itemHeight;
            }
        }
        protected void ChangePlaceIndex()
        {
            if (_indexPlace < _itemsPlaces.Count - 1)
            {
                _indexPlace += 1;
            }
            else
            {
                _indexPlace = 0;
                _yAxis += _itemHeight;
            }
        }

       
    }
    
}