using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class UpdateAutoVoucherCommand : IRequest<ServiceResult<int>>, IMapFrom<UpdateAutoVoucherCommand>
{
    public long VocherHeadId { get; set; }
    public List<JObject> DataList { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVouchersHeadCommand, VouchersHead>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateAutoVoucherCommandHandler : IRequestHandler<UpdateAutoVoucherCommand, ServiceResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    private readonly IMediator _mediator;
    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
    private int DailyId { get; set; } = default;
    VouchersService vochersService;

    public UpdateAutoVoucherCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IApplicationUser applicationUser,
        IMediator mediator,
        IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures
        )
    {
        _mapper = mapper;
        _mediator = mediator;
        _applicationUser = applicationUser;
        _unitOfWork = unitOfWork;
        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;

        vochersService = new VouchersService(_unitOfWork, _applicationUser);
    }

    //public record AutoVoucherResult(IEnumerable<int> VoucherNo, IEnumerable<int> VoucherHeadId)
    //{
    //    public override string ToString()
    //    {
    //        return $"{{ VoucherNo = {VoucherNo}, VoucherHeadId = {VoucherHeadId} }}";
    //    }
    //}

    public async Task<ServiceResult<int>> Handle(UpdateAutoVoucherCommand request, CancellationToken cancellationToken)
    {
        var dataListPerCodeVoucherGroup = vochersService.GroupByDataList<int>("CodeVoucherGroupId", request.DataList);

        //Delete all prev VoucherDetail contains DocumentIds in DataList
        await DeleteVoucherDetail(request.VocherHeadId, request.DataList);

        var vocherHead = await _unitOfWork.VouchersHeads.GetByIdAsync(request.VocherHeadId);

        vocherHead.ModifiedAt = DateTime.UtcNow;
        vocherHead.ModifiedById = _applicationUser.RoleId;
        ;

        //CAUTION: Only one CodeVoucherGroup is Supported
        foreach (var dataList in dataListPerCodeVoucherGroup)
        {
            var codeVoucherGroupId = dataList.Key;
            var documentDate = DateTime.UtcNow;
            //TODO:
            var firstRow = request.DataList[0];
            documentDate = firstRow.TryGetValue<DateTime>("DocumentDate");

            var codeVoucherGroup = await _unitOfWork.CodeVoucherGroups
                                         .GetByIdAsync(codeVoucherGroupId);

            var spesification = new Specification<AutoVoucherFormula>();
            spesification.ApplicationConditions.Add(x => x.VoucherTypeId == codeVoucherGroupId);
            spesification.OrderBy = y => y.OrderBy(x => x.OrderIndex);

            var formulaEntities = await _unitOfWork.AutoVoucherFormulas
                                         .GetListAsync(spesification);

            //Note: not path reques to make clear what method needs

            foreach (var autoVoucherFormula in formulaEntities)
            {
                var formulas = JsonConvert.DeserializeObject<List<FormulasModel.Formula>>(autoVoucherFormula.Formula);

                if (!string.IsNullOrEmpty(autoVoucherFormula.GroupBy))
                {
                    if (!vochersService.CheckCondition(autoVoucherFormula.Conditions, dataList.Value))
                    {
                        continue;
                    }

                    var temp = vochersService.GroupByDataList<DateTime>(autoVoucherFormula.GroupBy, dataList.Value);
                    foreach (var t in temp)
                    {
                        foreach (var row in t.Value)
                        {
                            if (!vochersService.CheckCondition(autoVoucherFormula.Conditions, null, row))
                            {
                                continue;
                            }

                            await vochersService.CreateVoucherDetail(codeVoucherGroup, row, autoVoucherFormula, formulas, vocherHead, dataList);
                        }
                    }
                    continue;
                }

                foreach (var row in dataList.Value)
                {
                    if (!vochersService.CheckCondition(autoVoucherFormula.Conditions, null, row))
                    {
                        continue;
                    }

                    await this.vochersService.GetRequiredData(new List<int>() { codeVoucherGroupId });
                    await vochersService.CreateVoucherDetail(codeVoucherGroup, row, autoVoucherFormula, formulas, vocherHead, dataList);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _accountingUnitOfWorkProcedures.StpUpdateTotalDebitCreditOnVoucherDetail();

        return ServiceResult.Success(vocherHead.Id);
    }

    private async Task DeleteVoucherDetail(long vocherHeadId, List<JObject> dataList)
    {
        var documetIds = new List<int?>(); ;
        foreach (var row in dataList)
        {
            var documentId = row.TryGetValue<int>(nameof(VouchersDetail.DocumentId));
            documetIds.Add(documentId);
        }
        var vocherDetails = await _unitOfWork.VouchersDetails
                                 .GetListAsync(x => x.VoucherId == vocherHeadId && 
                                                    documetIds.Contains(x.DocumentId));
        
        foreach (var detail in vocherDetails)
        {
            _unitOfWork.VouchersDetails.Delete(detail);
        }
    }
}