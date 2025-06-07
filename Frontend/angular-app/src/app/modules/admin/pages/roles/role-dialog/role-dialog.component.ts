import {Component, Inject} from '@angular/core';
import {Role} from "../../../entities/role";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Permission} from "../../../entities/permission";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {GetPermissionsQuery} from "../../../repositories/permission/queries/get-permissions-query";
import {CreateRoleCommand} from "../../../repositories/role/command/create-role-command";
import {UpdateRoleCommand} from "../../../repositories/role/command/update-role-command";
import {FormArray, FormControl, FormGroup} from "@angular/forms";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {DeleteRoleCommand} from "../../../repositories/role/command/delete-role-command";
import {Observable} from "rxjs";
import {map, startWith} from "rxjs/operators";
import {GetRoleQuery} from "../../../repositories/role/queries/get-role-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-roles-dialog',
  templateUrl: './role-dialog.component.html',
  styleUrls: ['./role-dialog.component.scss']
})
export class RoleDialogComponent extends BaseComponent {
  pageModes = PageModes;
  tableConfigurations!:TableConfigurations;
  role!:Role;
  roles!:Role[];
  filteredRoles!:Observable<Role[]>
  permissions:Permission[] = [];
  rolePermissions:Permission[] = [];
  copy: boolean = false;

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<RoleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super()
    this.role = data.role
    this.roles = data.roles;
    this.pageMode = data.pageMode;
    this.request = new CreateRoleCommand()
  }
 ngAfterViewInit() {
   this.actionBar.actions=[
     PreDefinedActions.delete()
   ]

 }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {

    this.formActions = [
      FormActionTypes.delete
    ]
    let columns :TableColumn[] = [
      new TableColumn('selected','',TableColumnDataType.Select,'2.5%'),
      new TableColumn('index','ردیف',TableColumnDataType.Index,'2.5%'),
      new TableColumn('title','عنوان',TableColumnDataType.Text,'25%'),
      new TableColumn('levelCode','کد',TableColumnDataType.Text,'25%'),

    ]
    this.tableConfigurations = new TableConfigurations(columns,new TableOptions(false,true,undefined,true))


    await this._mediator.send(new GetPermissionsQuery()).then(async (res) => {
      this.permissions = res.data;
      await this.initialize()
    })
  }


  async initialize() {


    if (this.pageMode === PageModes.Add) {
      let request = new CreateRoleCommand()
      request.parentId = this.role?.id;
      this.request = request;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateRoleCommand().mapFrom(this.role)
    }

    this.form.controls['cloneFromRole'].disable();

    this.filteredRoles = this.form.get('cloneFromRole').valueChanges.pipe(
      startWith(''),
      map(role => role ? this.roles.filter(x => x.title.toLowerCase().includes((<string>role).toLowerCase()) && x.id !== this.role.id) : this.roles)
    )

    this.form.controls['permissionsId'].valueChanges.subscribe((ids:number[]) => {
      this.rolePermissions = []
      ids.forEach((permissionId:number) => {
        let permission = this.permissions.find(x => x.id === permissionId)
        if (permission) this.rolePermissions.push(permission)
      })
      this.rolePermissions = [...this.rolePermissions]

    })

    this.rolePermissions = [];
    let permissionIds: number[] = this.form.controls['permissionsId']?.getRawValue()
    permissionIds.forEach(permissionId => {
      let permission = this.permissions.find(x => x.id === permissionId)
      if (permission) { // @ts-ignore
        permission._selected = true
        this.rolePermissions.push(permission)
      }
    })
    this.permissions = [...this.permissions]
    this.rolePermissions = [...this.rolePermissions];
  }

  handlePermissionSelection (permission:Permission) {
    // @ts-ignore
    if (permission._selected) {
      this.form.controls['permissionsId']?.push(new FormControl(permission.id))
    } else {
      this.form.controls['permissionsId'].controls.splice(
        this.form.controls['permissionsId'].controls.indexOf(
          this.form.controls['permissionsId'].controls.find((x : FormControl) => x.value === permission.id)
        ),1
      )
    }

    (this.form as FormGroup).patchValue(this.form.getRawValue())
  }

  removePermissionsFromRole() {
    // @ts-ignore
    let permissionsSelectedInTable = this.rolePermissions.filter(x => x.selected)
    permissionsSelectedInTable.forEach(permission => {
      // @ts-ignore
      permission._selected = false;
      let controllerToRemove = this.form.controls['permissionsId'].controls.find((x:FormControl) => x.value === permission.id)
      if (controllerToRemove) this.form.controls['permissionsId'].controls.splice(this.form.controls['permissionsId'].controls.indexOf(controllerToRemove),1)
    });

    (this.form as FormGroup).patchValue(this.form.getRawValue());

  }


  async add(param?: any) {
    await this._mediator.send(<CreateRoleCommand>this.request).then(response => {
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    })
  }

  async update(param?: any) {
    await this._mediator.send(<UpdateRoleCommand>this.request).then(response => {
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    })
  }

  async delete(param?: any) {
    await this._mediator.send(new DeleteRoleCommand((<UpdateRoleCommand>this.request).id ?? 0)).then(response => {
      this.dialogRef.close({
        response: response,
        pageMode: PageModes.Delete
      })
    })
  }

  get(param?: any): any {
  }

  async getPermissionsByRoleId(roleId:number) {
    let selectedRole = await  this._mediator.send(new GetRoleQuery(roleId))

    let rolePermissions = this.form.controls['permissionsId']?.value

    selectedRole.permissionsId.forEach(p => {
      if (!rolePermissions.find((x:number) =>  x === p)) {
        this.form.controls['permissionsId']?.push(new FormControl(p))
        // @ts-ignore
        this.permissions.find(x => x.id === p)._selected = true
      }
    });

    (this.form as FormGroup).patchValue(this.form.getRawValue())

  }

  close(): any {
  }
  roleDisplayFn(roleId: number) {
    return this.roles.find(x => x.id === roleId)?.title ?? ''
  }
}
