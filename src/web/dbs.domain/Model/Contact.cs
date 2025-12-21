using dbs.core.DomainObjects;
using dbs.core.Model;

namespace dbs.domain.Model
{
    public class Contact : Entity, IAggregateRoot
    {
        public string Name { get; protected set; } = string.Empty;
        public string Email { get; protected set; } = string.Empty;
        public string Message { get; protected set; } = string.Empty;
        public bool Received { get; protected set; } = false;
        protected Contact() { }
        public Contact(string name, string email, string message)
        {
            this.Name = name;
            this.Email = email;
            this.Message = message;
            this.Received = false;
        }

        public void MarkAsReceived()
        {
            this.Received = true;
        }
    }
}
