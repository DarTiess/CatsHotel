using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateWishes: State
    {
        private List<State> _wishesList;
        private List<State> _tutorialList;
        private int _indexWish;
        private readonly List<State> _randomizedList;
        private int _tutorialIndex;
        private bool _onTutorial;

        public event Action<State> TutorialWisheState;

        public StateWishes(StateMachine stateMachine, List<State> states) : base(stateMachine)
        {
            _indexWish = 0;
            _wishesList = new List<State>(states.Count);
            _wishesList = states;
            _tutorialList = new List<State>(4);

            foreach (State state in _wishesList)
            {
                if (state.GetType() == typeof(StateEat) ||
                    state.GetType() == typeof(StateSleep) ||
                    state.GetType() == typeof(StateTraining))
                {
                    _tutorialList.Add(state);
                }
                
            }
            

            var rnd = new Random();
            var _randomized = _wishesList.OrderBy(item => rnd.Next());
            _randomizedList=_randomized.ToList();
        }

        public override void Enter()
        {
            if (_onTutorial)
            {
                TutorialState();
            }
            else
            {
                SetWishState();
            }
           
        }

        private void TutorialState()
        {
            if (_tutorialIndex >= _tutorialList.Count)
            {
                _onTutorial = false;
                TutorialWisheState?.Invoke(_tutorialList[_tutorialIndex-1]);
                _stateMachine.SetState<StateGoHome>();
            }
            else
            {
                _stateMachine.SetWishState(_tutorialList[_tutorialIndex]);
                TutorialWisheState?.Invoke(_tutorialList[_tutorialIndex]);
                _tutorialIndex++;
            }
           
            
        }

        private void SetWishState()
        {
            if (_indexWish >= _wishesList.Count)
            {
                _indexWish = 0;
                _stateMachine.SetState<StateGoHome>();
            }
            else
            {
                _stateMachine.SetWishState(_randomizedList[_indexWish]);
                _indexWish++;
            }
        }

        public void OnTutorial()
        {
            _onTutorial = true;
        }
    }
}