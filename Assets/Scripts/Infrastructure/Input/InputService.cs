namespace Infrastructure.Input
{
    public abstract class InputService: IInputService
    {
        protected const string HORIZONTAL = "Horizontal";
        protected const string VERTICAL = "Vertical";
        
        public abstract float GetHorizontal
        {
            get;
        }
        public abstract float GetVertical { get; }

    }
}