using dbs.domain.Basics.Enum;
using dbs.domain.Model;

namespace dbs.blog.DTOs
{
    public class PostListItemDTO
    {
        private const int SHOR_SUMMARY_LENGTH = 70;

        public Guid Id { get; private set; }
        public string Url { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string Summary { get; private set; } = string.Empty;
        public string MainImageUrl { get; private set; } = string.Empty;
        public string Categories { get; private set; } = string.Empty;
        public PostStatus Status { get; private set; }
        public DateTime Date { get; private set; }

        public static PostListItemDTO ToPostListItemDTO(Post post)
        {
            return new PostListItemDTO
            {
                Id = post.Id,
                Url = post.UrlSlug,
                Title = post.Title,
                Summary = post.Summary,
                MainImageUrl = post.UrlMainImage,
                Categories = string.Join(", ", post.Categories.Select(c => c.Name)),
                Status = post.Status,
                Date = post.CreatedAt
            };
        }


        public string GetShorSummary()
        {
            if(Summary.Length <= SHOR_SUMMARY_LENGTH)
            {
                return Summary;
            }
            else
            {
                return Summary[.. SHOR_SUMMARY_LENGTH] + "...";
            }
        }
    }
}
