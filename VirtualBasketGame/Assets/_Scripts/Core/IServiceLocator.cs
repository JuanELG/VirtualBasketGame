namespace BasketGame.Core
{
    public interface IServiceLocator
    {
        T GetService<T>() where T : class;
        void RegisterService<T>(T service);
    }
}