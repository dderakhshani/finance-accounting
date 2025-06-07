using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateUserCommand : IRequest<ServiceResult<UserModel>>, IMapFrom<CreateUserCommand>
{
    public int PersonId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public DateTime PasswordExpiryDate { get; set; }
    public IList<int> RolesIdList { get; set; }
    public ICollection<int> UserAllowedYears { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUserCommand, User>()
            .IgnoreAllNonExisting();
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ServiceResult<UserModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UserModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = _mapper.Map<User>(request);


        if (request.Password != request.ConfirmPassword)
        {
            throw new Exception("InvalidConfirmPassword");
        }


        user.Password = Encryption.Symmetric.CreateMd5Hash(request.Password);
        user.Username = user.Username.ToLower();

        user.PasswordExpiryDate = DateTime.UtcNow.AddYears(1);

        _unitOfWork.Users.Add(user);

        foreach (var i in request.RolesIdList)
        {
            _unitOfWork.UserRoles.Add(new UserRole()
            { RoleId = i, User = user });
        }

        var companyIds = new List<int>();

        foreach (var requestUserAllowedYear in request.UserAllowedYears)
        {
            _unitOfWork.UsersYears.Add(new UserYear()
            { User = user, YearId = requestUserAllowedYear });

            Year year = await _unitOfWork.Years.GetByIdAsync(requestUserAllowedYear);
            if (companyIds.All(x => x != year.CompanyId))
            {
                companyIds.Add(year.CompanyId);
            }
        }

        foreach (var companyId in companyIds)
        {
            _unitOfWork.UsersCompanys.Add(new UserCompany()
            {
                CompanyInformationsId = companyId,
                User = user
            });
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<UserModel>(user));
    }
}