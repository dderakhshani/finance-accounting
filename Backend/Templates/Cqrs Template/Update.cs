using AutoMapper;
using Eefa.Admin.Application.Services.$fileinputname$.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Data.SqlServer;
using Infrastructure.Mappings;
using Infrastructure.Models;
using Infrastructure.Utility;
using Infrastructure.Interfaces;

namespace $rootnamespace$.$fileinputname$.Command.Update
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult> , IMapFrom<Data.Entities.$fileinputname$>, ICommand
    {
        public int Id { get; set; }

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
            return ServiceResult.Set(true);
        }
    }
}
