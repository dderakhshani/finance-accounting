using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;

namespace Eefa.Commodity.Application.Commands.CommodityCategory.Update
{
    public class UpdateCommodityCategoryCommand : CommandBase, IRequest<ServiceResult<CommodityCategoryModel>>, IMapFrom<Data.Entities.CommodityCategory>, ICommand
    {
        public int Id { get; set; }

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
        public int? MeasureId { get; set; } = default!;

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
            profile.CreateMap<UpdateCommodityCategoryCommand, Eefa.Commodity.Data.Entities.CommodityCategory>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCommodityCategoryCommandHandler : IRequestHandler<UpdateCommodityCategoryCommand, ServiceResult<CommodityCategoryModel>>
    {
        private readonly IRepository<Data.Entities.CommodityCategory> _repository;
        private readonly IMapper _mapper;

        public UpdateCommodityCategoryCommandHandler(IRepository<Data.Entities.CommodityCategory> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<CommodityCategoryModel>> Handle(UpdateCommodityCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);
            request.MeasureId = request.MeasureId == 0 ? null : request.MeasureId;

            _mapper.Map<UpdateCommodityCategoryCommand, Data.Entities.CommodityCategory>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult<CommodityCategoryModel>.Success(
               _mapper.Map<CommodityCategoryModel>(entity)
           );
        }
    }
}
