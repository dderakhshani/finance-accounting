import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {Role} from "../../entities/role";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {GetRolesQuery} from "../../repositories/role/queries/get-roles-query";
import {PageModes} from "../../../../core/enums/page-modes";
import {RoleDialogComponent} from "./role-dialog/role-dialog.component";

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent extends BaseComponent {
  roles: Role[] = [];
  public selectedNode!: Role;

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
    await this._mediator.send(new GetRolesQuery()).then(res => {
      this.roles = res.data
    })
  }


  async add(role:Role) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      role: role,
      roles:this.roles,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(RoleDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response,pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.roles.push(response)
        this.roles = [...this.roles]
      }
    })
  }

  async update(role:Role) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      role: role,
      roles:this.roles,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(RoleDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response,pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let roleToUpdate = this.roles.find(x => x.id === response.id)
          if (roleToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              roleToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let roleToRemove = this.roles.find(x => x.id === response.id)
          if (roleToRemove) {
            this.roles.splice(this.roles.indexOf(roleToRemove),1)
            this.roles = [...this.roles]
          }
        }
      }
    })
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  handleRoleClick(role:Role) {
    this.selectedNode = role;
  }

  initialize(): any {
  }

}
