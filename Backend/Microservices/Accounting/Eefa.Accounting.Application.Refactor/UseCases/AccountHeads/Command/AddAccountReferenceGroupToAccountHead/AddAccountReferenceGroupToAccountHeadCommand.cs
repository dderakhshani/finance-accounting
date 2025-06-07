using MediatR;

public class AddAccountReferenceGroupToAccountHeadCommand : IRequest<ServiceResult>, IMapFrom<AddAccountReferenceGroupToAccountHeadCommand>
{
    public int ReferenceGroupId { get; set; }
    public int ReferenceNo { get; set; }
}