using dbs.domain.Basics.Enum;
using dbs.domain.Model;

namespace dbs.blog.DTOs
{
    public class PostDTO
    {
        public string Title { get; private set; } = string.Empty;
        public string UrlSlug { get; private set; } = string.Empty;
        public string UrlMainImage { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public string Summary { get; private set; } = string.Empty;
        public PostStatus Status { get; private set; } = PostStatus.DRAFT;
        public SEODTO SEO { get; private set; } = new SEODTO();

        public IReadOnlyCollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>().AsReadOnly();
        public IReadOnlyCollection<string> Categories { get; set; } = new List<string>().AsReadOnly();
        public IReadOnlyCollection<string> Tags { get; set; } = new List<string>().AsReadOnly();

        public static PostDTO ToPostDTO(Post post)
        {
            return new PostDTO
            {
                Title = post.Title,
                UrlSlug = post.UrlSlug,
                UrlMainImage = post.UrlMainImage,
                Content = post.Content,
                Summary = post.Summary,
                Status = post.Status,
                SEO = SEODTO.ToSEODTO(post.SEO),
                Comments = post.Comments.Select(CommentDTO.ToCommentDTO).ToList().AsReadOnly(),
                Categories = post.Categories.Select(c => c.Name).ToList().AsReadOnly(),
                Tags = post.Tags.Select(t => t.Name).ToList().AsReadOnly()
            };
        }
    }

    public class SEODTO
    {
        public string MetaTitle { get; private set; } = string.Empty;
        public string MetaDescription { get; private set; } = string.Empty;

        public static SEODTO ToSEODTO(SEO seo)
        {
            return new SEODTO
            {
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription
            };
        }
    }
}
