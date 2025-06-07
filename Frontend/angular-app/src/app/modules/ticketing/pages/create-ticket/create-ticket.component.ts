import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from 'src/app/core/abstraction/base.component';
import {PageModes} from 'src/app/core/enums/page-modes';
import {Mediator} from 'src/app/core/services/mediator/mediator.service';

import {GetAllRoleQuery} from '../../repositories/tickets/queries/get-all-role';
import {GetAllRoleModel} from '../../entities/get-all-role';
import {GetUserByRoleIdQuery} from '../../repositories/tickets/queries/get-user-by-roleId';
import {GetUserByRoleIdModel} from '../../entities/get-user';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {TicketTopic, TicketTopic2LabelMapping} from '../../entities/ticket-topic';
import {TicketPriority, TicketPriority2LabelMapping} from '../../entities/ticket-priority';
import {CreateTicketCommand} from '../../repositories/tickets/commands/create-ticket-command';
import {Router} from '@angular/router';
import {AttachmentsModel, UploadFileData} from 'src/app/core/components/custom/uploader/uploader.component';
import {environment} from 'src/environments/environment';
import {IdentityService} from "../../../identity/repositories/identity.service";
import {ApplicationUser} from "../../../identity/repositories/application-user";

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.scss']
})
export class CreateTicketComponent extends BaseComponent {

  constructor(private _mediator: Mediator,
              private identityService: IdentityService,
              private dialogRef: MatDialogRef<CreateTicketComponent>,
              @Inject(MAT_DIALOG_DATA) data: any, private router: Router
  ) {
    super();
    this.request = new CreateTicketCommand()
  }

  //------------Attachment------------------------
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

  public ticketTopic2LabelMapping = TicketTopic2LabelMapping;
  public ticketTopics = Object.values(TicketTopic).filter(value => typeof value === 'number');

  public ticketPriority2LabelMapping = TicketPriority2LabelMapping;
  public ticketPriority = Object.values(TicketPriority).filter(value => typeof value === 'number');
  user: ApplicationUser = new ApplicationUser();

  pageModes = PageModes;
  roles: GetAllRoleModel[] = [];
  users: GetUserByRoleIdModel[] = [];

  async ngOnInit() {
    await this.resolve()
  }

  async ngAfterViewInit() {

    await this.getUser();
  }

  async getUser() {
    this.identityService._applicationUser.subscribe((res: ApplicationUser) => {
      if (res.isAuthenticated) {
        this.user = res;


      }
    });
  }

  async resolve(url?: string) {
    this.isLoading = true;
    let response = await this._mediator.send(new GetAllRoleQuery());
    this.roles = response.data;
    this.initialize()
    this.isLoading = false;
  }


  initialize(params?: any) {
    let request = new CreateTicketCommand();
    let url = this.router.url;
    if (url != "/tickets/list")
      request.pageUrl = url
    this.request = request;
  }

  async getUserRole(param: any) {
    this.isLoading = true;
    let response = await this._mediator.send(new GetUserByRoleIdQuery(param));
    this.users = response.data;
    this.isLoading = false;
  }

  async add() {
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
