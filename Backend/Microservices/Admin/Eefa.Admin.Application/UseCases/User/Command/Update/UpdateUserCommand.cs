using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.User.Query.Get;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.User.Command.Update
{
    public class UpdateUserCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.User>,
        ICommand
    {
        public int Id { get; set; }

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// نام کاربری
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        /// آیا قفل شده است؟
        /// </summary>
        public bool IsBlocked { get; set; } = default!;

        /// <summary>
        /// علت قفل شدن
        /// </summary>
        public int? BlockedReasonBaseId { get; set; }

        /// <summary>
        /// رمز موقت
        /// </summary>
        public string? OneTimePassword { get; set; }

        /// <summary>
        /// رمز
        /// </summary>
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public DateTime PasswordExpiryDate { get; set; }
        public ICollection<int> UserAllowedYears { get; set; }

        public IList<int> RolesIdList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserCommand, Data.Databases.Entities.User>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IMediator _mediator;

        public UpdateUserCommandHandler(IRepository repository, IMapper mapper,
            IConfigurationAccessor configurationAccessor, IMediator mediator)
        {
            _mapper = mapper;
            _configurationAccessor = configurationAccessor;
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.User>(c =>
                    c.ObjectId(request.Id)).Include(x => x.UserRoleUsers)
                .Include(x => x.UserYearUsers)
                .FirstOrDefaultAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.Password))
            {

                if (request.Password != request.ConfirmPassword)
                {
                    throw new Exception("InvalidConfirmPassword");
                }

                request.Password = Encryption.Symmetric.CreateMd5Hash(request.Password);


                entity.Password = request.Password;
                //entity.PasswordExpiryDate = request.PasswordExpiryDate;
            }

            entity.PersonId = request.PersonId;
            entity.Username = request.Username.ToLower();
            entity.IsBlocked = request.IsBlocked;
            if (request.BlockedReasonBaseId != null)
            {
                entity.BlockedReasonBaseId = request.BlockedReasonBaseId;
            }

            entity.OneTimePassword = request.OneTimePassword;
            
            foreach (var removedRole in entity.UserRoleUsers.Select(x => x.RoleId).Except(request.RolesIdList))
            {
                var deletingEntity = await _repository
                    .Find<Data.Databases.Entities.UserRole>(c =>
                        c.ConditionExpression(x => x.RoleId == removedRole && x.UserId == entity.Id))
                    .FirstOrDefaultAsync(cancellationToken);

                _repository.Delete(deletingEntity);
            }

            foreach (var addedRole in request.RolesIdList.Except(entity.UserRoleUsers.Select(x => x.RoleId)))
            {
                _repository.Insert(new Data.Databases.Entities.UserRole()
                {
                    UserId = entity.Id,
                    RoleId = addedRole,
                });
            }

            foreach (var removedyear in entity.UserYearUsers.Select(x => x.YearId).Except(request.UserAllowedYears))
            {
                var deletingEntity = await _repository
                    .Find<Data.Databases.Entities.UserYear>(c =>
                        c.ConditionExpression(x => x.YearId == removedyear && x.UserId == entity.Id))
                    .FirstOrDefaultAsync(cancellationToken);

                _repository.Delete(deletingEntity);
            }

            foreach (var addedYear in request.UserAllowedYears.Except(entity.UserYearUsers.Select(x => x.YearId)))
            {
                _repository.Insert(new Data.Databases.Entities.UserYear()
                {
                    UserId = entity.Id,
                    YearId = addedYear,
                });
            }


            var updateEntity = _repository.Update(entity);
            var res = await request.Save(_repository, updateEntity.Entity, cancellationToken);
            if (res.Succeed)
            {
                return await _mediator.Send(new GetUserQuery() { Id = updateEntity.Entity.Id }, cancellationToken);
            }
            else
            {
                return res;
            }
        }
    }
}