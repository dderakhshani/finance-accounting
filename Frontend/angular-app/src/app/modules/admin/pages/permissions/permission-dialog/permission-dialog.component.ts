import { Component, ElementRef, Inject, Renderer2, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { BaseValueType } from "../../../entities/base-value-type";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { CreatePermissionCommand, TermsOfAccess } from "../../../repositories/permission/commands/create-permission-command";
import { UpdatePermissionCommand } from "../../../repositories/permission/commands/update-permission-command";
import { DeletePermissionCommand } from "../../../repositories/permission/commands/delete-permission-command";
import { GetTableNameQuery } from '../../../repositories/permission/queries/get-table-name-query';
import { FormControl } from '@angular/forms';
import { GetTableFieldQuery } from '../../../repositories/permission/queries/get-table-field-query';

@Component({
  selector: 'app-permissions-dialog',
  templateUrl: './permission-dialog.component.html',
  styleUrls: ['./permission-dialog.component.scss']
})
export class PermissionDialogComponent extends BaseComponent {
  // 'varchar','nvarchar','datetime','bit'
  ColumnFilterOperands: any = [
    {
      title: 'مشمول بر',
      value: 'contains',
      dataTypes: ['varchar', 'nvarchar']
    },

    {
      title: 'مساوی با',
      value: 'equal',
      dataTypes: ['varchar', 'nvarchar', 'number', 'int', 'money', 'date', 'datetime', 'bit']

    },
    {
      title: 'نا مساوی با',
      value: 'notEqual',
      dataTypes: ['varchar', 'nvarchar', 'number', 'int', 'money', 'date', 'datetime']
    },
    {
      title: 'نامشتمل بر',
      value: 'notContains',
      dataTypes: ['varchar', 'nvarchar']
    },

    {
      title: 'بزرگتر از',
      value: 'greaterThan',
      dataTypes: ['number', 'int', 'money', 'date', 'datetime']
    },
    {
      title: 'کوچکتر از',
      value: 'lessThan',
      dataTypes: ['number', 'int', 'money', 'date', 'datetime']
    },
    {
      title: 'شروع با ',
      value: 'startsWith',
      dataTypes: ['varchar', 'nvarchar']
    },

  ];
  compositions: any = [
    { title: 'و', value: 'and' },
    { title: 'یا', value: 'or' }
  ];
  FilterOperands: any[] = this.ColumnFilterOperands;
  filterType: string = '';


  pageModes = PageModes;
  node!: BaseValueType;
  tableNames: string[] = [];
  tempTableNames: string[] = [];
  tablefields: any[] = [];
  tempTablefields: any[] = [];
  tableControl = new FormControl();
  selectedOperands: string = 'contains';

  termsOfAccess: any[] = [];

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<PermissionDialogComponent>, private renderer: Renderer2,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.node = data.node;
    this.pageMode = data.pageMode;
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    this.initialize()
  }

  async initialize() {

    this.isLoading = true;
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreatePermissionCommand()
      if (this.node) {
        newRequest.parentId = this.node.id;
      }
      this.request = newRequest;
    }

    if (this.tableNames.length == 0) {
      this.isLoading = true;
      await this._mediator.send(new GetTableNameQuery()).then(res => {
        this.tableNames = res;
        this.tempTableNames = res;
      })
      this.isLoading = false;
    }

    this.tableControl.valueChanges.subscribe(async (newValue) => {
      if (newValue.trim() != "")
        this.tableNames = this.tempTableNames.filter(a => a.toLowerCase().includes(newValue.toLowerCase()));
      else {
        this.tableNames = this.tempTableNames;
      }
    })
    //@ts-ignore
    if (this.node.isDataRowLimiter == true && this.node.permissionConditions.length > 0) {

      //@ts-ignore
      let tableName = this.node.permissionConditions[0].tableName;
      this.tableControl.setValue(tableName);
      await this.handleTableSelection(tableName);
      //@ts-ignore
      for (let i = 0; i < this.node.permissionConditions.length; i++) {
        //@ts-ignore
        let element = this.node.permissionConditions[i];
        //@ts-ignore
        if (this.node.accessToAll == false) {
          let jsoncondition = JSON.parse(element.jsonCondition);
          for (let j = 0; j < jsoncondition.length; j++) {
            let con = jsoncondition[j];
            this.termsOfAccess.push({ field: new FormControl(con.Field), oparation: new FormControl(con.Oparation), value: new FormControl(con.Value), composition: new FormControl(con.Composition) })
          }
        }

      }
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdatePermissionCommand().mapFrom(this.node)
      //@ts-ignore
      if (this.request.accessToAll == undefined) {
        //@ts-ignore
        this.form.controls['accessToAll'].value = true;
      }
    }
  }
  async add(param?: any) {
    let request = <CreatePermissionCommand>this.request;
    if (request.isDataRowLimiter == true) {
      request.tableName = this.tableControl.value;
      request.termsOfAccesses = [];
      this.termsOfAccess.forEach(element => {
        request.termsOfAccesses?.push(new TermsOfAccess(element.field.value, element.oparation.value, element.value.value, element.composition.value));
      });
    }

    let response = await this._mediator.send(request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  async update(entity?: any) {
    let request = <UpdatePermissionCommand>this.request;
    if (request.isDataRowLimiter == true) {
      request.tableName = this.tableControl.value;
      request.termsOfAccesses = [];
      this.termsOfAccess.forEach(element => {
        request.termsOfAccesses?.push(new TermsOfAccess(element.field.value, element.oparation.value, element.value.value, element.composition.value));
      });
    }
    let response = await this._mediator.send(request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }


  async delete() {
    let response = await this._mediator.send(new DeletePermissionCommand((<UpdatePermissionCommand>this.request).id ?? 0));
    this.dialogRef.close({
      response: response,
      pageMode: PageModes.Delete
    })
  }
  async checkDataRowLimiter(enent: any) {
    if (enent.checked == true && this.tableNames.length == 0) {
      this.isLoading = true;
      await this._mediator.send(new GetTableNameQuery()).then(res => {
        this.tableNames = res;
        this.tempTableNames = res;
      })
      this.isLoading = false;
    }
  }
  async handleTableSelection(value: string) {
    this.isLoading = true;
    this.termsOfAccess = [];
    await this._mediator.send(new GetTableFieldQuery(value)).then(res => {
      this.tablefields = res;
      this.tempTablefields = res;
    })
    this.isLoading = false;
  }
  addPermission() {
    this.termsOfAccess.push({ field: new FormControl(''), oparation: new FormControl(''), value: new FormControl(''), composition: new FormControl('') })
  }
  deletePermission(index: any) {
    this.termsOfAccess.splice(index, 1);
  }
  filterComparison(fieldname: any) {
    let filterType = this.tablefields.find(a => a.column == fieldname)?.type
    if (filterType.startsWith('varchar') || filterType.startsWith('nvarchar')) {
      this.ColumnFilterOperands = this.FilterOperands.filter(a => a.dataTypes.includes('varchar'));
      this.selectedOperands = 'contains';
    }
    else if (filterType == 'bit') {
      this.ColumnFilterOperands = this.FilterOperands.filter(a => a.dataTypes.contains('bit'));
      this.selectedOperands = 'equal';
    }

    else if (filterType.startsWith('datetime')) {
      this.ColumnFilterOperands = this.FilterOperands.filter(a => a.dataTypes.contains('datetime'));
      this.selectedOperands = 'equal';
    }
    else {
      this.ColumnFilterOperands = this.FilterOperands;

      this.selectedOperands = 'contains'
    }
  }
  close(): any {
  }

  get(id?: number): any {
  }



}
