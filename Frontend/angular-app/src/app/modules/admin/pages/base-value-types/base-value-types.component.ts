import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {BaseValueType} from "../../entities/base-value-type";
import {GetBaseValueTypesQuery} from "../../repositories/base-value-type/queries/get-base-value-types-query";
import {BaseValueTypeDialogComponent} from "./base-value-type-dialog/base-value-type-dialog.component";
import {PageModes} from "../../../../core/enums/page-modes";

@Component({
  selector: 'app-base-value-types',
  templateUrl: './base-value-types.component.html',
  styleUrls: ['./base-value-types.component.scss']
})
export class BaseValueTypesComponent extends BaseComponent {


  baseValueTypes: BaseValueType[] = [];

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
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
    await this._mediator.send(new GetBaseValueTypesQuery(0, 0, undefined, "id ASC")).then(res => {
      this.baseValueTypes = res.data;
    })
  }

  add(baseValueType: any) {
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

  update(baseValueType: any) {
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

  delete() {

  }

  async initialize() {

  }


  close() {
  }
}
