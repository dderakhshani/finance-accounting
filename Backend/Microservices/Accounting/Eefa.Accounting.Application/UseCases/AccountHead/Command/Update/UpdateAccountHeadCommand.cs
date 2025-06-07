using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountHead.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AccountHead.Command.Update
{
    public class UpdateAccountHeadCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<UpdateAccountHeadCommand>, ICommand
    {
        public int Id { get; set; }
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
            profile.CreateMap<UpdateAccountHeadCommand, Data.Entities.AccountHead>();
        }
    }


    public class UpdateAccountHeadCommandHandler : IRequestHandler<UpdateAccountHeadCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public List<Data.Entities.AccountHead> AccountHeads = new List<Data.Entities.AccountHead>();
        public UpdateAccountHeadCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateAccountHeadCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.AccountHead>(c =>
            c.ObjectId(request.Id))
                .Include(x => x.AccountHeadRelReferenceGroups)
            .FirstOrDefaultAsync(cancellationToken);

            if (request.Code != entity.Code)
            {
                this.AccountHeads = await _repository.GetAll<Data.Entities.AccountHead>().ToListAsync();

                await this.UpdateChildrenCode(entity.Id,entity.Code, request.Code);
            }

            _mapper.Map(request, entity);


            if (entity.AccountHeadRelReferenceGroups?.Count > 0 || request.Groups?.Count > 0)
            {
                var groupIdsToAdd = request.Groups?.Select(x => x.ReferenceGroupId).ToList().Except(entity.AccountHeadRelReferenceGroups?.Select(x => x.ReferenceGroupId).ToList()).ToList();
                var groupIdsToRemove = entity.AccountHeadRelReferenceGroups?.Select(x => x.ReferenceGroupId).ToList().Except(request.Groups?.Select(x => x.ReferenceGroupId).ToList()).ToList();

                foreach (var groupId in groupIdsToRemove)
                {
                    _repository.Delete(entity.AccountHeadRelReferenceGroups.FirstOrDefault(x => x.ReferenceGroupId == groupId));
                }
                foreach (var groupId in groupIdsToAdd)
                {
                    var newRelation = new Data.Entities.AccountHeadRelReferenceGroup
                    {
                        ReferenceGroupId = groupId,
                        ReferenceNo = request.Groups.FirstOrDefault(x => x.ReferenceGroupId == groupId).ReferenceNo
                    };
                    entity.AccountHeadRelReferenceGroups.Add(newRelation);
                    _repository.Insert(newRelation);
                }
            }
            _repository.Update(entity);


            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);
            return ServiceResult.Success(_mapper.Map<AccountHeadModel>(entity));

        }

        public async Task UpdateChildrenCode(int parentId, string oldCode, string newCode)
        {
            foreach (var childAccountHead in this.AccountHeads.Where(x => x.ParentId == parentId).ToList())
            {
                var itemCode = childAccountHead.Code.Substring(oldCode.Length, (childAccountHead.Code.Length - oldCode.Length));
                childAccountHead.Code = newCode + itemCode;
                _repository.Update(childAccountHead);
                if (this.AccountHeads.Any(x => x.ParentId == childAccountHead.Id)) await UpdateChildrenCode(childAccountHead.Id, oldCode, newCode);
            }
        }
    }
}
