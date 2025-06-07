using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.AutoVouchers.PayableDocs.Commands.Add
{
    public class AddPayableDocsAutovoucher : CommandBase, IRequest<ServiceResult<VouchersHead>>, ICommand
    {
        public int DocumentId { get; set; }
        public int OperationId { get; set; }
    }

    public class AddPayableDocsAutovoucherHandler : IRequestHandler<AddPayableDocsAutovoucher, ServiceResult<VouchersHead>>
    {
        private readonly IMediator _mediator;

        public AddPayableDocsAutovoucherHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ServiceResult<VouchersHead>> Handle(AddPayableDocsAutovoucher request, CancellationToken cancellationToken)
        {
            switch (request.OperationId)
            {
                case 28778:
                    {

                        var vchcmd = new AddPayableDocIssueAutovoucher()
                        {
                            DocumentId = request.DocumentId
                        };
                        return await _mediator.Send(vchcmd, cancellationToken);
                        break;
                    }
            }
            return null;
        }
    }
}
