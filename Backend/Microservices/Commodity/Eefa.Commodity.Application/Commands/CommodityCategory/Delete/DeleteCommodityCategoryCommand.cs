using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.CommodityCategory.Delete
{
    public class DeleteCommodityCategoryCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }

      
    }

    public class DeleteCommodityCategoryCommandHandler : IRequestHandler<DeleteCommodityCategoryCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityCategory> _repository;
        private readonly IMapper _mapper;

        public DeleteCommodityCategoryCommandHandler(IRepository<Data.Entities.CommodityCategory> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteCommodityCategoryCommand request, CancellationToken cancellationToken)
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
