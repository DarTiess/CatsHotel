using System.Collections;
using DG.Tweening;
using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;

namespace CamFollow {
    public class CamFollower : MonoBehaviour
    {
        [SerializeField] private float _speedVector;
        [SerializeField] private float _vectorY = 10;
        [SerializeField] private float _vectorZ = 10;
        [SerializeField] private float _vectorX;
        [SerializeField] private bool _vectorXFrom0;
        [SerializeField] private bool _lookAtTarget;
        [SerializeField] private float _tutorialWaitPosition;
        [SerializeField] private float _tutorialSpeedVector;


        private Transform _target;
        private Vector3 _temp;

        private IEventBus _levelEvents;
        private Transform _player;
        private float _speed;

        public void Init(IEventBus levelEvents, Transform player)
        {
            _levelEvents = levelEvents;
            _target = player;
            _player = player;
            _speed = _speedVector;
            _levelEvents.Subscribe<LevelWin>(OnLevelWin);
            _levelEvents.Subscribe<LevelLost>(OnLevelLost);
        }

        private void OnDisable()
        {
            _levelEvents.Unsubscribe<LevelWin>(OnLevelWin);
            _levelEvents.Unsubscribe<LevelLost>(OnLevelLost);
        }

        private void FixedUpdate()   
        {                 
            if (!_target) return;
       
            MoveVector(); 
        }

        private void OnLevelWin(LevelWin signal)               
        {
            
        }

        private void OnLevelLost(LevelLost signal)
        {
            SetStop();
        }

        private void MoveVector()
        {
            _temp = _target.position;
            _temp.y += _vectorY;
            _temp.z -= _vectorZ;
            _temp.x = _vectorXFrom0 ? _vectorX : _temp.x + _vectorX ;   
                                  
            transform.position = Vector3.Slerp(transform.position,_temp,_speed * Time.fixedDeltaTime);
            if (_lookAtTarget) transform.LookAt(_target.position); 
        }

       public void SetStop()
        {
            _target = null;  
        }

       public void GoToTutorialPosition(Transform cameraPosition)
       {
           StartCoroutine(ToTutorial(cameraPosition));
       }

       private IEnumerator ToTutorial(Transform cameraPosition)
       {
           yield return new WaitForSeconds(_tutorialWaitPosition);
           _target = cameraPosition;
           _speed = _tutorialSpeedVector; 
           StartCoroutine(BackToPlayer());
       }

       private IEnumerator BackToPlayer()
       {
           yield return new WaitForSeconds(_tutorialWaitPosition);
           _target = _player;
           _speed = _speedVector;
       }

       public void ChangeTarget(Transform cat)
       {
           _target = cat;
           _speed = _tutorialSpeedVector; 
       }
    }
}