using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetAllBySpecificCondition
{
    public class GetAllBySpecificQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }

        public int? FromNo { get; set; }
        public int? ToNo { get; set; }

        public int? VoucherStateId { get; set; }
        public int? CodeVoucherGroupId { get; set; }
    }

    public class GetAllBySpecificQueryHandler : IRequestHandler<GetAllBySpecificQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public GetAllBySpecificQueryHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllBySpecificQuery request, CancellationToken cancellationToken)
        {
            var year = await _repository.GetQuery<Data.Entities.Year>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetYearId(), cancellationToken: cancellationToken);

            var query = _repository
                .GetQuery<Data.Entities.VouchersHead>();

            query = query.Where(x =>
                x.VoucherStateId != 3);

            if (request.FromDateTime != null)
            {
                query = query.Where(x =>
                    x.VoucherDate >= request.FromDateTime
                );
            }
            else
            {
                query = query.Where(x =>
                    x.VoucherDate >= year.FirstDate
                );
            }

            if (request.ToDateTime != null)
            {
                query = query.Where(x =>
                    x.VoucherDate <= request.ToDateTime);
            }
            else
            {
                query = query.Where(x =>
                    x.VoucherDate <= year.LastDate);
            }

            if (request.FromNo != null)
            {
                query = query.Where(x =>
                    x.VoucherNo >= request.FromNo);
            }

            if (request.ToNo != null)
            {
                query = query.Where(x =>
                    x.VoucherNo <= request.ToNo);
            }

            if (request.VoucherStateId != null)
            {
                query = query.Where(
                    x => x.VoucherStateId == request.VoucherStateId);
            }

            if (request.CodeVoucherGroupId != null)
            {
                query = query.Where(
                    x => x.CodeVoucherGroupId == request.CodeVoucherGroupId);
            }


            var entitis = query
                .ProjectTo<VouchersHeadModel>(_mapper.ConfigurationProvider)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });

        }
    }
}
