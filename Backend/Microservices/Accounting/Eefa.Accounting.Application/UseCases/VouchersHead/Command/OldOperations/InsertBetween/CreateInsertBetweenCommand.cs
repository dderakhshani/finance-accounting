using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Create;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.InsertBetween
{
    public class CreateInsertBetweenCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateInsertBetweenCommand>, ICommand
    {
        public int InsertAfterVoucherNo { get; set; }
        public CreateVouchersHeadCommand CreateVouchersHeadCommand { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateInsertBetweenCommand, Data.Entities.VouchersHead>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateInsertBetweenCommandHandler : IRequestHandler<CreateInsertBetweenCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMediator _mediator;
        public CreateInsertBetweenCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IMediator mediator)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _mediator = mediator;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateInsertBetweenCommand request, CancellationToken cancellationToken)
        {
            var year = await _repository.GetQuery<Data.Entities.Year>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetYearId(), cancellationToken: cancellationToken);

            var query = _repository.GetQuery<Data.Entities.VouchersHead>()
                .Where(x => x.CompanyId == _currentUserAccessor.GetCompanyId() &&
                            x.VoucherNo > request.InsertAfterVoucherNo);

            query = query
                .OrderBy(x => x.VoucherNo)
                .ThenBy(x => x.CompanyId)
                .ThenBy(x => x.YearId)
                .ThenBy(x => x.CodeVoucherGroup.OrderIndex)
                .ThenBy(x => x.VoucherNo)
                .ThenBy(x => x.VoucherDailyId);

            var voucherHeads = await query.ToListAsync(cancellationToken: cancellationToken);

            var settings = await new SystemSettings(_repository).Get(SubSystemType.AccountingSettings);
            var voucherNumberType = new int();

            foreach (var baseValue in settings)
            {
                if (baseValue.UniqueName == "VoucherNumberType")
                {
                    voucherNumberType = int.Parse(baseValue.Value);
                    break;
                }
            }


            var minVoucherNo = request.InsertAfterVoucherNo;

            if (voucherNumberType == 2)
            {
                request.CreateVouchersHeadCommand.VoucherNo = int.Parse($"{_currentUserAccessor.GetBranchId()}{++minVoucherNo}");
            }
            else
            {
                request.CreateVouchersHeadCommand.VoucherNo = ++minVoucherNo;
            }

            request.SaveChanges = false;

            var res = await _mediator.Send(request.CreateVouchersHeadCommand, cancellationToken);
            if (res.Succeed is false)
            {
                return ServiceResult.Failure();
            }


            foreach (var vouchersHead in voucherHeads)
            {
                if (voucherNumberType == 2)
                {
                    vouchersHead.VoucherNo = int.Parse($"{_currentUserAccessor.GetBranchId()}{++minVoucherNo}");
                }
                else
                {
                    vouchersHead.VoucherNo = ++minVoucherNo;
                }

                _repository.Update(vouchersHead);
            }


            if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }

            return ServiceResult.Failure();
        }
    }
}

