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

namespace Eefa.Identity.Application.CommandQuery.Login.Command.Login
{
    public class LoginCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public IdentityModel IdentityModel { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResult>
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService, IUserService userService)
        {
            _identityService = identityService;
            _userService = userService;
        }


        public async Task<ServiceResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByUserPass(request.IdentityModel, cancellationToken);
            request.IdentityModel.Id = user.Id;

            var claims = await _userService.GetClaims(request.IdentityModel, user, cancellationToken);
            request.IdentityModel = _identityService.GenerateToken(request.IdentityModel, claims);
            return ServiceResult.Set(request.IdentityModel);
        }
    }
}
