using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.CommodityPropertyValue.Create
{
    public class CreateCommodityPropertyValueCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCommodityPropertyValueCommand>, ICommand
    {

        // <summary>
        /// کد کالا
        /// </summary>
        /// 
        public int Id { get; set; } = default!;
        public int CommodityId { get; set; } = default!;

        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
        /// کد آیتم ویژگی مقدار 
        /// </summary>
        public int? ValuePropertyItemId { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        public string? Value { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommodityPropertyValueCommand, Eefa.Commodity.Data.Entities.CommodityPropertyValue>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCommodityPropertyValueCommandHandler : IRequestHandler<CreateCommodityPropertyValueCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityPropertyValue> _repository;
        private readonly IMapper _mapper;

        public CreateCommodityPropertyValueCommandHandler(IRepository<Data.Entities.CommodityPropertyValue> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCommodityPropertyValueCommand request, CancellationToken cancellationToken)
        {
            var commodityPropertyValue = _mapper.Map<Data.Entities.CommodityPropertyValue>(request);

            var entity = _repository.Insert(commodityPropertyValue);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
