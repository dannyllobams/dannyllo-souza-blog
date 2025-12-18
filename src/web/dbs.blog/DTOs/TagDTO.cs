using dbs.domain.Model;

namespace dbs.blog.DTOs
{
    public class TagDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsUsed { get; set; }

        public static TagDTO ToTagDTO(Tag tag, bool isUsed)
        {
            return new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                IsUsed = isUsed
            };
        }
    }
}

