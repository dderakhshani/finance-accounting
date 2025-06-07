import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import Bom from "../../entities/bom";
import { BaseComponent } from "../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { PreDefinedActions } from "../../../../core/components/custom/action-bar/action-bar.component";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { PageModes } from "../../../../core/enums/page-modes";
import { TableColumnFilter } from "../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../core/components/custom/table/models/table-column";
import { BomHeadersDialogComponent } from './bom-headers-dialog/bom-headers-dialog.component';
import { BomHeader } from '../../entities/boms-header';
import { GetBomHeadersQuery } from '../../repositories/bom-headers/queries/get-bom-headers-query';
import { DeleteBomHeadersCommand } from '../../repositories/bom-headers/commands/delete-bom-headers-command';
import { NotificationService } from '../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-bom-headers-headers',
  templateUrl: './bom-headers.component.html',
  styleUrls: ['./bom-headers.component.scss']
})
export class BomHeadersComponent extends BaseComponent {

  @ViewChild('buttonCopy', { read: TemplateRef }) buttonCopy!: TemplateRef<any>;
  entities: BomHeader[] = [];
  tableConfigurations!: TableConfigurations;
  @Input() commodityId!: number
  @Input() commodityCategoryId!: number





  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog,
    public _notificationService: NotificationService,
  ) {
    super();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete(),
    ]
    this.resolve();
  }

  async ngOnInit() {

  }
  ngOnChanges() {
    this.resolve();
  }
  async resolve() {
    let colCopy = new TableColumn(
      'colCopy',
      'رونوشت',
      TableColumnDataType.Template,
      '5%',
      false
    );
    colCopy.template = this.buttonCopy;
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'bomTitle',
        'عنوان گروه',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('bomTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'name',
        'عنوان فرمول',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('name', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'bomDate',
        'تاریخ',
        TableColumnDataType.Date,
        '15%',
        true,
        new TableColumnFilter('bomDate', TableColumnFilterTypes.Date)
      ),
      colCopy
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));


    await this.get();
  }

  initialize(): any {
  }

  async get(id?: number) {
    this.entities = [];
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
    let request = new GetBomHeadersQuery(this.commodityId, this.tableConfigurations.pagination.pageIndex, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';


  }

  async add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add,
      commodityCategoryId: this.commodityCategoryId,
      commodityId: this.commodityId

    };

    let dialogReference = this.dialog.open(BomHeadersDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      this.get();
    })
  }

  async update(entity?: BomHeader, displayPage?:string) {

    // @ts-ignore
    if (entity || this.entities.filter(x => x.selected)[0]) {
      let dialogConfig = new MatDialogConfig();

      dialogConfig.data = {
        // @ts-ignore

        entity: entity ?? this.entities.filter(x => x.selected)[0],
        pageMode: PageModes.Update,
        displayPage: displayPage,
        commodityCategoryId: this.commodityCategoryId
      };

      let dialogReference = this.dialog.open(BomHeadersDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
        this.get();
      })
    }
  }


  async delete() {
    // @ts-ignore
    this.entities.filter(x => x.selected).forEach(async (bom) => {

      await this._mediator.send(new DeleteBomHeadersCommand(bom.id)).then(res => {
        this.entities.splice(this.entities.indexOf(bom), 1);
        this.entities = [...this.entities]
      });
    })

  }


  close(): any {
  }

}
