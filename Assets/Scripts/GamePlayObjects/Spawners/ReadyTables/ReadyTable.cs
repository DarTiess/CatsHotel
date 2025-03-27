using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class ReadyTable: MonoBehaviour
    {
        [SerializeField] private ReserveTableBase _toiletriesReserveTable;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
                if (!playerView.StackIsFull && !_toiletriesReserveTable.Empty)
                {
                    //count=playerView.StackCount
                    for (int i = 0; i < playerView.StackCount; i++)
                    {
                        _toiletriesReserveTable.PushItemToPlayer(playerView);
                    }
                    
                }
               
            }
        }
    }
}