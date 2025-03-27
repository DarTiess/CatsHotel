using UnityEngine;

namespace GamePlayObjects.Player
{
    public class PlayerAnimator : IMoveAnimator
    {
        private Animator _animator;
        private static readonly int IS_MOVE = Animator.StringToHash("IsMove");
        private static readonly int HAS_STACK = Animator.StringToHash("HasStack");

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void MoveAnimation(float speed)
        {
            _animator.SetFloat(IS_MOVE, speed);
        }
        
        public void HasStack()
        {
            _animator.SetBool(HAS_STACK, true);
        }
    
        public void StackEmpty()
        {
            _animator.SetBool(HAS_STACK, false);
        }
    }
}