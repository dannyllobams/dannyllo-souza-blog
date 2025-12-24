using dbs.core.DomainObjects;
using dbs.core.Model;

namespace dbs.domain.Model
{
    public class PageView : Entity, IAggregateRoot
    {
        public Guid PageId { get; protected set; }
        public DateOnly Date { get; protected set; }
        public int TotalViews { get; protected set; }

        protected PageView() { }

        public PageView(Guid pageId, DateOnly date)
        {
            PageId = pageId;
            Date = date;
            TotalViews = 1;
        }

        public void IncrementViews()
        {
            TotalViews++;
        }
    }
}
