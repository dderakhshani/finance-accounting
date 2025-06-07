import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetAllBanksQuery} from "../../../repositories/bank/queries/get-all-banks-query";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Bank} from "../../../entities/bank";
import {FormArray, FormControl} from "@angular/forms";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-bank-list',
  templateUrl: './bank-list.component.html',
  styleUrls: ['./bank-list.component.scss']
})
export class BankListComponent extends BaseComponent{

  banks!:Bank[];
  tableConfiguration!:TableConfigurations;
  constructor(
    private mediator:Mediator
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.banks = await this.get()


    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.refresh,
      FormActionTypes.delete,
    ]
    this.tableConfiguration = <TableConfigurations>{
      columns: [
        <TableColumn>{
          title:'کد بانک',
          name:'code',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'نام',
          name:'title',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'کد جهانی',
          name:'globalCode',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'کد سویفت',
          name:'swiftCode',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'نوع بانک',
          name:'bankTypeId',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'نام مدیر',
          name:'managerFullName',
          type: TableColumnDataType.Text,
          width:'10%',
        },
      ]
    }
  }

  initialize(): any {
  }


  add(): any {
  }

  async get(id?: number) {
    return await this.mediator.send(new GetAllBanksQuery());
  }


  update(): any {
  }


  delete(): any {
  }

  close(): any {
  }
}
