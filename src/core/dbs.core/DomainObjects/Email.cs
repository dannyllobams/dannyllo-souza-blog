using System.Text.RegularExpressions;

namespace dbs.core.DomainObjects
{
    public class Email
    {
        public const int AddressMaxLength = 254;
        public const int AddressMinLength = 5;
        public string Address { get; protected set; } = string.Empty;

        protected Email() { }

        public Email(string endereco)
        {
            if (!Validate(endereco)) throw new Exception("E-mail inválido");
            Address = endereco;
        }

        public static bool Validate(string email)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(email);
        }
    }
}
