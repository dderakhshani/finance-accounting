import { Component, HostListener, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from "@angular/router";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { WarehouseLayoutsCommodity } from '../../../entities/warehouse-layouts-commodity';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { FormControl, FormGroup } from '@angular/forms';
import { GetWarhouseLayoutHistotyByDocumentQuery } from '../../../repositories/warehouse-layout/queries/warhouse-layouts-report/get-warehouse-layouts-document-history-query';
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { Commodity } from '../../../../accounting/entities/commodity';
import { GetReceiptALLBaseValueQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-all-base-value-query';
import { BaseValueModel } from '../../../entities/base-value';

@Component({
  selector: 'app-daily-entry-to-warehouse-list',
  templateUrl: './daily-entry-to-warehouse-list.component.html',
  styleUrls: ['./daily-entry-to-warehouse-list.component.scss']
})
export class dailyEntryToWarehouseListComponent extends BaseComponent {
  @ViewChild('buttonDocumentNo', { read: TemplateRef }) buttonDocumentNo!: TemplateRef<any>;
  @ViewChild('txtDocumentStateBaseTitle', { read: TemplateRef }) txtDocumentStateBaseTitle!: TemplateRef<any>;

  ReceiptBaseValue: BaseValueModel[] = [];
  WarehouseLayouts: any[] = [];

  tableConfigurations!: TableConfigurations;
  SearchForm = new FormGroup({
    mode: new FormControl(),
    requestNo: new FormControl(),
    documentNo: new FormControl(),
    commodityId: new FormControl(),
    warehouseId: new FormControl(),
    documentStateBaseId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),

  });
  ToggleForPrint: boolean = false;
  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]

  constructor(public _mediator: Mediator,
    private router: Router,
    private sanitizer: DomSanitizer,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,

  ) {
    super();
  }

  async ngOnInit() {

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve() {
    let colDocumentStateBaseTitle = new TableColumn(
      'documentStateBaseTitle',
      'وضعیت',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('documentStateBaseTitle', TableColumnFilterTypes.Text)
    );
    colDocumentStateBaseTitle.template = this.txtDocumentStateBaseTitle;
    let columns: TableColumn[] = [
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'selected',
        'انتخاب',
        TableColumnDataType.Select,
        '5%',
        true,
        new TableColumnFilter('selected', TableColumnFilterTypes.Select)

      ),
      new TableColumn(
        'requestNo',
        'شماره درخواست',
        TableColumnDataType.Number,
        '8%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'documentNo',
        'شماره سند',
        TableColumnDataType.Number,
        '8%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'documentDate',
        'تاریخ سند',
        TableColumnDataType.Date,
        '8%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)

      ),
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'warehouseLayoutTitle',
        'مکان در انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseLayoutTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'quantity',
        'تعداد',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'measureTitle',
        'واحد',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'modeTitle',
        'ورودی/خروجی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('modeTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'warehousesTitle',
        'عنوان انبار',
        TableColumnDataType.Text,
        '12%',
        true,
        new TableColumnFilter('warehousesTitle', TableColumnFilterTypes.Text)

      ),
      colDocumentStateBaseTitle
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------

    await this.get();
    await this.getReceiptBaseValue();

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  initialize() {
  }
  async get() {
    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }

    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseId",
        values: [this.SearchForm.controls.warehouseId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.documentNo.value != undefined && this.SearchForm.controls.documentNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.documentNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.commodityId.value != undefined && this.SearchForm.controls.commodityId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "commodityId",
        values: [this.SearchForm.controls.commodityId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.documentStateBaseId.value != undefined && this.SearchForm.controls.documentStateBaseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentStateBaseId",
        values: [this.SearchForm.controls.documentStateBaseId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.mode.value != undefined && this.SearchForm.controls.mode.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "mode",
        values: [this.SearchForm.controls.mode.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    let request = new GetWarhouseLayoutHistotyByDocumentQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.WarehouseLayouts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }
  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }
  async print() {
    if (this.SearchForm.controls.warehouseId.value == undefined) {
      this.Service.showHttpFailMessage("انبار مورد نظر را انتخاب نمایید و جستجو فرمایید");
      return;
    }
    if (this.WarehouseLayouts.filter(a => a.selected).length == 0) {
      this.Service.showHttpFailMessage("لیست رسیدهاجهت چاپ را انتخاب نمایید");
      return;
    }
    let printContents = '';
    let index = 1;
    if (this.WarehouseLayouts.filter(a => a.selected).length > 0) {

      printContents += `
                     <table id='table' >
                      <tr id="header">
                        <td colspan="3"  style="width:25% ;text-align:center;border-left:unset" ">
                          <img src="/assets/images/logo.png" />
                        </td>
                        <td colspan="4" style="width:50%;text-align:center" ">
                          فرم قطعات تحویلی روزانه به
                          <b>
                          ${this.WarehouseLayouts[0].warehousesTitle}
                          </b>
                        </td>
                        <td colspan="2"  style="width: 25% ;text-align:right; border-right: unset" ">
                          <p>
                            تاریخ سند ${this.Service.toPersianDate(this.WarehouseLayouts[0].documentDate)}
                          </p>
                        </td>
                      </tr>
                     <tr>
                       <th>ردیف</th>
                       <th>شماره درخواست</th>
                        <th>کد کالا</th>
                        <th>محل جایگذاری</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>واحد</th>
                       <th>تعداد شمارش شده</th>
                        <th>توضیحات</th>
                     </tr>
                   <tbody>`;
      this.WarehouseLayouts.filter(a => a.selected).map(data => {
        printContents += `<tr>
                           <td>${index}</td>
                           <td>${data.requestNo}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.warehouseLayoutTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.measureTitle}</td>
                           <td></td>
                           <td></td>
                        </tr>`;
        index++;
      })
      printContents += `</tbody></table>
       <div style="margin-top:15px; width:100%;display:flex;text-align:center;font-size:14px;border:solid;border-width:1px;margin-bottom: 5px;">
        <div style = "width: 50%;height:35px;text-align:start;  padding:15px;" >
            <b>کد پرسنلی و امضا تحویل دهنده : </b>
        </div >
        <div style = "width: 50%;height:35px;text-align:start; padding:15px;" >
            <b>کد پرسنلی و امضا تحویل گیرنده : </b>
        </div >
       </div>
       <table style="width: 100%; font-size: 10px">
           <tr>
             <td style="width: 50%; padding: 10px">
                تاریخ بازنگری :99/06/22
            </td>
             <td style="width: 50%; text-align: left; padding: 10px">
                ST-F80/00
            </td>
        </tr>
        </table>
       `
      this.Service.onPrint(printContents, '')
    }
  }
  //پرینت جهت بازرگانی
  async CommercePrint() {
    if (this.WarehouseLayouts.filter(a => a.selected).length==0) {
      this.Service.showHttpFailMessage("لیست رسیدهاجهت چاپ را انتخاب نمایید");
      return;
    }
    let printContents = '';
    if (this.WarehouseLayouts.filter(a => a.selected).length > 0) {

      printContents += `<table  id='table' ><thead>
                     <tr>
                     <th colspan="11">
                      ${this.Service.toPersianDate(this.WarehouseLayouts[0].documentDate)}
                     </th>
                     </tr>
                     <tr>
                       <th>شماره سند</th>
                       <th>تاریخ سند</th>
                       <th>انبار</th>
                       <th>تامین کننده</th>
                       <th>کد کالا</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>واحد</th>
                       <th>ملاحضات</th>
                       <th>شماره درخواست</th>
                     </tr>
                   </thead><tbody>`;
      this.WarehouseLayouts.filter(a => a.selected).map(data => {
        printContents += `<tr>
                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.warehousesTitle}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.measureTitle}</td>
                           <td></td>
                           <td>${data.requestNo}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'ورودی روزانه انبار')
    }
  }
  //------------بدست آوردن حالت های مختلف تایید یک رسید--------------------
  async getReceiptBaseValue() {
    let request = new GetReceiptALLBaseValueQuery()
    let response = await this._mediator.send(request);
    this.ReceiptBaseValue = response.data;
  }
  async update() {
  }

  async add() {
  }

  close(): any {
  }

  delete(): any {
  }


}
