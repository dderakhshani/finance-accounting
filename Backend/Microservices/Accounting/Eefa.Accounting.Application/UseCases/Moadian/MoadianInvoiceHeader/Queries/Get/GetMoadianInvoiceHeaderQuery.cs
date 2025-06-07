using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Queries.Get
{
    public class GetMoadianInvoiceHeaderQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }
    public class GetMoadianInvoiceHeaderQueryHandler : IRequestHandler<GetMoadianInvoiceHeaderQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetMoadianInvoiceHeaderQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetMoadianInvoiceHeaderQuery request, CancellationToken cancellationToken)
        {

            var entity = await _repository
                .Find<Data.Entities.MoadianInvoiceHeader>(c
            => c.ObjectId(request.Id))
                .Include(x => x.Person)
                .Include(x => x.AccountReference)
                .Include(x => x.Customer)
            .ProjectTo<MoadianInvoiceHeaderDetailedModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

            return ServiceResult.Success(entity);
        }
    }

}
