using System;
using System.Collections.Generic;

namespace FrontlineShadow.Core
{
    /// <summary>
    /// Minimaler, zentraler Service-Locator. Der Bootstrap registriert hier alle
    /// Services; Systeme und UI holen sie per <see cref="Get{T}"/>.
    /// Bewusst schlicht gehalten — bei Bedarf später gegen echtes DI tauschbar.
    /// </summary>
    public static class ServiceLocator
    {
        static readonly Dictionary<Type, IService> Services = new();

        public static void Register<T>(T service) where T : class, IService
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            Services[typeof(T)] = service;
        }

        public static T Get<T>() where T : class, IService
        {
            if (Services.TryGetValue(typeof(T), out var s)) return (T)s;
            throw new InvalidOperationException(
                $"Service '{typeof(T).Name}' ist nicht registriert. " +
                "Wird er im Bootstrap verdrahtet?");
        }

        public static bool TryGet<T>(out T service) where T : class, IService
        {
            if (Services.TryGetValue(typeof(T), out var s))
            {
                service = (T)s;
                return true;
            }
            service = null;
            return false;
        }

        public static bool IsRegistered<T>() where T : class, IService
            => Services.ContainsKey(typeof(T));

        /// <summary>Alle Services entfernen (z. B. beim Neustart des Bootstraps / in Tests).</summary>
        public static void Clear() => Services.Clear();
    }
}
