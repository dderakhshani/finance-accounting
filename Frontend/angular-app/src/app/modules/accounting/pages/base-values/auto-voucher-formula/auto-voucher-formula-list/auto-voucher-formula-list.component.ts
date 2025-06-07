import {Component, Input} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {Router} from "@angular/router";
import {CodeVoucherGroup} from "../../../../entities/code-voucher-group";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {AutoVoucherFormula} from "../../../../entities/AutoVoucherFormula";
import {
  GetAutoVoucherFormulasQuery
} from "../../../../repositories/auto-voucher-formula/queries/get-auto-voucher-formulas-query";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import {state} from "@angular/animations";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-auto-voucher-formula-list',
  templateUrl: './auto-voucher-formula-list.component.html',
  styleUrls: ['./auto-voucher-formula-list.component.scss']
})
export class AutoVoucherFormulaListComponent extends BaseComponent {
  codeVoucherGroups: CodeVoucherGroup[] = [];
  tableConfigurations!: TableConfigurations;
  entities:AutoVoucherFormula[] = [];
  SearchForm = new FormGroup({

    sourceVoucherTypeTitle: new FormControl(),
    voucherTypeTitle: new FormControl()
  });
  constructor(
    private mediator: Mediator,
    private router:Router
  ) {
    super();
    this.request = new GetAutoVoucherFormulasQuery()

  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
      PreDefinedActions.delete(),

    ]
  }
  async ngOnInit() {

    await this.resolve()
  }

  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.delete
    ]
    let columns = [
      new TableColumn('selected','',TableColumnDataType.Select,'2.5%'),
      new TableColumn('index','',TableColumnDataType.Index,'2.5%'),
      new TableColumn('accountHeadId','کد حساب',TableColumnDataType.Number,'10%',true),
      new TableColumn('sourceVoucherTypeTitle','سند مبدا',TableColumnDataType.Text ,'10%',true,new TableColumnFilter('sourceVoucherTypeTitle', TableColumnFilterTypes.Text)),
      new TableColumn('voucherTypeTitle','سند مقصد',TableColumnDataType.Text ,'10%',true,new TableColumnFilter('voucherTypeTitle', TableColumnFilterTypes.Text)),
      new TableColumn(
        'debitCreditStatus',
        'نوع حساب',
        TableColumnDataType.Text,
        '10%',
        true,
        undefined,
        (formula:AutoVoucherFormula) => {
          return formula.debitCreditStatus === 1 ? 'بدهکار' : 'بستانکار'
        }
      ),
      new TableColumn(
        'orderIndex',
        'ترتیب نمایش',
        TableColumnDataType.Number,
        '10%',
        true,
      ),
      new TableColumn(
        'rowDescription',
        'شرح',
        TableColumnDataType.Text,
        '10%',
        true,
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns,new TableOptions(false,true,undefined,true,true,true))


    // await this.mediator.send(new GetCodeVoucherGroupsQuery()).then(res => {
    //   this.codeVoucherGroups = res.data
    // })
    // await this.get();
    // await this.initialize();

    // await this.mediator.send(new GetAutoVoucherFormulasQuery(0,10)).then(res => {
    //   this.entities = res.data
    //})
    await this.get();
  }

  initialize(params?: any): any {
  }
  async get() {
    let searchQueries:SearchQuery[]=[];
    // if (this.tableConfigurations.filters) {
    //   this.tableConfigurations.filters.forEach(filter => {
    //     searchQueries.push(new SearchQuery({
    //       propertyName: filter.columnName,
    //       values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
    //       comparison: filter.searchCondition,
    //       nextOperand: filter.nextOperand
    //     }))
    //   })
    // }
//if(this.voucherTypeId)
    if (this.SearchForm.controls.sourceVoucherTypeTitle.value != undefined && this.SearchForm.controls.voucherTypeTitle.value != "") {
      searchQueries.push(
        new SearchQuery({
          propertyName: 'voucherTypeTitle',
          comparison: 'equal',
          nextOperand: 'or',
          values: [(this.request as GetAutoVoucherFormulasQuery).voucherTypeTitle]
        }))
      new SearchQuery({
        propertyName: 'sourceVoucherTypeTitle',
        comparison: 'equal',
        nextOperand: 'or',
        values: [(this.request as GetAutoVoucherFormulasQuery).sourceVoucherTypeId]
      })
    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    await this.mediator.send(new GetAutoVoucherFormulasQuery(this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries,orderByProperty)).then(res => {
      this.entities = res.data;
      res.totalCount && (this.tableConfigurations.pagination.totalItems = res.totalCount);
    })


  }

  async add() {
    await this.router.navigateByUrl("/accounting/autoVoucherFormula/add")
  }

  async update(entity?: AutoVoucherFormula) {
      // @ts-ignore
      let autoVoucherFormula=this.entities.filter(x => x.selected)[0];
      if (autoVoucherFormula || entity) {
        await this.router.navigateByUrl(`/accounting/autoVoucherFormula/add?id=${autoVoucherFormula.id}`)
      }
    }



  close(): any {
  }

  delete(param?: any): any {
  }

  protected readonly state = state;
}
