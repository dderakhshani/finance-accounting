using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Model;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Delete
{
    public class DeleteCodeVoucherExtendTypeCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCodeVoucherExtendTypeCommandHandler : IRequestHandler<DeleteCodeVoucherExtendTypeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCodeVoucherExtendTypeCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteCodeVoucherExtendTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.CodeVoucherExtendType>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);
            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CodeVoucherExtendTypeModel>(entity));
            }
            return ServiceResult.Failure();

        }
    }
}
