using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Create
{
    public class CreateCommodityCategoryPropertyItemCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCommodityCategoryPropertyItemCommand>, ICommand
    {

        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// کد
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommodityCategoryPropertyItemCommand, Data.Entities.CommodityCategoryPropertyItem>();
        }
    }

    public class CreateCommodityCategoryPropertyItemCommandHandler : IRequestHandler<CreateCommodityCategoryPropertyItemCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityCategoryPropertyItem> _repository;
        private readonly IMapper _mapper;

        public CreateCommodityCategoryPropertyItemCommandHandler(IRepository<Data.Entities.CommodityCategoryPropertyItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCommodityCategoryPropertyItemCommand request, CancellationToken cancellationToken)
        {
            var commodityCategoryPropertyItem = _mapper.Map<Data.Entities.CommodityCategoryPropertyItem>(request);

            var entity = _repository.Insert(commodityCategoryPropertyItem);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
