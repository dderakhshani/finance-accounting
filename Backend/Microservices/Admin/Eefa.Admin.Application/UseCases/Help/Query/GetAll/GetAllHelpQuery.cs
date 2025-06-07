using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.UseCases.Help.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Eefa.Admin.Application.UseCases.Help.Query.GetAll
{
    public class GetAllHelpQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public bool IsMinify { get; set; } = false;
        public List<Condition> Conditions { get; set; }
    }

    public class GetAllHelpQueryHandler : IRequestHandler<GetAllHelpQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllHelpQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllHelpQuery request, CancellationToken cancellationToken)
        {
            //var entities = _repository
            //        .GetAll<Data.Databases.Entities.Help>();

            //if (request.IsMinify)
            //{
            //    entities.ProjectTo<MinifiedHelpModel>(_mapper.ConfigurationProvider);
            //}
            //else if (!request.IsMinify)
            //{
            //    entities.ProjectTo<HelpModel>(_mapper.ConfigurationProvider);
            //}

            //entities
            //   .WhereQueryMaker(request.Conditions)
            //   .OrderByMultipleColumns(request.OrderByProperty);

            if (request.IsMinify)
            {
                var minifiedEntities = _repository
                        .GetAll<Data.Databases.Entities.Help>()
                        .ProjectTo<MinifiedHelpModel>(_mapper.ConfigurationProvider)
                        .WhereQueryMaker(request.Conditions)
                        .OrderByMultipleColumns(request.OrderByProperty);

                return ServiceResult.Success(new PagedList()
                {
                    Data = await minifiedEntities
                         .Paginate(request.Paginator())
                         .ToListAsync(cancellationToken),
                    TotalCount = request.PageIndex <= 1
                         ? await minifiedEntities
                         .CountAsync(cancellationToken)
                         : 0
                });
            }
            else
            {
                var entities = _repository
                        .GetAll<Data.Databases.Entities.Help>()
                        .ProjectTo<HelpModel>(_mapper.ConfigurationProvider)
                        .WhereQueryMaker(request.Conditions)
                        .OrderByMultipleColumns(request.OrderByProperty);

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
}