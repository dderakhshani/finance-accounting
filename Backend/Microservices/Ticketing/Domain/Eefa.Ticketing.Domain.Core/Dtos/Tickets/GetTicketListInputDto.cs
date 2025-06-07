
using Eefa.Common.CommandQuery;
using Eefa.Common.Data.Query;

namespace Eefa.Ticketing.Domain.Core.Dtos.Tickets
{
    public class GetTicketListInputDto : PaginatedQueryModel, IQuery
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
