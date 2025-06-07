using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Domain.Entities.EditRequest;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.EditRequest.Commands.Add
{
    public class CreateEditRequestCommand : CommandBase, IRequest<ServiceResult<CorrectionRequests>>, IMapFrom<CreateEditRequestCommand>, ICommand
    {
        public int CodeVoucherGroupId { get; set; } = default!;
        public int DocumentId { get; set; }
        public string OldData { get; set; } = default!;
        public int VerifierUserId { get; set; } = default!;
        public string PayLoad { get; set; }
        public string ApiUrl { get; set; } = default!;
        public string ViewUrl { get; set; }
        public string Description { get; set; }
        public string VerifierDescription { get; set; }
        public string RequesterDescription { get; set; }
        public int? AccessPermissionId { get; set; } = default!;
        public int YearId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateEditRequestCommand, CorrectionRequests>().IgnoreAllNonExisting();
        }
    }

    public class CreateEditRequestCommandHandler : IRequestHandler<CreateEditRequestCommand, ServiceResult<CorrectionRequests>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateEditRequestCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<CorrectionRequests>> Handle(CreateEditRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<CorrectionRequests>(request);

            if (entity == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }



            return null;
        }
    }

}
