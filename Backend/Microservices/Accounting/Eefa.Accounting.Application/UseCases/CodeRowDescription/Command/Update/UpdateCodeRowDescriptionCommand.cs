using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Update
{
    public class UpdateCodeRowDescriptionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.CodeRowDescription>, ICommand
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCodeRowDescriptionCommand, Data.Entities.CodeRowDescription>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCodeRowDescriptionCommandHandler : IRequestHandler<UpdateCodeRowDescriptionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public UpdateCodeRowDescriptionCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateCodeRowDescriptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.CodeRowDescription>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.CompanyId = _currentUserAccessor.GetCompanyId();
            entity.Title = request.Title;

            _repository.Update(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CodeRowDescriptionModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
