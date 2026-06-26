using System;
using System.Collections.Generic;

namespace FrontlineShadow.Core
{
    /// <summary>
    /// Leichtes, typisiertes Event-System für lose Kopplung zwischen Systemen
    /// (Simulation ⟂ Präsentation). Events sind beliebige Typen (am besten kleine
    /// structs), z. B. <c>HunterDamaged</c>, <c>RepairCompleted</c>.
    /// </summary>
    public static class EventBus
    {
        static readonly Dictionary<Type, Delegate> Handlers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            Handlers.TryGetValue(typeof(T), out var existing);
            Handlers[typeof(T)] = (existing as Action<T>) + handler;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            if (Handlers.TryGetValue(typeof(T), out var existing))
                Handlers[typeof(T)] = (existing as Action<T>) - handler;
        }

        public static void Publish<T>(T evt)
        {
            if (Handlers.TryGetValue(typeof(T), out var d))
                (d as Action<T>)?.Invoke(evt);
        }

        public static void Clear() => Handlers.Clear();
    }
}
