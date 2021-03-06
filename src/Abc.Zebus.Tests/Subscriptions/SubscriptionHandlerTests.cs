using Abc.Zebus.Directory;
using Abc.Zebus.Subscriptions;
using Abc.Zebus.Testing.Extensions;
using Abc.Zebus.Tests.Messages;
using NUnit.Framework;

namespace Abc.Zebus.Tests.Subscriptions
{
    [TestFixture]
    public class SubscriptionHandlerTests
    {
        private class MessageTest : IMessage
        {
        }

        private class SubscriptionHandlerTest : SubscriptionHandler<MessageTest>
        {
            public bool OnSubscriptionExecuted { get; private set; }

            protected override void OnSubscriptionsUpdated(SubscriptionsForType subscriptions, PeerId peerId)
            {
                OnSubscriptionExecuted = true;
            }
        }

        [Test]
        public void should_call_onSubscription_when_type_of_message_matches_generic_type()
        {
            // Arrange
            var handler = new SubscriptionHandlerTest();

            // Act
            handler.Handle(new SubscriptionsUpdated(new SubscriptionsForType(new MessageTypeId(typeof(MessageTest))), new PeerId("testPeerId")));

            // Assert
            handler.OnSubscriptionExecuted.ShouldBeTrue();
        }

        [Test]
        public void should_not_call_onSubscription_when_type_of_message_does_not_match_generic_type()
        {
            // Arrange
            var handler = new SubscriptionHandlerTest();

            // Act
            handler.Handle(new SubscriptionsUpdated(new SubscriptionsForType(new MessageTypeId(typeof(FakeEvent))), new PeerId("testPeerId")));

            // Assert
            handler.OnSubscriptionExecuted.ShouldBeFalse();
        }
    }
}
