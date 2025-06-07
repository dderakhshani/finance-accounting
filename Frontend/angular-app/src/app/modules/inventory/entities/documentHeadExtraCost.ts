export class DocumentHeadExtraCost {
  public id !: number;
  public documentHeadId!: number;
  public extraCostAccountHeadId!: number;
  public extraCostAccountReferenceGroupId!: number | null;
  public extraCostAccountReferenceId!: number | null;
  public extraCostAccountHeadTitle!: string;
  public extraCostAccountReferenceGroupTitle!: string;
  public extraCostAccountReferenceTitle!: string;
  public extraCostAmount!: number | null;
  public extraCostDescription!: string;
  public extraCostCurrencyTypeBaseId!: number | null;
  public extraCostCurrencyFee!: number | null;
  public extraCostCurrencyAmount!: number | null;
  public financialOperationNumber!: string;
  public barCode!: number | null;
  public isDeleted: boolean = false;
}
