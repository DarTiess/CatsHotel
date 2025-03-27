using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class FoodReserveTable:ReserveTableBase
    {
        [SerializeField] private Transform _triggerZone;
        private bool _onTutorial;

        public Transform TriggerZone()
        {
            _onTutorial = true;
            return _triggerZone;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (player.StackIsFull && !_isFull && player.HasPackages)
                {
                    for (int i = 0; i < FreePlace; i++)
                    {
                        player.PushItemFromStack(this);
                    }

                    if (_onTutorial)
                    {
                        player.NextTutorialStep();
                    }
                }
            }
        }

        public void PushItemToTarget(CookingSpawner cookingSpawner)
        {
            if (_queue.Count > 0)
            {
                _isPushing = true;
                _timer = 0;
                _count--;
                _queue.Pop().MoveToTarget(cookingSpawner.ItemPlace, cookingSpawner);
                _isFull = false;
                ChangeFreePlace();
                if (_queue.Count <= 0)
                {
                    _isEmpty = true;
                }
            }
        }
    }
}