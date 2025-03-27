namespace Infrastructure.Level
{
    public interface ILevelLoader
    {
        void LoadNextLevel();
        void RestartScene();
    }
}