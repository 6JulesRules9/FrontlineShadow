using FrontlineShadow.Core;
using NUnit.Framework;

namespace FrontlineShadow.Tests
{
    public class EventBusTests
    {
        struct Ping { public int N; }

        [SetUp]
        public void Setup() => EventBus.Clear();

        [Test]
        public void Publish_Invokes_Subscriber()
        {
            int received = 0;
            void Handler(Ping p) => received = p.N;

            EventBus.Subscribe<Ping>(Handler);
            EventBus.Publish(new Ping { N = 7 });

            Assert.AreEqual(7, received);
        }

        [Test]
        public void Unsubscribe_StopsDelivery()
        {
            int count = 0;
            void Handler(Ping p) => count++;

            EventBus.Subscribe<Ping>(Handler);
            EventBus.Unsubscribe<Ping>(Handler);
            EventBus.Publish(new Ping { N = 1 });

            Assert.AreEqual(0, count);
        }

        [Test]
        public void Publish_NoSubscribers_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => EventBus.Publish(new Ping { N = 99 }));
        }
    }
}
