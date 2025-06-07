import {MoadianInvoiceDetail} from "./moadian-invoice-detail";

export class MoadianInvoiceHeader {
  public id!: number;

  public isSandbox!:boolean;
  public personFullName!:string;
  public accountReferenceCode!:string;
  public customerCode!:string;

  public errors!: string;
  public statusTitle! : string;
  public referenceId!: string;
  public submissionDate!: Date;

  public taxId!: string;
  public indatim!: number;
  public indati2m!: number;
  public IntyTitle!: string;
  public inno!: string;
  public invoiceNumber!: string;
  public irtaxid!: string;
  public inpTitle!: string;
  public ins!: number;
  public tins!: string;
  public tobTitle!: string;
  public bid!: string;
  public tinb!: string;
  public sbc!: string;
  public bpc!: string;
  public bbc!: string;
  public ft!: number;
  public bpn!: string;
  public scln!: string;
  public scc!: string;
  public crn!: string;
  public billid!: string;
  public tprdis!: number;
  public tdis!: number;
  public tadis!: number;
  public tvam!: number;
  public todam!: number;
  public tbill!: number;
  public setm!: number;
  public cap!: number;
  public insp!: number;
  public tvop!: number;
  public tax17!: number;
  public cdcn!: string;
  public cdcd!: number;
  public tonw!: number;
  public torv!: number;
  public tocv!: number;

  public creator!: string;

  public moadianInvoiceDetails: MoadianInvoiceDetail[] = [];
}
