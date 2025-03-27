using System;
using UnityEngine;

namespace GamePlayObjects.Cat
{
    public interface ITriggerObserver
    {
        event Action<Collider> TriggerEnter;
        event Action<Collider> TriggerExit;
        void EnableTrigger();
        void DisableTrigger();
    }
}