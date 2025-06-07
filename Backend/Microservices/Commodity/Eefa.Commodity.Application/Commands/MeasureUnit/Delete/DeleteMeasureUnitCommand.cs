using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.MeasureUnit.Delete
{
    public class DeleteMeasureUnitCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteMeasureUnitCommandHandler : IRequestHandler<DeleteMeasureUnitCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.MeasureUnit> _repository;
        private readonly IMapper _mapper;

        public DeleteMeasureUnitCommandHandler(IRepository<Data.Entities.MeasureUnit> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteMeasureUnitCommand request, CancellationToken cancellationToken)
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
