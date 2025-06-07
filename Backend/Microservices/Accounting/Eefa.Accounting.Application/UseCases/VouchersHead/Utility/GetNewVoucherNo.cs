using System;
using System.Linq;
using System.Threading.Tasks;
using Library.Interfaces;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Utility
{
    public class VoucherNo
    {
        public static async Task<int> GetNewVoucherNo(IRepository repository,ICurrentUserAccessor currentUserAccessor,DateTime voucherDate, int? requestVoucherNo)
        {
            if (!repository.GetQuery<Data.Entities.VouchersHead>().Any())
            {
                return 1;
            }
            var settings =
              await new SystemSettings(repository).Get(SubSystemType.AccountingSettings);
            var voucherNumberType = new int();

            foreach (var baseValue in settings)
            {
                if (baseValue.UniqueName == "VoucherNumberType")
                {
                    voucherNumberType = int.Parse(baseValue.Value);
                    break;
                }
            }


            if (!string.IsNullOrEmpty(requestVoucherNo.ToString()))
            {
                if (voucherNumberType == 2)
                {
                    if (!requestVoucherNo.ToString().StartsWith(currentUserAccessor.GetBranchId().ToString()))
                    {
                        throw new Exception("Wrong voucherNo");
                    }
                }
            }
            else
            {
                var voucherAndDateCompatibility = false;
                foreach (var baseValue in settings)
                {
                    if (baseValue.UniqueName == "VoucherAndDateCompatibility")
                    {
                        voucherAndDateCompatibility = bool.Parse(baseValue.Value);
                        break;
                    }
                }

                if (voucherAndDateCompatibility)
                {
                    if (voucherNumberType == 2)
                    {
                        return repository.GetQuery<Data.Entities.VouchersHead>()
                            .Where(x => x.VoucherDate.Year == voucherDate.Year &&
                                        x.VoucherNo.ToString()
                                            .StartsWith(currentUserAccessor.GetBranchId().ToString()))
                            .Select(x => x.VoucherNo).Max() + 1;
                    }
                    else
                    {
                        return repository.GetQuery<Data.Entities.VouchersHead>()
                            .Where(x => x.VoucherDate.Year == voucherDate.Year)
                            .Select(x => x.VoucherNo).Max() + 1;
                    }

                }
                else
                {
                    if (voucherNumberType == 2)
                    {
                        return repository.GetQuery<Data.Entities.VouchersHead>()
                            .Where(x => x.VoucherDate.Year == voucherDate.Year &&
                                        x.VoucherNo.ToString()
                                            .StartsWith(currentUserAccessor.GetBranchId().ToString()))
                            .Select(x => x.VoucherNo).Max() + 1;
                    }
                    else
                    {
                        return repository.GetQuery<Data.Entities.VouchersHead>()
                            .Select(x => x.VoucherNo).Max() + 1;
                    }
                }
            }

            throw new Exception();
        }
    }
}