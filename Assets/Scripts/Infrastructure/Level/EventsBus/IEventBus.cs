using System;

namespace Infrastructure.Level.EventsBus
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> callback, int priority = 0);
        void Invoke<T>(T signal);
        void Unsubscribe<T>(Action<T> callback);
    }
}