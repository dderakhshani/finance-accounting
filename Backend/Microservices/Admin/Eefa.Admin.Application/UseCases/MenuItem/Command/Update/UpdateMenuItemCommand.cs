using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.MenuItem.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.MenuItem.Command.Update
{
    public class UpdateMenuItemCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.MenuItem>, ICommand
    {
        public int Id { get; set; }
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
        public int OrderIndex { get; set; }

        /// <summary>
        /// عنوان منو
        /// </summary>
        public string Title { get; set; } = default!;
        public string? HelpUrl { get; set; }

        /// <summary>
        /// لینک تصویر
        /// </summary>
        public string? ImageUrl { get; set; }

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
            profile.CreateMap<UpdateMenuItemCommand, Data.Databases.Entities.MenuItem>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateMenuItemCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.MenuItem>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.FormUrl = request.FormUrl;
            entity.ImageUrl = request.FormUrl;
            entity.IsActive = request.IsActive;
            entity.PageCaption = request.PageCaption;
            entity.PermissionId = request.PermissionId;
            entity.Title = request.Title;
            entity.ParentId = request.ParentId;
            entity.OrderIndex = request.OrderIndex;

            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.MenuItem, MenuItemModel>(_repository, _mapper,
                updateEntity.Entity, cancellationToken);
        }
    }
}
