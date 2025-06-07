import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';
import { GetjaratReportImportQuery } from '../../../../repositories/reports/get-tejarat-reports-query';
import { FormControl, FormGroup } from '@angular/forms';
import { spTejaratReportImportCommodity } from '../../../../entities/spTejaratReportImportCommodity';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import * as XLSX from 'xlsx';
@Component({
  selector: 'app-export-to-excel-tejarat-system',
  templateUrl: './export-to-excel-tejarat-system.component.html',
  styleUrls: ['./export-to-excel-tejarat-system.component.scss']
})
export class ExportToExcelTejaratSystemComponent extends BaseComponent {

  ButtonId: number = 0;
  responce: spTejaratReportImportCommodity[]= [];
  
  SearchForm = new FormGroup({
    date: new FormControl(new Date(new Date().setHours(0, 0, 0, 0))),
   
  });
  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    private _mediator: Mediator,
    public ApiCallService: ApiCallService,
  ) {
    super(route, router);

  }
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
   
    await this.initialize()
  }


  async initialize(entity?: any) {



  }

  
  async get(buttonId: number) {

    this.ButtonId = buttonId;
    this.responce = [];
    
    await this._mediator.send(new GetjaratReportImportQuery(this.Service.formatDate(this.SearchForm.controls['date']?.value), this.ButtonId)).then(async (res) => {
      this.responce = res;
    })

  }
  exportexcel(): void {
    var fileName = 'ExcelSheet.xlsx';
    /* pass here the table id */
    let element = document.getElementById('excel-table');


    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    /* save to file */
    XLSX.writeFile(wb, fileName);

  }


  async reset() {

  }


  async update() {

  }
  delete() {


  }
  async edit() {
  }
  close(): any {
  }
  add(param?: any) {

  }

 
  

}
