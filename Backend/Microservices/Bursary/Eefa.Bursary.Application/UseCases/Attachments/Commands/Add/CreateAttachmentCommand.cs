using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Attachments.Commands.Add
{
    public class CreateAttachmentCommand : CommandBase, IRequest<ServiceResult<Attachment>>, IMapFrom<CreateAttachmentCommand>, ICommand
    {
        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string FileNumber { get; set; } = default!;
        public string? Description { get; set; }
        public string? KeyWords { get; set; }
        public string Url { get; set; } = default!;
        public bool IsUsed { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAttachmentCommand, Attachment>().IgnoreAllNonExisting();
        }
    }

    public class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, ServiceResult<Attachment>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateAttachmentCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Attachment>> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var atch = _mapper.Map<Attachment>(request);

            if (atch == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            _uow.Attachment.Add(atch);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات دسته چک");

            return ServiceResult<Attachment>.Success(atch);
        }
    }


}
