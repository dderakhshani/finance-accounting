using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Newtonsoft.Json;


namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Create
{
    public class CreateAutoVoucherFormulaCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateAutoVoucherFormulaCommand>, ICommand
    {

        public int VoucherTypeId { get; set; } = default!;
        public int SourceVoucherTypeId { get; set; } = default!;

        public int OrderIndex { get; set; } = default!;

        public byte DebitCreditStatus { get; set; } = default!;

        public int AccountHeadId { get; set; } = default!;


        public string? RowDescription { get; set; }

        public string? ConditionsJson { get; set; }
        public string? FormulaJson { get; set; }
        public string? GroupBy { get; set; }       

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAutoVoucherFormulaCommand, Data.Entities.AutoVoucherFormula>()
                .ForMember(x => x.Formula, opt => opt.MapFrom(x => x.FormulaJson))
                .ForMember(x => x.Conditions, opt => opt.MapFrom(x => x.ConditionsJson));

            
        }
    }

    public class CreateAutoVoucherFormulaCommandHandler : IRequestHandler<CreateAutoVoucherFormulaCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateAutoVoucherFormulaCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateAutoVoucherFormulaCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Entities.AutoVoucherFormula>(request));
            try
            {
                if (request.SaveChanges)
                {
                    if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
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
            catch (System.Exception ex)
            {

                throw;
            }
           
        }
    }
}
