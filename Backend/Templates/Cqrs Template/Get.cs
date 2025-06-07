using AutoMapper;
using Eefa.Admin.Application.Services.$fileinputname$.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Infrastructure.Data.SqlServer;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Interfaces;

namespace $rootnamespace$.$fileinputname$.Query.Get
{
    public class $safeitemname$ : IRequest<ServiceResult>, IQuery
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
            return ServiceResult.Set(true);
        }
    }
}
    