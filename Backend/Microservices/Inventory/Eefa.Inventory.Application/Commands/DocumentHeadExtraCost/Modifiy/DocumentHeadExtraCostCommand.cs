using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Eefa.Invertory.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class DocumentHeadExtraCostCommand : CommandBase, IRequest<ServiceResult<DocumentHeadExtraCostModel>>, IMapFrom<DocumentHeadExtraCostCommand>, ICommand
    {
        public List<DocumentHeadExtraCost> DocumentHeadExtraCosts { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<DocumentHeadExtraCostCommand, Domain.DocumentHeadExtraCost>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateDocumentHeadExtraCostCommandHandler : IRequestHandler<DocumentHeadExtraCostCommand, ServiceResult<DocumentHeadExtraCostModel>>
    {

        private readonly IMapper _mapper;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IDocumentHeadExtraCostRepository _DocumentHeadExtraCostRepository;

        public CreateDocumentHeadExtraCostCommandHandler(

            IMapper mapper,
            IReceiptRepository receiptRepository,
            IDocumentHeadExtraCostRepository DocumentHeadExtraCostRepository

            )
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
            _DocumentHeadExtraCostRepository = DocumentHeadExtraCostRepository;

        }

        public async Task<ServiceResult<DocumentHeadExtraCostModel>> Handle(DocumentHeadExtraCostCommand request, CancellationToken cancellationToken)
        {



            if (!request.DocumentHeadExtraCosts.Any())
            {
                throw new ValidationError("هیچ اطلاعاتی جهت ثبت وجود ندارد");
            }
            foreach (var item in request.DocumentHeadExtraCosts)
            {
                var entity = await _DocumentHeadExtraCostRepository.Find(item.Id);
                if (item.Id > 0 && !item.IsDeleted)
                {
                    entity.ExtraCostDescription= item.ExtraCostDescription;
                    entity.ExtraCostAmount= item.ExtraCostAmount;
                    entity.ExtraCostCurrencyFee= item.ExtraCostCurrencyFee;
                    entity.ExtraCostCurrencyAmount= item.ExtraCostCurrencyAmount;
                    entity.FinancialOperationNumber= item.FinancialOperationNumber;
                    _DocumentHeadExtraCostRepository.Update(entity);

                }
                else if(item.Id == 0 && !item.IsDeleted)
                {
                    _DocumentHeadExtraCostRepository.Insert(_mapper.Map<Domain.DocumentHeadExtraCost>(item));
                }
                else if(item.IsDeleted)
                {
                    _DocumentHeadExtraCostRepository.Delete(entity);
                }

            }

            await _DocumentHeadExtraCostRepository.SaveChangesAsync();

            return ServiceResult<DocumentHeadExtraCostModel>.Success(new DocumentHeadExtraCostModel());


        }

    }


}
