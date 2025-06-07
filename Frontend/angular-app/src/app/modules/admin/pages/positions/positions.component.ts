import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {Position} from "../../entities/position";
import {GetPositionsQuery} from "../../repositories/position/queries/get-positions-command";
import {PageModes} from "../../../../core/enums/page-modes";
import {PositionDialogComponent} from "./position-dialog/position-dialog.component";

@Component({
  selector: 'app-positions',
  templateUrl: './positions.component.html',
  styleUrls: ['./positions.component.scss']
})
export class PositionsComponent extends BaseComponent {


  positions:Position[] = []
  constructor(
    private _mediator:Mediator,
    public dialog: MatDialog
  ) {
    super()
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    await this.get()
  }

  async get() {
    await this._mediator.send(new GetPositionsQuery(0,0)).then(res => {
      this.positions = res.data
    })
  }

  add(node: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      node: node,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(PositionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response,pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.positions.push(response)
        this.positions = [...this.positions]
      }
    })
  }

  update(node: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      node: node,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(PositionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response,pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let position = this.positions.find(x => x.id === response.id)
          if (position) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              position[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let positionToDelete = this.positions.find(x => x.id === response.id);
          if (positionToDelete) {
            this.positions.splice(this.positions.indexOf(positionToDelete),1)
            this.positions = [...this.positions]
          }
        }
      }
    })
  }


  close(): any {
  }

  delete(param?: any): any {
  }

  initialize(): any {
  }


}
