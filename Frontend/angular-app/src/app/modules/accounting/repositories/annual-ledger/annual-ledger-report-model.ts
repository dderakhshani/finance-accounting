export class AnnualLedgerReportModel {
    beforeDebit: number = 0;
    beforeCredit: number = 0;
    sumDebit: number = 0;
    sumCredit: number = 0;
    datas: AnnualLedgerData[] = [];

}
export class AnnualLedgerData {
    level2Code: string = "";
    level2Title: string = "";
    debit: number = 0;
    credit: number = 0;

}