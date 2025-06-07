using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete
{
    public class DeleteVouchersDetailCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// بدهکار
        /// </summary>
        public long Debit { get; set; } = default!;

        /// <summary>
        /// اعتبار
        /// </summary>
        public long Credit { get; set; } = default!;
    }

    public class DeleteVouchersDetailCommandHandler : IRequestHandler<DeleteVouchersDetailCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteVouchersDetailCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteVouchersDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.VouchersDetail>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            var deleted = _repository.Delete(entity);
            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(_mapper.Map<VouchersDetailModel>(entity));
                }
            }
            else
            {
                return ServiceResult.Success(deleted.Entity);
            }

            return ServiceResult.Failure();
        }
    }
}
