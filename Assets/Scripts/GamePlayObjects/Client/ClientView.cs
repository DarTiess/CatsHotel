using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Fabrics
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ClientStack))]
    [RequireComponent(typeof(Animator))]
    public class ClientView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _catPlaces;
        private NavMeshAgent _navMesh;
        private ClientMove _clientMove;
        private ClientStack _clientStack;
        private ClientAnimator _clientAnimator;
        private bool _canMove;
        private Animator _animator;
        public ClientStack Stack=>_clientStack;

        public event Action<ClientView> ClientPlaced; 
        public event Action<ClientView> ClientTakeCat;

        public void Init(PlayerStackConfig config, float moveSpeed, float stopDistance, Transform target, Transform home)
        {
            _navMesh = GetComponent<NavMeshAgent>();
          
            _clientStack = GetComponent<ClientStack>();
            _clientStack.Init(config, _catPlaces);
            _navMesh.enabled = true;
            _animator = GetComponent<Animator>();
            _clientAnimator = new ClientAnimator(_animator);
            _clientMove = new ClientMove(_navMesh,_clientAnimator, moveSpeed,stopDistance, target, home);

            _clientMove.Placed += ClientOnPlace;
            _clientMove.AtHome += ClientAtHome;
            _canMove = true;
        }

        private void FixedUpdate()
        {
            if(_canMove)
                _clientMove.FixUpdate();
        }

        public void AddItem()
        {
            _clientAnimator.HasStack();
            _clientStack.AddItem(ItemType.ClientsCat);
            _clientMove.GoHome();
            ClientTakeCat?.Invoke(this);
        }

        public void ChangeTarget(Transform target)
        {
            _clientMove.ChangeTarget(target);
        }

        private void ClientAtHome()
        {
            _canMove = false;
            _clientMove.Placed -= ClientOnPlace;
            _clientMove.AtHome -= ClientAtHome;
            _clientStack.Reset();
            _clientAnimator.StackEmpty();
            _navMesh.enabled = false;
           
            gameObject.SetActive(false);
        }

        private void ClientOnPlace()
        {
            ClientPlaced?.Invoke(this);
        }
    }
}