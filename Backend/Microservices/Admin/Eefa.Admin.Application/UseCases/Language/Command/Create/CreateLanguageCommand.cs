using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;


namespace Eefa.Admin.Application.CommandQueries.Language.Command.Create
{
    public class CreateLanguageCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateLanguageCommand>, ICommand
    {
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
            profile.CreateMap<CreateLanguageCommand, Data.Databases.Entities.Language>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUpLoader _upLoader;

        public CreateLanguageCommandHandler(IRepository repository, IMapper mapper, IUpLoader upLoader)
        {
            _mapper = mapper;
            _upLoader = upLoader;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Language>(request));
            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    if (!string.IsNullOrEmpty(request.FlagImageUrl))
                    {
                        var profileUrl = await _upLoader.UpLoadAsync(request.FlagImageUrl,
                            CustomPath.FlagImage, cancellationToken);

                        entity.Entity.FlagImageUrl = profileUrl.ReletivePath;

                        _repository.Update(entity.Entity);
                        await _repository.SaveChangesAsync(request.MenueId,cancellationToken);
                    }
                    return ServiceResult.Success(entity.Entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity.Entity);
            }

            return ServiceResult.Failure();
        }
    }
}
