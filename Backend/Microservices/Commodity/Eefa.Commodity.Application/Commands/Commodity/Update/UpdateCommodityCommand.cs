using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Commodity.Data;
using System.Linq.Dynamic.Core;
using System.Linq;
using Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Update;
using Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Update;
using Eefa.Commodity.Application.Commands.CommodityPropertyValue.Create;
using Microsoft.EntityFrameworkCore;
using Eefa.Commodity.Data.Entities;

namespace Eefa.Commodity.Application.Commands.Commodity.Update
{
    public class UpdateCommodityCommand : CommandBase, IRequest<ServiceResult<CommodityModel>>, IMapFrom<Data.Entities.Commodity>, ICommand
    {

        public int Id { get; set; }

        public string? CommodityNationalId { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int? CommodityCategoryId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد محصول
        /// </summary>
        public string? Code { get; set; }
        public string? SecondaryCode { get; set; }
        public string? ThirdCode { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Descriptions { get; set; }

        /// <summary>
        /// کد واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }



        /// <summary>
        /// حداقل تعداد
        /// </summary>
        public double MinimumQuantity { get; set; } = default!;

        /// <summary>
        /// حداکثر تعداد
        /// </summary>
        public double? MaximumQuantity { get; set; }

        /// <summary>
        /// نوع محاسبه قیمت
        /// </summary>
        public int? PricingTypeBaseId { get; set; }
        /// <summary>
        /// عنوان کد ملی کالا
        /// </summary>
        public string CommodityNationalTitle { get; set; }
        public bool? InventoryType { get; set; }

        public bool? IsActive { get; set; }
        public List<CreateCommodityPropertyValueCommand> PropertyValues { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCommodityCommand, Data.Entities.Commodity>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCommodityCommandHandler : IRequestHandler<UpdateCommodityCommand, ServiceResult<CommodityModel>>
    {
        private readonly IRepository<Data.Entities.Commodity> _repository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<Data.Entities.CommodityPropertyValue> _commodityPropertyValue;
        private readonly IMapper _mapper;

        public UpdateCommodityCommandHandler(
            IRepository<DocumentItem> documentItemRepository,
            IRepository<Data.Entities.Commodity> repository,
            IRepository<Data.Entities.CommodityPropertyValue> commodityPropertyValue,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _documentItemRepository = documentItemRepository;
            _commodityPropertyValue = commodityPropertyValue;
        }

        public async Task<ServiceResult<CommodityModel>> Handle(UpdateCommodityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);
            List<Data.Entities.CommodityPropertyValue> entities = await _commodityPropertyValue.GetAll().Where(a => a.CommodityId == request.Id).ToListAsync();

            //اگر واحد کالا اشتباه بود ، وقبلا در روی رسیدهای انبار اعلام اشتباهی کرده باشند
            if (entity.MeasureId != request.MeasureId)
            {
                UpdateWrongMeasure(request, entity);

            }

            _mapper.Map<UpdateCommodityCommand, Data.Entities.Commodity>(request, entity);

            await DeletePropertyItems(request, entities);


            foreach (var item in request.PropertyValues)
            {
                if (item.Id == 0)
                {
                    var newItem = _mapper.Map<CreateCommodityPropertyValueCommand, Data.Entities.CommodityPropertyValue>(item, new Data.Entities.CommodityPropertyValue());

                    _commodityPropertyValue.Insert(newItem);


                }
                if (item.Id > 0)
                {
                    var itemToModify = entity.CommodityPropertyValues.Where(x => x.Id == item.Id).FirstOrDefault();
                    _commodityPropertyValue.Update(
                        _mapper.Map<CreateCommodityPropertyValueCommand, Data.Entities.CommodityPropertyValue>(item, itemToModify)
                        ); ;
                    _commodityPropertyValue.Update(itemToModify);
                }
            }

            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);


            return ServiceResult<CommodityModel>.Success(
                _mapper.Map<CommodityModel>(entity));

        }
        private void UpdateWrongMeasure(UpdateCommodityCommand request, Data.Entities.Commodity entity)
        {

            var items = _documentItemRepository.GetAll().Where(a => a.CommodityId == request.Id && a.IsWrongMeasure == true).ToList();
            items.ForEach(a =>
            {
                a.IsWrongMeasure = false;
                _documentItemRepository.Update(a);
            });


        }
        private async Task DeletePropertyItems(UpdateCommodityCommand request, List<Data.Entities.CommodityPropertyValue> entities)
        {
           
            var ListId = request.PropertyValues.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            if(entities != null && ListId.Any()  && entities.Any() )
            {
                var DeleteList = entities.Where(a => !ListId.Contains(a.Id)).ToList();
                //---------حذف شده
                foreach (var item in DeleteList)
                {
                    item.IsDeleted = true;
                    _commodityPropertyValue.Update(item);

                }
                await _commodityPropertyValue.SaveChangesAsync();
            }
            
        }
    }
}
