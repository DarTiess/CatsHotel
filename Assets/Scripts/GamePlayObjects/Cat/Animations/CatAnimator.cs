using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Cat
{
    public class CatAnimator: IEatAnimator, ISleepAnimator, IRunAnimator, IIdleAnimator, IWashAnimator
    {
       
            private Animator _animator;
            private static readonly int IS_MOVE = Animator.StringToHash("IsMove");
            private static readonly int EAT = Animator.StringToHash("Eat");
            private static readonly int SIT = Animator.StringToHash("Sit");
            private static readonly int SLEEP = Animator.StringToHash("Sleep");
            private static readonly int RUN = Animator.StringToHash("Run");
            private static readonly int PLAY_BALL = Animator.StringToHash("PlayBall");
            private static readonly int PLAY_PLANTS = Animator.StringToHash("PlayPlants");
            private static readonly int WASH = Animator.StringToHash("Wash");

            public CatAnimator(Animator animator)
            {
                _animator = animator;
            }

            public void MoveAnimation(float speed)
            {
                _animator.SetFloat(IS_MOVE, speed);
                _animator.SetBool(RUN, false);
                _animator.SetBool(EAT, false);
                _animator.SetBool(SLEEP, false);
                _animator.SetBool(SIT, false);
                _animator.SetBool(PLAY_BALL, false);
                _animator.SetBool(PLAY_PLANTS, false);
                _animator.SetBool(WASH, false);
            }

            public void EatAnimation()
            {
                _animator.SetBool(EAT, true);
                _animator.SetBool(SIT, false);
                _animator.SetFloat(IS_MOVE, 0);
            }

            public void SitAnimation()
            {
                _animator.SetBool(SIT, true);
            }

            public void SleepAnimation()
            {
                _animator.SetBool(SLEEP, true);
                _animator.SetBool(SIT, false);
                _animator.SetFloat(IS_MOVE, 0);
            }

            public void RunAnimation()
            {
                _animator.SetBool(RUN, true);
                _animator.SetBool(SIT, false);
                _animator.SetFloat(IS_MOVE, 0);
            }

            public void PlayBallAnimation()
            {
                _animator.SetFloat(IS_MOVE, 0);
                _animator.SetBool(PLAY_BALL, true);
            }

            public void PlayPlantAnimation()
            {
                _animator.SetBool(PLAY_PLANTS, true);
                _animator.SetFloat(IS_MOVE, 0);
            }

            public void WashingAnimation()
            {
                _animator.SetBool(WASH, true);
            }
    }
}