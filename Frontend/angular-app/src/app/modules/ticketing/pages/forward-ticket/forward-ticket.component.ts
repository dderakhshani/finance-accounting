import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { CreateTicketComponent } from '../create-ticket/create-ticket.component';
import { ActivatedRoute, Router } from '@angular/router';
import { PageModes } from 'src/app/core/enums/page-modes';
import { ForwardTicketCommand } from '../../repositories/tickets/commands/forward-ticket-command';
import { GetAllRoleModel } from '../../entities/get-all-role';
import { GetAllRoleQuery } from '../../repositories/tickets/queries/get-all-role';
import { GetTicketDetailHistoryQuery } from '../../repositories/tickets/queries/get-ticket-detail-history-query';
import { TicketDetailHistoryModel } from '../../entities/ticket-detail-history';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';

@Component({
  selector: 'app-forward-ticket',
  templateUrl: './forward-ticket.component.html',
  styleUrls: ['./forward-ticket.component.scss']
})
export class ForwardTicketComponent extends BaseComponent {


  constructor(private _mediator: Mediator,
    private dialogRef: MatDialogRef<ForwardTicketComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private router: Router, private route: ActivatedRoute, public identityService: IdentityService) {
    super(route, router);
    this.request = new ForwardTicketCommand()
    this.ticketDetailRoleId = data.ticketDetailRoleId;
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.userRoleId = res.roleId;
      }
    });
  }
  public ticketDetailHistorys: TicketDetailHistoryModel[] = []
  roles: GetAllRoleModel[] = [];
  pageModes = PageModes;
  tableConfigurations!: TableConfigurations;
  public ticketDetailRoleId!: number;
  public userRoleId!: number;
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(url?: string) {


    let tableColumns = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index),
      new TableColumn('primaryRoleName', 'دپارتمان اولیه', TableColumnDataType.Text),
      new TableColumn('secondaryRoleName', 'دپارتمان ثانویه', TableColumnDataType.Text),
      new TableColumn('message', 'پیام', TableColumnDataType.Text),
      new TableColumn('creatDate', 'تاریخ ارجاع', TableColumnDataType.Date)
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));
    this.tableConfigurations.options.exportOptions.showExportButton = false;
    this.tableConfigurations.options.usePagination = false;



    this.initialize();
  }

  async initialize(params?: any) {

    this.get();

    let response = await this._mediator.send(new GetAllRoleQuery());
    this.roles = response.data;
    let request = new ForwardTicketCommand();
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

    let historyList = await this._mediator.send(new GetTicketDetailHistoryQuery(this.data.detailTicketId))
    this.ticketDetailHistorys = historyList;
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
