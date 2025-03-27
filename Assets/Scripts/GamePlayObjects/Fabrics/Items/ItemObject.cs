using DG.Tweening;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class ItemObject: MonoBehaviour
    {
        [SerializeField] private ItemType _type;
        private float _jumpDuration;
        private float _jumpForce;

        public void Init(float duration,float jumpForce, ItemType type)
        {
            _jumpDuration = duration;
            _jumpForce = jumpForce;
            _type = type;
        }

        public void PlacedItem(Transform startPosition)
        {
            transform.position = startPosition.position;
        }
       
        public void HideItem()
        {
            gameObject.SetActive(false);
        }

        public void MoveToStackPlace(Vector3 stackPosition)
        {
            transform.DOComplete(this);
            transform.DOJump(stackPosition, _jumpForce, 1, _jumpDuration)
                     .SetEase(Ease.Linear);
        }

        public void MoveToPlayer(PlayerView player)
        {
            // transform.DOComplete(this);
            transform.DOJump(player.transform.position,_jumpForce, 1, _jumpDuration)
                     .OnComplete(() =>
                     {
                         HideItem();
                         player.AddItemInStack(_type);
                     });
        }

        public void MoveToTarget(Transform factory, IAddItem itemStack)
       {
           transform.DOJump(factory.transform.position, _jumpForce, 1, _jumpDuration)
                    .OnComplete(() =>
                    {
                        HideItem();
                        itemStack.AddItem();
                    });
            
       }  
        public void MoveToClient(Transform factory, ClientView itemStack)
       {
           transform.DOComplete(this);
            transform.DOJump(factory.transform.position, _jumpForce, 1, _jumpDuration)
                     .OnComplete(() =>
                     {
                         HideItem();
                         itemStack.AddItem();
                     });
            
       }
    }
}