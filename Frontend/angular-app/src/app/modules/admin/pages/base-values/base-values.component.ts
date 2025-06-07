import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseValueType} from "../../entities/base-value-type";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {BaseValue} from "../../entities/base-value";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {GetBaseValueTypesQuery} from "../../repositories/base-value-type/queries/get-base-value-types-query";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import {FormActionTypes} from "../../../../core/constants/form-action-types";
import {PageModes} from "../../../../core/enums/page-modes";
import {BaseValueDialogComponent} from "./base-value-dialog/base-value-dialog.component";
import {PreDefinedActions} from "../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import {
  BaseValueTypeDialogComponent
} from "../base-value-types/base-value-type-dialog/base-value-type-dialog.component";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";

@Component({
  selector: 'app-base-values',
  templateUrl: './base-values.component.html',
  styleUrls: ['./base-values.component.scss']
})
export class BaseValuesComponent extends BaseComponent {
  baseValueTypes: BaseValueType[] = [];
  baseValues: BaseValue[] = [];

  activeBaseValueType!: BaseValueType;
  tableConfigurations!: TableConfigurations;
   levelCode: any;
  searchTitleNode! :any | undefined;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super();
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.levelCode = this.getQueryParam('levelCode')
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.history,
    ]
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '10%'),
      new TableColumn(
        'uniqueName',
        'نام یکتا',
        TableColumnDataType.Text,
        '10%'),
      new TableColumn(
        'code',
        'کد',
        TableColumnDataType.Text,
        '10%'),
      new TableColumn(
        'value',
        'مقدار',
        TableColumnDataType.Text,
        '10%'),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))
    let searchQueries: SearchQuery[]
    if (this.levelCode)
    {
      let levelCodeFirst =this.levelCode
      if(this.levelCode.length > 4){
        levelCodeFirst = this.levelCode.slice(0, 4);
      }
      searchQueries = [
        {
          propertyName: 'levelCode',
          values: [levelCodeFirst],
          comparison: 'contains',
          nextOperand: 'and'

        }
      ]
    }

// @ts-ignore
    await this._mediator.send(new GetBaseValueTypesQuery(0, 0, this.levelCode ? searchQueries: undefined, "id ASC")).then(res => {
      this.baseValueTypes = res.data;
      if(this.levelCode){
       const tempObj =res.data.find(x=>x.levelCode == this.levelCode)
      this.searchTitleNode = tempObj?.id
        // @ts-ignore
      this.get(tempObj)
      }
    })
  }

  initialize(): any {
  }

  async get(baseValueType: BaseValueType) {
    await this._mediator.send(new GetBaseValuesByUniqueNameQuery(baseValueType.uniqueName)).then(res => {
      this.baseValues = res;
      this.activeBaseValueType = baseValueType;
      this.tableConfigurations.pagination.totalItems = this.baseValues.length
    })
  }

  add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      baseValueType: this.activeBaseValueType,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(BaseValueDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.baseValues.push(response)
        this.baseValues = [...this.baseValues]
      }
    })
  }

  update(entity?:BaseValue) {
    // @ts-ignore
    if (entity  || this.baseValues.filter(x => x.selected)[0]) {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        // @ts-ignore
        baseValue: entity ?? this.baseValues.filter(x => x.selected)[0],
        pageMode: PageModes.Update
      };

      let dialogReference = this.dialog.open(BaseValueDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe(({response, pageMode}) => {
        if (response) {
          if (pageMode === PageModes.Update){
            let baseValue = this.baseValues.find(x => x.id === response.id)
            if (baseValue) {
              Object.keys(response).forEach((key: string) => {
                // @ts-ignore
                baseValue[key] = response[key]
              })
            }
          }
          else if (pageMode === PageModes.Delete){
            let baseValueToRemove = this.baseValues.find(x => x.id === response.id);
            if (baseValueToRemove) {
              this.baseValues.splice(this.baseValues.indexOf(baseValueToRemove),1);
              this.baseValues = [...this.baseValues]
            }
          }
        }
      })
    }
  }

  close(): any {
  }

  delete(): any {
  }

  addBaseValueType(baseValueType: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      baseValueType: baseValueType,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(BaseValueTypeDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.baseValueTypes.push(response)
        this.baseValueTypes = [...this.baseValueTypes]
      }
    })
  }

  updateBaseValueType(baseValueType: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      baseValueType: baseValueType,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(BaseValueTypeDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let baseValueTypeToUpdate = this.baseValueTypes.find(x => x.id === response.id)
          if (baseValueTypeToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              baseValueTypeToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let baseValueTypeToRemove = this.baseValueTypes.find(x => x.id === response.id)
          if (baseValueTypeToRemove) {
            this.baseValueTypes.splice(this.baseValueTypes.indexOf(baseValueTypeToRemove), 1)
            this.baseValueTypes = [...this.baseValueTypes]
          }
        }
      }
    })
  }
}
