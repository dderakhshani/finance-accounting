using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.CommodityPropertyValue.Update
{
    public class UpdateCommodityPropertyValueCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.CommodityPropertyValue>, ICommand
    {
        public int Id { get; set; }

        // <summary>
        /// کد کالا
        /// </summary>
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
            profile.CreateMap<UpdateCommodityPropertyValueCommand, Eefa.Commodity.Data.Entities.CommodityPropertyValue>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCommodityPropertyValueCommandHandler : IRequestHandler<UpdateCommodityPropertyValueCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityPropertyValue> _repository;
        private readonly IMapper _mapper;

        public UpdateCommodityPropertyValueCommandHandler(IRepository<Data.Entities.CommodityPropertyValue> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateCommodityPropertyValueCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateCommodityPropertyValueCommand, Data.Entities.CommodityPropertyValue>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
