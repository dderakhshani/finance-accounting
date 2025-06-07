using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.Services.$fileinputname$.Model;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace $rootnamespace$.$fileinputname$.Query.GetAll
{
    public class $safeitemname$ : Pagination,IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
      
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
    