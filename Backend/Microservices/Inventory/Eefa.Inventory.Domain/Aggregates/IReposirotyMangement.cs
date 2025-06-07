using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Inventory.Domain.Aggregates
{
    public interface IRepositoryManager
    {
        IAccessToWarehouseRepository accessToWarehouseRepository { get; }
        IRepository<AssetAttachments> assetAttachmentsRepository { get; }
        IRepository<Assets> assetsRepository { get; }
        IRepository<DocumentHeadExtend> documentHeadExtendRepository { get; }
        IRepository<PersonsDebitedCommodities> personsDebitedCommoditiesRepository { get; }
        IRepository<AccessToWarehouse> quotaGroupRepository { get; }
       


    }
}
