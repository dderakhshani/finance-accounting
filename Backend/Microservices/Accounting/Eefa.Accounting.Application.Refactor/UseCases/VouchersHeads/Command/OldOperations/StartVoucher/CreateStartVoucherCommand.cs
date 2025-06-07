using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
// TODO check the line 69
public class CreateStartVoucherCommand : IRequest<ServiceResult<YearModel>>, IMapFrom<CreateStartVoucherCommand>
{
    public int LastYearId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateStartVoucherCommand, Year>()
            .IgnoreAllNonExisting();
    }
}

public class CreateStartVoucherCommandHandler : IRequestHandler<CreateStartVoucherCommand, ServiceResult<YearModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;

    public CreateStartVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
    {
        _mapper = mapper;
        _applicationUser = applicationUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<YearModel>> Handle(CreateStartVoucherCommand request, CancellationToken cancellationToken)
    {
        var endCodeVocuerGroup = await _unitOfWork.CodeVoucherGroups
            .GetAsync(x => x.UniqueName == "end");

        var startCodeVocuerGroup = await _unitOfWork.CodeVoucherGroups
            .GetAsync(x => x.UniqueName == "start");

        Specification<VouchersHead> specification = new Specification<VouchersHead>();
        specification.ApplicationConditions.Add(x => x.YearId == request.LastYearId &&
            x.CodeVoucherGroupId == endCodeVocuerGroup.Id);
        specification.Includes = y => y.Include(z => z.VouchersDetails);

        var lastYearEndVoucherHead = await _unitOfWork.VouchersHeads.GetAsync(specification, false);

        var inputeYear = _mapper.Map<Year>(request);
        inputeYear.CompanyId = _applicationUser.CompanyId;
        _unitOfWork.Years.Add(inputeYear);

        var entity = new VouchersHead()
        {
            VoucherNo = 1,
            VoucherDailyId = 1,
            VoucherDate = DateTime.Now,
            CodeVoucherGroupId = startCodeVocuerGroup.Id,
            CompanyId = _applicationUser.CompanyId,
            VoucherStateId = 3, // دائم
            Year = inputeYear,
            VoucherDescription = "افتتاحیه",
            TotalCredit = lastYearEndVoucherHead.VouchersDetails.Aggregate(new double(), (c, n) => c + n.Debit),
            TotalDebit = lastYearEndVoucherHead.VouchersDetails.Aggregate(new double(), (c, n) => c + n.Credit)
        };
        _unitOfWork.VouchersHeads.Add(entity);

        foreach (var lastYearEndVouchersDetail in lastYearEndVoucherHead.VouchersDetails)
        {
            var startVouchersDetail = (VouchersDetail)(lastYearEndVouchersDetail)/*.Clone()*/;
            startVouchersDetail.Id = default;
            startVouchersDetail.Debit = lastYearEndVouchersDetail.Credit;
            startVouchersDetail.Credit = lastYearEndVouchersDetail.Debit;
            startVouchersDetail.Voucher = entity;
            _unitOfWork.VouchersDetails.Add(startVouchersDetail);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ServiceResult.Success(_mapper.Map<YearModel>(inputeYear));
    }
}