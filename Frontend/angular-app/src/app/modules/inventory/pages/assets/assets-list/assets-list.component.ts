import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { TableConfigurations} from '../../../../../core/components/custom/table/models/table-configurations';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { GetAssetsesQuery } from '../../../repositories/assets/queries/get-assetses-query';
import { Assets } from '../../../entities/Assets';
import { Commodity } from '../../../../commodity/entities/commodity';
import { PageModes } from '../../../../../core/enums/page-modes';
import { AssetsCommoditySerialDialog } from './assets-commodity-serial-dialog/assets-commodity-serial-dialog.component';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-assets-list',
  templateUrl: './assets-list.component.html',
  styleUrls: ['./assets-list.component.scss']
})

export class AssetsListComponent extends BaseComponent {

  @ViewChild('buttonEdit', { read: TemplateRef }) buttonEdit!: TemplateRef<any>;
  @ViewChild('buttonIsActive', { read: TemplateRef }) buttonIsActive!: TemplateRef<any>;
  @ViewChild('txtCommodity', { read: TemplateRef }) txtCommodity!: TemplateRef<any>;
  @ViewChild('buttonDocumentNo', { read: TemplateRef }) buttonDocumentNo!: TemplateRef<any>;

  Assets: Assets[] = [];

  tableConfigurations!: TableConfigurations;

  SearchForm = new FormGroup({
    fromDate: new FormControl(),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    commodityId: new FormControl(),
    assetSerial: new FormControl(),
    isActive: new FormControl(),
  });

  listActions: FormAction[] = [

  ]


  constructor(
    public _mediator: Mediator,
    private router: Router,
    public _notificationService: NotificationService,
    public dialog: MatDialog,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    await this.ApiCallService.getReceiptAllStatus('');

    let colEdit = new TableColumn(
      'colEdit',
      'ویرایش',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colIsActive = new TableColumn(
      'isActive',
      'فعال',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colDocumentNo = new TableColumn(
      'documentNo',
      'شماره رسید',
      TableColumnDataType.Template,
      '8%',
      true,
      new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)
    );
    let colCommodityCode = new TableColumn(
      'commodityCode',
      'کد کالا',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
    );
    colEdit.template = this.buttonEdit;
    colIsActive.template = this.buttonIsActive;
    colCommodityCode.template = this.txtCommodity;
    colDocumentNo.template = this.buttonDocumentNo;
    let columns: TableColumn[] = [

       new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      colDocumentNo,

      new TableColumn(
        'documentDate',
        'تاریخ',
        TableColumnDataType.Date,
        '5%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)

      ),
      new TableColumn(
        'assetSerial',
        'شماره اموال',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('assetSerial', TableColumnFilterTypes.Text)

      ),

      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
      ),

      colCommodityCode,

      new TableColumn(
        'assetGroupTitle',
        'گروه اموال',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('assetGroupTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commoditySerial',
        'شماره سریال',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('commoditySerial', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'warehousesTitle',
        'انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehousesTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'totalDescription',
        'شرح',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('totalDescription', TableColumnFilterTypes.Text)
      ),

      colIsActive,
      colEdit,

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();


  }

  async initialize() {

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
    if (this.SearchForm.controls.assetSerial.value != undefined && this.SearchForm.controls.assetSerial.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "assetSerial",
        values: [this.SearchForm.controls.assetSerial.value],
        comparison: "contains",
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
    if (this.SearchForm.controls.isActive.value != undefined && this.SearchForm.controls.isActive.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "isActive",
        values: [this.SearchForm.controls.isActive.value],
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

    let request = new GetAssetsesQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Assets = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }

  async navigateToRecive(Receipt: any) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.documentHeadId}&displayPage=archive`)
  }
  async navigateToHistory(ReceiptItem: any) {


    var url = `inventory/commodityReceiptReports?commodityId=${ReceiptItem.commodityId}&warehouseId=${ReceiptItem.warehouseId}`
    this.router.navigateByUrl(url)

  }
  async updateAssetSerial(Assets: any) {
    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      Assets: Assets,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(AssetsCommoditySerialDialog, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        this.get();
      }
    })

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
