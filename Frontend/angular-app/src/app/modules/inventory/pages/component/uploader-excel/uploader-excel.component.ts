
import { HttpErrorResponse, HttpEvent, HttpEventType, HttpProgressEvent, HttpResponse } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UploadFileService } from 'src/app/core/services/upload-file.service';
import { spReportControlsWarehouseLayoutQuantitiesByTadbir } from '../../../entities/spGetDocumentItemForTadbir';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Warehouse } from '../../../entities/warehouse';
import { FormControl, FormGroup } from '@angular/forms';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import * as XLSX from 'xlsx';
import { UpdateWarehouseCommodityAvailableCommand } from '../../../repositories/warehouse-layout/commands/update-warehouse-commodity-available-command';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { GetWarehouseLayoutQuantitiesByTadbirQuery } from '../../../repositories/reports/get-warehouse-layout-quantities-by-tadbir';
import { IdentityService } from '../../../../identity/repositories/identity.service';
import { UserYear } from '../../../../identity/repositories/models/user-year';
import { UpdateWarehouseCommodityPriceCommand } from '../../../repositories/warehouse-layout/commands/update-commodity-price-command';
import { UpdateWarehouseLayoutALLCommand } from '../../../repositories/warehouse-layout/commands/update-warehouse-layout-all';
import { RemoveCommodityFromWarehouseCommand } from '../../../repositories/warehouse-layout/commands/remove-commodity-from-warehouse';

@Component({
  selector: 'app-uploader-excel',
  templateUrl: './uploader-excel.component.html',
  styleUrls: ['./uploader-excel.component.scss']
})
export class UploaderExcelComponent implements OnInit {

  @ViewChild('fileUpload', { static: false }) fileUpload!: ElementRef;
  @Input() autoUpload: boolean = true;
  @Input() allowFileUpload: boolean = true;

  @Input() singleFile: boolean = false;
  @Output() filesChange: EventEmitter<UploadFileData[]> = new EventEmitter<UploadFileData[]>();
  @Output() isUploadingChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() fileFilters: string = "image/x-png,image/jpeg,image/gif,xlsx"
  allowedYears: UserYear[] = [];
  SearchForm = new FormGroup({

    warehouseId: new FormControl(),
    yearId: new FormControl(),

  });

  isUploading: boolean = false;
   files:  UploadFileData={
    file: undefined,
     progressStatus: 'empty',
    progress: 0,
    filePath: '',
    fullFilePath: '',
     extention: '',
     id:0

  }
  fileName: string = '';

  response: spReportControlsWarehouseLayoutQuantitiesByTadbir[]=[]
  constructor(
    private uploadFileService: UploadFileService,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,
    private _mediator: Mediator,
    private identityService: IdentityService,

  ) {
    identityService._applicationUser.subscribe(res => {
      
        this.allowedYears = res.years;

        this.SearchForm.patchValue({
          yearId: +res.yearId,

        });
       
      
    });
  }
  async handleYearChange(yearId: number) {
   
    this.SearchForm.patchValue({
      yearId: yearId,

    });
  }
  ngOnInit(): void {


  }
  async ngOnChanges() {

  }
  ngAfterViewInit(): void {
    const fileUpload = this.fileUpload.nativeElement;
    fileUpload.onchange = () => {
      const file = fileUpload.files[0];


      const fileData = <UploadFileData>{
        file: file,
        progressStatus: 'empty',
        progress: 0,
        filePath: '',
        fullFilePath: '',
        extention: file.name.split(".")[1].toUpperCase(),

      }
      if (FileReader) {
        var fr = new FileReader();
        fr.onload = function () {
          fileData.fullFilePath = fr.result;
        }
        fr.readAsDataURL(file);
      }


      if (this.autoUpload)
        this.uploadFile(fileData);
    };


  }

  onClickUpload() {
    const fileUpload = this.fileUpload.nativeElement;
    fileUpload.click();
  }

  

  uploadFile(data: UploadFileData) {

    if (this.SearchForm.controls.warehouseId.value == undefined) {
      this.Service.showHttpFailMessage('انبار مورد نظر را انتخاب نمایید');
      return;
    }

    let formData = new FormData();
    formData.append('file', data.file);
    formData.append('warehouseId', this.SearchForm.controls.warehouseId.value);
    formData.append('yearId', this.SearchForm.controls.yearId.value);

    this.isUploadingChange.emit(true);
    this.files.progressStatus = 'in-progress';
    this.fileName = data.file.name;
    this._notificationService.isLoader = true;

    this.uploadFileService.uploadInventory(formData).pipe(map((event: HttpEvent<any>) => {

      switch (event.type) {
        case HttpEventType.UploadProgress:

          this.files.progress = Math.round(
            (event.loaded * 100) / (event.total ?? 1)
          );
          break;
        case HttpEventType.Response:
          let Response = event.body?.objResult;
          this.files.progressStatus = 'uploaded';
          this.response = Response;
          if (this.response.length ==0) {
            this.Service.showWarrningMessage('هیچ مورد مغایرتی  بین سیستم تدبیر و دانا یافت نشد')
          }
          return Response;


      }
      this._notificationService.isLoader = false;
    }),
      catchError((error: HttpErrorResponse) => {
        this._notificationService.isLoader = false;
        this.files.progressStatus = 'failed';
        return of(`${data.file.name} upload failed.`);

      })
    )
      .subscribe((event: any) => {
        this._notificationService.isLoader = false;
        if (typeof event === 'object') {
        }
      });



  }
  exportexcel(): void {
    var fileName = 'ExcelSheet.xlsx';
    /* pass here the table id */
    let element = document.getElementById('table-tadbir-Quantity');


    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    /* save to file */
    XLSX.writeFile(wb, fileName);

  }

  isImageFile(file: UploadFileData) {
    return ['PNG', 'JPG', 'JPEG', 'XLS', 'xlsx'].includes(file.extention.toUpperCase());
  }

  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  add() {
    window.location.reload();
  }

  async repair() {
    var request = new UpdateWarehouseCommodityAvailableCommand()
    request.warehouseId = this.SearchForm.controls.warehouseId.value;
    request.yearId = this.SearchForm.controls.yearId.value;
    let response = await this._mediator.send(<UpdateWarehouseCommodityAvailableCommand>request);
  }
  async repairLayout() {
    var request = new UpdateWarehouseLayoutALLCommand()
    request.warehouseId = this.SearchForm.controls.warehouseId.value;
   
    let response = await this._mediator.send(<UpdateWarehouseLayoutALLCommand>request);
  }
  async removeCommodityFromWarehouse() {
    var request = new RemoveCommodityFromWarehouseCommand()
    request.warehouseId = this.SearchForm.controls.warehouseId.value;

    let response = await this._mediator.send(<RemoveCommodityFromWarehouseCommand>request);
  }
  
  async updatePrice() {
    var request = new UpdateWarehouseCommodityPriceCommand()
    request.warehouseId = this.SearchForm.controls.warehouseId.value;
    request.yearId = this.SearchForm.controls.yearId.value;
    let response = await this._mediator.send(<UpdateWarehouseCommodityPriceCommand>request);
  }

  async get() {
    this.response = await this._mediator.send(new GetWarehouseLayoutQuantitiesByTadbirQuery(this.SearchForm.controls.warehouseId.value, this.SearchForm.controls.yearId.value))
  }
}

export interface UploadFileData {
  file: any;
  progressStatus: '' | 'in-progress' | 'uploaded' | 'failed' | 'empty';
  progress: number;
  filePath: string;
  fullFilePath: any;
  extention: string;
  id: number;
}

