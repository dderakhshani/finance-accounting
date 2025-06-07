import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { PageModes } from "../../../../core/enums/page-modes";
import { UnitCommodityQuotaDialogComponent } from './unitCommodityQuota-dialog/unitCommodityQuota-dialog.component';
import { GetUnitCommodityQuotasQuery } from '../../repositories/unitCommodityQuota/queries/get-unitCommodityQuotas-query';
import { TableConfigurations} from '../../../../core/components/custom/table/models/table-configurations';
import { UnitCommodityQuota } from '../../entities/unitCommodityQuota';
import { FormAction } from '../../../../core/models/form-action';
import { FormActionTypes } from '../../../../core/constants/form-action-types';
import { Router } from '@angular/router';
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import { SearchQuery } from '../../../../shared/services/search/models/search-query';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-unitCommodityQuota-list',
  templateUrl: './unitCommodityQuota-list.component.html',
  styleUrls: ['./unitCommodityQuota-list.component.scss']
})
export class UnitCommodityQuotaListComponent extends BaseComponent {
  @ViewChild('buttonEdit', { read: TemplateRef }) buttonEdit!: TemplateRef<any>;

  UnitCommodityQuotas: UnitCommodityQuota[] = [];
  tableConfigurations!: TableConfigurations;
  listActions: FormAction[] = [
    FormActionTypes.refresh,
    FormActionTypes.print,
    FormActionTypes.add
  ]

  constructor(
    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {
    super();
  }
  async ngOnInit() {
    //await this.resolve()
  }
  async ngAfterViewInit() {

    await this.resolve()
  }

  async resolve() {
    let colEdit = new TableColumn(
      'colEdit',
      'ویرایش',
      TableColumnDataType.Template,
      '10%',
      false

    );
    colEdit.template = this.buttonEdit;
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),


      new TableColumn(
        'unitsTitle',
        ' واحد سازمانی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('unitsTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'quotaGroupName',
        'گروه سهمیه',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('quotaGroupName', TableColumnFilterTypes.Text)
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
        'commodityQuota',
        'تعداد سهمیه',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('commodityQuota', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'quotaDays',
        'تعداد روز مصرف',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('quotaDays', TableColumnFilterTypes.Number)
      ),
      colEdit,

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

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetUnitCommodityQuotasQuery(
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.UnitCommodityQuotas = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }


  async add(UnitCommodityQuota: any = undefined) {
    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      UnitCommodityQuota: UnitCommodityQuota,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(UnitCommodityQuotaDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response && pageMode === PageModes.Add) {
        this.get();
      }
    })
  }


 async update(UnitCommodityQuota: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      UnitCommodityQuota: UnitCommodityQuota,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UnitCommodityQuotaDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        if (pageMode === PageModes.Update) {
           this.get();

        } else if (pageMode === PageModes.Delete) {
          let UnitCommodityQuotasToRemove = this.UnitCommodityQuotas.find(x => x.id === response.id)

          if (UnitCommodityQuotasToRemove) {
            this.UnitCommodityQuotas.splice(this.UnitCommodityQuotas.indexOf(UnitCommodityQuotasToRemove), 1)
            this.UnitCommodityQuotas = [...this.UnitCommodityQuotas]
          }
        }
      }
    })
  }

  delete() {
  }
  initialize() {
  }
  close() {
  }
}
