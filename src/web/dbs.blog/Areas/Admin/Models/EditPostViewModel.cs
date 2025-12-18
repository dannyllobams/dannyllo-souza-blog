using dbs.blog.DTOs;
using dbs.domain.Basics.Enum;
using System.ComponentModel.DataAnnotations;

namespace dbs.blog.Models
{
    public class EditPostViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "{0} is required")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "{0} is required")]
        public string Slug { get; set; } = string.Empty;

        [Display(Name = "Content")]
        [Required(ErrorMessage = "{0} is required")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Excerpt")]
        [Required(ErrorMessage = "{0} is required")]
        public string Excerpt { get; set; } = string.Empty;

        [Display(Name = "Meta Title")]
        [Required(ErrorMessage = "{0} is required")]
        public string MetaTitle { get; set; } = string.Empty;

        [Display(Name = "Meta Description")]
        [Required(ErrorMessage = "{0} is required")]
        public string MetaDescription { get; set; } = string.Empty;

        [Display(Name = "Main Image URL")]
        [Required(ErrorMessage = "{0} is required")]
        public string UrlMainImage { get; set; } = string.Empty;

        public PostStatus Status { get; set; } = PostStatus.DRAFT;

        [Required(ErrorMessage = "{0} is required")]
        public List<string> Categories { get; set; } = new();

        [Display(Name = "Tags")]
        [Required(ErrorMessage = "{0} is required")]
        public List<string> Tags { get; set; } = new();

        public static EditPostViewModel FromPostDTO(PostDTO post)
        {
            return new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.UrlSlug,
                Content = post.Content,
                Excerpt = post.Summary,
                MetaTitle = post.SEO.MetaTitle,
                MetaDescription = post.SEO.MetaDescription,
                UrlMainImage = post.UrlMainImage,
                Status = post.Status,
                Categories = post.Categories.ToList(),
                Tags = post.Tags.ToList()
            };

        }
    }
}
