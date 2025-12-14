using dbs.domain.Model;

namespace dbs.blog.DTOs
{
    public class CommentDTO
    {
        public string UserName { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;

        public IReadOnlyCollection<CommentDTO> Replies { get; private set; } = new List<CommentDTO>().AsReadOnly();

        public static CommentDTO ToCommentDTO(Comment comment)
        {
            return new CommentDTO
            {
                UserName = comment.UserName,
                Content = comment.Content,
                Replies = comment.Replies.Select(ToCommentDTO).ToList().AsReadOnly()
            };
        }
    }
}
