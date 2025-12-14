using dbs.blog.DTOs;

namespace dbs.blog.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalPosts { get; set; }
        public int TotalPageViews { get; set; }
        public int TotalComments { get; set; }
        public List<PostListItemDTO> Posts { get; set; } = new();
    }
}
