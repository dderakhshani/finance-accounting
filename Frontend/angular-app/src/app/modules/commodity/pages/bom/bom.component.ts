import {Component, TemplateRef, ViewChild} from '@angular/core';
import Bom from "../../entities/bom";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PreDefinedActions} from "../../../../core/components/custom/action-bar/action-bar.component";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {PageModes} from "../../../../core/enums/page-modes";
import {GetBomsQuery} from "../../repositories/bom/queries/get-boms-query";
import {BomDialogComponent} from "./bom-dialog/bom-dialog.component";
import {DeleteBomCommand} from "../../repositories/bom/commands/delete-bom-command";
import {CommodityCategory} from "../../entities/commodity-category";
import {
  GetCommodityCategoriesQuery
} from "../../repositories/commodity-category/queries/get-commodity-categories-query";
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import { NotificationService } from '../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-bom',
  templateUrl: './bom.component.html',
  styleUrls: ['./bom.component.scss']
})
export class BomComponent extends BaseComponent {
  @ViewChild('buttonCopy', { read: TemplateRef }) buttonCopy!: TemplateRef<any>;

  entities: Bom[] = [];
  tableConfigurations!: TableConfigurations;




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
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commodityCategoryTitle',
        'گروه کالا',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('commodityCategoryTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'levelCode',
        'کد',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('levelCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'description',
        'شرح',
        TableColumnDataType.Text,
        '40%',
        true,
        new TableColumnFilter('description', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'isActive',
        'فعال',
        TableColumnDataType.CheckBox,
        '5%',
        true,
        new TableColumnFilter('isActive', TableColumnFilterTypes.Text)
      ),
      colCopy,
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

    let request = new GetBomsQuery(this.tableConfigurations.pagination.pageIndex, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
  }

  async add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(BomDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.get();
      }
    })
  }

  async update(entity?: Bom, displayPage?: string) {

// @ts-ignore
    if (entity || this.entities.filter(x => x.selected)[0]) {
      let dialogConfig = new MatDialogConfig();

      dialogConfig.data = {
        // @ts-ignore

        entity: entity ?? this.entities.filter(x => x.selected)[0],
        pageMode: PageModes.Update,
        displayPage: displayPage
      };

      let dialogReference = this.dialog.open(BomDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe(({response, pageMode}) => {
        this.get();
      })
    }
  }


  async delete() {
    // @ts-ignore
    this.entities.filter(x => x.selected).forEach(async (bom) => {

      await this._mediator.send(new DeleteBomCommand(bom.id)).then(res => {
        this.get();
      });
    })

  }


  close(): any {
  }

}
