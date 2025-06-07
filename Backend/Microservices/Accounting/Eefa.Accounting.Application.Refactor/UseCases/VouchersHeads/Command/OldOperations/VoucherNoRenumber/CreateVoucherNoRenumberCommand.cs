using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateVoucherNoRenumberCommand : IRequest<ServiceResult>, IMapFrom<CreateVoucherNoRenumberCommand>
{
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }

    public int? FromNo { get; set; }
    public int? ToNo { get; set; }

    public int? VoucherStateId { get; set; }
    public int? CodeVoucherGroupId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVoucherNoRenumberCommand, VouchersHead>()
            .IgnoreAllNonExisting();
    }
}

//public class CreateVoucherNoRenumberCommandHandler : IRequestHandler<CreateVoucherNoRenumberCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMediator _mediator;
//    private readonly IUnitOfWork _accountingUnitOfWork;

//    public CreateVoucherNoRenumberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser, IMediator mediator, IUnitOfWork accountingUnitOfWork)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _mediator = mediator;
//        this._accountingUnitOfWork = accountingUnitOfWork;
//        _unitOfWork= unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(CreateVoucherNoRenumberCommand request, CancellationToken cancellationToken)
//    {
//        var year = await _unitOfWork.Years.GetByIdAsync(_applicationUser.YearId);

//        Specification<VouchersHead> specification = new Specification<VouchersHead>();
//        specification.ApplicationConditions.Add(x => x.CompanyId == _applicationUser.CompanyId);

//        if (request.FromDateTime != null)
//        {
//            specification.ApplicationConditions.Add(x =>
//                    x.VoucherDate >= request.FromDateTime);
//        }
//        else
//        {
//            specification.ApplicationConditions.Add(x =>
//                x.VoucherDate >= year.FirstDate);
//        }

//        if (request.ToDateTime != null)
//        {
//            specification.ApplicationConditions.Add(x =>
//                x.VoucherDate <= request.ToDateTime);
//        }
//        else
//        {
//            specification.ApplicationConditions.Add(x =>
//                x.VoucherDate <= year.LastDate);
//        }

//        if (request.FromNo != null)
//        {
//                    specification.ApplicationConditions.Add(x =>
//                x.VoucherNo >= request.FromNo);
//        }

//        if (request.ToNo != null)
//        {
//                    specification.ApplicationConditions.Add(x =>
//                x.VoucherNo <= request.ToNo);
//        }

//        if (request.VoucherStateId != null)
//        {
//            specification.ApplicationConditions.Add(x => x.VoucherStateId == request.VoucherStateId);
//        }

//        if (request.CodeVoucherGroupId != null)
//        {
//            specification.ApplicationConditions.Add(x => x.CodeVoucherGroupId == request.CodeVoucherGroupId);
//        }

//        specification.OrderBy = x => 
//           x.OrderBy(x => x.CompanyId)
//            .ThenBy(x => x.YearId)
//            .ThenBy(x => x.VoucherDate)
//            .ThenBy(x => x.CodeVoucherGroup.OrderIndex)
//            .ThenBy(x => x.VoucherNo)
//            .ThenBy(x => x.VoucherDailyId);

//        var query = _unitOfWork.VouchersHeads.GetListAsync(specification);
        
//        var minVoucherNo = voucherHeads.First().VoucherNo - 1;
//        if (request.FromNo == null && request.FromNo == null)
//        {
//            minVoucherNo = 0;
//        }
//        var settings = await new SystemSettings(_repository).Get(SubSystemType.AccountingSettings);
//        var voucherNumberType = new int();

//        foreach (var baseValue in settings)
//        {
//            if (baseValue.UniqueName == "VoucherNumberType")
//            {
//                voucherNumberType = int.Parse(baseValue.Value);
//                break;
//            }
//        }

//        var cy = voucherHeads.FirstOrDefault()?.YearId;
//        foreach (var vouchersHead in voucherHeads)
//        {
//            if (vouchersHead.YearId != cy)
//            {
//                cy = vouchersHead.YearId;
//                minVoucherNo = 0;
//            }
//            if (voucherNumberType == 2)
//            {
//                vouchersHead.VoucherNo = int.Parse($"{_applicationUser.BranchId}{++minVoucherNo}");
//            }
//            else
//            {
//                vouchersHead.VoucherNo = ++minVoucherNo;
//            }

//            _unitOfWork.VouchersHeads.Update(vouchersHead);
//        }

//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//            CallRenumberSPForSalesSystem();

//            return ServiceResult.Success();
//    }

//    public async void CallRenumberSPForSalesSystem()
//    {
//        var parameters = new List<SqlParameter>();
//        parameters.Add(new SqlParameter
//        {
//            ParameterName = "UserId",
//            Value = _applicationUser.Id
//        });

//        var response = await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_UpdateVouchersNoInSale]  {QueryUtility.SqlParametersToQuey(parameters)}",
//                parameters.ToArray(),
//                new CancellationToken());
//    }
//}