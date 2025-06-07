import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { CreateTicketComponent } from '../create-ticket/create-ticket.component';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { ReplyTicketCommand } from '../../repositories/tickets/commands/reply-ticket-command';
import { PageModes } from 'src/app/core/enums/page-modes';
import { AttachmentsModel, UploadFileData } from 'src/app/core/components/custom/uploader/uploader.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-reply-ticket',
  templateUrl: './reply-ticket.component.html',
  styleUrls: ['./reply-ticket.component.scss']
})
export class ReplyTicketComponent extends BaseComponent {

  constructor(private _mediator: Mediator,
    private dialogRef: MatDialogRef<CreateTicketComponent>,
    @Inject(MAT_DIALOG_DATA) data: any, private router: Router, private route: ActivatedRoute) {
    super(route, router);
    this.request = new ReplyTicketCommand()
  }
  attachmentAssets: number[] = [];
  public attachmentsModel: AttachmentsModel = {
    typeBaseId: 28702,
    title: 'فایل تیکت',
    description: 'فایل تیکت',
    keyWords: 'Ticketing',
  };
  public isUploading !: boolean;
  public attachmentIds: number[] = [];
  public imageUrls: UploadFileData[] = [];
  public baseUrl: string = environment.fileServer + "/";

  //----------call by uploder component-------------
  set files(values: string[]) {
    this.imageUrls = [];

    this.attachmentIds = [];
    values.forEach((value: any) => {
      this.imageUrls.push(value);

      this.attachmentIds.push(value.id);
    })
  }
  pageModes = PageModes;
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(url?: string) {
    this.initialize();
  }


  initialize(params?: any) {
    let request = new ReplyTicketCommand();
    request.ticketId = this.getQueryParam('ticketId');
    this.request = request;
  }
  async add(param?: any) {
    //@ts-ignore
    this.request.attachmentIds = "";
    this.attachmentIds.forEach(element => {
      //@ts-ignore
      this.request.attachmentIds += element + ",";
    });
    let response = await this._mediator.send(this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }
  get(param?: any) {
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
