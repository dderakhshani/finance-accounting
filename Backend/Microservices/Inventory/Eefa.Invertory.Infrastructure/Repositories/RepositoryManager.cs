using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IAccessToWarehouseRepository> _lazyAccessToWarehouseRepository;



        public IAccessToWarehouseRepository accessToWarehouseRepository => _lazyAccessToWarehouseRepository.Value;


       
        public IRepository<AssetAttachments> assetAttachmentsRepository => throw new NotImplementedException();

        public IRepository<Assets> assetsRepository => throw new NotImplementedException();

        public IRepository<DocumentHeadExtend> documentHeadExtendRepository => throw new NotImplementedException();

        public IRepository<PersonsDebitedCommodities> personsDebitedCommoditiesRepository => throw new NotImplementedException();

        public IRepository<AccessToWarehouse> quotaGroupRepository => throw new NotImplementedException();
    }
}
