using System;
using DataBus.Data.Entities;

namespace DataBus.Data.Commands
{
    public interface IDataCommand
    {
        
    }

    public class CreateMessageQueueItemCommand : IDataCommand
    {
        private MessageQueueItem _item;

        public CreateMessageQueueItemCommand(MessageQueueItem item)
        {
            _item = item;
            _item.id = Guid.NewGuid();

        }
    }

    public class CreateChannelCommand : IDataCommand
    {
        private DataBusChannel _channel;

        public CreateChannelCommand(DataBusChannel channel)
        {
            _channel = channel;
            _channel.idChannel = Guid.NewGuid();
        }
    }

    public class CreatePublicationCommand : IDataCommand
    {
        private Publication _pub;

        public CreatePublicationCommand(Publication pub)
        {
            _pub = pub;
            _pub.idPublication = Guid.NewGuid();
        }
    }

    public class CreateSubscriberCommand : IDataCommand
    {
        private Subscriber _subscriber;

        public CreateSubscriberCommand(Subscriber subscriber)
        {
            _subscriber = subscriber;
            _subscriber.idSubscriber = Guid.NewGuid();
        }
    }

    public class CreateSubscriptionCommand : IDataCommand
    {
        private Subscription _subscription;

        public CreateSubscriptionCommand(Subscription subscription)
        {
            _subscription = subscription;
            _subscription.idSubscription = Guid.NewGuid();
        }
    }
     
}