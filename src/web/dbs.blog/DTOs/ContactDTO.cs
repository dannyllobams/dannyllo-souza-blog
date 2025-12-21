using dbs.domain.Model;

namespace dbs.blog.DTOs
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Received { get; set; }
        public DateTime CreatedAt { get; set; }

        public static ContactDTO ToContactDTO(Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                Received = contact.Received,
                CreatedAt = contact.CreatedAt
            };
        }
    }
}

