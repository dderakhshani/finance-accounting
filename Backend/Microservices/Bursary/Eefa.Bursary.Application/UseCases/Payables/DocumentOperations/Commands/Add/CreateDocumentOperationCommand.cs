using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Add
{
    public class CreateDocumentOperationCommand : CommandBase, IRequest<ServiceResult<Payables_DocumentsOperations>>, IMapFrom<CreateDocumentOperationCommand>, ICommand
    {
        public int[] DocumentIds { get; set; }
        public DateTime? OperationDate { get; set; }
        public int OperationId { get; set; }
        public string Descp { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateDocumentOperationCommand, Payables_DocumentsOperations>().IgnoreAllNonExisting();
        }
    }

    public class CreateDocumentOperationCommandHandler : IRequestHandler<CreateDocumentOperationCommand, ServiceResult<Payables_DocumentsOperations>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateDocumentOperationCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_DocumentsOperations>> Handle(CreateDocumentOperationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
                }

                foreach (var i in request.DocumentIds)
                {
                    var ent = new Payables_DocumentsOperations();
                    _mapper.Map(request, ent);
                    ent.DocumentId = i;
                    _uow.Payables_DocumentsOperations.Add(ent);
                }

                var value = await _uow.SaveChangesAsync(cancellationToken);
                if (value <= 0)
                {
                    throw new Exception("بروز خطا در ثبت عملیات سند پرداختی");
                }
                return ServiceResult<Payables_DocumentsOperations>.Success(null);
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }

}
