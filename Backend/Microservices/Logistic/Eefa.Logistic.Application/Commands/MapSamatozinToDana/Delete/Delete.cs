using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;

using Eefa.Logistic.Domain;

using MediatR;

namespace Eefa.Logistic.Application
{ 
    public class DeleteMapSamatozinToDanaCommand : CommandBase, IRequest<ServiceResult<MapSamatozinToDanaModel>>, IMapFrom<MapSamatozinToDana>, ICommand
    {
        public int Id { get; set; }
    }

   
    public class DeleteMapSamatozinToDanaCommandHandler : IRequestHandler<DeleteMapSamatozinToDanaCommand, ServiceResult<MapSamatozinToDanaModel>>
    {
        private readonly IMapSamatozinToDanaRepository _mapSamatozinToDanaRepository;
        private readonly IMapper _mapper;

        public DeleteMapSamatozinToDanaCommandHandler(IMapSamatozinToDanaRepository mapSamatozinToDanaRepository, IMapper mapper)
        {
            _mapper = mapper;
            _mapSamatozinToDanaRepository = mapSamatozinToDanaRepository;
        }

        public async Task<ServiceResult<MapSamatozinToDanaModel>> Handle(DeleteMapSamatozinToDanaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _mapSamatozinToDanaRepository.Find(request.Id);



            _mapSamatozinToDanaRepository.Delete(entity);
            if(await _mapSamatozinToDanaRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                var model = _mapper.Map<MapSamatozinToDanaModel>(entity);
                return ServiceResult<MapSamatozinToDanaModel>.Success(model);
            }
            else
            {
                return ServiceResult<MapSamatozinToDanaModel>.Failed();
            }

           
        }
    }
}
