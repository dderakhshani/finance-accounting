import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {FormControl} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {
  ImportMoadianInvoicesByExcelCommand
} from "../../../../../repositories/moadian/moadian-invoice-header/commands/import-moadian-invoices-by-excel-command";
import {
  ConvertRaahkaranSalaryExcelToVoucherDetailsCommand
} from "../../../../../repositories/voucher-head/commands/convert-raahkaran-salary-excel-to-voucherDetails-command";

@Component({
  selector: 'app-salary-excel-import-dialog',
  templateUrl: './salary-excel-import-dialog.component.html',
  styleUrls: ['./salary-excel-import-dialog.component.scss']
})
export class SalaryExcelImportDialogComponent  extends BaseComponent {
  pageModes = PageModes;

  fileName = '';

  isFileForFactoryEmployeesFormControl = new FormControl(false);

  employeeGroups = [
    {
      title:'کارکنان کارخانه',
      value:true
    },
    {
      title:'کارکنان دفتر مرکزی',
      value:false
    }
  ]

  constructor(
    private dialogRef: MatDialogRef<SalaryExcelImportDialogComponent>,
    private mediator: Mediator,
    @Inject(MAT_DIALOG_DATA) data : any
  ) {
    super();
  }


  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.initialize()
  }

  initialize(params?: any): any {
    this.request = new ConvertRaahkaranSalaryExcelToVoucherDetailsCommand()
  }

  async add(param?: any) {
    this.isLoading = true;
    let request = this.request as ConvertRaahkaranSalaryExcelToVoucherDetailsCommand;
    request.isFactory = this.isFileForFactoryEmployeesFormControl.value;
    await this.mediator.send(request).then(res => {
      this.isLoading = false;
      this.dialogRef.close(res)
    }).catch(() => {
      this.isLoading = false;
    })
  }
  onFileSelected(event: any): void {
    this.form.controls['excelFile'].setValue(event.target.files[0] ?? null);

    this.fileName = event.target.files[0].name ?? ''
  }

  update(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

}
