using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountReference.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Data.Entities;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.Update
{
    public class UpdateAccountReferenceCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.AccountReference>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public string Code { get; set; }
        public string? Description { get; set; }
        public ICollection<int> ReferenceGroupsId { get; set; } = new List<int>();
        public ICollection<int> AccountHeadIds { get; set; } = new List<int>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAccountReferenceCommand, Data.Entities.AccountReference>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAccountReferenceCommandHandler : IRequestHandler<UpdateAccountReferenceCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor currentUser;

        public UpdateAccountReferenceCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUser)
        {
            _mapper = mapper;
            this.currentUser = currentUser;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateAccountReferenceCommand request, CancellationToken cancellationToken)
        {

            var entity = await _repository
                .Find<Data.Entities.AccountReference>(c =>
            c.ObjectId(request.Id))
                .Include(x => x.AccountReferencesRelReferencesGroups)
            .FirstOrDefaultAsync(cancellationToken);

            entity.Code = request.Code;
            entity.Title = request.Title;
            entity.IsActive = request.IsActive;
            entity.Description = request.Description;
            _repository.Update(entity);

            var groupRelationIdsToRemove = entity.AccountReferencesRelReferencesGroups.Select(x => x.ReferenceGroupId).Except(request.ReferenceGroupsId);
            var groupRelationsToAdd = request.ReferenceGroupsId.Except(entity.AccountReferencesRelReferencesGroups.Select(x => x.ReferenceGroupId));

            foreach (var groupIdToRemove in groupRelationIdsToRemove) _repository.Delete(entity.AccountReferencesRelReferencesGroups.FirstOrDefault(x => x.ReferenceGroupId == groupIdToRemove));


            foreach (var groupIdToAdd in groupRelationsToAdd)
            {
                var newGroupRelation = new AccountReferencesRelReferencesGroup() { ReferenceGroupId = groupIdToAdd, ReferenceId = entity.Id };
                entity.AccountReferencesRelReferencesGroups.Add(newGroupRelation);
                _repository.Insert(newGroupRelation);
            }


            var personalGroup = await _repository.GetQuery<Data.Entities.AccountReferencesGroup>().Include(x => x.AccountHeadRelReferenceGroups).Where(x => x.IsVisible == false && entity.AccountReferencesRelReferencesGroups.Select(x => x.ReferenceGroupId).Contains(x.Id)).FirstOrDefaultAsync();
            if (personalGroup != null)
            {
                personalGroup.Code = entity.Code;
                personalGroup.Title = "(بدون گروه)" + " / " + entity.Code + " " + entity.Title;
                _repository.Update(personalGroup);
            }
            if (request.AccountHeadIds.Any())
            {
                if (personalGroup == null)
                {
                    personalGroup = new Data.Entities.AccountReferencesGroup
                    {
                        IsVisible = false,
                        Code = entity.Code,
                        Title = "(بدون گروه)" + " / " + entity.Code + " " + entity.Title,
                        IsEditable = false,
                        CompanyId = currentUser.GetCompanyId(),
                        AccountHeadRelReferenceGroups = new List<Data.Entities.AccountHeadRelReferenceGroup>(),
                        AccountReferencesRelReferencesGroups = new List<Data.Entities.AccountReferencesRelReferencesGroup>()
                    };
                    var personalGroupRelationToReference = new AccountReferencesRelReferencesGroup { ReferenceGroup = personalGroup, ReferenceId = entity.Id };
                    personalGroup.AccountReferencesRelReferencesGroups.Add(personalGroupRelationToReference);
                    _repository.Insert(personalGroupRelationToReference);
                    _repository.Insert(personalGroup);
                }
                var accountHeadRelationIdsToRemove = personalGroup.AccountHeadRelReferenceGroups.Select(x => x.AccountHeadId).Except(request.AccountHeadIds);
                var accountHeadRelationIdsToAdd = request.AccountHeadIds.Except(personalGroup.AccountHeadRelReferenceGroups.Select(x => x.AccountHeadId));

                foreach (var accountHeadToRemove in accountHeadRelationIdsToRemove) _repository.Delete(personalGroup.AccountHeadRelReferenceGroups.FirstOrDefault(x => x.AccountHeadId == accountHeadToRemove));
                foreach (var accountHeadToAdd in accountHeadRelationIdsToAdd)
                {
                    var newAccountHeadRelation = new Data.Entities.AccountHeadRelReferenceGroup { AccountHeadId = accountHeadToAdd, AccountReferencesGroup = personalGroup };
                    personalGroup.AccountHeadRelReferenceGroups.Add(newAccountHeadRelation);
                    _repository.Insert(newAccountHeadRelation);
                }
            }


            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);

            return ServiceResult.Success(await _repository
                .Find<Data.Entities.AccountReference>(c
            => c.ObjectId(request.Id))
            .ProjectTo<AccountReferenceModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
