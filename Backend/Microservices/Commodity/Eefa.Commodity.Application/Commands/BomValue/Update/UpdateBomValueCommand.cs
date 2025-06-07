using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.BomValue.Update
{
    public class UpdateBomValueCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.BomValue>, ICommand
    {
        public int Id { get; set; }

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
            profile.CreateMap<UpdateBomValueCommand, Eefa.Commodity.Data.Entities.BomValue>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBomValueCommandHandler : IRequestHandler<UpdateBomValueCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomValue> _repository;
        private readonly IMapper _mapper;

        public UpdateBomValueCommandHandler(IRepository<Data.Entities.BomValue> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateBomValueCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateBomValueCommand, Data.Entities.BomValue>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();

        }
    }
}
