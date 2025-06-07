using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Attachments.Queries
{
    public class GetAllAttachmentsQuery : Pagination, IRequest<ServiceResult<PagedList<Attachment>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllAttachmentsQueryHandler : IRequestHandler<GetAllAttachmentsQuery, ServiceResult<PagedList<Attachment>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Attachment> _repository;

        public GetAllAttachmentsQueryHandler(IMapper mapper, IRepository<Attachment> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Attachment>>> Handle(GetAllAttachmentsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Attachment>>.Success(new PagedList<Attachment>()
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
