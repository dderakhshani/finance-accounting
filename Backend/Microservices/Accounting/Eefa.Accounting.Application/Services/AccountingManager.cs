using Eefa.Accounting.Application.Common.Interfaces;
using Eefa.Accounting.Application.Services.Models;
using Library.Interfaces;
using Library.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Services
{
    public class AccountingManager : IAccountingManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IRepository _repository;
        private AccountingSettings accountingSettings;
        public AccountingManager(IMemoryCache memoryCache,
            ICurrentUserAccessor currentUser,
            IRepository repository)
        {
            this._memoryCache = memoryCache;
            this._currentUser = currentUser;
            this._repository = repository;

            accountingSettings = this._memoryCache.Get<AccountingSettings>(nameof(AccountingSettings));
            if (accountingSettings == null) { accountingSettings = new AccountingSettings(); }
        }

        private void UpdateCachedSettings()
        {
            this._memoryCache.Set(nameof(AccountingSettings), accountingSettings);
        }


        public async Task<ServiceResult> SetVoucherHeadAsBeingModified(int voucherHeadId)
        {
            var voucherHeadBeingModified = this.accountingSettings.VoucherHeadsBeingModified.FirstOrDefault(x => x.Id == voucherHeadId);
            if (voucherHeadBeingModified != null)
            {
                if (voucherHeadBeingModified.ModifierId == this._currentUser.GetId())
                {
                    voucherHeadBeingModified.LockDueDate = DateTime.UtcNow.AddMinutes(5);
                    this.UpdateCachedSettings();
                    return ServiceResult.Success();
                }
                else
                {
                    return ServiceResult.Failure(message: $"سند شماره {voucherHeadBeingModified.Id} توسط {voucherHeadBeingModified.ModifierName} در حال ویرایش است.");
                }

            }
            else
            {
                voucherHeadBeingModified = new VoucherHeadBeingModified
                {
                    Id = voucherHeadId,
                    ModifierId = _currentUser.GetId(),
                    ModifierName = _currentUser.GetFullName(),
                    LockDueDate = DateTime.UtcNow.AddMinutes(5),
                };
                this.accountingSettings.VoucherHeadsBeingModified.Add(voucherHeadBeingModified);
                this.UpdateCachedSettings();
                return ServiceResult.Success();
            }
        }


    }
}
