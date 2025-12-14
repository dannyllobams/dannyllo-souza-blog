using System.ComponentModel.DataAnnotations;

namespace dbs.blog.Models
{
    public class AddPostViewModel
    {
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

        [Required(ErrorMessage = "{0} is required")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Tags")]
        [Required(ErrorMessage = "{0} is required")]
        public List<string> Tags { get; set; } = new();
    }
}
