using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Attachment.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Attachment.Query.Get
{
    public class GetAttachmentQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetAttachmentQueryHandler : IRequestHandler<GetAttachmentQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAttachmentQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAttachmentQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Attachment>(c
            => c.ObjectId(request.Id))
            .ProjectTo<AttachmentModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
