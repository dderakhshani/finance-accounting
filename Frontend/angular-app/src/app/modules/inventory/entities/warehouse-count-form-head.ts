import { CommodityCategory } from "../../commodity/entities/commodity-category";
import { ReceiptAllStatusModel } from "./receipt-all-status";

export class WarehouseCountFormHead {
  public  id!: number;
  public parentId!: number ;
  public formDate!: string ;
  public warehouseId!: number ;
  public warehouseLayoutTitle!: string;
  public counterUserName!: string;
  public confirmerUserName!:string;
  public counterUserId!: number;
  public ConfirmerUserId!: number;
  public description!: string;
  public formState!: number;
  public formStateMessage!:string
}

export class CountFormSelectedCommodity{
  public warehouseLayoutId: number | undefined = undefined;
  public warehouseLayoutQuantityId: number | undefined = undefined;
}
