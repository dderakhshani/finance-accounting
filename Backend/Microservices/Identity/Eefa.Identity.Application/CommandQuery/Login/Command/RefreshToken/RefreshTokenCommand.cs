using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Identity.Services.Identity;
using Eefa.Identity.Services.Interfaces;
using Infrastructure.Common;
using Infrastructure.Data.Models;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using MediatR;

namespace Eefa.Identity.Application.CommandQuery.Login.Command.RefreshToken
{
    public class RefreshTokenCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ServiceResult>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService, IUserService userService, ICurrentUserAccessor currentUserAccessor)
        {
            _identityService = identityService;
            _userService = userService;
            _currentUserAccessor = currentUserAccessor;
        }


        public async Task<ServiceResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserById(_currentUserAccessor.GetId(), cancellationToken);

            if (!_identityService.GetRefreshTokenByUsername(user.UserName).Equals(_currentUserAccessor.GetRefreshToken())) throw new NotImplementedException();

            var identityModel = new IdentityModel()
            {
                RoleId = _currentUserAccessor.GetRoleId(),
                CompanyId = _currentUserAccessor.GetCompanyId(),
                YearId = _currentUserAccessor.GetYearId(),
                Username = user.UserName,
                Ip = _currentUserAccessor.GetIp(),
                Id = user.Id
            };

            var claims = await _userService.GetClaims(identityModel, user, cancellationToken);

            identityModel = _identityService.GenerateToken(identityModel, claims);

            return ServiceResult.Set(identityModel);
        }
    }
}
