using DocumentFormat.OpenXml.EMMA;
using Eefa.Accounting.Application.Common.Extensions;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Create;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.AutoVoucher;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.Exceptions;
using DocumentFormat.OpenXml.Presentation;
using System.Diagnostics;
using System.Data;
namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.ConvertRaahKaranSalaryExcelToVoucherDetails
{
    public class ConvertRaahKaranSalaryExcelToVoucherDetailsCommand : IRequest<ServiceResult>
    {
        public IFormFile excelFile { get; set; }
        public bool IsFactory { get; set; }
    }

    public class ConvertRaahKaranSalaryExcelToVoucherDetailsCommandHandler : IRequestHandler<ConvertRaahKaranSalaryExcelToVoucherDetailsCommand, ServiceResult>
    {
        private readonly IAccountingUnitOfWork unitOfWork;

        public ConvertRaahKaranSalaryExcelToVoucherDetailsCommandHandler(
            IAccountingUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult> Handle(ConvertRaahKaranSalaryExcelToVoucherDetailsCommand request, CancellationToken cancellationToken)
        {
            var headersColumnIndex = new List<string>
            {
                nameof(SalaryVoucherDetailImportModel.IssueYearAndMonth) +",0",
                nameof(SalaryVoucherDetailImportModel.EmployeeCode) +",2",
                nameof(SalaryVoucherDetailImportModel.EmployeeFirstName) +",3",
                nameof(SalaryVoucherDetailImportModel.EmployeeLastName) +",4",
                nameof(SalaryVoucherDetailImportModel.EmployeeFathersName) +",6",
                nameof(SalaryVoucherDetailImportModel.NationalCode) +",8",


                nameof(SalaryVoucherDetailImportModel.MozdeShoghl) +",75",
                nameof(SalaryVoucherDetailImportModel.MozdeSanavat) +",74",
                nameof(SalaryVoucherDetailImportModel.HagheMaskanoKharobar) +",58",
                nameof(SalaryVoucherDetailImportModel.HagheOlad) +",53",
                nameof(SalaryVoucherDetailImportModel.BonKargari) +",47",
                nameof(SalaryVoucherDetailImportModel.HagheJazb) +",55",
                nameof(SalaryVoucherDetailImportModel.Sarparasti) +",64",
                nameof(SalaryVoucherDetailImportModel.EzafeKari) +",43",
                nameof(SalaryVoucherDetailImportModel.EzafeKariTatili) +",44",
                nameof(SalaryVoucherDetailImportModel.HagheKeshik) +",57",
                nameof(SalaryVoucherDetailImportModel.SakhtieKar) +",62",
                nameof(SalaryVoucherDetailImportModel.FogholadeModiriat) +",66",
                nameof(SalaryVoucherDetailImportModel.HagheTaahol) +",54",
                nameof(SalaryVoucherDetailImportModel.NobateKari) +",76",
                nameof(SalaryVoucherDetailImportModel.MablagheHagheMamooriat) +",70",
                nameof(SalaryVoucherDetailImportModel.MablagheMamooriateAddi) +",73",
                nameof(SalaryVoucherDetailImportModel.PadasheAnbarGardani) +",49",
                nameof(SalaryVoucherDetailImportModel.PadasheBahrevari) +",50",
                nameof(SalaryVoucherDetailImportModel.PadasheModiriat) +",51",
                nameof(SalaryVoucherDetailImportModel.KasreKar) +",98",
                nameof(SalaryVoucherDetailImportModel.TafavoteTatbigh) +",52",
                nameof(SalaryVoucherDetailImportModel.Karane) +",67",
                nameof(SalaryVoucherDetailImportModel.KaraneTolid) +",68",
                nameof(SalaryVoucherDetailImportModel.BedehieMaheJari) +",46",
                nameof(SalaryVoucherDetailImportModel.BedehieMaheGhabl) +",81",
                nameof(SalaryVoucherDetailImportModel.SayereKosoor) +",88",
                nameof(SalaryVoucherDetailImportModel.AyyabZahab) +",45",
                nameof(SalaryVoucherDetailImportModel.AlalHesabeHoghoogh) +",92",
                nameof(SalaryVoucherDetailImportModel.KhalesiePardakhti) +",138",
                nameof(SalaryVoucherDetailImportModel.EjraeiyateDadgah) +",79",
                nameof(SalaryVoucherDetailImportModel.Maliat) +",99",
                nameof(SalaryVoucherDetailImportModel.BimeTakmiliSahmeKarmand) +",83",
                nameof(SalaryVoucherDetailImportModel.BimeTaminEjtemaeiSahmeKarfarma) +",105",
                nameof(SalaryVoucherDetailImportModel.BimeBikari) +",104",
                nameof(SalaryVoucherDetailImportModel.MavaredeEnzebati) +",101",
                nameof(SalaryVoucherDetailImportModel.SayereMazaya) +",61",


                nameof(SalaryVoucherDetailImportModel.JameAghsateVaam) +",86",
                nameof(SalaryVoucherDetailImportModel.GhesteVaamAzDaftareMarkazi) +",96",
                nameof(SalaryVoucherDetailImportModel.SandoghePasandazeKarkhane) +",89",
                nameof(SalaryVoucherDetailImportModel.Mosaede) +",100",

                nameof(SalaryVoucherDetailImportModel.JaraemVasaeteNaghlie) +",85",
                nameof(SalaryVoucherDetailImportModel.GhesteVaameKhaas) +",97",


                nameof(SalaryVoucherDetailImportModel.BimeTaminEjtemaeiSahmeKarmand) +",82",

            };


            List<SalaryVoucherDetailsToBeGeneratedModel> voucherDetailsToBeGenerated = new List<SalaryVoucherDetailsToBeGeneratedModel>
            {
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 1,IsCredit = false, Title= "مزد شغل", PropertyName = nameof(SalaryVoucherDetailImportModel.MozdeShoghl), AccountHeadCode = "80101", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 2,IsCredit = false, Title= "مزد سنوات", PropertyName = nameof(SalaryVoucherDetailImportModel.MozdeSanavat), AccountHeadCode = "80102", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 3,IsCredit = false, Title= "حق مسکن و خواربار", PropertyName = nameof(SalaryVoucherDetailImportModel.HagheMaskanoKharobar), AccountHeadCode = "80103", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 4,IsCredit = false, Title= "حق اولاد", PropertyName = nameof(SalaryVoucherDetailImportModel.HagheOlad), AccountHeadCode = "80104", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 5,IsCredit = false, Title= "بن کارگری", PropertyName =  nameof(SalaryVoucherDetailImportModel.BonKargari), AccountHeadCode = "80105", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 6,IsCredit = false, Title= "حق جذب", PropertyName =  nameof(SalaryVoucherDetailImportModel.HagheJazb), AccountHeadCode = "80106", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 7,IsCredit = false, Title= "سرپرستى", PropertyName =  nameof(SalaryVoucherDetailImportModel.Sarparasti), AccountHeadCode = "80107", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 8,IsCredit = false, Title= "اضافه کاری", PropertyName =  nameof(SalaryVoucherDetailImportModel.EzafeKari), AccountHeadCode = "80108", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 9,IsCredit = false, Title= "اضافه کاری تعطیلی", PropertyName =  nameof(SalaryVoucherDetailImportModel.EzafeKariTatili), AccountHeadCode = "80109", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 10,IsCredit = false, Title= "حق کشیک", PropertyName =  nameof(SalaryVoucherDetailImportModel.HagheKeshik), AccountHeadCode = "80110", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 11,IsCredit = false, Title= "سختی کار", PropertyName =  nameof(SalaryVoucherDetailImportModel.SakhtieKar), AccountHeadCode = "80111", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 12,IsCredit = false, Title= "فوق العاده مدیریت", PropertyName =  nameof(SalaryVoucherDetailImportModel.FogholadeModiriat), AccountHeadCode = "80112", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 13,IsCredit = false, Title= "حق تاهل", PropertyName =  nameof(SalaryVoucherDetailImportModel.HagheTaahol), AccountHeadCode = "80113", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 14,IsCredit = false, Title= "نوبت کاری", PropertyName =  nameof(SalaryVoucherDetailImportModel.NobateKari), AccountHeadCode = "80114", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 16,IsCredit = false, Title= "پاداش انبارگردانی =", PropertyName =  nameof(SalaryVoucherDetailImportModel.PadasheAnbarGardani), AccountHeadCode = "80101", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 17,IsCredit = false, Title= "پاداش بهره وری", PropertyName =  nameof(SalaryVoucherDetailImportModel.PadasheBahrevari), AccountHeadCode = "80119", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 18,IsCredit = false, Title= "پاداش مدیریت", PropertyName =  nameof(SalaryVoucherDetailImportModel.PadasheModiriat), AccountHeadCode = "80123", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 19,IsCredit = true, Title= "کسر کار", PropertyName =  nameof(SalaryVoucherDetailImportModel.KasreKar), AccountHeadCode = "80127", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 20,IsCredit = false, Title= "تفاوت تطبیق", PropertyName =  nameof(SalaryVoucherDetailImportModel.TafavoteTatbigh), AccountHeadCode = "80128", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 22,IsCredit = false, Title= "بدهی ماه جاری", PropertyName =  nameof(SalaryVoucherDetailImportModel.BedehieMaheJari), AccountHeadCode = "80130", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 23,IsCredit = true, Title= "بدهی ماه قبل", PropertyName =  nameof(SalaryVoucherDetailImportModel.BedehieMaheGhabl), AccountHeadCode = "80135", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 24,IsCredit = false, Title= "سایر کسور", PropertyName =  nameof(SalaryVoucherDetailImportModel.SayereKosoor), AccountHeadCode = "80131", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 25,IsCredit = false, Title= "ایاب و ذهاب", PropertyName =  nameof(SalaryVoucherDetailImportModel.AyyabZahab), AccountHeadCode = "80213", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 26,IsCredit = true, Title= "علی الحساب حقوق", PropertyName =  nameof(SalaryVoucherDetailImportModel.AlalHesabeHoghoogh), AccountHeadCode = "50202", AccountReferenceGroupCode = "90", AccountReferenceCode = "38005002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 27,IsCredit = false, Title= "سایر مزایا", PropertyName =  nameof(SalaryVoucherDetailImportModel.SayereMazaya), AccountHeadCode = "80133", AccountReferenceGroupCode = "81-82", AccountReferenceCode = "77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 28,IsCredit = true, Title= "اجراییات دادگاه", PropertyName =  nameof(SalaryVoucherDetailImportModel.EjraeiyateDadgah), AccountHeadCode = "50201", AccountReferenceGroupCode = "90", AccountReferenceCode = "38010003"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 29,IsCredit = true, Title= "موارد انضباطی", PropertyName =  nameof(SalaryVoucherDetailImportModel.MavaredeEnzebati), AccountHeadCode = "80136", AccountReferenceGroupCode = "81-82", AccountReferenceCode = "77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 33,IsCredit = true, Title= "مالیات بر حقوق", PropertyName =  nameof(SalaryVoucherDetailImportModel.Maliat), AccountHeadCode = "50218",  AccountReferenceGroupCode = "62", AccountReferenceCode = "120017"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 34,IsCredit = true, Title= "بیمه تکمیلی سهم کارمند", PropertyName = nameof(SalaryVoucherDetailImportModel.BimeTakmiliSahmeKarmand), AccountHeadCode = "50201", AccountReferenceGroupCode = "62", AccountReferenceCode = "120013"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 35,IsCredit = false, Title= "20% بیمه  تامین اجتماعی سهم کارفرما ", PropertyName =  nameof(SalaryVoucherDetailImportModel.BimeTaminEjtemaeiSahmeKarfarma), AccountHeadCode = "80116", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
                new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 36,IsCredit = false, Title= "3% بیمه بیکاری", PropertyName =  nameof(SalaryVoucherDetailImportModel.BimeBikari), AccountHeadCode = "80125", AccountReferenceGroupCode="81-82", AccountReferenceCode="77002"},
            };



            if (request.IsFactory)
            {
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 27, IsCredit = true, Title = "خالص پرداختی", PropertyName = nameof(SalaryVoucherDetailImportModel.KhalesiePardakhti), AccountHeadCode = "50202", AccountReferenceGroupCode = "90", AccountReferenceCode = "38005002" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 15, IsCredit = false, Title = "مبلغ حق ماموریت", PropertyName = nameof(SalaryVoucherDetailImportModel.MablagheHagheMamooriat), AccountHeadCode = "80117", AccountReferenceGroupCode = "81-82", AccountReferenceCode = "77002" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 30, IsCredit = true, Title = "مساعده", PropertyName = nameof(SalaryVoucherDetailImportModel.Mosaede), AccountHeadCode = "20522", AccountReferenceGroupCode = "41-42", AccountReferenceCode = "420102" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 31, IsCredit = true, Title = "جمع اقساط وام", PropertyName = nameof(SalaryVoucherDetailImportModel.JameAghsateVaam), AccountHeadCode = "50201", AccountReferenceGroupCode = "41-42", AccountReferenceCode = "38010001" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 32, IsCredit = true, Title = "صندوق پس انداز کارخانه", PropertyName = nameof(SalaryVoucherDetailImportModel.SandoghePasandazeKarkhane), AccountHeadCode = "50201", AccountReferenceGroupCode = "41-42", AccountReferenceCode = "38010001" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 21, IsCredit = false, Title = "کارانه تولید", PropertyName = nameof(SalaryVoucherDetailImportModel.KaraneTolid), AccountHeadCode = "80129", AccountReferenceGroupCode = "81-82", AccountReferenceCode = "77002" });
            }
            else
            {
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 27, IsCredit = true, Title = "خالص پرداختی", PropertyName = nameof(SalaryVoucherDetailImportModel.KhalesiePardakhti), AccountHeadCode = "50202", AccountReferenceGroupCode = "90", AccountReferenceCode = "38005001" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 15, IsCredit = false, Title = "مبلغ حق ماموریت", PropertyName = nameof(SalaryVoucherDetailImportModel.MablagheMamooriateAddi), AccountHeadCode = "80117", AccountReferenceGroupCode = "81-82", AccountReferenceCode = "77002" });
                voucherDetailsToBeGenerated.Add(new SalaryVoucherDetailsToBeGeneratedModel { OrderIndex = 21, IsCredit = false, Title = "کارانه", PropertyName = nameof(SalaryVoucherDetailImportModel.Karane), AccountHeadCode = "80129", AccountReferenceGroupCode = "81-82", AccountReferenceCode = "77002" });
            }

            voucherDetailsToBeGenerated.OrderBy(x => x.OrderIndex);
            var excelData = ExcelExtensions.ParseTo<SalaryVoucherDetailImportModel>(request.excelFile, headersColumnIndex);

            List<Data.Entities.VouchersDetail> voucherDetails = new List<Data.Entities.VouchersDetail>();

            var accountHeads = await unitOfWork.AccountHeads.AsNoTracking().Where(x => !x.IsDeleted).Select(x => new
            {
                x.Id,
                x.Code,
                x.ParentId
            }).ToListAsync();
            var accountReferencesGroups = await unitOfWork.AccountReferencesGroups.AsNoTracking().Where(x => !x.IsDeleted).Select(x => new
            {
                x.Code,
                x.Id
            }).ToListAsync();
            var employeesAccountReferenceGroupId = accountReferencesGroups.Where(x => x.Code == (request.IsFactory ? "42" : "41")).FirstOrDefault().Id;
            var accountReferences = await unitOfWork.AccountReferences.AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive)
                .Where(x => x.AccountReferencesRelReferencesGroups.Any(x => x.ReferenceGroupId == employeesAccountReferenceGroupId && !x.IsDeleted))
                .Include(x => x.Person)
                .Select(x => new
                {
                    NationalNumber = x.Person.NationalNumber ?? string.Empty,
                    x.Id,
                    x.Code,
                    x.Title
                })
                .ToListAsync();

            var year = excelData.FirstOrDefault()?.IssueYearAndMonth.Substring(0, 4);
            var month = "";
            switch (excelData.FirstOrDefault()?.IssueYearAndMonth.Substring(4, 2))
            {
                case "01":
                    month = "فروردین";
                    break;
                case "02":
                    month = "اردیبهشت";
                    break;
                case "03":
                    month = "خرداد";
                    break;
                case "04":
                    month = "تیر";
                    break;
                case "05":
                    month = "مرداد";
                    break;
                case "06":
                    month = "شهریور";
                    break;
                case "07":
                    month = "مهر";
                    break;
                case "08":
                    month = "آبان";
                    break;
                case "09":
                    month = "آذر";
                    break;
                case "10":
                    month = "دی";
                    break;
                case "11":
                    month = "بهمن";
                    break;
                case "12":
                    month = "اسفند";
                    break;
                default:
                    break;
            }


            foreach (var item in voucherDetailsToBeGenerated)
            {
                PropertyInfo propInfo = typeof(SalaryVoucherDetailImportModel).GetProperty(item.PropertyName);
                var voucherDetail = new Data.Entities.VouchersDetail
                {
                    VoucherRowDescription = "بابت " + item.Title + " پرسنل " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                    CurrencyTypeBaseId = 28306,
                    RowIndex = voucherDetails.Max(x => x.RowIndex) + 1
                };
                if (voucherDetail.RowIndex == null) voucherDetail.RowIndex = 1;
                if (item.IsCredit) voucherDetail.Credit = excelData.Sum(x => (double)propInfo.GetValue(x));
                else voucherDetail.Debit = excelData.Sum(x => (double)propInfo.GetValue(x));

                if (voucherDetail.Debit == 0 && voucherDetail.Credit == 0) continue;

                if (!string.IsNullOrEmpty(item.AccountHeadCode))
                {
                    voucherDetail.AccountHeadId = accountHeads.Where(x => x.Code == item.AccountHeadCode).Select(x => x.Id).FirstOrDefault();
                    voucherDetail.Level3 = voucherDetail.AccountHeadId;
                    voucherDetail.Level2 = accountHeads.Where(x => x.Id == voucherDetail.Level3).Select(x => x.ParentId).FirstOrDefault();
                    voucherDetail.Level1 = accountHeads.Where(x => x.Id == voucherDetail.Level2).Select(x => x.ParentId).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(item.AccountReferenceGroupCode))
                {
                    if (item.AccountReferenceGroupCode.Contains("-"))
                    {
                        var groupCode = "";
                        if (item.AccountReferenceGroupCode == "41-42") groupCode = request.IsFactory ? "42" : "41";
                        if (item.AccountReferenceGroupCode == "81-82") groupCode = request.IsFactory ? "81" : "82";
                        voucherDetail.AccountReferencesGroupId = accountReferencesGroups.Where(x => x.Code == groupCode).Select(x => x.Id).FirstOrDefault();
                    }
                    else
                    {
                        voucherDetail.AccountReferencesGroupId = accountReferencesGroups.Where(x => x.Code == item.AccountReferenceGroupCode).Select(x => x.Id).FirstOrDefault();

                    }
                }
                if (!string.IsNullOrEmpty(item.AccountReferenceCode))
                {
                    if (item.AccountReferenceCode == "77002") item.AccountReferenceCode = request.IsFactory ? item.AccountReferenceCode.Split("-").First() : item.AccountReferenceCode.Split("-").Last();

                    voucherDetail.ReferenceId1 = await unitOfWork.AccountReferences.Where(x => x.Code == item.AccountReferenceCode && !x.IsDeleted).Select(x => x.Id).FirstOrDefaultAsync();
                }

                voucherDetails.Add(voucherDetail);

            }


            var bime30Darsad = new Data.Entities.VouchersDetail
            {
                VoucherRowDescription = "بابت 30% حق بیمه پرداختنی " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                Credit = excelData.Sum(x => x.BimeTaminEjtemaeiSahmeKarfarma) + excelData.Sum(x => x.BimeBikari) + excelData.Sum(x => x.BimeTaminEjtemaeiSahmeKarmand),
                CurrencyTypeBaseId = 28306,
                RowIndex = voucherDetails.Max(x => x.RowIndex) + 1,
                AccountReferencesGroupId = accountReferencesGroups.Where(x => x.Code == "62").Select(x => x.Id).FirstOrDefault(),
                ReferenceId1 = await unitOfWork.AccountReferences.Where(x => x.Code == (request.IsFactory ? "120010" : "120011")).Select(x => x.Id).FirstOrDefaultAsync(),
                AccountHeadId = accountHeads.FirstOrDefault(x => x.Code == "50203").Id
            };
            bime30Darsad.Level3 = bime30Darsad.AccountHeadId;
            bime30Darsad.Level2 = accountHeads.Where(x => x.Id == bime30Darsad.Level3).Select(x => x.ParentId).FirstOrDefault();
            bime30Darsad.Level1 = accountHeads.Where(x => x.Id == bime30Darsad.Level2).Select(x => x.ParentId).FirstOrDefault();

            voucherDetails.Add(bime30Darsad);

            var accountsErrors = new List<ApplicationErrorModel>();


            foreach (var item in excelData.Where(x => x.JaraemVasaeteNaghlie > 0).ToArray())
            {

                var groupCode = request.IsFactory ? "42" : "41";
                var accountReferenceGroup = accountReferencesGroups.FirstOrDefault(x => x.Code == groupCode);

                var accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode) && x.Code.StartsWith(groupCode));
                if (accountReference == null) accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode));
                if (accountReference == null)
                {
                    accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور فعال برای کد ملی {item.NationalCode}, شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} یافت نشد" });
                    continue;
                };
                if (!accountReference.Code.StartsWith(groupCode))
                {
                    accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور {accountReference.Code} برای کد ملی {item.NationalCode} شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} با گروه {groupCode} تطابق ندارد." });
                    continue;
                }
                var voucherDetail = new Data.Entities.VouchersDetail
                {
                    VoucherRowDescription = "بابت جرائم وسایط نقلیه " + accountReference.Title + " پرسنل " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                    Credit = item.JaraemVasaeteNaghlie,
                    CurrencyTypeBaseId = 28306,
                    RowIndex = voucherDetails.Max(x => x.RowIndex) + 1,
                    AccountReferencesGroupId = accountReferenceGroup.Id,
                    ReferenceId1 = accountReference.Id,
                    AccountHeadId = accountHeads.FirstOrDefault(x => x.Code == "20515").Id
                };
                voucherDetail.Level3 = voucherDetail.AccountHeadId;
                voucherDetail.Level2 = accountHeads.Where(x => x.Id == voucherDetail.Level3).Select(x => x.ParentId).FirstOrDefault();
                voucherDetail.Level1 = accountHeads.Where(x => x.Id == voucherDetail.Level2).Select(x => x.ParentId).FirstOrDefault();

                voucherDetails.Add(voucherDetail);

            }

            foreach (var item in excelData.Where(x => x.GhesteVaameKhaas > 0).ToArray())
            {

                var groupCode = request.IsFactory ? "42" : "41";
                var accountReferenceGroup = accountReferencesGroups.FirstOrDefault(x => x.Code == groupCode);

                var accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode) && x.Code.StartsWith(groupCode));
                if (accountReference == null) accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode));
                if (accountReference == null)
                {
                    accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور فعال برای کد ملی {item.NationalCode}, شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} یافت نشد" });
                    continue;
                };
                if (!accountReference.Code.StartsWith(groupCode))
                {
                    accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور {accountReference.Code} برای کد ملی {item.NationalCode} شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} با گروه {groupCode} تطابق ندارد." });
                    continue;
                }
                var voucherDetail = new Data.Entities.VouchersDetail
                {
                    VoucherRowDescription = "بابت وام خاص " + accountReference.Title + " پرسنل " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                    Credit = item.GhesteVaameKhaas,
                    CurrencyTypeBaseId = 28306,
                    RowIndex = voucherDetails.Max(x => x.RowIndex) + 1,
                    AccountReferencesGroupId = accountReferenceGroup.Id,
                    ReferenceId1 = accountReference.Id,
                    AccountHeadId = accountHeads.FirstOrDefault(x => x.Code == "20520").Id
                };
                voucherDetail.Level3 = voucherDetail.AccountHeadId;
                voucherDetail.Level2 = accountHeads.Where(x => x.Id == voucherDetail.Level3).Select(x => x.ParentId).FirstOrDefault();
                voucherDetail.Level1 = accountHeads.Where(x => x.Id == voucherDetail.Level2).Select(x => x.ParentId).FirstOrDefault();

                voucherDetails.Add(voucherDetail);

            }


            if (!request.IsFactory)
            {
                foreach (var item in excelData.Where(x => x.Mosaede > 0).ToArray())
                {

                    var groupCode = request.IsFactory ? "42" : "41";
                    var accountReferenceGroup = accountReferencesGroups.FirstOrDefault(x => x.Code == groupCode);

                    var accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode) && x.Code.StartsWith(groupCode));
                    if (accountReference == null) accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode));
                    if (accountReference == null)
                    {
                        accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور فعال برای کد ملی {item.NationalCode}, شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} یافت نشد" });
                        continue;
                    };
                    if (!accountReference.Code.StartsWith(groupCode))
                    {
                        accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور {accountReference.Code} برای کد ملی {item.NationalCode} شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} با گروه {groupCode} تطابق ندارد." });
                        continue;
                    }
                    var voucherDetail = new Data.Entities.VouchersDetail
                    {
                        VoucherRowDescription = "بابت مساعده " + accountReference.Title + " پرسنل " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                        Credit = item.Mosaede,
                        CurrencyTypeBaseId = 28306,
                        RowIndex = voucherDetails.Max(x => x.RowIndex) + 1,
                        AccountReferencesGroupId = accountReferenceGroup.Id,
                        ReferenceId1 = accountReference.Id,
                        AccountHeadId = accountHeads.FirstOrDefault(x => x.Code == "20521").Id
                    };
                    voucherDetail.Level3 = voucherDetail.AccountHeadId;
                    voucherDetail.Level2 = accountHeads.Where(x => x.Id == voucherDetail.Level3).Select(x => x.ParentId).FirstOrDefault();
                    voucherDetail.Level1 = accountHeads.Where(x => x.Id == voucherDetail.Level2).Select(x => x.ParentId).FirstOrDefault();

                    voucherDetails.Add(voucherDetail);

                }

                foreach (var item in excelData.Where(x => x.JameAghsateVaam > 0).ToArray())
                {
                    var groupCode = request.IsFactory ? "42" : "41";
                    var accountReferenceGroup = accountReferencesGroups.FirstOrDefault(x => x.Code == groupCode);

                    var accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode) && x.Code.StartsWith(groupCode));
                    if (accountReference == null) accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode));
                    if (accountReference == null)
                    {
                        accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور فعال برای کد ملی {item.NationalCode}, شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} یافت نشد" });
                        continue;
                    };
                    if (!accountReference.Code.StartsWith(groupCode))
                    {
                        accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور {accountReference.Code} برای کد ملی {item.NationalCode} شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} با گروه {groupCode} تطابق ندارد." });
                        continue;
                    }
                    var voucherDetail = new Data.Entities.VouchersDetail
                    {
                        VoucherRowDescription = "بابت وام قرض الحسنه  " + accountReference.Title + " پرسنل " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                        Credit = item.JameAghsateVaam,
                        CurrencyTypeBaseId = 28306,
                        RowIndex = voucherDetails.Max(x => x.RowIndex) + 1,
                        AccountReferencesGroupId = accountReferenceGroup.Id,
                        ReferenceId1 = accountReference.Id,
                        AccountHeadId = accountHeads.FirstOrDefault(x => x.Code == "20520").Id
                    };
                    voucherDetail.Level3 = voucherDetail.AccountHeadId;
                    voucherDetail.Level2 = accountHeads.Where(x => x.Id == voucherDetail.Level3).Select(x => x.ParentId).FirstOrDefault();
                    voucherDetail.Level1 = accountHeads.Where(x => x.Id == voucherDetail.Level2).Select(x => x.ParentId).FirstOrDefault();

                    voucherDetails.Add(voucherDetail);

                }

            }

            if (request.IsFactory)
            {

                foreach (var item in excelData.Where(x => x.GhesteVaamAzDaftareMarkazi > 0).ToArray())
                {

                    var groupCode = request.IsFactory ? "42" : "41";
                    var accountReferenceGroup = accountReferencesGroups.FirstOrDefault(x => x.Code == groupCode);

                    var accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode) && x.Code.StartsWith(groupCode));
                    if (accountReference == null) accountReference = accountReferences.FirstOrDefault(x => x.NationalNumber.Contains(item.NationalCode));
                    if (accountReference == null)
                    {
                        accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور فعال برای کد ملی {item.NationalCode}, شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} یافت نشد" });
                        continue;
                    };
                    if (!accountReference.Code.StartsWith(groupCode))
                    {
                        accountsErrors.Add(new ApplicationErrorModel { Message = $"کد تفصیل شناور {accountReference.Code} برای کد ملی {item.NationalCode} شخص {item.EmployeeFirstName + ' ' + item.EmployeeLastName} با گروه {groupCode} تطابق ندارد." });
                        continue;
                    }
                    var voucherDetail = new Data.Entities.VouchersDetail
                    {
                        VoucherRowDescription = "بابت وام قرض الحسنه" + accountReference.Title + " پرسنل " + (request.IsFactory ? "کارخانه " : "دفتر مرکزی ") + "در " + month + " ماه " + year,
                        Credit = item.GhesteVaamAzDaftareMarkazi,
                        CurrencyTypeBaseId = 28306,
                        RowIndex = voucherDetails.Max(x => x.RowIndex) + 1,
                        AccountReferencesGroupId = accountReferenceGroup.Id,
                        ReferenceId1 = accountReference.Id,
                        AccountHeadId = accountHeads.FirstOrDefault(x => x.Code == "20520").Id
                    };
                    voucherDetail.Level3 = voucherDetail.AccountHeadId;
                    voucherDetail.Level2 = accountHeads.Where(x => x.Id == voucherDetail.Level3).Select(x => x.ParentId).FirstOrDefault();
                    voucherDetail.Level1 = accountHeads.Where(x => x.Id == voucherDetail.Level2).Select(x => x.ParentId).FirstOrDefault();

                    voucherDetails.Add(voucherDetail);

                }

            }





            if (accountsErrors.Count > 0) throw new ApplicationValidationException(accountsErrors);



            return ServiceResult.Success(voucherDetails.OrderBy(x => x.RowIndex));
        }
    }

}
