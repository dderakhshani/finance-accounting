using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Attachments.Queries
{
    public class GetAttachmentQuery : IRequest<ServiceResult<Attachment>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetAttachmentQueryHandler : IRequestHandler<GetAttachmentQuery, ServiceResult<Attachment>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAttachmentQueryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<Attachment>> Handle(GetAttachmentQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Attachment
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<Attachment>.Success(r);
        }
    }


}
