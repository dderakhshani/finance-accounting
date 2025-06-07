using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.BomItem.Update
{
    public class UpdateBomItemCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.BomItem>, ICommand
    {
        public int Id { get; set; }
        public int BomId { get; set; } = default!;
        public int SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBomItemCommand, Data.Entities.BomItem>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBomCommandHandler : IRequestHandler<UpdateBomItemCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomItem> _repository;
        private readonly IMapper _mapper;

        public UpdateBomCommandHandler(IRepository<Data.Entities.BomItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateBomItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateBomItemCommand, Data.Entities.BomItem>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
