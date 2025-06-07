using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.Delete
{
    public class DeleteVouchersHeadCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteVouchersHeadCommandHandler : IRequestHandler<DeleteVouchersHeadCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteVouchersHeadCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteVouchersHeadCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.VouchersHead>(c =>
             c.ObjectId(request.Id))
                .Include(x => x.VouchersDetails)
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);

            foreach (var detail in entity.VouchersDetails) _repository.Delete(detail);

            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
