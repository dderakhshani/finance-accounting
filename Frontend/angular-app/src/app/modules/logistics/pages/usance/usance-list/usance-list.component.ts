import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Usance} from "../../../entities/usance";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {GetAllUsanceQuery} from "../../../repositories/usance/queries/get-all-usance-query";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-usance-list',
  templateUrl: './usance-list.component.html',
  styleUrls: ['./usance-list.component.scss']
})
export class UsanceListComponent extends BaseComponent {

  usance!:Usance[];
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
    this.usance = await this.get()

    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.refresh,
      FormActionTypes.delete,
    ]

    this.tableConfiguration = <TableConfigurations>{
      columns: [
        <TableColumn>{
          title:'کد شخص',
          name:'personId',
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
    return await this.mediator.send(new GetAllUsanceQuery());
  }


  update(): any {
  }


  delete(): any {
  }

  close(): any {
  }

}
