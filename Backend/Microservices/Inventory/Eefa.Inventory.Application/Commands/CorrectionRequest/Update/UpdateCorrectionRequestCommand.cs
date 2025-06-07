using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Inventory.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UpdateCorrectionRequestCommand : CommandBase, IRequest<ServiceResult<CorrectionRequest>>, IMapFrom<UpdateCorrectionRequestCommand>, ICommand
    {
       public int Id { get; set; }
        public int DocumentId { get; set; }
        public int Status { get; set; }
        public int VerifierUserId { get; set; }
        public string Description { get; set; }

    }

    public class UpdateCorrectionRequestCommandHandler : IRequestHandler<UpdateCorrectionRequestCommand, ServiceResult<CorrectionRequest>>
    {
        private readonly IRepository<CorrectionRequest> _repository;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IMapper _mapper;

        public UpdateCorrectionRequestCommandHandler(
            IRepository<CorrectionRequest> repository,
            IMapper mapper,
             IInvertoryUnitOfWork context
            )
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;
        }


        public async Task<ServiceResult<CorrectionRequest>> Handle(UpdateCorrectionRequestCommand request, CancellationToken cancellationToken)
        {
            
            var model = await _context.CorrectionRequest.Where(a => a.Id ==request.Id).FirstOrDefaultAsync();
            model.Status = request.Status;
            model.VerifierDescription = request.Description;
            model.VerifierUserId = request.VerifierUserId;
            _repository.Update(model);
            await _repository.SaveChangesAsync();

            return ServiceResult<CorrectionRequest>.Success(_mapper.Map<CorrectionRequest>(model));
        }
    }
}
