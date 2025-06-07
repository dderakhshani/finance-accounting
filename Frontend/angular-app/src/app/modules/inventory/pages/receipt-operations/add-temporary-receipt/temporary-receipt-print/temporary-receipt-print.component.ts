import { Component, OnInit, EventEmitter, HostListener } from '@angular/core';
import { ActivatedRoute, Data, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { BaseComponent } from '../../../../../../core/abstraction/base.component';
import { Receipt } from '../../../../entities/receipt';
import { FormAction } from '../../../../../../core/models/form-action';
import { FormActionTypes } from '../../../../../../core/constants/form-action-types';
import { GetRecepitRequesterReferenceQuery } from '../../../../repositories/receipt/queries/receipt/get-receipts-requester-reference-query';
import { GetTemporaryRecepitQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { UpdateStatusReceiptCommand } from '../../../../repositories/receipt/commands/reciept/update-status-receipt-command';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { environment } from '../../../../../../../environments/environment';
import { LocalStorageRepository } from '../../../../../../core/services/storage/local-storage-repository.service';
import { GetRecepitListIdQuery } from '../../../../repositories/receipt/queries/receipt/get-receipt-list-Id-query';
import { UpdatePrintCountCommand } from '../../../../repositories/receipt/commands/reciept/update-print-count-receipt';

@Component({
  selector: 'app-temporary-receipt-print',
  templateUrl: './temporary-receipt-print.component.html',
  styleUrls: ['./temporary-receipt-print.component.scss']
})
export class TemporaryReceiptPrintComponent extends BaseComponent {

  public receipt: Receipt | undefined = undefined;
  date: Date = new Date();
  DivId: string = '';

  @HostListener('window:afterprint', ['$event'])
  async afterPrint() {
  }

  formActions: FormAction[] = [
    FormActionTypes.refresh,

  ]
  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    private cookieService: LocalStorageRepository,
    private identityService: IdentityService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
  }


  async ngOnInit() {
    await this.resolve()

  }


  async resolve(params?: any) {

    await this.initialize();
  }
  async initialize(entity?: any) {
    //-----------------------------------
    if (this.getQueryParam('id') != undefined) {
      this.receipt = await this.get(this.getQueryParam('id'))
      this.DivId = "div"+this.getQueryParam('id');
    }
    if (this.getQueryParam('accountReferencesId') != undefined) {

      this.receipt = await this.getRequester()
      this.Service.ListId = [];
      this.DivId = "div" + this.getQueryParam('accountReferencesId');
    }


  }
  async print(cmpName: string = 'mainTable') {
    let printContents = document.getElementById(cmpName)?.innerHTML;
    let callBack =await this.update_Status();
    this.Service.onPrint(printContents?.toString(),'', callBack)
  }
  //----------------------تغییر وضعیت به پرینت شده----------------------------
  async update_Status() {
    var uri = environment.apiURL
    var htmlData = '';
    await this.receipt?.items.forEach(async a => {

      htmlData = htmlData + await this.createApiCall(a.documentHeadId, uri)
    })
    return htmlData;
  }

  async createApiCall(documentHeadId: any, uri: any) {


    var request_ = new UpdatePrintCountCommand();
    request_.id = documentHeadId;


    uri = uri + request_.url;
    var htmlData = '';

    htmlData = `

    const item = {
      id:${request_.id},


    };

    fetch('${uri}', {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ${this.cookieService.get('token')}'
    },
    body: JSON.stringify(item)
  })
    .then(response => response.json())
    .catch(error => alert(error))
    `;

    return htmlData
  }
  async get(Id: number) {
    let response = await this._mediator.send(new GetTemporaryRecepitQuery(Id))

    return response
  }
  async getRequester() {

    var request = new GetRecepitListIdQuery();
    request.ListIds = this.Service.ListId

    return await this._mediator.send(<GetRecepitListIdQuery>request);


  }
  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/temporaryReceiptList')
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
