using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.AccountHead.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.ComponentModel;

namespace Eefa.Accounting.Application.UseCases.AccountHead.Command.Create
{
    public class CreateAccountHeadCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateAccountHeadCommand>, ICommand
    {
        public string Code { get; set; } = default!;
        public bool LastLevel { get; set; } = default!;
        public int? ParentId { get; set; }
        public string Title { get; set; } = default!;
        public int? BalanceId { get; set; }
        public int BalanceBaseId { get; set; } = default!;
        public int TransferId { get; set; } = default!;
        public int? GroupId { get; set; }
        public int CurrencyBaseTypeId { get; set; } = default!;
        public bool CurrencyFlag { get; set; } = default!;
        public bool ExchengeFlag { get; set; } = default!;
        public bool TraceFlag { get; set; } = default!;
        public bool QuantityFlag { get; set; } = default!;
        public bool? IsActive { get; set; } = default!;
        public string Description { get; set; }

        public List<AddAccountReferenceGroupToAccountHeadCommand> Groups { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAccountHeadCommand, Data.Entities.AccountHead>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateAccountHeadCommandHandler : IRequestHandler<CreateAccountHeadCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IConfigurationAccessor _configurationAccessor;
        public CreateAccountHeadCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IConfigurationAccessor configurationAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _configurationAccessor = configurationAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateAccountHeadCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.Entities.AccountHead>(request);
            entity.AccountHeadRelReferenceGroups = new List<Data.Entities.AccountHeadRelReferenceGroup>();
            if (request.ParentId != default)
            {
                entity.CodeLevel =  await _repository.GetQuery<Data.Entities.AccountHead>().Where(x => x.Id == request.ParentId).Select(x => x.CodeLevel).FirstOrDefaultAsync() + 1;
            } else
            {
                entity.CodeLevel = 1;
            }
            entity.CompanyId = _currentUserAccessor.GetCompanyId();

            if (request.Groups.Count > 0) {
                foreach (var group in request.Groups)
                {
                    var newGroup = new Data.Entities.AccountHeadRelReferenceGroup
                    {
                        AccountHead = entity,
                        ReferenceGroupId = group.ReferenceGroupId,
                        ReferenceNo = group.ReferenceNo,
                    };
                    entity.AccountHeadRelReferenceGroups.Add(newGroup);
                    _repository.Insert(newGroup);
                }
            }
            _repository.Insert(entity);


            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);
            return ServiceResult.Success(_mapper.Map<AccountHeadModel>(entity));
        }
    }
}
