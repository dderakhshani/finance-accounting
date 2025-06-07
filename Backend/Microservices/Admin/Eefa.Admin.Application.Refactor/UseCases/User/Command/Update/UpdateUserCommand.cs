using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateUserCommand : IRequest<ServiceResult<UserModel>>, IMapFrom<User>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public bool IsBlocked { get; set; } = default!;
    public int? BlockedReasonBaseId { get; set; }
    public string? OneTimePassword { get; set; }
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public DateTime PasswordExpiryDate { get; set; }
    public ICollection<int> UserAllowedYears { get; set; }
    public IList<int> RolesIdList { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserCommand, User>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResult<UserModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UserModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User entity = await _unitOfWork.Users.GetByIdAsync(request.Id,
                            x => x.Include(y => y.UserRoleUsers)
                                  .Include(y => y.UserYearUsers));

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

        entity.Username = request.Username.ToLower();
        entity.IsBlocked = request.IsBlocked;
        if (request.BlockedReasonBaseId != null)
        {
            entity.BlockedReasonBaseId = request.BlockedReasonBaseId;
        }

        entity.OneTimePassword = request.OneTimePassword;

        foreach (var removedRole in entity.UserRoleUsers.Select(x => x.RoleId).Except(request.RolesIdList))
        {
            UserRole deletingEntity = await _unitOfWork.UserRoles
                                            .GetAsync(x => x.RoleId == removedRole &&
                                                           x.UserId == entity.Id);

            _unitOfWork.UserRoles.Delete(deletingEntity);
        }

        foreach (var addedRole in request.RolesIdList.Except(entity.UserRoleUsers.Select(x => x.RoleId)))
        {
            _unitOfWork.UserRoles.Add(new UserRole()
            {
                UserId = entity.Id,
                RoleId = addedRole,
            });
        }

        foreach (var removedyear in entity.UserYearUsers.Select(x => x.YearId).Except(request.UserAllowedYears))
        {
            UserYear deletingEntity = await _unitOfWork.UsersYears
                                        .GetAsync(x => x.YearId == removedyear &&
                                                       x.UserId == entity.Id);

            _unitOfWork.UsersYears.Delete(deletingEntity);
        }

        foreach (var addedYear in request.UserAllowedYears.Except(entity.UserYearUsers.Select(x => x.YearId)))
        {
            _unitOfWork.UsersYears.Add(new UserYear()
            {
                UserId = entity.Id,
                YearId = addedYear,
            });
        }

        _unitOfWork.Users.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<UserModel>(entity));
    }
}