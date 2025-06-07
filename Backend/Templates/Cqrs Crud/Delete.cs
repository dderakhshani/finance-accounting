using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Application.CommandQueries.$fileinputname$.Model;
using Infrastructure.Data.Models;

namespace $rootnamespace$.$fileinputname$.Command.Delete
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id {get;set;}
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
            var entity =await _repository
                .Find<Data.Entities.$fileinputname$>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);
             
            _repository.Delete(entity);
            return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity));       
        }
    }
}
