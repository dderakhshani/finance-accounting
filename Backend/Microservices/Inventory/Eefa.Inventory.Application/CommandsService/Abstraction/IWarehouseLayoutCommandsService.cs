using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IWarehouseLayoutCommandsService:IQuery
    {
        //---------------------ثبت تاریخچه------------------------------------
        Task<int> InsertAndUpdateWarehouseHistory(int CommodityId, double Quantity, int WarehouseLayoutId, int? documentItemId, int historyMode);
        //Task<int> InsertWarehouseHistory(int CommodityId, double Quantity, int WarehouseLayoutId, int? documentItemId, int historyMode);
        //---------------------تغییر ظرفیت فعلی در مکان-----------------------
        Task<int> InsertLayoutQuantity(int CommodityId, double Quantity, int historyMode, WarehouseLayoutQuantity warehouseLayoutQuantity, int WarehouseLayoutId);

        Task<int> InsertStock(int WarehouseId, int CommodityId, double Quantity, int historyMode, WarehouseStocks stock);
        Task<int> DeleteWarehouseHistory(int? documentItemId, int CommodityId);
        Task<WarehouseLayout> FindLayout(int warehouseId, int CommodityId);
    }
}

