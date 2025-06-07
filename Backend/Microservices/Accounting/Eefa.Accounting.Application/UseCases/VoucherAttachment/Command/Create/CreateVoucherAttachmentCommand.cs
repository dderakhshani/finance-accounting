using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;

namespace Eefa.Accounting.Application.UseCases.VoucherAttachment.Command.Create
{
    public class CreateVoucherAttachmentCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateVoucherAttachmentCommand>, ICommand
    {
        public int VoucherHeadId { get; set; }
        public ICollection<int> AttachmentIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateVoucherAttachmentCommand, Data.Entities.VoucherAttachment>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateVoucherAttachmentCommandHandler : IRequestHandler<CreateVoucherAttachmentCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateVoucherAttachmentCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateVoucherAttachmentCommand request, CancellationToken cancellationToken)
        {
            foreach (var attachment in request.AttachmentIds)
            {
                _repository.Insert(new Data.Entities.VoucherAttachment()
                    { VoucherHeadId = request.VoucherHeadId, AttachmentId = attachment });
            }

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success();
                }
            }
            else
            {
                return ServiceResult.Success();
            }

            return ServiceResult.Failure();
        }
    }
}
