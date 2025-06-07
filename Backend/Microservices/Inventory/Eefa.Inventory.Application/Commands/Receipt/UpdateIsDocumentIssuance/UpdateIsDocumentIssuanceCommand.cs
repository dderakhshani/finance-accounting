using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Application.Commands.Receipt.Update;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Inventory.Application
{

    public class UpdateIsDocumentIssuanceCommand : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<Domain.Receipt>, ICommand
    {
        public List<int> Ids { get; set; }
      
      
        public class UpdateIsDocumentIssuanceCommandHandler : IRequestHandler<UpdateIsDocumentIssuanceCommand, ServiceResult<ReceiptQueryModel>>
        {

            private readonly IReceiptRepository _receiptRepository;

            public UpdateIsDocumentIssuanceCommandHandler(IReceiptRepository receiptRepository) => _receiptRepository = receiptRepository;
            
            public async Task<ServiceResult<ReceiptQueryModel>> Handle(UpdateIsDocumentIssuanceCommand request, CancellationToken cancellationToken)
            {

                foreach(var Id in request.Ids)
                {
                    var entity = await _receiptRepository.Find(Id);

                    entity.IsDocumentIssuance = false;

                    _receiptRepository.Update(entity);
                }
                
                await _receiptRepository.SaveChangesAsync();

               
                return ServiceResult<ReceiptQueryModel>.Success(null);

            }



        }
    }
}
