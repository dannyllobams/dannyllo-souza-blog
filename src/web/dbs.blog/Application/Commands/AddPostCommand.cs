using dbs.core.Messages;
using FluentValidation;
using FluentValidation.Results;

namespace dbs.blog.Application.Commands
{
    public class AddPostCommand : Command<AddPostCommand.Result>
    {
        public class Result : CommandResult
        {
            public Guid PostId { get; set; }

            public Result(ValidationResult validationResult) : base(validationResult)
            {
            }

            public Result(Guid postId, ValidationResult validationResult) : base(validationResult)
            {
                this.PostId = postId;
            }
        }

        public class SEOData
        {
            public string MetaTitle { get; set; } = string.Empty;
            public string MetaDescription { get; set; } = string.Empty;
        }

        public string Title { get; set; } = string.Empty;
        public string UrlSlug { get; set; } = string.Empty;
        public string UrlMainImage { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public SEOData SEO { get; set; } = new SEOData();
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Categories { get; set; } = new List<string>();

        public override bool IsValid()
        {
            this.ValidationResult = new AddPostCommandValidator().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }

    public class AddPostCommandValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostCommandValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(c => c.UrlSlug)
                .NotEmpty().WithMessage("Url field is required.")
                .MaximumLength(200).WithMessage("Url field cannot exceed 200 characters.");

            RuleFor(c => c.UrlMainImage)
                .NotEmpty().WithMessage("Image field is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(c => c.Summary)
                .NotEmpty().WithMessage("Summary is required.");

            RuleFor(c => c.Tags)
                .NotNull().WithMessage("Tags is required")
                .Must(c => c.Count >= 1).WithMessage("Tags must contains  at least one element");

            RuleFor(c => c.Categories)
                .NotNull().WithMessage("Categories is required")
                .Must(c => c.Count >= 1).WithMessage("Categories must contains  at least one element");

            RuleFor(c => c.SEO)
                .Must(c => !string.IsNullOrEmpty(c.MetaTitle) && !string.IsNullOrEmpty(c.MetaDescription))
                .WithMessage("SEO Title and Description is required.");
        }
    }
}
