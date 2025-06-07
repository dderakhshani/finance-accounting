using AutoMapper;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Query.Get
{
    public class GetAutoVoucherFormulaQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetAutoVoucherFormulaQueryHandler : IRequestHandler<GetAutoVoucherFormulaQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAutoVoucherFormulaQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAutoVoucherFormulaQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Entities.AutoVoucherFormula>(c
            => c.ObjectId(request.Id))
            .ProjectTo<AutoVoucherFormulaModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
