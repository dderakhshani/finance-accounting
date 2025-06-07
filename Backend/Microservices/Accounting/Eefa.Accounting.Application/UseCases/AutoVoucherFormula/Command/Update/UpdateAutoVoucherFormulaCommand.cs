using AutoMapper;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Update
{
    public class UpdateAutoVoucherFormulaCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.AutoVoucherFormula>, ICommand
    {
        public int Id { get; set; }

        public int VoucherTypeId { get; set; } = default!;
        public int SourceVoucherTypeId { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// وضعیت مانده حساب
        /// </summary>
        public byte DebitCreditStatus { get; set; } = default!;

        /// <summary>
        /// کد سطح
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
        /// توضیحات سطر
        /// </summary>
        public string? RowDescription { get; set; }

        /// <summary>
        /// ستونهای مقصد
        /// </summary>
    
        public string? FormulaJson { get; set; }

        /// <summary>
        /// شرط
        /// </summary>
        public string? ConditionsJson { get; set; }
        public string? GroupBy { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAutoVoucherFormulaCommand, Data.Entities.AutoVoucherFormula>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAutoVoucherFormulaCommandHandler : IRequestHandler<UpdateAutoVoucherFormulaCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAutoVoucherFormulaCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateAutoVoucherFormulaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.AutoVoucherFormula>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.VoucherTypeId = request.VoucherTypeId;
            entity.OrderIndex = request.OrderIndex;
            entity.DebitCreditStatus = request.DebitCreditStatus;
            entity.AccountHeadId = request.AccountHeadId;
            entity.RowDescription = request.RowDescription;
            entity.Formula = request.FormulaJson;
            entity.Conditions = request.ConditionsJson;

            _repository.Update(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<AutoVoucherFormulaModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
