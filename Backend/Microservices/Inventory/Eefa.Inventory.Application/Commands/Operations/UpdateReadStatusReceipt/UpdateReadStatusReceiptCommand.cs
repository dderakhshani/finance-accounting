
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Eefa.Inventory.Domain.Common.ConstantValues;

namespace Eefa.Inventory.Application.Commands.Operations.UpdateReadStatusRecipt
{
    public class UpdateReadStatusReceiptCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public List<int> Ids { get; set; }
        public bool IsRead { get; set; }
    }
    public class UpdateReadStatusReceiptCommandHandler : IRequestHandler<UpdateReadStatusReceiptCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        public UpdateReadStatusReceiptCommandHandler(IMapper mapper,
            IInvertoryUnitOfWork contex,
            IReceiptRepository receiptRepository,
            IReceiptCommandsService receiptCommandsService)
        {
            _mapper = mapper;
            _context = contex;
            _receiptRepository = receiptRepository;
            _receiptCommandsService = receiptCommandsService;
        }
        public async Task<ServiceResult> Handle(UpdateReadStatusReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipts = _receiptRepository.GetAll().Where(x => request.Ids.Contains(x.Id));
            if (request.IsRead)
            {
                foreach (var receipt in receipts)
                {
                    if (!receipt.Tags.Contains(ConstBaseValue.viewAccounting))
                        receipt.Tags = _receiptCommandsService.AppendNewTagToReceipt(ConstBaseValue.viewAccounting, receipt);
                    _receiptRepository.Update(receipt);
                }
            }
            else
            {
                foreach (var receipt in receipts)
                {
                    if (receipt.Tags.Contains(ConstBaseValue.viewAccounting))
                        receipt.Tags = _receiptCommandsService.RemoveTagFromReceipt(ConstBaseValue.viewAccounting, receipt);
                    _receiptRepository.Update(receipt);
                }

            }

            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();

        }
    }
}
