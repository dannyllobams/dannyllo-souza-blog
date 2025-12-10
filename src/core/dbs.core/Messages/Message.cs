namespace dbs.core.Messages
{
    public class Message
    {
        public Guid AggregateId { get; protected set; } = Guid.Empty;
        public string MessageType { get; protected set; } = string.Empty;

        public Message()
        {
            this.MessageType = GetType().Name;
        }
    }
}
