using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.BulkVoucherStatusUpdate
{
    public class BulkVoucherStatusUpdateCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public List<Condition> Conditions { get; set; }

        public List<int> VoucherIds { get; set; }
        public int Status { get; set; }
    }

    public class  BulkVoucherStatusUpdateCommandHandler : IRequestHandler<BulkVoucherStatusUpdateCommand, ServiceResult>
    {
        private readonly IRepository _repository;

        public  BulkVoucherStatusUpdateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(BulkVoucherStatusUpdateCommand request, CancellationToken cancellationToken)
        {

            var query = _repository
              .GetAll<Data.Entities.VouchersHead>();

            if (request.VoucherIds?.Count > 0)
            {
                query = query.Where(x => request.VoucherIds.Any(y => y == x.Id));

            }
            else
            {
                query = query.WhereQueryMaker(request.Conditions);
            }
              

            await query.UpdateFromQueryAsync(x => new Data.Entities.VouchersHead() { VoucherStateId = request.Status },
                cancellationToken);

            return ServiceResult.Success(null);
        }
    }
}