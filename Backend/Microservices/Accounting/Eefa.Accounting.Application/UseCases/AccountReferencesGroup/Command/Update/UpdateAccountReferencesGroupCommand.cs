using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Model;
using Library.Attributes;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Update
{
    public class UpdateAccountReferencesGroupCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.AccountReferencesGroup>, ICommand
    {
        public int Id { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [UniqueIndex]
        public string Title { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAccountReferencesGroupCommand, Eefa.Accounting.Data.Entities.AccountReferencesGroup>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateReferencesGroupCommandHandler : IRequestHandler<UpdateAccountReferencesGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public UpdateReferencesGroupCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateAccountReferencesGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Eefa.Accounting.Data.Entities.AccountReferencesGroup>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.CompanyId = _currentUserAccessor.GetCompanyId();
            entity.ParentId = request.ParentId;
            entity.Title = request.Title;
            entity.Code = request.Code;
            entity.IsEditable = request.IsEditable;

            _repository.Update(entity);


            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<AccountReferencesGroupModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
