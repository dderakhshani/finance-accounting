using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Commodity.Application.Commands.Bom.Delete
{
    public class DeleteBomCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }

    }

    public class DeleteBomCommandHandler : IRequestHandler<DeleteBomCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.Bom> _repository;
        private readonly IRepository<Data.Entities.BomItem> _BomItemRepository;
        private readonly IMapper _mapper;

        public DeleteBomCommandHandler(
            IRepository<Data.Entities.Bom> repository,
            IRepository<Data.Entities.BomItem> bomItemRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _BomItemRepository = bomItemRepository;
        }

        public async Task<ServiceResult> Handle(DeleteBomCommand request, CancellationToken cancellationToken)
        {
            var BomItem = await _BomItemRepository.GetAll().Where(a => a.BomId == request.Id).ToListAsync();
            BomItem.ForEach(a =>
            {
                _BomItemRepository.Delete(a);
            });
            if(await _BomItemRepository.SaveChangesAsync() > 0)
            {
                var entity = await _repository.Find(request.Id);

                var deletedEntity = _repository.Delete(entity);
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return ServiceResult.Success();
                }
            }
            
            return ServiceResult.Failed();
        }
    }
}
