namespace Eefa.Inventory.Application
{
    //public class UpdatePersonsDebitedCommoditiesCommand : CommandBase, IRequest<ServiceResult<PersonsDebitedCommoditiesModel>>, IMapFrom<PersonsDebitedCommodities>, ICommand
    //{
    //    public int Id { get; set; }
    //    public int AssetGroupId { get; set; }
    //    public int? UnitId { get; set; }
    //    public string CommoditySerial { get; set; } = default!;
    //    public string PersonsDebitedCommoditieserial { get; set; } = default!;
    //    public int? DepreciationTypeBaseId { get; set; } = default!;
    //    public bool IsActive { get; set; } = default!;

    //}


    //public class UpdatePersonsDebitedCommoditiesCommandHandler : IRequestHandler<UpdatePersonsDebitedCommoditiesCommand, ServiceResult<PersonsDebitedCommoditiesModel>>
    //{
    //    private readonly IPersonsDebitedCommoditiesRepository _warehousRepository;
    //    private readonly IMapper _mapper;

    //    public UpdatePersonsDebitedCommoditiesCommandHandler(IPersonsDebitedCommoditiesRepository warehousRepository, IMapper mapper)
    //    {
    //        _mapper = mapper;
    //        _warehousRepository = warehousRepository;
    //    }

    //    public async Task<ServiceResult<PersonsDebitedCommoditiesModel>> Handle(UpdatePersonsDebitedCommoditiesCommand request, CancellationToken cancellationToken)
    //    {
    //        var entity = await _warehousRepository.Find(request.Id);

    //        //_mapper.Map<UpdatePersonsDebitedCommoditiesCommand, PersonsDebitedCommodities>(request, entity);

    //        _warehousRepository.Update(entity);
    //        await _warehousRepository.SaveChangesAsync(cancellationToken);

    //        var model = _mapper.Map<PersonsDebitedCommoditiesModel>(entity);
    //        return ServiceResult<PersonsDebitedCommoditiesModel>.Success(model);
    //    }
    //}
}
