export class TableExportOptions {
  public exportTypes: string[] = ["Excel", "CSV"];
  public showExportButton: boolean = true;

  public customExportButtonTitle: string = '';
  public customExportCallbackFn!: Function;
}
