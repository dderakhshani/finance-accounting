import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { PageModes } from "../../../../core/enums/page-modes";

import {
  TableConfigurations
} from '../../../../core/components/custom/table/models/table-configurations';

import { FormAction } from '../../../../core/models/form-action';
import { FormActionTypes } from '../../../../core/constants/form-action-types';
import { Router } from '@angular/router';
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import { SearchQuery } from '../../../../shared/services/search/models/search-query';
import { PersonsDebitedCommodities } from '../../entities/persons-debited-commodities';
import {
  GetPersonsDebitedCommoditiesQuery
} from '../../repositories/persons-debited-commodities/queries/get-persons-debited-commodities-query';
import { FormControl, FormGroup } from '@angular/forms';
import {
  UpdateReturnToWarehousePersonsDebitedDialogComponent
} from './return-to-warehouse-persons-debited-commodities-dialog/return-to-warehouse-persons-debited-commodities-dialog.component';

import { Commodity } from '../../../commodity/entities/commodity';
import {
  UpdateNewPersonsDebitedDialogComponent
} from './return-new-person-persons-debited-commodities-dialog/return-new-persons-debited-commodities-dialog.component';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import { UpdateAttachmentAssetsDialogComponent } from './asset-attachments-dialog/asset-attachments-dialog.component';
import { UpdateBarcodeDialogComponent } from './update-barcode-dialog/update-barcode-dialog.component';


@Component({
  selector: 'app-persons-debited-commodities-list',
  templateUrl: './persons-debited-commodities-list.component.html',
  styleUrls: ['./persons-debited-commodities-list.component.scss']
})
export class PersonsDebitedCommodityListComponent extends BaseComponent {
  update(param?: any) {
    throw new Error('Method not implemented.');
  }



  @ViewChild('buttonRetturnToWarehouse', { read: TemplateRef }) buttonRetturnToWarehouse!: TemplateRef<any>;
  @ViewChild('buttonAassignment', { read: TemplateRef }) buttonAassignment!: TemplateRef<any>;
  @ViewChild('buttonIsHaveWast', { read: TemplateRef }) buttonIsHaveWast!: TemplateRef<any>;
  @ViewChild('buttonAttachment', { read: TemplateRef }) buttonAttachment!: TemplateRef<any>;
  @ViewChild('buttonIsActive', { read: TemplateRef }) buttonIsActive!: TemplateRef<any>;
  @ViewChild('buttonBarcode', { read: TemplateRef }) buttonBarcode!: TemplateRef<any>;


  PersonsDebitedCommodities: PersonsDebitedCommodities[] = [];
  tableConfigurations!: TableConfigurations;
  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  SearchForm = new FormGroup({
    fromDate: new FormControl(),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    assetSerial: new FormControl(),
    bebitAccountReferenceId: new FormControl(),
    isActive: new FormControl(),
    commodityId: new FormControl(),
  });

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public _notificationService: NotificationService,

    public dialog: MatDialog,
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
    let colAassignment = new TableColumn(
      'colAassignment',
      'فعالیت ها',
      TableColumnDataType.Template,
      '15%',
      false
    );

    let colIsHaveWast = new TableColumn(
      'isHaveWast: ',
      'داغی دارد؟',
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
    colAassignment.template = this.buttonAassignment;

    colIsHaveWast.template = this.buttonIsHaveWast;
    colIsActive.template = this.buttonIsActive;

    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'documentNo',
        'شماره حواله',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)
      ),
      //new TableColumn(
      //  'fullName',
      //  'نام',
      //  TableColumnDataType.Text,
      //  '10%',
      //  true,
      //  new TableColumnFilter('fullName', TableColumnFilterTypes.Text)
      //),

      //new TableColumn(
      //  'unitsTitle',
      //  ' واحد سازمانی',
      //  TableColumnDataType.Text,
      //  '10%',
      //  true,
      //  new TableColumnFilter('unitsTitle', TableColumnFilterTypes.Text)
      //),
      new TableColumn(
        'assetSerial',
        ' شماره اموال',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('assetSerial', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'debitReferenceTitle',
        'طرف بدهکار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('debitReferenceTitle', TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'measureTitle',
        'واحد کالا',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text)
      )
      ,
      new TableColumn(
        'quantity',
        ' تعداد',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      ),

      new TableColumn(
        'documentDate',
        'تاریخ دریافت',
        TableColumnDataType.Date,
        '5%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'expierDate',
        'تاریخ انقضا',
        TableColumnDataType.Date,
        '5%',
        true,
        new TableColumnFilter('expierDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'commoditySerial',
        'شماره سریال',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('commoditySerial', TableColumnFilterTypes.Text)
      ),
      //new TableColumn(
      //  'depreciationTitle',
      //  'شرح',
      //  TableColumnDataType.Text,
      //  '10%',
      //  true,
      //  new TableColumnFilter('depreciationTitle', TableColumnFilterTypes.Text)
      //),
      new TableColumn(
        'totalDescription',
        'شرح درخواست',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('totalDescription', TableColumnFilterTypes.Text)
      ),

      colIsActive,

      colAassignment,

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
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
    if (this.SearchForm.controls.bebitAccountReferenceId.value != undefined && this.SearchForm.controls.bebitAccountReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "bebitAccountReferenceId",
        values: [this.SearchForm.controls.bebitAccountReferenceId.value],
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

    let request = new GetPersonsDebitedCommoditiesQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.PersonsDebitedCommodities = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }


  async add() {

  }


  async updateRetturnToWarehouse(PersonsDebitedCommodity: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      PersonsDebitedCommodity: PersonsDebitedCommodity,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateReturnToWarehousePersonsDebitedDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          this.get();

        } else if (pageMode === PageModes.Delete) {
          let PersonsDebitedCommoditiesToRemove = this.PersonsDebitedCommodities.find(x => x.id === response.id)

          if (PersonsDebitedCommoditiesToRemove) {
            this.PersonsDebitedCommodities.splice(this.PersonsDebitedCommodities.indexOf(PersonsDebitedCommoditiesToRemove), 1)
            this.PersonsDebitedCommodities = [...this.PersonsDebitedCommodities]
          }
        }
      }
    })
  }

  async updateAassignmentOtherPerson(PersonsDebitedCommodity: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      PersonsDebitedCommodity: PersonsDebitedCommodity,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateNewPersonsDebitedDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        this.get();
      }
    })

  }
  async updateBarcodeDialog(PersonsDebitedCommodity: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      PersonsDebitedCommodity: PersonsDebitedCommodity,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateBarcodeDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        this.get();
      }
    })

  }

  async updateAttachments(PersonsDebitedCommodity: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      PersonsDebitedCommodity: PersonsDebitedCommodity,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateAttachmentAssetsDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        this.get();
      }
    })

  }

  getCommodityById(item: Commodity) {


    this.SearchForm.controls.commodityId.setValue(item?.id);
  }

  personSelect(item: any) {

    this.SearchForm.controls.bebitAccountReferenceId.setValue(item?.id);

  }

  delete() {

  }

  async initialize() {

  }


  close() {
  }
}

function UpdateAttachmentAssetsDilogComponent(UpdateAttachmentAssetsDilogComponent: any, dialogConfig: MatDialogConfig<any>) {
    throw new Error('Function not implemented.');
}
