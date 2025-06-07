using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Accounting.Data.Entities;
using Library.Interfaces;
using LinqToDB;

namespace Eefa.Accounting.Application
{
    public enum SubSystemType
    {
        AccountingSettings,
        StoreSetting,
        AdminSetting
    }
    public class SystemSettings
    {
        private readonly IRepository _repository;

        public SystemSettings(IRepository repository)
        {
            _repository = repository;
        }


        public async Task<ICollection<BaseValue>> Get(SubSystemType subSystemType)
        {
            //Get id of base value type witch is represent the system setting

            return await _repository.GetAll<BaseValue>(x =>
                    x.ConditionExpression(t =>
                        t.BaseValueType.UniqueName == subSystemType.ToString()).AsNoTraking(true))
                .ToListAsync();
        }
    }
}