using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class UpdateAssetsCommand : CommandBase, IRequest<ServiceResult<AssetsModel>>, IMapFrom<Assets>, ICommand
    {
        public int Id { get; set; }
        public int AssetGroupId { get; set; }
        public int? UnitId { get; set; }
        public string CommoditySerial { get; set; } = default!;
        public string AssetSerial { get; set; } = default!;
        public int? DepreciationTypeBaseId { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public string Description { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAssetsCommand, Assets>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAssetsCommandHandler : IRequestHandler<UpdateAssetsCommand, ServiceResult<AssetsModel>>
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly IMapper _mapper;

        public UpdateAssetsCommandHandler(IAssetsRepository warehousRepository, IMapper mapper)
        {
            _mapper = mapper;
            _assetsRepository = warehousRepository;
        }

        public async Task<ServiceResult<AssetsModel>> Handle(UpdateAssetsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _assetsRepository.Find(request.Id);

            _mapper.Map<UpdateAssetsCommand, Assets>(request, entity);

            _assetsRepository.Update(entity);
            await _assetsRepository.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<AssetsModel>(entity);
            return ServiceResult<AssetsModel>.Success(model);
        }
    }
}
