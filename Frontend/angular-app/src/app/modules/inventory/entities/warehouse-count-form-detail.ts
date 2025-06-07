import { CommodityCategory } from "../../commodity/entities/commodity-category";
import { ReceiptAllStatusModel } from "./receipt-all-status";

export class WarehouseCountFormDetail {
  public id!:number;
  public warehouseLayoutQuantitiesId!: number ;
  public lastWarehouseLayoutStatus!: number ;
  public countedQuantity!: number ;
  public description!: string;
  public commodityName!:string;
  public CommodityId!: number;
  public systemQuantity!: number;
  public conflictQuantity!: number;
  public warehouseLayoutTitle!: string;
  public commodityCompactCode!:string;
  public commodityCode!:string;
  public measureTitle!:string;
}
