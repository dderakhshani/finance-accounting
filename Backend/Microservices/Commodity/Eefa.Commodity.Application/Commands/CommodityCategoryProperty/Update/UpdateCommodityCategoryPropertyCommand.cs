using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Update;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Update
{
    public class UpdateCommodityCategoryPropertyCommand : CommandBase, IRequest<ServiceResult<CommodityCategoryPropertyModel>>, IMapFrom<Data.Entities.CommodityCategoryProperty>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
        /// قوانین حاکم بر مولفه
        /// </summary>
        public string? PropertyTypeBaseId { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        public List<UpdateCommodityCategoryPropertyItemCommand> Items { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCommodityCategoryPropertyCommand, Eefa.Commodity.Data.Entities.CommodityCategoryProperty>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCommodityCategoryPropertyCommandHandler : IRequestHandler<UpdateCommodityCategoryPropertyCommand, ServiceResult<CommodityCategoryPropertyModel>>
    {
        private readonly IRepository<Data.Entities.CommodityCategoryProperty> _repository;
        private readonly IRepository<Data.Entities.CommodityCategoryPropertyItem> _propertyItemRepository;
        private readonly IMapper _mapper;

        public UpdateCommodityCategoryPropertyCommandHandler(
            IRepository<Data.Entities.CommodityCategoryProperty> repository,
            IMapper mapper,
            IRepository<Data.Entities.CommodityCategoryPropertyItem> propertyItemRepository

            )
        {
            _mapper = mapper;
            _propertyItemRepository = propertyItemRepository;
            _repository = repository;
        }

        public async Task<ServiceResult<CommodityCategoryPropertyModel>> Handle(UpdateCommodityCategoryPropertyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.AsQueryable()
                .Where(x => x.Id == request.Id)
                .Include(x => x.CommodityCategoryPropertyItems)
                .FirstOrDefaultAsync();

            _mapper.Map<UpdateCommodityCategoryPropertyCommand, Data.Entities.CommodityCategoryProperty>(request, entity);

            
            await  DeletePropertyItems(request, entity);


            foreach (var item in request.Items)
            {
                if (item.Id == 0)
                {
                    
                    var newItem = _mapper.Map<UpdateCommodityCategoryPropertyItemCommand, Data.Entities.CommodityCategoryPropertyItem>(item, new Data.Entities.CommodityCategoryPropertyItem());
                    entity.CommodityCategoryPropertyItems.Add(newItem);
                }
                if (item.Id > 0)
                {
                    var itemToModify = entity.CommodityCategoryPropertyItems.Where(x => x.Id == item.Id ).FirstOrDefault();
                    _propertyItemRepository.Update(
                        _mapper.Map<UpdateCommodityCategoryPropertyItemCommand, Data.Entities.CommodityCategoryPropertyItem>(item, itemToModify)
                        ); ;
                    _propertyItemRepository.Update(itemToModify);
                }
            }
            _repository.Update(entity);


            await _repository.SaveChangesAsync(cancellationToken);


            return ServiceResult<CommodityCategoryPropertyModel>.Success(
                _mapper.Map<CommodityCategoryPropertyModel>(entity));
        }
        private async Task DeletePropertyItems(UpdateCommodityCategoryPropertyCommand request, Data.Entities.CommodityCategoryProperty entity)
        {
            var ItemsList = entity.CommodityCategoryPropertyItems;
            var ListId = request.Items.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var DeleteList = ItemsList.Where(a => !ListId.Contains(a.Id)).ToList();
            //---------حذف شده
            foreach (var item in DeleteList)
            {
                item.IsDeleted = true;
                _propertyItemRepository.Update(item);

            }
            await _repository.SaveChangesAsync();
        }
    }
}
