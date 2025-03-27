using UnityEngine;

namespace GamePlayObjects.Cat.StateMachine
{
    public abstract class State
    {
        protected readonly StateMachine _stateMachine;
        protected bool _activeState;
        protected bool _finishState;
        public Transform TutorialZone { get; protected set; }
    

        protected State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Enter(){}
        public virtual void Exit(){}
        public virtual void Update(){}

      
    }
}