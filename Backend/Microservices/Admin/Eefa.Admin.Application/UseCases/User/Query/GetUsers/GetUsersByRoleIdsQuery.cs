using Library.Interfaces;
using Library.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.User.Query.GetUsers
{
    public class GetUsersByIdsQuery : IRequest<ServiceResult>
    {
        public List<int> UserIds { get; set; }
    }
    public class GetUsersByIdsQueryHandler : IRequestHandler<GetUsersByIdsQuery, ServiceResult>
    {
        private readonly IRepository _repository;

        public GetUsersByIdsQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {

            var result = (
                  from u in _repository.GetQuery<Data.Databases.Entities.User>()
                  join ur in _repository.GetQuery<Data.Databases.Entities.UserRole>() on u.Id equals ur.UserId
                  join r in _repository.GetQuery<Data.Databases.Entities.Role>() on ur.RoleId equals r.Id
                  join p in _repository.GetQuery<Data.Databases.Entities.Person>() on u.PersonId equals p.Id
                  where request.UserIds.Contains(u.Id)
                  select new
                  {
                      Fullname = p.FirstName + " " + p.LastName,
                      UserId = u.Id,
                      RoleId = r.Id,
                      RoleTitle = r.Title
                  }).ToList();

            return ServiceResult.Success(result);
        }
    }
}
