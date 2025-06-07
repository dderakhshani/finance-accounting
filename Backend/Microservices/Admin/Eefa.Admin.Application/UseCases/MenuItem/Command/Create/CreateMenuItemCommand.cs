using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;

namespace Eefa.Admin.Application.CommandQueries.MenuItem.Command.Create
{
    public class CreateMenuItemCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateMenuItemCommand>, ICommand
    {
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد دسترسی
        /// </summary>
        public int? PermissionId { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int? OrderIndex { get; set; }

        /// <summary>
        /// عنوان منو
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// لینک تصویر
        /// </summary>
        public string? ImageUrl { get; set; }
        public string? HelpUrl { get; set; }

        /// <summary>
        /// لینک فرم
        /// </summary>
        public string? FormUrl { get; set; }

        /// <summary>
        /// عنوان صفحه
        /// </summary>
        public string? PageCaption { get; set; }

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMenuItemCommand, Data.Databases.Entities.MenuItem>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateMenuItemCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.MenuItem>(request));
            
            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
