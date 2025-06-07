using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Query.Get
{
    public class GetCodeRowDescriptionQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCodeRowDescriptionQueryHandler : IRequestHandler<GetCodeRowDescriptionQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetCodeRowDescriptionQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetCodeRowDescriptionQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Entities.CodeRowDescription>(c
            => c.ObjectId(request.Id))
            .ProjectTo<CodeRowDescriptionModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
