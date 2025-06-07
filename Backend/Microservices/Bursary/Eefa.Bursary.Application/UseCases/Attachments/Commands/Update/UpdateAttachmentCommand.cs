using AutoMapper;
using Eefa.Bursary.Application.UseCases.Attachments.Commands.Add;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Attachments.Commands.Update
{
    public class UpdateAttachmentCommand : CommandBase, IRequest<ServiceResult<Attachment>>, IMapFrom<UpdateAttachmentCommand>, ICommand
    {
        public int Id { get; set; }
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

    public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, ServiceResult<Attachment>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateAttachmentCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Attachment>> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var atch = await _uow.Attachment.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.Id == request.Id);
            if (atch == null)
            {
                throw new ValidationError("فایل مورد نظر در سیستم وجود ندارد");
            }
            _mapper.Map(request, atch);

            _uow.Attachment.Update(atch);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
            {
                throw new Exception("بروز خطا در ثبت اطلاعات فایل");
            }
            return ServiceResult<Attachment>.Success(atch);
        }
    }


}
