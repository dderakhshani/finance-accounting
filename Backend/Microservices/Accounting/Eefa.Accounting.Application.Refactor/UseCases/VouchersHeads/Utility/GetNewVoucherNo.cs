using System;
using System.Linq;
using System.Threading.Tasks;

//public class VoucherNo
//{
//    public static async Task<int> GetNewVoucherNo(IUnitOfWork unitOfWork, IApplicationUser applicationUser, DateTime voucherDate, int? requestVoucherNo)
//    {
//        if (!await unitOfWork.VouchersHeads.ExistsAsync())
//        {
//            return 1;
//        }
//        var settings =
//          await new SystemSettings(repository).Get(SubSystemType.AccountingSettings);
//        var voucherNumberType = new int();

//        foreach (var baseValue in settings)
//        {
//            if (baseValue.UniqueName == "VoucherNumberType")
//            {
//                voucherNumberType = int.Parse(baseValue.Value);
//                break;
//            }
//        }


//        if (!string.IsNullOrEmpty(requestVoucherNo.ToString()))
//        {
//            if (voucherNumberType == 2)
//            {
//                if (!requestVoucherNo.ToString().StartsWith(applicationUser.BranchId.ToString()))
//                {
//                    throw new Exception("Wrong voucherNo");
//                }
//            }
//        }
//        else
//        {
//            var voucherAndDateCompatibility = false;
//            foreach (var baseValue in settings)
//            {
//                if (baseValue.UniqueName == "VoucherAndDateCompatibility")
//                {
//                    voucherAndDateCompatibility = bool.Parse(baseValue.Value);
//                    break;
//                }
//            }

//            if (voucherAndDateCompatibility)
//            {
//                if (voucherNumberType == 2)
//                {
//                    return repository.GetQuery<VouchersHead>()
//                        .Where(x => x.VoucherDate.Year == voucherDate.Year &&
//                                    x.VoucherNo.ToString()
//                                        .StartsWith(currentUserAccessor.GetBranchId().ToString()))
//                        .Select(x => x.VoucherNo).Max() + 1;
//                }
//                else
//                {
//                    return repository.GetQuery<VouchersHead>()
//                        .Where(x => x.VoucherDate.Year == voucherDate.Year)
//                        .Select(x => x.VoucherNo).Max() + 1;
//                }
//            }
//            else
//            {
//                if (voucherNumberType == 2)
//                {
//                    return repository.GetQuery<VouchersHead>()
//                        .Where(x => x.VoucherDate.Year == voucherDate.Year &&
//                                    x.VoucherNo.ToString()
//                                        .StartsWith(currentUserAccessor.GetBranchId().ToString()))
//                        .Select(x => x.VoucherNo).Max() + 1;
//                }
//                else
//                {
//                    return repository.GetQuery<VouchersHead>()
//                        .Select(x => x.VoucherNo).Max() + 1;
//                }
//            }
//        }
//        throw new Exception();
//    }
//}