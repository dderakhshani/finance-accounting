using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Delete
{
    public class DeleteCommodityCategoryPropertyCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCommodityCategoryPropertyCommandHandler : IRequestHandler<DeleteCommodityCategoryPropertyCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityCategoryProperty> _repository;
        private readonly IMapper _mapper;

        public DeleteCommodityCategoryPropertyCommandHandler(IRepository<Data.Entities.CommodityCategoryProperty> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteCommodityCategoryPropertyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            entity.IsDeleted = true;
            var deletedEntity = _repository.Update(entity);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
