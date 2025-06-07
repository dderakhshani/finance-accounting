using System.Collections.Generic;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountReference.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using System.Linq;
using System;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Query.GetAll
{
    public class GetAllAccountReferenceQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

        public GetAllAccountReferenceQuery()
        {

        }
    }


    public class GetAllAccountReferenceQueryHandler : IRequestHandler<GetAllAccountReferenceQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAccountReferenceQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllAccountReferenceQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Entities.AccountReference>()
                .ProjectTo<AccountReferenceModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            try
            {


            var result = ServiceResult.Success(new PagedList()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                throw new Exception(e.Message);
            }

       
        }
    }
}
