using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// TODO Removed GenerateVoucherHead Function And All line where this was called
public class CreateAutoVoucher2Command : IRequest<ServiceResult<CreateAutoVoucher2CommandHandler.AutoVoucherResault>>, IMapFrom<CreateAutoVoucher2Command>
{
    public List<JObject> DataList { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVouchersHeadCommand, VouchersHead>()
            .IgnoreAllNonExisting();
    }
}

public class CreateAutoVoucher2CommandHandler : IRequestHandler<CreateAutoVoucher2Command, ServiceResult<CreateAutoVoucher2CommandHandler.AutoVoucherResault>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    private readonly IMediator _mediator;
    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;

    // private int DailyId { get; set; } = default;
    private readonly ICollection<EntityEntry<VouchersHead>> _insertedVoucherHeadsCollection;
    VouchersService vouchersService;

    public CreateAutoVoucher2CommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
        IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures,
        IApplicationUser applicationUser, IMediator mediator)
    {
        _mapper = mapper;
        _applicationUser = applicationUser;
        _mediator = mediator;
        _unitOfWork= unitOfWork;
        _insertedVoucherHeadsCollection = new List<EntityEntry<VouchersHead>>();
        vouchersService = new VouchersService(_unitOfWork, _applicationUser);
        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
    }

    public record AutoVoucherResault(IEnumerable<int> VoucherNo, IEnumerable<int> VoucherHeadId)
    {
        public ICollection<VouchersHead> VoucherHeads { get; set; }
    }

    public class AutoVoucherMappedPayload
    {
        public int CodeVoucherGroupId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public JObject Json { get; set; }
        public dynamic dynamicData { get; set; }
    }

    public async Task<ServiceResult<CreateAutoVoucher2CommandHandler.AutoVoucherResault>> Handle(CreateAutoVoucher2Command request, CancellationToken cancellationToken)
    {

        var mappedData = request.DataList.Select(x => new AutoVoucherMappedPayload
        {
            CodeVoucherGroupId = x.SelectToken("CodeVoucherGroupId").Value<int>(),
            DocumentId = x.SelectToken("DocumentId").Value<int>(),
            DocumentNo = x.SelectToken("DocumentNo").Value<string>(),
            DocumentDate = DateTime.Parse(x.SelectToken("DocumentDate").Value<string>()),
            Json = x,
            dynamicData = JsonConvert.DeserializeObject(x.ToString(Newtonsoft.Json.Formatting.None))
        }).ToList();

        foreach (var item in mappedData)
        {
            var isAlreadySubmitted = false;

            if (item.DocumentId != default) isAlreadySubmitted = await _unitOfWork.VouchersDetails.ExistsAsync(x => !x.IsDeleted && x.DocumentId == item.DocumentId && x.Voucher.CodeVoucherGroupId == item.CodeVoucherGroupId);
            if (isAlreadySubmitted) throw new ValidationException("", new List<string> { $" سند با شماره  {item.DocumentNo} قبلا ثبت شده است" });
        }

        var groupByDataList = new Dictionary<int, List<JObject>>();

        foreach (var group in mappedData.GroupBy(x => (x.CodeVoucherGroupId.ToString() + Convert.ToDateTime(x.DocumentDate.ToShortDateString()).ToUniversalTime().ToString()).GetHashCode()))
        {
            groupByDataList.Add(group.Key, group.Select(x => x.Json).ToList());
        }

        var dataListPerCodeVoucherGroup = vouchersService.GroupByDataList<int>("CodeVoucherGroupId", request.DataList);

        await vouchersService.GetRequiredData(groupByDataList.Select(x => x.Value.Select(x => (int)x.SelectToken("CodeVoucherGroupId")).FirstOrDefault()).ToList());

        foreach (var item in groupByDataList.Select((value, index) => new { index, value }))
        {
            var dataList = item.value;
            var codeVoucherGroupId = item.value.Value.Select(x => (int)x.SelectToken("CodeVoucherGroupId")).FirstOrDefault();
            var documentDate = DateTime.UtcNow;
            //TODO:
            var firstRow = dataList.Value[0];
            documentDate = firstRow.TryGetValue<DateTime>("DocumentDate");

            var codeVoucherGroup = vouchersService.codeVoucherGroups.FirstOrDefault(x => x.Id == codeVoucherGroupId);
            var formulaEntities = vouchersService.formulas.Where(x => x.VoucherTypeId == codeVoucherGroupId).ToList();

            //var insertVoucherHead = await GenerateVoucherHead(codeVoucherGroup, documentDate, item.index);
            //_insertedVoucherHeadsCollection.Add(insertVoucherHead);

            foreach (var autoVoucherFormula in formulaEntities)
            {
                var formulas = JsonConvert.DeserializeObject<List<FormulasModel.Formula>>(autoVoucherFormula.Formula);

                if (!string.IsNullOrEmpty(autoVoucherFormula.GroupBy))
                {
                    if (!vouchersService.CheckCondition(autoVoucherFormula.Conditions, dataList.Value)) continue;


                    var temp = vouchersService.GroupByDataList<DateTime>(autoVoucherFormula.GroupBy, dataList.Value);
                    foreach (var t in temp)
                    {
                        foreach (var row in t.Value)
                        {
                            if (!vouchersService.CheckCondition(autoVoucherFormula.Conditions, null, row))
                            {
                                continue;
                            }
                            //await vouchersService.CreateVoucherDetail(codeVoucherGroup, row, autoVoucherFormula, formulas, insertVoucherHead.Entity, dataList);
                        }
                    }
                    continue;
                }

                foreach (var row in dataList.Value)
                {
                    if (!vouchersService.CheckCondition(autoVoucherFormula.Conditions, null, row))
                    {
                        continue;
                    }

                    //await vouchersService.CreateVoucherDetail(codeVoucherGroup, row, autoVoucherFormula, formulas, insertVoucherHead.Entity, dataList);
                }
            }

            //foreach (var voucherDetail in insertVoucherHead.Entity.VouchersDetails.Select((value, index) => new { index, value }))
            //{
            //    voucherDetail.value.RowIndex = voucherDetail.index + 1;
            //}
        }

        foreach (var voucher in _insertedVoucherHeadsCollection)
        {
            voucher.Entity.TotalCredit = voucher.Entity.VouchersDetails.Sum(x => x.Credit);
            voucher.Entity.TotalDebit = voucher.Entity.VouchersDetails.Sum(x => x.Debit);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            var result = new AutoVoucherResault(_insertedVoucherHeadsCollection.Select(x => x.Entity.VoucherNo), _insertedVoucherHeadsCollection.Select(x => x.Entity.Id));
            //result.VoucherHeads = _insertedVoucherHeadsCollection.Select(x => x.Entity).ToList();
            return ServiceResult.Success(result);
    }

    //private async Task<EntityEntry<VouchersHead>> GenerateVoucherHead(CodeVoucherGroup codeVoucherGroup, DateTime voucherDate, int voucherIndex)
    //{
    //    PersianCalendar pc = new PersianCalendar();
    //    var persianDate = string.Format("{0}/{1}/{2}", pc.GetYear(voucherDate), pc.GetMonth(voucherDate), pc.GetDayOfMonth(voucherDate));
    //    //Note time assumed to be UTC

    //    return _unitOfWork.VouchersHeads.Add(new VouchersHead()
    //    {
    //        VoucherNo = await VoucherNo.GetNewVoucherNo(_unitOfWork, _applicationUser,
    //            voucherDate,
    //            null) + voucherIndex,
    //        CompanyId = _applicationUser.CompanyId,
    //        YearId = _applicationUser.YearId,
    //        CodeVoucherGroupId = codeVoucherGroup.Id,
    //        VoucherDailyId = await vouchersService.GetDailyId(voucherDate),
    //        VoucherStateId = 1,
    //        VoucherDate = voucherDate,
    //        VoucherDescription =
    //            $"سند مکانیزه {codeVoucherGroup.Title} مورخ {persianDate}"
    //    });
    //}
}