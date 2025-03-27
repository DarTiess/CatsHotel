using System;
using UnityEngine;

namespace GamePlayObjects.Cat
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour, ITriggerObserver
    {
        private Collider _collider;
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            DisableTrigger();
        }

        public void EnableTrigger()
        {
            _collider.enabled = true;
        }

        public void DisableTrigger()
        {
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) => 
            TriggerEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) => 
            TriggerExit?.Invoke(other);
    }
}