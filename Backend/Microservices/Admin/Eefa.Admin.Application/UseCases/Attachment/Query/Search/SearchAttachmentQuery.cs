using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Attachment.Model;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Attachment.Query.Search
{
    public class SearchAttachmentQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions{ get; set; }
    }

    public class SearchAttachmentQueryHandler : IRequestHandler<SearchAttachmentQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public SearchAttachmentQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(SearchAttachmentQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(
              (await _repository
                  .GetAll<Data.Databases.Entities.Attachment>(config => config
                      .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Data.Databases.Entities.Attachment>(request.Conditions))
                       .Paginate(new Pagination()
                       {
                           PageSize = request.PageSize,
                           PageIndex = request.PageIndex,
                            
                            
                       }))
                  .ProjectTo<AttachmentModel>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken)));
        }
    }
}
