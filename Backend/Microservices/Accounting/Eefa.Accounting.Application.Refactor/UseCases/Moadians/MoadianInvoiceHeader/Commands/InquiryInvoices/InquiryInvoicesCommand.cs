﻿using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;

public class InquiryInvoicesCommand : IRequest<ServiceResult>
{
    public List<int> InvoiceIds { get; set; }
    public bool IsProduction { get; set; } = false;
}

//public class InquiryInvoicesCommandHandler : IRequestHandler<InquiryInvoicesCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    public InquiryInvoicesCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
//    {
//        _unitOfWork= unitOfWork;
//    }
//    public async Task<ServiceResult> Handle(InquiryInvoicesCommand request, CancellationToken cancellationToken)
//    {

//        var entities = await _unitOfWork.MoadianInvoiceHeaders
//            .GetListAsync(x => request.InvoiceIds.Contains(x.Id) && x.ReferenceId != null);

//        if (request.IsProduction)
//        {
//            TaxApiService.Instance.Init(
//                MoadianConstants.ProductionProtectorId,
//                new SignatoryConfig(MoadianConstants.PrivateKey, null),
//                new NormalProperties(ClientType.SELF_TSP, "v1"),
//                MoadianConstants.ProductionApiUrl
//                );
//        }
//        else
//        {
//            TaxApiService.Instance.Init(
//                MoadianConstants.SandboxProtectorId,
//                new SignatoryConfig(MoadianConstants.PrivateKey, null),
//                new NormalProperties(ClientType.SELF_TSP, "v1"),
//                MoadianConstants.SandboxApiUrl
//                );
//        }
//        var serverInformation = await TaxApiService.Instance.TaxApis.GetServerInformationAsync();
//        var token = await TaxApiService.Instance.TaxApis.RequestTokenAsync();

//        var inquiryResultModels = await TaxApiService.Instance.TaxApis.InquiryByReferenceIdAsync(entities.Select(x => x.ReferenceId).ToList());

//        foreach (var entity in entities)
//        {
//            var inquiryResult = inquiryResultModels.FirstOrDefault(x => x.ReferenceNumber == entity.ReferenceId);

//            if (inquiryResult != null)
//            {
//                var responseData = inquiryResult?.Data;
//                var mappedResponseData = new MoadianInquiryResultModel();
//                if (responseData != null)
//                {
//                    mappedResponseData = JsonConvert.DeserializeObject<MoadianInquiryResultModel>(responseData.ToString());
//                }
//                entity.Status = inquiryResult?.Status ?? "";
//                entity.Errors = "";
//                entity.Errors += mappedResponseData?.Error?.Count > 0 ? "\nخطاها:\n\n" + mappedResponseData.Error.Select(x => x.Message).ToList().Join("\n") : "";
//                entity.Errors += mappedResponseData?.Warning?.Count > 0 ? "\nاخطار ها:\n\n" + mappedResponseData.Warning.Select(x => x.Message).ToList().Join("\n") : "";

//            }
//        }

//        await this._unitOfWork.SaveChangesAsync(cancellationToken);

//        return ServiceResult.Success(null);
//    }
//}