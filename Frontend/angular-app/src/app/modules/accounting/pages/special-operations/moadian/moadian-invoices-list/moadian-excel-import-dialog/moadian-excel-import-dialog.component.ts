import {Component, Inject, OnInit} from '@angular/core';
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {
  CreateMoadianInvoiceDetailCommand
} from "../../../../../repositories/moadian/moadian-invoice-detail/commands/create-moadian-invoice-detail-command";
import {
  UpdateMoadianInvoiceDetailCommand
} from "../../../../../repositories/moadian/moadian-invoice-detail/commands/update-moadian-invoice-detail-command";
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {
  ImportMoadianInvoicesByExcelCommand
} from "../../../../../repositories/moadian/moadian-invoice-header/commands/import-moadian-invoices-by-excel-command";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {FormControl} from "@angular/forms";

@Component({
  selector: 'app-moadian-excel-import-dialog',
  templateUrl: './moadian-excel-import-dialog.component.html',
  styleUrls: ['./moadian-excel-import-dialog.component.scss']
})
export class MoadianExcelImportDialogComponent extends BaseComponent {
  pageModes = PageModes;

  fileName = '';

  isProduction = false;
  generateTaxIdWithListNumber = new FormControl(false);

  taxIdGenerateMethods = [
    {
      title:'بر اساس شماره صورتحساب',
      value:false
    },
    {
      title:'بر اساس شماره صورتحساب و شماره لیست',
      value:true
    }
  ]
  constructor(
    private dialogRef: MatDialogRef<MoadianExcelImportDialogComponent>,
    private mediator: Mediator,
    @Inject(MAT_DIALOG_DATA) data : any
  ) {
    super();
    this.isProduction = data.isProduction;
  }


  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.initialize()
  }

  initialize(params?: any): any {
    this.request = new ImportMoadianInvoicesByExcelCommand()
  }

  async add(param?: any) {
    this.isLoading = true;
    let request = this.request as ImportMoadianInvoicesByExcelCommand;
    request.isProduction = this.isProduction;
    request.generateTaxIdWithListNumber = this.generateTaxIdWithListNumber.value;
    await this.mediator.send(request).then(res => {
      this.isLoading = false;
      this.dialogRef.close(true)
    }).catch(() => {
      this.isLoading = false;
    })
  }
  onFileSelected(event: any): void {
    this.form.controls['file'].setValue(event.target.files[0] ?? null);

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
