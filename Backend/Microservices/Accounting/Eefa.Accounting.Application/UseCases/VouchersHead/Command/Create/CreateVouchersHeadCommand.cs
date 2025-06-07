using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Create;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using Eefa.Accounting.Application.UseCases.VouchersHead.Utility;
using Eefa.Accounting.Data.Entities;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Eefa.Accounting.Application.UseCases.VouchersHead.Services;
using Eefa.Accounting.Application.Services.EventManager;
using Eefa.Accounting.Data.Events.VoucherHead;
using Eefa.Accounting.Application.Services.Logs;
using Eefa.Accounting.Application.Common.Extensions;


namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.Create
{
    public class CreateVouchersHeadCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateVouchersHeadCommand>, ICommand
    {
        public int? VoucherNo { get; set; } = default!;
        public int VoucherDailyId { get; set; }
        public DateTime VoucherDate { get; set; } = default!;
        public string VoucherDescription { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public int VoucherStateId { get; set; } = default!;
        public List<CreateVouchersDetailCommand> VouchersDetailsList { get; set; } = new List<CreateVouchersDetailCommand> { };


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateVouchersHeadCommand, Data.Entities.VouchersHead>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateVouchersHeadCommandHandler : IRequestHandler<CreateVouchersHeadCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMediator _mediator;
        private readonly IApplicationEventsManager _applicationEventsManager;
        private readonly IApplicationRequestLogManager _logManager;

        public IVoucherHeadCacheServices _voucherHeadCacheServices { get; }
        private List<Data.Entities.AccountHead> AccountHeads { get; set; }

        public CreateVouchersHeadCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IMediator mediator, IVoucherHeadCacheServices voucherHeadCacheServices, IApplicationEventsManager applicationEventsManager, IApplicationRequestLogManager logManager)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _mediator = mediator;
            _voucherHeadCacheServices = voucherHeadCacheServices;
            this._applicationEventsManager = applicationEventsManager;
            this._logManager = logManager;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateVouchersHeadCommand request, CancellationToken cancellationToken)
        {
            await _logManager.CommitLog(request);

            AccountHeads = await _repository.GetAll<Data.Entities.AccountHead>().ToListAsync();

            var voucher = new Data.Entities.VouchersHead();
            var lastVoucherDailyId = await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.VoucherDate.Date == request.VoucherDate.Date).MaxAsync(x => (int?)x.VoucherDailyId);

            // modify voucher date to 12PM

            var voucherDate = DateTime.SpecifyKind(request.VoucherDate, DateTimeKind.Utc);
            var persianDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(voucherDate, "Iran Standard Time");
            request.VoucherDate = new DateTime(persianDate.Year, persianDate.Month, persianDate.Day, 12, 0, 0, DateTimeKind.Utc);

            var createVoucherHeadEvent = new CreateVoucherHeadEvent
            {
                VoucherDate = request.VoucherDate,
                YearId = _currentUserAccessor.GetYearId(),
                CompanyId = _currentUserAccessor.GetCompanyId(),
                VoucherNo = await _voucherHeadCacheServices.GetNewVoucherNumber(),
                VoucherDailyId = (lastVoucherDailyId != null ? (int)lastVoucherDailyId + 1 : 1),
                TotalCredit = request.VouchersDetailsList.Sum(x => x.Credit),
                TotalDebit = request.VouchersDetailsList.Sum(x => x.Debit),
                VoucherDescription = request.VoucherDescription,
                CodeVoucherGroupId = request.CodeVoucherGroupId,
                VoucherStateId = request.VoucherStateId,
            };

            voucher.Apply(createVoucherHeadEvent);
            var originEvent = _applicationEventsManager.AddEvent(voucher, createVoucherHeadEvent, "");
            _repository.Insert(voucher);



            foreach (var voucherDetail in request.VouchersDetailsList)
            {
                voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Trim();
                voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Replace("ي", "ی");
                voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Replace("ى", "ی");
                voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Replace("ك", "ک");
                voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.ToEnglishNumbers();
                var createVoucherDetailEvent = new CreateVoucherDetailEvent
                {
                    VoucherDate = voucher.VoucherDate,
                    AccountHeadId = voucherDetail.AccountHeadId,
                    AccountReferencesGroupId = voucherDetail.AccountReferencesGroupId,
                    ReferenceId1 = voucherDetail.ReferenceId1,
                    VoucherRowDescription = voucherDetail.VoucherRowDescription,
                    Credit = voucherDetail.Credit,
                    Debit = voucherDetail.Debit,
                    RowIndex = voucherDetail.RowIndex,
                    Level1 = voucherDetail.Level1,
                    Level2 = voucherDetail.Level2,
                    Level3 = voucherDetail.Level3,
                    CurrencyAmount = voucherDetail.CurrencyAmount,
                    CurrencyFee = voucherDetail.CurrencyFee,
                    CurrencyTypeBaseId = voucherDetail.CurrencyTypeBaseId
                };

                var newVoucherDetail = voucher.Apply(createVoucherDetailEvent);
                AssignVoucherDetailLevels(newVoucherDetail);

                _applicationEventsManager.AddEvent(newVoucherDetail, createVoucherDetailEvent, "", originEvent.Id);
                _repository.Insert(newVoucherDetail);
            }

            SetVoucherDetailsRowIndexes(voucher);
            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);
            return ServiceResult.Success(_mapper.Map<VouchersHeadWithDetailModel>(voucher));
        }

        public void AssignVoucherDetailLevels(Data.Entities.VouchersDetail voucherDetail)
        {
            Data.Entities.AccountHead level3 = null;
            Data.Entities.AccountHead level2 = null;
            Data.Entities.AccountHead level1 = null;

            level3 = AccountHeads.FirstOrDefault(x => x.Id == voucherDetail.AccountHeadId);
            if (level3 != null && level3.ParentId != null) level2 = AccountHeads.FirstOrDefault(x => x.Id == level3.ParentId);
            if (level2 != null && level2.ParentId != null) level1 = AccountHeads.FirstOrDefault(x => x.Id == level2.ParentId);

            voucherDetail.Level1 = level1.Id;
            voucherDetail.Level2 = level2.Id;
            voucherDetail.Level3 = level3.Id;
        }

        private void SetVoucherDetailsRowIndexes(Data.Entities.VouchersHead voucherHead)
        {
            var voucherDetails = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).OrderBy(x => x.RowIndex).ThenBy(x => x.CreatedAt).ToArray();
            for (int i = 0; i < voucherDetails.Length; i++)
            {
                var voucherDetail = voucherDetails.ElementAt(i);
                voucherDetail.RowIndex = i + 1;
                if (voucherDetail.Id != default) _repository.Update(voucherDetail);

            }
        }

    }
}
