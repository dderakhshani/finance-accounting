import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Cheque} from "../../../entities/cheque";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {BankBranch} from "../../../entities/bank-branch";
import {FormArray} from "@angular/forms";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-cheque-list',
  templateUrl: './cheque-list.component.html',
  styleUrls: ['./cheque-list.component.scss']
})
export class ChequeListComponent extends BaseComponent {

  cheques: Cheque[] = []
  chequesForm!: FormArray;

  bankBranches: BankBranch[] = []
  tableConfigurations!: TableConfigurations;

  constructor(
    private _mediator: Mediator
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    // todo delete
    this.bankBranches.push({
      id: 1,
      title: 'zafar',
      bankId: 1
    })

    this.formActions = [
      FormActionTypes.edit,
      FormActionTypes.refresh,
      FormActionTypes.delete
    ]
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'sheba',
        'شبا',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('sheba', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'accountNumber',
        'شماره حساب',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('accountNumber', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'sheetsCount',
        'تعداد برگ',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('sheetsCount', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'bankBranchId',
        'شعبه',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('bankBranchId', TableColumnFilterTypes.Number),
        (cheque: Cheque) => {
          return this.bankBranches.find(x => x.id === cheque.bankBranchId)?.title
        }
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(true, true))
    await this.get()
  }

  async get() {
//todo delete
    this.cheques.push(
      <Cheque>{
        id: 1,
        sheba: "1234567890",
        bankBranchId: 1,
        accountNumber: "hesabe jaari",
        sheetsCount: 20,
      },
      <Cheque>{
        id: 1,
        sheba: "1234567890",
        bankBranchId: 1,
        accountNumber: "hesabe jaari",
        sheetsCount: 20,
      },
      <Cheque>{
        id: 1,
        sheba: "1234567890",
        bankBranchId: 1,
        accountNumber: "hesabe jaari",
        sheetsCount: 20,
      }
    )

    this.chequesForm = new FormArray(
      this.cheques.map( x => this.createForm(x))
    )

  }


  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  initialize(): any {
  }


  update(param?: any): any {
  }

}
