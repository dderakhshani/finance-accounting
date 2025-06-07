using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Common.Common.Abstraction
{
    public interface ITejaratBankServices
    {
        string SetDepositId(string inputNumber);
        Task<ServiceResult<ResponseTejaratModel>> CallGetCreditAccountBalance(string fromPersianDate, string toPersianDate, string accountNumber, string creditDebit);
        Task<ServiceResult<ResponseTejaratModel>> CallGetAccountBalance(string fromPersianDate, string toPersianDate, string accountNumber);

    }
}
