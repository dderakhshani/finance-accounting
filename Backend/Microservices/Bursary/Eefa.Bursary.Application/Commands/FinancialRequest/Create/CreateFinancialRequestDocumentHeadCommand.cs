//using AutoMapper;
//using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
//using Eefa.Common.CommandQuery;
 
 
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Eefa.Common;

//namespace Eefa.Bursary.Application.Commands.FinancialRequest.Create
//{
//  public class CreateFinancialRequestDocumentHeadCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateFinancialRequestDocumentHeadCommand>, ICommand
//    {
//        public int DocumentHeadId { get; set; } = default!;
//        public int FinancialRequestId { get; set; } = default!;

//        public bool IsActive { get; set; } = default;

//        public int InvoiceBaseId { get; set; }

//        public void Mapping(Profile profile)
//        {
//            profile.CreateMap<CreateFinancialRequestDocumentHeadCommand, Domain.Entities.FinancialRequestDocumentHead>().IgnoreAllNonExisting();
//        }

//    }



//    public class CreateFinancialRequestDocumentHeadCommandHandler : IRequestHandler<CreateFinancialRequestDocumentHeadCommand, ServiceResult>
//    {
//        private readonly IMapper _autoMaper;
//        private readonly IFinancialRequestDocumentHeadRepository _financialRequestDocumentHeadRepository;

//        public CreateFinancialRequestDocumentHeadCommandHandler(IMapper autoMaper, IFinancialRequestDocumentHeadRepository financialRequestDocumentHeadRepository)
//        {
//            _autoMaper = autoMaper;
//            _financialRequestDocumentHeadRepository = financialRequestDocumentHeadRepository;
//        }

//        public async Task<ServiceResult> Handle(CreateFinancialRequestDocumentHeadCommand request, CancellationToken cancellationToken)
//        {
//            var item = _autoMaper.Map<Domain.Entities.FinancialRequestDocumentHead>(request);

//            var entity = _financialRequestDocumentHeadRepository.Insert(item);

//            await _financialRequestDocumentHeadRepository.SaveChangesAsync(cancellationToken);

//            return ServiceResult.Success();
//        }

//    }
//}
