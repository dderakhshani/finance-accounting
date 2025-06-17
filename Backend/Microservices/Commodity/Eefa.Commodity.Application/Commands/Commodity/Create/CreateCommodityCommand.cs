using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Common.Exceptions;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Commodity.Application.Commands.Commodity.Create
{
    public class CreateCommodityCommand : CommandBase, IRequest<ServiceResult<CommodityModel>>, IMapFrom<CreateCommodityCommand>, ICommand
    {
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
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

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
        public List<Eefa.Commodity.Data.Entities.CommodityPropertyValue> PropertyValues { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommodityCommand, Data.Entities.Commodity>()
                .ForMember(x => x.CommodityPropertyValues, opt => opt.MapFrom(x => x.PropertyValues));
        }
    }

    public class CreateCommodityCommandHandler : IRequestHandler<CreateCommodityCommand, ServiceResult<CommodityModel>>
    {
        private readonly IRepository<Data.Entities.Commodity> _commodityRepository;
        private readonly IRepository<Data.Entities.CommodityPropertyValue> _propertyValueRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor currentUserAccessor;

        public CreateCommodityCommandHandler(IRepository<Data.Entities.Commodity> commodityRepository, 
            IRepository<Data.Entities.CommodityPropertyValue> propertyValueRepository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            this.currentUserAccessor = currentUserAccessor;
            _commodityRepository = commodityRepository;
            _propertyValueRepository = propertyValueRepository;
        }


        public async Task<ServiceResult<CommodityModel>> Handle(CreateCommodityCommand request, CancellationToken cancellationToken)
        {
            var com1 = await _commodityRepository.GetAll().Where(a => a.Code.ToLower() == request.Code.ToLower()).ToListAsync();
            if (com1.Any())
            {
                throw new ValidationError("کالایی قبلا با این کد در سیستم ثبت شده است");
            }
            var com2 = await _commodityRepository.GetAll().Where(a => a.TadbirCode.ToLower() == request.TadbirCode.ToLower()).ToListAsync();
            if (com2.Any())
            {
                throw new ValidationError("کالایی قبلا با این کد تدبیر در سیستم ثبت شده است");
            }
           
            var commodity = _mapper.Map<Data.Entities.Commodity>(request);

            commodity.YearId = currentUserAccessor.GetYearId();

            var entity = _commodityRepository.Insert(commodity);
            foreach (var pv in entity.CommodityPropertyValues)
                _propertyValueRepository.Insert(pv);

            await _commodityRepository.SaveChangesAsync(cancellationToken);
            
            return ServiceResult<CommodityModel>.Success(
                _mapper.Map<CommodityModel>(commodity));
        }
    }
}
