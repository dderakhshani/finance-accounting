using System;
using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Attributes;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Create
{
    public class CreateVouchersDetailCommand : CommandBase, ICloneable, IRequest<ServiceResult>, IMapFrom<
    CreateVouchersDetailCommand>, ICommand
    {
        [SwaggerExclude]
        public Data.Entities.VouchersHead EntityEntryVouchersHead { get; set; }
        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }
        public int? VoucherId { get; set; }

        /// <summary>
        /// کد حساب سرپرست
        /// </summary>
        public int AccountHeadId { get; set; } = default!;
        public int? AccountReferencesGroupId { get; set; } = default!;

        /// <summary>
        /// شرح آرتیکل  سند
        /// </summary>
        public string VoucherRowDescription { get; set; } = default!;

        /// <summary>
        /// بدهکار
        /// </summary>
        public double Debit { get; set; } = default!;

        /// <summary>
        /// اعتبار
        /// </summary>
        public double Credit { get; set; } = default!;

        /// <summary>
        /// ترتیب سطر
        /// </summary>
        public int? RowIndex { get; set; }

        /// <summary>
        /// شماره سند مرتبط 
        /// </summary>
        public int? DocumentId { get; set; }


        /// <summary>
        /// تاریخ مرجع
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

        /// <summary>
        /// مقدار مرجع
        /// </summary>
        public double? ReferenceQty { get; set; }

        /// <summary>
        /// کد مرجع1
        /// </summary>
        public int? ReferenceId1 { get; set; }

        /// <summary>
        /// کد مرجع2
        /// </summary>
        public int? ReferenceId2 { get; set; }

        /// <summary>
        /// کد مرجع3
        /// </summary>
        public int? ReferenceId3 { get; set; }

        /// <summary>
        /// سطح 1
        /// </summary>
        public int? Level1 { get; set; }

        /// <summary>
        /// سطح 2
        /// </summary>
        public int? Level2 { get; set; }

        /// <summary>
        /// سطح 3
        /// </summary>
        public int? Level3 { get; set; }
        //public int? CurrencyBaseTypeId { get; set; }
        //public int? CurrencyValue { get; set; }
        //public int? ExchengeValue { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public double? CurrencyFee { get; set; }
        public double? CurrencyAmount { get; set; }
        public int? TraceNumber { get; set; }
        public double? Quantity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateVouchersDetailCommand, Data.Entities.VouchersDetail>()
                .IgnoreAllNonExisting();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class CreateVouchersDetailCommandHandler : IRequestHandler<CreateVouchersDetailCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CreateVouchersDetailCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateVouchersDetailCommand request, CancellationToken cancellationToken)
        {
            if (request.ReferenceId1 is not (null or 0) ||
                request.ReferenceId2 is not (null or 0) ||
                request.ReferenceId3 is not (null or 0))
            {
                var referenceId = 0;
                if (request.ReferenceId1 is not (null or 0))
                {
                    referenceId = request.ReferenceId1 ?? 0;
                }
                if (request.ReferenceId2 is not (null or 0))
                {
                    referenceId = request.ReferenceId2 ?? 0;
                }
                if (request.ReferenceId3 is not (null or 0))
                {
                    referenceId = request.ReferenceId3 ?? 0;
                }

                var accountRefreneceGroups = await _repository
                    .GetQuery<Eefa.Accounting.Data.Entities.AccountReferencesRelReferencesGroup>()
                    .Where(x => x.ReferenceId == referenceId)
                    .Select(x => x.ReferenceGroupId)
                    .ToListAsync(cancellationToken);

                var accountHeadRelReferenceGroup = await _repository
                    .GetQuery<Data.Entities.AccountHeadRelReferenceGroup>().FirstOrDefaultAsync(x =>
                            x.AccountHeadId == request.AccountHeadId &&
                            accountRefreneceGroups.Contains(x.ReferenceGroupId),
                        cancellationToken: cancellationToken);

                if (accountHeadRelReferenceGroup.ReferenceNo == 0)
                {
                    throw new Exception("");
                }
                else
                {
                    switch (accountHeadRelReferenceGroup.ReferenceNo)
                    {
                        case 1:
                            if (request.ReferenceId1 is (null or 0))
                            {
                                throw new Exception("ReferenceId1");
                            }

                            break;
                        case 2:
                            if (request.ReferenceId2 is (null or 0) &&
                                (request.ReferenceId1 is (null or 0)))
                            {
                                throw new Exception("ReferenceId2");
                            }

                            break;
                        case 3:
                            if (request.ReferenceId3 is (null or 0) &&
                                (request.ReferenceId1 is (null or 0) &&
                                 (request.ReferenceId2 is (null or 0))))
                            {
                                throw new Exception("ReferenceId3");
                            }

                            break;
                    }
                }
            }

            var details = _mapper.Map<Data.Entities.VouchersDetail>(request);
            if (request.VoucherId != 0 && request.EntityEntryVouchersHead != null)
            {
                details.Voucher = request.EntityEntryVouchersHead;
            }
            var entity = _repository.Insert(details);

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(entity.Entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity.Entity);
            }

            return ServiceResult.Failure();
        }
    }
}
