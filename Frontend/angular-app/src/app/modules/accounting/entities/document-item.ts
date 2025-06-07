import {Commodity} from "./commodity";

export class DocumentItem {
  public id!: number
  public documentHeadId!: number
  public commodityId!: number
  public commoditySerial!: number
  public unitPrice!: number
  public productionCost!: number
  public currencyBaseId!: number
  public currencyPrice!: number
  public quantity!: number
  public weight!: number
  public discount!: number

  public commodity!:Commodity
}
