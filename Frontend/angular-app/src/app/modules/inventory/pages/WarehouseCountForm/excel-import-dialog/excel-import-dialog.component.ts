import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {UpdateBulkWarehouseCountQuantityCommand} from "../../../repositories/warehouse-count-form/commands/update-balk-warehouse-count-quantity";
import {UpdateWarehouseCountQuantity} from "../../../entities/update-warehouse-count-quantity";
import {  GetWarehouseCountFormDetailQuery} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-count-form-detail-query";
import {WarehouseCountFormDetail} from "../../../entities/warehouse-count-form-detail";
import {Toastr_Service} from "../../../../../shared/services/toastrService/toastr_.service";
import {Router} from "@angular/router";
import * as XLSX from 'xlsx';
@Component({
  selector: 'app-excel-import-dialog',
  templateUrl: './excel-import-dialog.component.html',
  styleUrls: ['./excel-import-dialog.component.scss']
})
export class ExcelImportDialogComponent  extends BaseComponent {
  pageModes = PageModes;
  fileName = '';
  headerId:number;
  warehouseCountDetail:WarehouseCountFormDetail[]=[];
  countedExcelList: UpdateWarehouseCountQuantity[]=[];

  constructor(
    private _dialogRef: MatDialogRef<ExcelImportDialogComponent>,
    private _mediator: Mediator,
    private _toaster:Toastr_Service,
    @Inject(MAT_DIALOG_DATA) data : any
  ) {
    super();
    this.headerId=data.headerId
  }
  async ngOnInit() {
    await this.resolve()
  }
  async resolve() {
    this.initialize()
  }
  initialize(params?: any): any {
   this.request=new UpdateBulkWarehouseCountQuantityCommand();
   this.getDetail();
  }
  async add(param?: any) {
    this.isLoading = true;
    let ids = this.countedExcelList.map((item: any) => item.id);
    let detailSet = new Set(this.warehouseCountDetail.map((item: any) => item.id)); // Set فقط از id ها ساخته شود

    const containsAll = ids.every((id: any) => detailSet.has(id));
    if (!containsAll) {
      this.isLoading=false;
      this._dialogRef.close();
      return this._toaster.showError('دیتای فایل اکسل آپلود شده صحیح نمی باشد.');
    }
    let request = this.request as UpdateBulkWarehouseCountQuantityCommand;
    request.warehouseCountedQuantities=this.countedExcelList;
    await this._mediator.send(request).then(res => {
      this.isLoading = false;
      this._dialogRef.close(res);

    }).catch(() => {
      this.isLoading = false;

    })
  }
  onFileSelected(event: any): void {
    let target: DataTransfer = <DataTransfer>event.target;
    if (target.files.length !== 1) {
      console.error('Cannot use multiple files');
      return;
    }
    const reader: FileReader = new FileReader();
    reader.onload = (e: any) => {
      const binaryStr: string = e.target.result;
      const workbook: XLSX.WorkBook = XLSX.read(binaryStr, { type: 'binary' });

      const sheetName: string = workbook.SheetNames[0]; // Read first sheet
      const sheet: XLSX.WorkSheet = workbook.Sheets[sheetName];
      let excelData = XLSX.utils.sheet_to_json(sheet); // Convert sheet to JSON

      this.countedExcelList= excelData.map((item:any)=>({
            id: item['شماره'],
            countedQuantity: item['موجودی شمارش شده'],
            description:item['توضیحات']
          }))
     // @ts-ignore
      this.countedExcelList = this.countedExcelList.filter(x=>x.countedQuantity!='')
    };
    reader.readAsBinaryString(target.files[0]);
  }
 getDetail(){
  let request = new GetWarehouseCountFormDetailQuery()
  if(this.headerId)
    request.warehouseCountFormHeadId = this.headerId;
    this._mediator.send(request).then(response => {this.warehouseCountDetail=response.data;
})
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
