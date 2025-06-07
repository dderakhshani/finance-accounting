import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';
import { GetAllRoleModel } from '../../entities/get-all-role';
import { PageModes } from 'src/app/core/enums/page-modes';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { GetPrivateMessageQuery } from '../../repositories/tickets/queries/get-private-message-query';
import { PrivateMessageModel } from '../../entities/private-message';
import { PrivetMessageCommand } from '../../repositories/tickets/commands/private-message-command';

@Component({
  selector: 'app-private-message',
  templateUrl: './private-message.component.html',
  styleUrls: ['./private-message.component.scss']
})
export class PrivateMessageComponent extends BaseComponent {


  roles: GetAllRoleModel[] = [];
  pageModes = PageModes;
  tableConfigurations!: TableConfigurations;
  public ticketDetailRoleId!: number;
  public userRoleId!: number;
  constructor(private _mediator: Mediator,
    private dialogRef: MatDialogRef<PrivateMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private router: Router, private route: ActivatedRoute, public identityService: IdentityService) {
    super(route, router);
    //this.request = new ForwardTicketCommand()
    this.ticketDetailRoleId = data.ticketDetailRoleId;
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.userRoleId = res.roleId;
      }
    });
  }

  public privateMessageModels: PrivateMessageModel[] = []
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(url?: string) {


    let tableColumns = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index),
      new TableColumn('message', 'پیام', TableColumnDataType.Text),
      new TableColumn('creatDate', 'تاریخ', TableColumnDataType.Date)
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));
    this.tableConfigurations.options.exportOptions.showExportButton = false;
    this.tableConfigurations.options.usePagination = false;



    this.initialize();
  }

  async initialize(params?: any) {

    this.get();

    let request = new PrivetMessageCommand();
    request.ticketDetailId = this.data.detailTicketId;
    this.request = request;
  }
  async add(param?: any) {

    let response = await this._mediator.send(this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }
  async get(param?: any) {

    var result = await this._mediator.send(new GetPrivateMessageQuery(this.data.detailTicketId))
    this.privateMessageModels = result.data;
  }
  checkForwardAccess(): boolean {
    if (this.userRoleId != this.ticketDetailRoleId) {
      return false;
    }
    else {
      return true;
    }
  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }
}
