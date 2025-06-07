import { CommodityCategory } from "../../commodity/entities/commodity-category";
import { ReceiptAllStatusModel } from "./receipt-all-status";

export class Warehouse {
  public  id!: number;
  public parentId!: number ;
  public levelCode!: string ;
  public title!: string;
  public isActive!: boolean;
  public commodityCategoryId!: number;
  public accessPermission!: string;
  public accessPermissionTitle!: string;
  public accountRererenceGroupId!: number;
  public accountReferenceId !: number;
  public accountHeadId !: number;
  public sort !: number;
  public countable!:boolean;
  public commodityCategories: CommodityCategory[] = [];
  public receiptAllStatus: ReceiptAllStatusModel[] = [];

}
