using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateAccountReferenceCommand : IRequest<ServiceResult<AccountReferenceModel>>, IMapFrom<CreateAccountReferenceCommand>
{
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public string Code { get; set; }
    public string? Description { get; set; }
    public int AccountReferenceTypeBaseId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateAccountReferenceCommand, AccountReference>()
            .IgnoreAllNonExisting();
    }
}

public class CreateAccountReferenceCommandHandler : IRequestHandler<CreateAccountReferenceCommand, ServiceResult<AccountReferenceModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAccountReferenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResult<AccountReferenceModel>> Handle(CreateAccountReferenceCommand request, CancellationToken cancellationToken)
    {
        AccountReference entity = _mapper.Map<AccountReference>(request);

        if (string.IsNullOrEmpty(request.Code))
        {
            var accountReferencesType = await _unitOfWork.BaseValues.GetByIdAsync(request.AccountReferenceTypeBaseId);

            var lastAccountReference = await _unitOfWork.AccountReferences
                            .GetAsync(x => x.Code.Length == 6 && x.Code.StartsWith(accountReferencesType.Value));

            if (lastAccountReference.Code.EndsWith("9999")) throw new Exception("Maximum code limit has been reached for this accountReference Type");
            if (lastAccountReference == null) lastAccountReference = new AccountReference { Code = accountReferencesType.Value + "0000" };

            entity.Code = (Convert.ToInt32(lastAccountReference.Code) + 1).ToString();
        }
        else
        {
            entity.Code = request.Code;
        }

        _unitOfWork.AccountReferences.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountReferenceModel>(entity));
    }
}