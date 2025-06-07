using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Data.SqlServer;
using Infrastructure.Mappings;
using Infrastructure.Models;
using Infrastructure.Interfaces;

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
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IMapper mapper)
        {
            _mapper = mapper;
        }


        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
            _baseValueService.Add(_mapper.Map<BaseValueServiceModel>(request));
            return ServiceResult.Set(true);
        }
    }
}
