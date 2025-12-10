using dbs.core.DomainObjects;
using dbs.domain.Basics.Enum;

namespace dbs.domain.Model
{
    public class Post : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string UrlSlug { get; set; } = string.Empty;
        public string UrlMainImage { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public PostStatus Status { get; set; } = PostStatus.DRAFT;
        public SEO SEO { get; set; } = new SEO(string.Empty, string.Empty); 

        private List<Category> _categories = new();
        private List<Tag> _tags = new();
        private List<Comment> _comments = new();

        public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
        public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        protected Post() { }

        public Post(
            string title, 
            string urlSlug, 
            string urlMainImage, 
            string content, 
            string summary, 
            string metaTitle,
            string metaDescription)
        {
            this.Title = title;
            this.UrlSlug = urlSlug;
            this.UrlMainImage = urlMainImage;
            this.Content = content;
            this.Summary = summary;
            this.Status = PostStatus.DRAFT;

            this.SEO = new SEO(metaTitle, metaDescription);

        }

        public void AddCategory(Category category)
        {
            if (!_categories.Any(c => c.Id == category.Id))
            {
                _categories.Add(category);
            }
        }

        public void RemoveCategory(Category category)
        {
            _categories.Remove(category);

        }

        public void AddTag(Tag tag)
        {
            if (!_tags.Any(t => t.Id == tag.Id))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(Tag tag)
        {
            _tags.Remove(tag);
        }

        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }
    }

    public class Comment : Entity
    {
        public string UserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        private List<Comment> _replies = new();
        public IReadOnlyCollection<Comment> Replies => _replies.AsReadOnly();

        protected Comment() { }

        public Comment(string userName, string content)
        {
            this.UserName = userName;
            this.Content = content;
        }

        public void AddReply(Comment reply)
        {
            _replies.Add(reply);
        }
    }

    public class Category : Entity
    {
        public string Name { get; set; } = string.Empty;
        private List<Post> _posts = new();

        public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

        protected Category() { }

        public Category(string name)
        {
            this.Name = name;
        }
    }

    public class Tag : Entity
    {
        public string Name { get; set; } = string.Empty;
        private List<Post> _posts = new();

        public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

        protected Tag() { }

        public Tag(string name)
        {
            this.Name = name;
        }
    }

    public class SEO
    {
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        protected SEO() { }
        public SEO(string metaTitle, string metaDescription)
        {
            this.MetaTitle = metaTitle;
            this.MetaDescription = metaDescription;
        }
    }
}
