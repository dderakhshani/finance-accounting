using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.BomItem.Create
{
    public class CreateBomItemCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateBomItemCommand>, ICommand
    {
        public int BomId { get; set; } = default!;
        public int SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBomItemCommand, Data.Entities.BomItem>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBomCommandHandler : IRequestHandler<CreateBomItemCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomItem> _repository;
        private readonly IMapper _mapper;

        public CreateBomCommandHandler(IRepository<Data.Entities.BomItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateBomItemCommand request, CancellationToken cancellationToken)
        {
            var bom = _mapper.Map<Data.Entities.BomItem>(request);

            var entity = _repository.Insert(bom);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
