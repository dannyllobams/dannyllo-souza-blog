using dbs.domain.Model;

namespace dbs.blog.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsUsed { get; set; }

        public static CategoryDTO ToCategoryDTO(Category category, bool isUsed)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                IsUsed = isUsed
            };
        }
    }
}

