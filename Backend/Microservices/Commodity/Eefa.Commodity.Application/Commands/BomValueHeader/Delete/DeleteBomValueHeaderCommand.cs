using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Commodity.Application.Commands.BomValueHeader.Delete
{
    public class DeleteBomValueHeaderCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }

    }

    public class DeleteBomValueHeaderCommandHandler : IRequestHandler<DeleteBomValueHeaderCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomValueHeader> _repository;
        private readonly IRepository<Data.Entities.BomValue> _bomValueHeaderRepository;
        private readonly IMapper _mapper;

        public DeleteBomValueHeaderCommandHandler(
            IRepository<Data.Entities.BomValueHeader> repository,
            IRepository<Data.Entities.BomValue> bomValueHeaderRepository,
            IMapper mapper
            
            )
        {
            _mapper = mapper;
            _repository = repository;
            _bomValueHeaderRepository = bomValueHeaderRepository;
        }

        public async Task<ServiceResult> Handle(DeleteBomValueHeaderCommand request, CancellationToken cancellationToken)
        {
            var BomItem = await _bomValueHeaderRepository.GetAll().Where(a => a.BomValueHeaderId == request.Id).ToListAsync();
            BomItem.ForEach(a =>
            {
                _bomValueHeaderRepository.Delete(a);
            });
            if( await _bomValueHeaderRepository.SaveChangesAsync() > 0)
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
