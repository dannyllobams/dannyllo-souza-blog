using Cortex.Mediator.Notifications;

namespace dbs.core.Messages
{
    public class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }
        public Event()
        {
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
