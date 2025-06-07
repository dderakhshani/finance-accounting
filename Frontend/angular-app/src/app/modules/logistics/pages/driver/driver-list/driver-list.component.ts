import { Component } from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {Driver} from "../../../entities/driver";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {GetAllDriversQuery} from "../../../repositories/driver/queries/get-all-drivers-query";
import {Router} from "@angular/router";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-driver-list',
  templateUrl: './driver-list.component.html',
  styleUrls: ['./driver-list.component.scss']
})
export class DriverListComponent extends BaseComponent{

  drivers!:Driver[];
  tableConfiguration!:TableConfigurations;

  row!:Driver;

  constructor(
    private mediator:Mediator,
    private router:Router
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.drivers = await this.get(),



    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.refresh,
      FormActionTypes.delete,
    ]
    this.tableConfiguration = <TableConfigurations>{
      columns: [
        <TableColumn>{
          title:' شخص',
          name:'lastName',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'شماره گواهینامه',
          name:'driveingLisenceNumber',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'تاریخ انقضای گواهینامه',
          name:'driveingLisenceExpiryDate',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'نرخ',
          name:'rate',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'شماره کارت هوشمند سلامت',
          name:'healthSmartCardNumber',
          type: TableColumnDataType.Text,
          width:'10%',
        },
        <TableColumn>{
          title:'تاریخ انقضای کارت هوشمند سلامت',
          name:'healthSmartCardExpiryDate',
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
    return await this.mediator.send(new GetAllDriversQuery());
  }

  async openDriverDetail(driver: Driver){
    await this.router.navigateByUrl(`/logistic/driver/add?id=${driver.id}`)
  }


  update(): any {
  }


  delete(): any {
  }

  close(): any {
  }
}
