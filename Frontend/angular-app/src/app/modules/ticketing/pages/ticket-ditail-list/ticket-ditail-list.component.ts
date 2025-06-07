import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { GetTicketDetailQuery } from '../../repositories/tickets/queries/get-ticket-detail-query';
import { TicketDetailModel } from '../../entities/ticket-detail';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PageModes } from 'src/app/core/enums/page-modes';
import { ReplyTicketComponent } from '../reply-ticket/reply-ticket.component';
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';
import { TicketModel } from '../../entities/ticket';
import { GetTicketById } from '../../repositories/tickets/queries/get-ticket-by-id';
import { ForwardTicketComponent } from '../forward-ticket/forward-ticket.component';
import { PrivateMessageComponent } from '../private-message/private-message.component';
import { CloseTicketCommand } from '../../repositories/tickets/commands/close-ticket-command';

@Component({
  selector: 'app-ticket-ditail-list',
  templateUrl: './ticket-ditail-list.component.html',
  styleUrls: ['./ticket-ditail-list.component.scss']
})
export class TicketDitailListComponent extends BaseComponent {


  constructor(private _mediator: Mediator, private router: Router, private route: ActivatedRoute, public dialog: MatDialog,
    public identityService: IdentityService) {
    super(route, router);

    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.userId = res.id;
        this.roleId = res.roleId;
      }
    });
  }

  
  public ticketId!: number;
  public userId!: number;
  public roleId!: number;
  public ticketDetails: TicketDetailModel[] | undefined;
  public ticket: TicketModel | undefined;
  public ticketTitle: string = '';
  public isCloseTicket: boolean = false;

  async ngOnInit(): Promise<void> {
    await this.resolve();
  }
  async resolve() {
    this.ticketId = this.getQueryParam('ticketId');
    await this.get();
  }


  async get(param?: any) {

    let ticketrequest = new GetTicketById(this.ticketId);
    let ticketresponse = await this._mediator.send(ticketrequest);
    this.ticket = ticketresponse;
    this.ticketTitle = this.ticket.title;


    let request = new GetTicketDetailQuery(this.ticketId);
    let response = await this._mediator.send(request);
    this.ticketDetails = response.data;
    if (this.ticketDetails[0].ticketStatus >= 5) {
      this.isCloseTicket = true;
    }
    else {
      this.isCloseTicket = false;
    }
    this.ticketDetails.forEach(element => {
      element.attachmentIdsNumber = element.attachmentIds.split(',').filter(a => a != '');
    });
  }
  getAttachmentIds(ticketDetail: TicketDetailModel) {
    let ids = ticketDetail.attachmentIds.split(',').filter(a => a != '');

    return ids;
  }
  replyTicket() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(ReplyTicketComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      this.get();
    })
  }

  forwardTicket(ticketDetailModel: TicketDetailModel) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add,
      detailTicketId: ticketDetailModel.id,
      ticketDetailRoleId: ticketDetailModel.roleId
    };

    let dialogReference = this.dialog.open(ForwardTicketComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      this.get();
    })
  }

  privateMessage(ticketDetailModel: TicketDetailModel) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add,
      detailTicketId: ticketDetailModel.id,
      ticketDetailRoleId: ticketDetailModel.roleId
    };

    let dialogReference = this.dialog.open(PrivateMessageComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      this.get();
    })
  }
  async closeTicket() {

    let request = new CloseTicketCommand(this.ticketId);
    this.request = request;
    let response = await this._mediator.send(this.request);
    this.get();
  }
  checkForwardAccess(ticketDetail: TicketDetailModel): boolean {
    if (ticketDetail.detailCreatorUserId == this.userId || ticketDetail.ticketCreatorUserId == this.userId) {
      return false;
    }
    else {
      return true;
    }
  }
  checkPrivateMessageAccess(ticketDetail: TicketDetailModel): boolean {
    if (ticketDetail.detailCreatorUserId == this.userId || ticketDetail.ticketCreatorUserId == this.userId || ticketDetail.roleId != this.roleId) {
      return false;
    }
    else {
      return true;
    }
  }
  headerColor(ticketDetail: TicketDetailModel): string {
    if (ticketDetail.detailCreatorUserId == ticketDetail.ticketCreatorUserId) {
      return 'header-requester';
    }
    else {
      return 'header-reciver';
    }
  }
  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
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
