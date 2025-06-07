using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Language.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Language.Command.Update
{
    public class UpdateLanguageCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Language>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نماد
        /// </summary>
        public string Culture { get; set; } = default!;

        /// <summary>
        /// کد سئو
        /// </summary>
        public string? SeoCode { get; set; }

        /// <summary>
        /// نماد پرچم کشور
        /// </summary>
        public string? FlagImageUrl { get; set; }

        /// <summary>
        /// راست چین
        /// </summary>
        public int DirectionBaseId { get; set; } = default!;

        /// <summary>
        /// واحد پول پیش فرض
        /// </summary>
        public int DefaultCurrencyBaseId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateLanguageCommand, Data.Databases.Entities.Language>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateLanguageCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Language>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.DefaultCurrencyBaseId = request.DefaultCurrencyBaseId;
            entity.DirectionBaseId = request.DirectionBaseId;
            entity.FlagImageUrl = request.FlagImageUrl;
            entity.Culture = request.Culture;
            entity.Title = request.Title;
            entity.SeoCode = request.SeoCode;


            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.Language,LanguageModel>(_repository,_mapper, updateEntity.Entity, cancellationToken);
        }
    }
}
