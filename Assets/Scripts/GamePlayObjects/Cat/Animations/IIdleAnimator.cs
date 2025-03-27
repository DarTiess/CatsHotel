using GamePlayObjects.Player;

namespace GamePlayObjects.Cat
{
    public interface IIdleAnimator: ISitAnimator
    {
        void PlayBallAnimation();
        void PlayPlantAnimation();
    }
}