import {Component, Input} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {FormActionTypes} from "../../../../../../../core/constants/form-action-types";
import {
  TableConfigurations
} from "../../../../../../../core/components/custom/table/models/table-configurations";
import {FormArray} from "@angular/forms";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-voucher-attachments-list',
  templateUrl: './voucher-attachments-list.component.html',
  styleUrls: ['./voucher-attachments-list.component.scss']
})
export class VoucherAttachmentsListComponent extends BaseComponent {

  tableConfigurations!: TableConfigurations;


  @Input() set formSetter(form: FormArray) {
    this.form = form;
  }

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.resolve()
  }

  resolve(params?: any): any {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.delete
    ]
    let columns = [
      new TableColumn('selected', '', TableColumnDataType.Select,'5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '5%'),
      new TableColumn(
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '25%'
      ),
      new TableColumn(
        'description',
        'شرح',
        TableColumnDataType.Text,
        '50%'
      )
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))
  }

  initialize(params?: any): any {
  }


  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }


  update(param?: any): any {
  }

}
