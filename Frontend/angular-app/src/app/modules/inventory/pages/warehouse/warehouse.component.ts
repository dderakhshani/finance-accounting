import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import { Warehouse } from "../../entities/warehouse";
import { GetWarehousesQuery } from "../../repositories/warehouse/queries/get-warehouses-query";
import {WarehouseDialogComponent} from "./warehouse-dialog/warehouse-dialog.component";
import {PageModes} from "../../../../core/enums/page-modes";
import { NotificationService } from '../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.scss']
})
export class WarehouseComponent extends BaseComponent {


  Warehouses: Warehouse[] = [];

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog,
    public _notificationService: NotificationService,
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    await this.get()
  }

  async get() {
    await this._mediator.send(new GetWarehousesQuery(0, 0, undefined, "id ASC")).then(res => {

      this.Warehouses = res.data;
    })
  }

  add(Warehouse: any) {
    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      Warehouse: Warehouse,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(WarehouseDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response && pageMode === PageModes.Add) {
        this.Warehouses.push(response)
        this.Warehouses = [...this.Warehouses]
      }
    })
  }


  update(Warehouse: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      Warehouse: Warehouse,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(WarehouseDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let WarehousToUpdate = this.Warehouses.find(x => x.id === response.id)
          if (WarehousToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              WarehousToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let WarehousToRemove = this.Warehouses.find(x => x.id === response.id)

          if (WarehousToRemove) {
            this.Warehouses.splice(this.Warehouses.indexOf(WarehousToRemove), 1)
            this.Warehouses = [...this.Warehouses]
          }
        }
      }
    })
  }

  delete() {

  }

  async initialize() {

  }


  close() {
  }
}
