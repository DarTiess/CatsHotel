namespace Infrastructure.Input
{
    public interface IInputService 
    {
        float GetHorizontal { get; }
        float GetVertical { get; }
    }
}