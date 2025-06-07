using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{ 
    public class DeleteDocumentHeadExtraCostCommand : CommandBase, IRequest<ServiceResult<DocumentHeadExtraCostModel>>, IMapFrom<Domain.DocumentHeadExtraCost>, ICommand
    {
        public int Id { get; set; }
    }

   
    public class DeleteDocumentHeadExtraCostCommandHandler : IRequestHandler<DeleteDocumentHeadExtraCostCommand, ServiceResult<DocumentHeadExtraCostModel>>
    {
        private readonly IDocumentHeadExtraCostRepository _DocumentHeadExtraCostRepository;
        private readonly IMapper _mapper;

        public DeleteDocumentHeadExtraCostCommandHandler(IDocumentHeadExtraCostRepository DocumentHeadExtraCostRepository, IMapper mapper)
        {
            _mapper = mapper;
            _DocumentHeadExtraCostRepository = DocumentHeadExtraCostRepository;
        }

        public async Task<ServiceResult<DocumentHeadExtraCostModel>> Handle(DeleteDocumentHeadExtraCostCommand request, CancellationToken cancellationToken)
        {
            var entity = await _DocumentHeadExtraCostRepository.Find(request.Id);

            

            _DocumentHeadExtraCostRepository.Delete(entity);
            if(await _DocumentHeadExtraCostRepository.SaveChangesAsync(cancellationToken) > 0)
            {
               
                return ServiceResult<DocumentHeadExtraCostModel>.Success(_mapper.Map<DocumentHeadExtraCostModel>(entity));
            }
            else
            {
                return ServiceResult<DocumentHeadExtraCostModel>.Failed();
            }

           
        }
    }
}
