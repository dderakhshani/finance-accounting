using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;
// TODO the handler
public class SubmitMoadianInvoiceHeaderCommand : IRequest<ServiceResult>, IMapFrom<ImportByExcelMoadianInvoiceHeaderCommand>
{
    public List<int> InvoiceIds { get; set; }

    public bool IsProduction { get; set; } = false;

    public string Code { get; set; }
    public bool GenerateCode { get; set; }
}

//public class SubmitMoadianInvoiceHeaderCommandHandler : IRequestHandler<SubmitMoadianInvoiceHeaderCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    //private readonly ISMSService smsService;

//    public SubmitMoadianInvoiceHeaderCommandHandler(IUnitOfWork unitOfWork/*, ISMSService smsService*/)
//    {
//        _unitOfWork= unitOfWork;
//        //this.smsService = smsService;
//    }
//    public async Task<ServiceResult> Handle(SubmitMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
//    {
//        var query = _unitOfWork.MoadianInvoiceHeaders;
//        var entities = new Task<List<MoadianInvoiceHeader>>(null);
//        if (request.IsProduction)
//        {
//            entities = query.GetListAsync(x => request.InvoiceIds.Contains(x.Id) && 
//                            (x.ReferenceId == null || x.Status == "FAILED"),
//                             y => y.Include(z => z.MoadianInvoiceDetails));
//        }
//        else
//        {
//            entities = query.GetListAsync(x => request.InvoiceIds.Contains(x.Id),
//                                          y => y.Include(z => z.MoadianInvoiceDetails));
//        }

//        if (request.GenerateCode && request.IsProduction)
//        {
//            var R = new Random();

//            var verificationCode = new VerificationCode
//            {
//                Code = R.Next(100000, 999999).ToString(),
//                Description = "جهت ثبت صورتحساب های مودیان",
//                ExpiryDate = DateTime.UtcNow.AddMinutes(5)
//            };
//            foreach (var entity in entities.Result)
//            {
//                entity.VerificationCode = verificationCode;
//            }
//            await _unitOfWork.SaveChangesAsync(cancellationToken);

//            var phoneNumber = await _unitOfWork.BaseValues.GetAsync(x => x.UniqueName == "ModianSystemVerificationPhoneNumber");
//            var message = await _unitOfWork.BaseValues.GetAsync(x => x.UniqueName == "ModianSystemVerificationMessage");

//            var res = await smsService.SendSMSAsync(new SendSMSRequest
//            {
//                fromNumber = "10009611",
//                toNumbers = new string[] { phoneNumber.Value },
//                userName = "c.eefa.setayesh82",
//                password = "39825",
//                messageContent = message.Value + verificationCode.Code
//            });


//            return ServiceResult.Success(null);
//        }
//        if ((!request.GenerateCode && !string.IsNullOrEmpty(request.Code)) || !request.IsProduction)
//        {
//            if (request.IsProduction)
//            {
//                var code = await _unitOfWork.VerificationCodes.GetAsync(x => !x.IsUsed && x.Code == request.Code && x.ExpiryDate > DateTime.UtcNow);

//                if (code == null)
//                {
//                    throw new Exception("کد نا معتبر است");
//                }
//                else
//                {
//                    code.IsUsed = true;
//                    await _unitOfWork.SaveChangesAsync(cancellationToken);
//                    entities = entities.Where(x => x.VerificationCodeId == code.Id);
//                }

//            }

//            if (request.IsProduction)
//            {
//                TaxApiService.Instance.Init(
//                    MoadianConstants.ProductionProtectorId,
//                    new SignatoryConfig(MoadianConstants.PrivateKey, null),
//                    new NormalProperties(ClientType.SELF_TSP, "v1"),
//                    MoadianConstants.ProductionApiUrl
//                    );
//            }
//            else
//            {
//                TaxApiService.Instance.Init(
//                    MoadianConstants.SandboxProtectorId,
//                    new SignatoryConfig(MoadianConstants.PrivateKey, null),
//                    new NormalProperties(ClientType.SELF_TSP, "v1"),
//                    MoadianConstants.SandboxApiUrl
//                    );
//            }
//            var serverInformation = await TaxApiService.Instance.TaxApis.GetServerInformationAsync();
//            var token = await TaxApiService.Instance.TaxApis.RequestTokenAsync();


//            foreach (var entity in entities)
//            {
//                var wrapper = new InvoiceDtoWrapper();
//                wrapper.Uid = entity.UId;
//                wrapper.FiscalId = MoadianConstants.ProductionProtectorId;
//                var invoice = new InvoiceDto
//                {
//                    Header = new InvoiceHeaderDto
//                    {
//                        Taxid = entity.TaxId,
//                        Indatim = entity.Indatim,
//                        Indati2m = entity.Indati2m,
//                        Inty = entity.Inty,
//                        Inno = entity.Inno,
//                        Irtaxid = entity.Irtaxid,
//                        Inp = entity.Inp,
//                        Ins = entity.Ins,
//                        Tins = entity.Tins,
//                        Tob = entity.Tob,
//                        Bid = entity.Bid,
//                        Tinb = entity.Tinb,
//                        Sbc = entity.Sbc,
//                        Bpc = entity.Bpc,
//                        Bbc = entity.Bbc,
//                        Ft = entity.Ft,
//                        Bpn = entity.Bpn,
//                        Scln = entity.Scln,
//                        Scc = entity.Scc,
//                        Crn = entity.Crn,
//                        Billid = entity.Billid,
//                        Tprdis = entity.Tprdis,
//                        Tdis = entity.Tdis,
//                        Tadis = entity.Tadis,
//                        Tvam = entity.Tvam,
//                        Todam = entity.Todam,
//                        Tbill = entity.Tbill,
//                        Setm = entity.Setm,
//                        Cap = entity.Cap,
//                        Insp = entity.Insp,
//                        Tvop = entity.Tvop,
//                        Tax17 = entity.Tax17,

//                        Cdcn = entity.Cdcn,
//                        Cdcd = entity.Cdcd,
//                        Tonw = entity.Tonw,
//                        Torv = entity.Torv,
//                        Tocv = entity.Tocv,

//                    },
//                    Payments = new List<PaymentDto> { new PaymentDto
//                    {
//                        Iinn = null,
//                        Acn = null,
//                        Trmn = null,
//                        Trn = null,
//                        Pcn = null,
//                        Pid = null,
//                        Pdt = entity.Indatim,
//                        Pmt = null,
//                        Pv = 0
//                    }},
//                    Body = new List<InvoiceBodyDto>()
//                };

//                foreach (var detail in entity.MoadianInvoiceDetails)
//                {
//                    invoice.Body.Add(
//                        new InvoiceBodyDto
//                        {
//                            Sstid = detail.Sstid,
//                            Sstt = detail.Sstt,
//                            Mu = detail.Mu,
//                            Am = detail.Am,
//                            Fee = detail.Fee,
//                            Cfee = detail.Cfee,
//                            Cut = detail.Cut,
//                            Exr = detail.Exr,
//                            Prdis = detail.Prdis,
//                            Dis = detail.Dis,
//                            Adis = detail.Adis,
//                            Vra = detail.Vra,
//                            Vam = detail.Vam,
//                            Odt = detail.Odt,
//                            Odr = detail.Odr,
//                            Odam = detail.Odam,
//                            Olt = detail.Olt,
//                            Olr = detail.Olr,
//                            Olam = detail.Olam,
//                            Consfee = detail.Consfee,
//                            Spro = detail.Spro,
//                            Bros = detail.Bros,
//                            Tcpbs = detail.Tcpbs,
//                            Cop = detail.Cop,
//                            Vop = detail.Vop,
//                            Bsrn = detail.Bsrn,
//                            Tsstam = detail.Tsstam,
//                            Nw = detail.Nw,
//                            Ssrv = detail.Ssrv,
//                            Sscv = detail.Sscv,
//                            Pspd = detail.Pspd,
//                        });
//                }

//                wrapper.Invoice = invoice;

//                var response = await TaxApiService.Instance.TaxApis.SendInvoicesAsync(new List<InvoiceDto> { wrapper.Invoice }, null);
//                var packetResponse = response?.Body?.Result;
//                var invoiceResponse = packetResponse.FirstOrDefault();

//                entity.ReferenceId = invoiceResponse.ReferenceNumber ?? null;
//                entity.SubmissionDate = DateTime.UtcNow;
//                entity.Status = "SENT";
//                await this._unitOfWork.SaveChangesAsync(cancellationToken);
//            }

//            //foreach (var entity in entities)
//            //{
//            //    var inquiryResultModels = await TaxApiService.Instance.TaxApis.InquiryByReferenceIdAsync(new() { entity?.ReferenceId });

//            //    var responseData = inquiryResultModels?.First()?.Data;
//            //    var mappedResponseData = new MoadianInquiryResultModel();
//            //    if (responseData != null)
//            //    {
//            //        mappedResponseData = JsonConvert.DeserializeObject<MoadianInquiryResultModel>(responseData.ToString());
//            //    }
//            //    entity.Status = inquiryResultModels?.First()?.Status ?? "";
//            //    entity.Errors = "";
//            //    entity.Errors += mappedResponseData?.Error?.Count > 0 ? "\nخطاها:\n\n" + mappedResponseData.Error.Select(x => x.Msg).ToList().Join("\n") : "";
//            //    entity.Errors += mappedResponseData?.Warning?.Count > 0 ? "\nاخطار ها:\n\n" + mappedResponseData.Warning.Select(x => x.Msg).ToList().Join("\n") : "";

//            //    await this._unitOfWorkSaveChangesAsync(cancellationToken);
//            //}

//            return ServiceResult.Success(null);
//        }

//        return ServiceResult.Success(null);
//    }
//}