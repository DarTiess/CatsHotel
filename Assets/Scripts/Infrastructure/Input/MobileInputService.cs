namespace Infrastructure.Input
{
    public class MobileInputService : InputService
    {
       
        public override float GetHorizontal { 
       
            get
            {
                return SimpleInput.GetAxis(HORIZONTAL);
            }
        }
        public override float GetVertical
        {
            get
            {
                return SimpleInput.GetAxis(VERTICAL);
            }
        }
    }
}