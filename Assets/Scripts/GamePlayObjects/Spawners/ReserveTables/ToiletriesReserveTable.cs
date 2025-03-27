using System;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class ToiletriesReserveTable:ReserveTableBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!_isFull && player.HasPapper)
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