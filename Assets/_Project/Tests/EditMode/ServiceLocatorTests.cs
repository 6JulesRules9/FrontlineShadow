using System;
using FrontlineShadow.Core;
using NUnit.Framework;

namespace FrontlineShadow.Tests
{
    sealed class DummyService : IService
    {
        public int Value;
    }

    public class ServiceLocatorTests
    {
        [SetUp]
        public void Setup() => ServiceLocator.Clear();

        [Test]
        public void Register_Then_Get_ReturnsSameInstance()
        {
            var s = new DummyService { Value = 42 };
            ServiceLocator.Register(s);

            Assert.AreSame(s, ServiceLocator.Get<DummyService>());
            Assert.AreEqual(42, ServiceLocator.Get<DummyService>().Value);
        }

        [Test]
        public void Get_Unregistered_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => ServiceLocator.Get<DummyService>());
        }

        [Test]
        public void TryGet_Unregistered_ReturnsFalse()
        {
            Assert.IsFalse(ServiceLocator.TryGet<DummyService>(out var svc));
            Assert.IsNull(svc);
        }

        [Test]
        public void Register_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceLocator.Register<DummyService>(null));
        }
    }
}
