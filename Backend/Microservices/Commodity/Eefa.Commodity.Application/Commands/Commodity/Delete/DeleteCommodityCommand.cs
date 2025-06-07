using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eefa.Commodity.Data.Context;
using System.Linq.Dynamic.Core;
using System.Linq;
using Eefa.Common.Exceptions;

namespace Eefa.Commodity.Application.Commands.Commodity.Delete
{
    public class DeleteCommodityCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCommodityCommandHandler : IRequestHandler<DeleteCommodityCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.Commodity> _repository;
        private readonly ICommodityUnitOfWork _context;
        private readonly IMapper _mapper;

        public DeleteCommodityCommandHandler(IRepository<Data.Entities.Commodity> repository, IMapper mapper, ICommodityUnitOfWork context)
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;
        }

        public async Task<ServiceResult> Handle(DeleteCommodityCommand request, CancellationToken cancellationToken)
        {

            var entity = await _repository.Find(request.Id);
            if (!_context.DocumentItems.Where(a => a.CommodityId == request.Id).Any())
            {
                entity.Code = entity.Code + "-" + (request.Id).ToString();
                entity.IsDeleted = true;
                var deletedEntity = _repository.Update(entity);
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return ServiceResult.Success();
                }
            }
            else
            {
                throw new ValidationError("این کالا مورد استفاده قرار گرفته شده است");
            }

            return ServiceResult.Failed();
        }
    }
}
