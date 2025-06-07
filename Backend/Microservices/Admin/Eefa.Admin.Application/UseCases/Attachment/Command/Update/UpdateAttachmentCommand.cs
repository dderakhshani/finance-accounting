using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Attachment.Command.Update
{
    public class UpdateAttachmentCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Attachment>, ICommand
    {
        public int Id { get; set; }
        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? KeyWords { get; set; }
        public string Url { get; set; } = default!;
        public bool IsUsed { get; set; }
        public string FileNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAttachmentCommand, Data.Databases.Entities.Attachment>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUpLoader _upLoader;
        public UpdateAttachmentCommandHandler(IRepository repository, IMapper mapper, IUpLoader upLoader)
        {
            _mapper = mapper;
            _upLoader = upLoader;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Attachment>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);


            var profileUrl = await _upLoader.UpLoadAsync(request.Url,
                CustomPath.Attachment, cancellationToken);

            entity.Url = profileUrl.ReletivePath;
            entity.Title = request.Title;
            entity.LanguageId = request.LanguageId;
            entity.Description = request.Description;
            entity.Extention = request.Extention;
            entity.KeyWords = request.KeyWords;
            entity.TypeBaseId = request.TypeBaseId;
            entity.IsUsed = request.IsUsed;

            var updatedEntity =  _repository.Update(entity);

            return await request.Save(_repository, updatedEntity.Entity, cancellationToken);

        }
    }
}
