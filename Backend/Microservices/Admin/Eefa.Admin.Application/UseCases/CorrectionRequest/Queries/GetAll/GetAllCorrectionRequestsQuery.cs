using AutoMapper.QueryableExtensions;
using AutoMapper;
using Library.Interfaces;
using Library.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.CorrectionRequest.Models;
using Library.Utility;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Microsoft.EntityFrameworkCore;
using Eefa.Admin.Data.Databases.SqlServer.Context;
using System.Linq;
using System;

namespace Eefa.Admin.Application.CommandQueries.CorrectionRequest.Queries.GetAll
{
    public class GetAllCorrectionRequestsQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }


    public class GetAllCorrectionRequestsQueryHandler : IRequestHandler<GetAllCorrectionRequestsQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public GetAllCorrectionRequestsQueryHandler(IRepository repository, IMapper mapper, IAdminUnitOfWork adminUnitOfWork, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _adminUnitOfWork = adminUnitOfWork;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult> Handle(GetAllCorrectionRequestsQuery request, CancellationToken cancellationToken)
        {
            var permissions =await _repository.GetAll<Data.Databases.Entities.RolePermission>().Where(a=>a.RoleId== _currentUserAccessor.GetRoleId()).Select(a=>a.PermissionId).ToListAsync();
                
             
            IQueryable<CorrectionRequestModel> entities = (from c in _adminUnitOfWork.CorrectionRequests.ApplyPermission(_adminUnitOfWork, _currentUserAccessor, false, false)
                                                          from r in _adminUnitOfWork.Users.Include(a => a.Person).Where(a => a.Id == c.CreatedById).DefaultIfEmpty()
                                                          from v in _adminUnitOfWork.Users.Include(a => a.Person).Where(a => a.Id == c.VerifierUserId).DefaultIfEmpty()
                                                          from cv in _adminUnitOfWork.CodeVoucherGroups.Where(a => a.Id == c.CodeVoucherGroupId).DefaultIfEmpty()
                                                          select new CorrectionRequestModel()
                                                          {
                                                              ApiUrl = c.ApiUrl,
                                                              CodeVoucherGroupId = c.CodeVoucherGroupId,
                                                              DocumentId = c.DocumentId,
                                                              CodeVoucherGroupTitle = cv.Title,
                                                              CreatedAt = c.CreatedAt,
                                                              CreatedUserName = r.Person.LastName,
                                                              AccessPermissionId= c.AccessPermissionId,
                                                              Description = c.Description,
                                                              Id = c.Id,
                                                              ModifiedAt = c.ModifiedAt,
                                                              OldData = c.OldData,
                                                              PayLoad = c.PayLoad,
                                                              Status = c.Status.ToString(),
                                                              VerifierUserId = c.VerifierUserId,
                                                              VerifierUserName = v.Person.LastName,
                                                              CreatedById = c.CreatedById,
                                                              ViewUrl = c.ViewUrl
                                                          }).WhereQueryMaker(request.Conditions).OrderByMultipleColumns(request.OrderByProperty);


           
            //entities = entities.Where(a => permissions.Contains((int)a.AccessPermissionId) || a.CreatedById == _currentUserAccessor.GetId());

            return ServiceResult.Success(new PagedList()
            {
                Data = await entities
                .Paginate(request.Paginator())
                .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                ? await entities
                    .CountAsync(cancellationToken)
                : 0
            });


        }
    }
}
