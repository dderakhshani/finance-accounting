import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { PageModes } from "../../../../core/enums/page-modes";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { TableConfigurations} from '../../../../core/components/custom/table/models/table-configurations';
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

import { GetMapSamatozinToDanaQuery } from '../../repositories/map-samatozin-to-dana/queries/get-map-samatozin-to-dana-query';
import { MapSamatozinToDanaDialogComponent } from './map-samatozin-to-dana-dialog/samatozin-to-dana-dialog.component';
import { MapSamatozinToDana } from '../../entities/map-samatozin-to-dana';

@Component({
  selector: 'app-map-samatozin-to-dana-list',
  templateUrl: './map-samatozin-to-dana-list.component.html',
  styleUrls: ['./map-samatozin-to-dana-list.component.scss']
})
export class MapSamatozinToDanaListComponent extends BaseComponent {
  @ViewChild('buttonEdit', { read: TemplateRef }) buttonEdit!: TemplateRef<any>;

  MapSamatozinToDanas: MapSamatozinToDana[] = [];
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
      'ویرایش / حذف',
      TableColumnDataType.Template,
      '10%',
      false

    );

    colEdit.template = this.buttonEdit;
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),


      new TableColumn(
        'accountReferencesTitle',
        'عنوان تفصیل',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('accountReferencesTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'accountReferenceCode',
        'کد تفصیل',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('accountReferenceCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'samaTozinTitle',
        'عنوان سما توزین',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('samaTozinTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'samaTozinCode',
        'کد سما توزین',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('samaTozinCode', TableColumnFilterTypes.Text)
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

    let request = new GetMapSamatozinToDanaQuery(
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.MapSamatozinToDanas = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }


  async add(MapSamatozinToDana: any = undefined) {
    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      MapSamatozinToDana: MapSamatozinToDana,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(MapSamatozinToDanaDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response && pageMode === PageModes.Add) {
        this.get();
      }
    })
  }


 async update(MapSamatozinToDana: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      MapSamatozinToDana: MapSamatozinToDana,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(MapSamatozinToDanaDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        if (pageMode === PageModes.Update) {
           this.get();

        } else if (pageMode === PageModes.Delete) {
          let MapSamatozinToDanasToRemove = this.MapSamatozinToDanas.find(x => x.id === response.id)

          if (MapSamatozinToDanasToRemove) {
            this.MapSamatozinToDanas.splice(this.MapSamatozinToDanas.indexOf(MapSamatozinToDanasToRemove), 1)
            this.MapSamatozinToDanas = [...this.MapSamatozinToDanas]
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
