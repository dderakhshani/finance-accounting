using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Commodity.Application.Commands.BomItem.Delete
{
    public class DeleteBomItemCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }

    }

    public class DeleteBomCommandHandler : IRequestHandler<DeleteBomItemCommand, ServiceResult>
    {
        private readonly IRepository<Eefa.Commodity.Data.Entities.BomItem> _repository;
        private readonly IMapper _mapper;

        public DeleteBomCommandHandler(IRepository<Eefa.Commodity.Data.Entities.BomItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteBomItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            var deletedEntity = _repository.Delete(entity);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
