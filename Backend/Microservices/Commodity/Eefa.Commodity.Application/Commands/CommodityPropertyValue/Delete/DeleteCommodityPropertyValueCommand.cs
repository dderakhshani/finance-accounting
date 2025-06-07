using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.CommodityPropertyValue.Delete
{
    public class DeleteCommodityPropertyValueCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCommodityPropertyValueCommandHandler : IRequestHandler<DeleteCommodityPropertyValueCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityPropertyValue> _repository;
        private readonly IMapper _mapper;

        public DeleteCommodityPropertyValueCommandHandler(IRepository<Data.Entities.CommodityPropertyValue> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteCommodityPropertyValueCommand request, CancellationToken cancellationToken)
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
