using GamePlayObjects.Cat.StateMachine;
using UnityEngine;

namespace Infrastructure.Level.EventsBus.Signals
{
    public struct CatState
    {
        private Transform _state;
        public Transform State => _state;

        public CatState(Transform state)
        {
            _state = state;
        }
    }
}