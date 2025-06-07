using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.BomValue.Create
{
    public class CreateBomValueCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateBomValueCommand>, ICommand
    {

        /// <summary>
        /// کد سند فرمول ساخت
        /// </summary>
        public int BomValueHeaderId { get; set; } = default!;

        /// <summary>
        /// کد کالای مصرفی
        /// </summary>
        public int UsedCommodityId { get; set; } = default!;

        /// <summary>
        /// کد فرمول ساخت زیر مجموعه
        /// </summary>
        public int SubBomId { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public double Value { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBomValueCommand, Data.Entities.BomValue>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBomValueCommandHandler : IRequestHandler<CreateBomValueCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomValue> _repository;
        private readonly IMapper _mapper;

        public CreateBomValueCommandHandler(IRepository<Data.Entities.BomValue>  repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateBomValueCommand request, CancellationToken cancellationToken)
        {
            var bomValue = _mapper.Map<Data.Entities.BomValue>(request);

            var entity = _repository.Insert(bomValue);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
