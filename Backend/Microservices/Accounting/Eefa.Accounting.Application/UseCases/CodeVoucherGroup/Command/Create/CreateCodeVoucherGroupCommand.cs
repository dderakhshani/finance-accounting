using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;


namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Create
{
    public class CreateCodeVoucherGroupCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCodeVoucherGroupCommand>, ICommand
    {
        public string Code { get; set; }

        public string Title { get; set; }
        public string UniqueName { get; set; }
        public DateTime? LastEditableDate { get; set; }
        public bool IsAuto { get; set; }
        public bool IsEditable { get; set; }
        public bool IsActive { get; set; }
        public bool AutoVoucherEnterGroup { get; set; }
        public string BlankDateFormula { get; set; }
        public int? ViewId { get; set; }
        public int? ExtendTypeId { get; set; }
        public string? TableName { get; set; }
        public string? SchemaName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCodeVoucherGroupCommand, Data.Entities.CodeVoucherGroup>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCodeVoucherGroupCommandHandler : IRequestHandler<CreateCodeVoucherGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CreateCodeVoucherGroupCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCodeVoucherGroupCommand request, CancellationToken cancellationToken)
        {
            var input = _mapper.Map<Data.Entities.CodeVoucherGroup>(request);
            input.CompanyId = _currentUserAccessor.GetCompanyId();
            var entity =  _repository.Insert(input);

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(entity.Entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity.Entity);
            }

            return ServiceResult.Failure();
        }
    }
}
