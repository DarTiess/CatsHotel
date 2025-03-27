using System.Collections.Generic;
using GamePlayObjects.Player;

namespace GamePlayObjects.Fabrics
{
    public class ClientStack : PlayerBaseStack
    {
        public override void PushItemsToTarget(IAddItem target) { }

        public void Reset()
        {
            foreach (Item item in _list)
            {
               item.itemObject.HideItem();
            }
            _list.Clear();
           /* foreach (ItemObject itemObject in _itemsList)
            {
                itemObject.HideItem();
            }
            _itemsList.Clear();
            _itemsList = null;  
            _indexItem = 0;*/
        }
    }
}