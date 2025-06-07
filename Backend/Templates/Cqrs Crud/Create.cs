using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data.Models;

namespace $rootnamespace$.$fileinputname$.Command.Create
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult> , IMapFrom<$safeitemname$> , ICommand
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<$safeitemname$, Data.Entities.$fileinputname$>()
                .IgnoreAllNonExisting();
        }
    }

    public class $safeitemname$Handler : IRequestHandler<$safeitemname$, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Insert(_mapper.Map<Data.Entities.$fileinputname$>(request));
            return ServiceResult.Set(true);
        }
    }
}
