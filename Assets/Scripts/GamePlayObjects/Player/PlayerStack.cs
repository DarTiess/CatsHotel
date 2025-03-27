using GamePlayObjects.Fabrics;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlayObjects.Player
{
    public class PlayerStack: PlayerBaseStack
    {
        public override void PushItemsToTarget(IAddItem target)
        {
            if (_list.Count>0)
            {
                SpawnSystem spawner = target as SpawnSystem;
                if (spawner != null)
                {
                    MoveItem(spawner.Type, target, spawner.transform);
                }
            }
        }

        public void PushItemsToClient(ClientView target)
        {
            if (_list.Count>0)
            {
                Object gObject = target;
                MoveItem(target, gObject.GameObject().transform);
            }
        }
        private void MoveItem(ClientView target, Transform gObject)
        {
            foreach (Item item in _list)
            {
                if (item.Type == ItemType.Depart)
                {
                    item.itemObject.MoveToClient(gObject, target);
                    _list.Remove(item);
                    TryReduceIndex();
                    return;
                }
            }
        }

       
    }
}