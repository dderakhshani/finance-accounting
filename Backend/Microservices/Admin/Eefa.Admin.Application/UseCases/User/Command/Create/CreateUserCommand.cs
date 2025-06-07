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

namespace Eefa.Admin.Application.CommandQueries.User.Command.Create
{
    public class CreateUserCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateUserCommand>, ICommand
    {
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// نام کاربری
        /// </summary>
        public string Username { get; set; } = default!;


        /// <summary>
        /// رمز
        /// </summary>
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public DateTime PasswordExpiryDate { get; set; }
        public IList<int> RolesIdList { get; set; }
        public ICollection<int> UserAllowedYears { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserCommand, Data.Databases.Entities.User>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IMediator _mediator;
        public CreateUserCommandHandler(IRepository repository, IMapper mapper, IConfigurationAccessor configurationAccessor, IMediator mediator)
        {
            _mapper = mapper;
            _configurationAccessor = configurationAccessor;
            _mediator = mediator;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Data.Databases.Entities.User>(request);


            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("InvalidConfirmPassword");
            }


            user.Password = Encryption.Symmetric.CreateMd5Hash(request.Password);
            user.Username = user.Username.ToLower();

            user.PasswordExpiryDate = DateTime.UtcNow.AddYears(1);

            var entity = _repository.Insert(user);

            foreach (var i in request.RolesIdList)
            {
                _repository.Insert<Data.Databases.Entities.UserRole>(new Data.Databases.Entities.UserRole()
                { RoleId = i, User = entity.Entity });
            }


            var companyIds = new List<int>();

            foreach (var requestUserAllowedYear in request.UserAllowedYears)
            {
                _repository.Insert(new Data.Databases.Entities.UserYear()
                { User = entity.Entity, YearId = requestUserAllowedYear });
                var year = await _repository.Find<Data.Databases.Entities.Year>(x => x.ObjectId(requestUserAllowedYear)).FirstOrDefaultAsync();
                if (companyIds.All(x => x != year.CompanyId))
                {
                    companyIds.Add(year.CompanyId);
                }
            }

            foreach (var companyId in companyIds)
            {
                _repository.Insert(new Data.Databases.Entities.UserCompany() {
                    CompanyInformationsId = companyId,
                    User = entity.Entity
                });
            }


            var res = await request.Save(_repository, entity.Entity, cancellationToken);
            if (res.Succeed)
            {
                return await _mediator.Send(new GetUserQuery() { Id = entity.Entity.Id }, cancellationToken);
            }
            else
            {
                return res;
            }
        }
    }
}
