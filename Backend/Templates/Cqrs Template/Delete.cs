using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Data.SqlServer;
using Infrastructure.Models;
using Eefa.Admin.Application.Services.$fileinputname$.Model;
using Infrastructure.Interfaces;

namespace $rootnamespace$.$fileinputname$.Command.Delete
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id {get;set;}
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
            return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity));       
        }
    }
}
