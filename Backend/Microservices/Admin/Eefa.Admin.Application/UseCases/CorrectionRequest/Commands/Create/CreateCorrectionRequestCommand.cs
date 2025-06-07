using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using LinqToDB;
using MediatR;
using ServiceStack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.CorrectionRequest.Commands.Create
{
    public class CreateCorrectionRequestCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCorrectionRequestCommand>, ICommand
    {
        public short Status { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public int? AccessPermissionId { get; set; } = default!;
        public int? DocumentId { get; set; }
        public string OldData { get; set; } = default!;
        public int VerifierUserId { get; set; } = default!;
        public string? PayLoad { get; set; }
        public string ApiUrl { get; set; } = default!;
        public string? ViewUrl { get; set; }
        public string? Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCorrectionRequestCommand, Data.Databases.Entities.CorrectionRequest>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCorrectionRequestCommandHandler : IRequestHandler<CreateCorrectionRequestCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateCorrectionRequestCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCorrectionRequestCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyAPendingRequestExist = await _repository.Find<Data.Databases.Entities.CorrectionRequest>().AnyAsync(x => x.CodeVoucherGroupId == request.CodeVoucherGroupId && x.DocumentId == request.DocumentId && x.Status == 0);
            if (isAlreadyAPendingRequestExist) throw new Exception("There is already a request for this voucher");

            
            var input = _mapper.Map<Data.Databases.Entities.CorrectionRequest>(request);
            input.AccessPermissionId = 1251;
            var entry = _repository.Insert(_mapper.Map<Data.Databases.Entities.CorrectionRequest>(input));
            entry.Entity.ModifiedAt = new DateTime();
            
            return await request.Save(_repository, entry.Entity, cancellationToken);
        }
    }

}
