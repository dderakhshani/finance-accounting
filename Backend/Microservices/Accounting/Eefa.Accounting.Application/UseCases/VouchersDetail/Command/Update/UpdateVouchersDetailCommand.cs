using System;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Update
{
    public class UpdateVouchersDetailCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.VouchersDetail>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// کد سند
        /// </summary>
        public int VoucherId { get; set; } = default!;

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime VoucherDate { get; set; }

        /// <summary>
        /// کد حساب سرپرست
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

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
        public int RowIndex { get; set; }

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

        public int? AccountReferencesGroupId { get; set; } = default!;


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

        public int? CurrencyTypeBaseId { get; set; }
        public double? CurrencyFee { get; set; }
        public double? CurrencyAmount { get; set; }
        public int TraceNumber { get; set; }
        public double? Quantity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateVouchersDetailCommand, Data.Entities.VouchersDetail>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateVouchersDetailCommandHandler : IRequestHandler<UpdateVouchersDetailCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateVouchersDetailCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateVouchersDetailCommand request, CancellationToken cancellationToken)
        {
          
            
            var entity = await _repository
                .Find<Data.Entities.VouchersDetail>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.VoucherId = request.VoucherId;
            entity.VoucherDate = request.VoucherDate;
            entity.AccountHeadId = request.AccountHeadId;
            entity.AccountReferencesGroupId = request.AccountReferencesGroupId;
            entity.VoucherRowDescription = request.VoucherRowDescription;
            entity.Debit = request.Debit;
            entity.Credit = request.Credit;
            entity.RowIndex = request.RowIndex;
            entity.DocumentId = request.DocumentId;
            entity.ReferenceDate = request.ReferenceDate;
            entity.Weight = request.ReferenceQty;
            entity.ReferenceId1 = request.ReferenceId1;
            entity.ReferenceId2 = request.ReferenceId2;
            entity.ReferenceId3 = request.ReferenceId3;
            entity.Level1 = request.Level1;
            entity.Level2 = request.Level2;
            entity.Level3 = request.Level3;
            entity.CurrencyFee = request.CurrencyFee;
            entity.CurrencyTypeBaseId = request.CurrencyTypeBaseId;
            entity.CurrencyAmount = request.CurrencyAmount;
            entity.TraceNumber = request.TraceNumber;
            entity.Quantity = request.Quantity;

            var updateEntity = _repository.Update(entity);

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(_mapper.Map<VouchersDetailModel>(entity));
                }
            }
            else
            {
                return ServiceResult.Success(updateEntity.Entity);
            }
            
            return ServiceResult.Failure();
        }
    }
}
