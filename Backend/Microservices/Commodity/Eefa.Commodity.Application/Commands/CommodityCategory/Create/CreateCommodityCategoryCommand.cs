using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;

namespace Eefa.Commodity.Application.Commands.CommodityCategory.Create
{
    public class CreateCommodityCategoryCommand : CommandBase, IRequest<ServiceResult<CommodityCategoryModel>>, IMapFrom<CreateCommodityCategoryCommand>, ICommand
    {

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;
        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; } = default!;
        public int CodingMode { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// کدواحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        public bool? RequireParentProduct { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommodityCategoryCommand, Data.Entities.CommodityCategory>()
                .IgnoreAllNonExisting();

        }
    }

    public class CreateCommodityCategoryCommandHandler : IRequestHandler<CreateCommodityCategoryCommand, ServiceResult<CommodityCategoryModel>>
    {
        private readonly IRepository<Data.Entities.CommodityCategory> _repository;
        private readonly IMapper _mapper;

        public CreateCommodityCategoryCommandHandler(IRepository<Data.Entities.CommodityCategory> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult<CommodityCategoryModel>> Handle(CreateCommodityCategoryCommand request, CancellationToken cancellationToken)
        {
            var commodityCategory = _mapper.Map<Data.Entities.CommodityCategory>(request);
            request.MeasureId = request.MeasureId == 0 ? null : request.MeasureId;

            var entity = _repository.Insert(commodityCategory);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult<CommodityCategoryModel>.Success(
                _mapper.Map<CommodityCategoryModel>(entity)
            );
        }
    }
}
