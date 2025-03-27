using System;
using DefaultNamespace;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Spawners
{
    public class HospitalTreatmentTable: SpawnSystem, IAddItem
    { 
        [SerializeField] private HospitalMedsTable _medsTable;
        [SerializeField] private Transform _catArrivePosition;
        private int _resource;
        private int _count = 0;
        private bool _isFull;
    private bool _isEmpty = true;
    public int FreePlace => _stackConfig.MaxCountItems - _count;
    public bool Empty => _isEmpty;
    public event Action<Transform> FreeCat;


    public void InitMedicament(SpawnerSetting medsSettings)
    {
       _medsTable.Init(medsSettings);
       _medsTable.InitHospitalTreatmentTable(this);
    }

    public void AddItem()
    {
        if (_isFull)
            return;

        _resource++;
        _isEmpty = false;
        _isPushing = true;
        _timer = 0;
        if (_resource >= _stackConfig.MaxCountItems)
        {
            _isFull = true;
        }
        
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView player))
        {
            if (!_isFull && player.CatHospital)
            {
                for (int i = 0; i < FreePlace; i++)
                {
                    player.PushItemFromStack(this);
                }
            }

            if (_isFull && player.HasMeds)
            {
                player.DisplayTimer(_medsTable);

            }
        }
    }

    protected override void SpawnItem()
    {
        if (_resource <= 0)
        {
            return;
        }

        ItemObject obj = _poole.GetObject();
        if (obj != null)
        {
            obj.Init(_stackConfig.JumpDuration, _stackConfig.JumpForce, _stackConfig.Type);

            Vector3 placePosition = new Vector3(_itemsPlaces[_indexPlace].position.x,
                                                _itemsPlaces[_indexPlace].position.y + _yAxis,
                                                _itemsPlaces[_indexPlace].position.z);
            obj.PlacedItem(transform);
            obj.MoveToStackPlace(placePosition);
            _queue.Push(obj);
            ChangePlaceIndex();

            _resource -= 1;
            _count++;
            if (_count >= _stackConfig.MaxCountItems)
            {
                _isFull = true;
            }

            _isPushing = true;
            _timer = 0;
        }

    }

    public override void PushItemToPlayer(PlayerView playerView)
    {
        if (_isEmpty)
        {
            return;
        }

        _isPushing = true;
        _timer = 0;
        _count--;
        if (_count <= 0)
        {
            _isEmpty = true;
        }

        _queue.Pop().MoveToPlayer(playerView);
        _isFull = false;
        ChangeFreePlace();
    }

    public void PushCat()
    {
        
        _isPushing = true;
        _timer = 0;
        _count--;
        if (_count <= 0)
        {
            _isEmpty = true;
        }

        _queue.Peek().transform.DOJump(_catArrivePosition.position, 1f, 1, 0.3f)
              .OnStart(() =>
              {
                  _queue.Peek().transform.LookAt(_catArrivePosition);
              })
              .OnComplete(() =>
             {
              _queue.Pop().gameObject.SetActive(false);
              FreeCat?.Invoke(_catArrivePosition);
              _isFull = false;
              ChangeFreePlace();
             });
        
    }
    
    }
}