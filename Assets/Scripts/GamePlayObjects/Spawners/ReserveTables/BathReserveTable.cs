using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class BathReserveTable:ReserveTableBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!_isFull && player.HasBath)
                {
                    for (int i = 0; i < FreePlace; i++)
                    {
                        player.PushItemFromStack(this);
                    }
                }
            }
        }
    }
}