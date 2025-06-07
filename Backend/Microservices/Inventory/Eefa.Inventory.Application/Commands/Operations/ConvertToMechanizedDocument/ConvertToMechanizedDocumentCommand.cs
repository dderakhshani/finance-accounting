using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class ConvertToMechanizedDocumentCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Domain.Receipt>, ICommand
    {
       
    }

    public class ConvertToMechanizedDocumentCommandHandler : IRequestHandler<ConvertToMechanizedDocumentCommand, ServiceResult>
    {
        
        private readonly IMapper _mapper;
        private readonly IProcedureCallService _procedureCallService;

        public ConvertToMechanizedDocumentCommandHandler(
              IMapper mapper
            , IProcedureCallService procedureCallService
            )
        {
            _mapper = mapper;

            _procedureCallService = procedureCallService;

        }

        public async Task<ServiceResult> Handle(ConvertToMechanizedDocumentCommand request, CancellationToken cancellationToken)
        {
            
            await _procedureCallService.ValidationIssuedDocuments();
                
            
            return ServiceResult.Success();


        }

        

       
        
    }
}
