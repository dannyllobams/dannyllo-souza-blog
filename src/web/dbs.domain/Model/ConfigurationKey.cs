using dbs.core.DomainObjects;
using dbs.core.Model;
using dbs.domain.Basics.Enum;

namespace dbs.domain.Model
{
    public class ConfigurationKey : Entity, IAggregateRoot
    {
        public KeyType Key { get; protected set; }
        public string? Value { get; protected set; }

        protected ConfigurationKey() { }
        public ConfigurationKey(KeyType key, string? value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateValue(string? value)
        {
            Value = value;
        }
    }
}
