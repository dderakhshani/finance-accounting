using AutoMapper;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Delete
{
    public class DeleteAutoVoucherFormulaCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteAutoVoucherFormulaCommandHandler : IRequestHandler<DeleteAutoVoucherFormulaCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteAutoVoucherFormulaCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteAutoVoucherFormulaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.AutoVoucherFormula>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);
            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<AutoVoucherFormulaModel>(entity));
            }
            return ServiceResult.Failure();

        }
    }
}
