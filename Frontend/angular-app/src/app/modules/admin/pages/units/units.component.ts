import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {UnitDialogComponent} from "./unit-dialog/unit-dialog.component";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {Unit} from "../../entities/unit";
import {GetUnitsQuery} from "../../repositories/unit/queries/get-units-query";
import {PageModes} from "../../../../core/enums/page-modes";


@Component({
  selector: 'app-unit',
  templateUrl: './units.component.html',
  styleUrls: ['./units.component.scss']
})
export class UnitsComponent extends BaseComponent {

  units: Unit[] = []

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    await this.get()
  }


  async get() {
    let request = new GetUnitsQuery();
    request.orderByProperty = 'id ASC';
    await this._mediator.send(<GetUnitsQuery>request).then(res => {
      this.units = res.data

    })
  }

  async add(unit: Unit) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      unit: unit,
      units: this.units,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(UnitDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.units.push(response)
        this.units = [...this.units]
      }
    })
  }

  async update(unit: Unit) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      unit: unit,
      units: this.units,
      pageMode: PageModes.Update
    };
    let dialogReference = this.dialog.open(UnitDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let unitToUpdate = this.units.find(x => x.id === response.id)
          if (unitToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              unitToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let unitToRemove = this.units.find(x => x.id === response.id)
          if (unitToRemove) {
            this.units.splice(this.units.indexOf(unitToRemove), 1)
            this.units = [...this.units]
          }
        }
      }
    })
  }


  close(): any {
  }

  delete(param?: any): any {
  }

  handleUnitClick(unit: Unit) {
  }
  initialize(): any {
  }
}
