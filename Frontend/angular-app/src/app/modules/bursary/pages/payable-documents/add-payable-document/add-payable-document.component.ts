import {AfterViewInit, Component, OnInit} from '@angular/core';
import {PageModes} from "../../../../../core/enums/page-modes";
import {CreateArchiveCommand} from "../../../../archive/repositories/archives/create-archive-command";
import {UpdateArchiveCommand} from "../../../../archive/repositories/archives/update-archive-command";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  CreatePayableDocumentsCommand
} from "../../../repositories/payables-documents/commands/create-payables-documents";
import {PayableDocument, PayableDocumentPayOrder} from "../../../entities/payableDocument";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {FormControl} from "@angular/forms";
import {Router} from "@angular/router";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {
  GetWarehousesLastLevelByCodeVoucherGroupIdQuery
} from "../../../../inventory/repositories/warehouse/queries/get-warehousesLastLevel-by-codeVoucherGroupId-query ";

@Component({
  selector: 'app-add-payable-document',
  templateUrl: './add-payable-document.component.html',
  styleUrls: ['./add-payable-document.component.scss']
})
export class AddPayableDocumentComponent extends BaseComponent implements OnInit, AfterViewInit {

  titleHeader = 'افزودن سند '
  payableDocument!: PayableDocument;

  constructor(private _router: Router,
              private _mediator: Mediator,) {

    super()
  }

  ngOnInit(): void {
    this.pageMode = +this.getQueryParam('pageMode')
    if (this.pageMode === PageModes.Add) {

      this.titleHeader = 'افزودن سند '
      let request = new CreatePayableDocumentsCommand()
      request.monetarySystemId = 29340;
      // request.payTypeId = 28497;
      request.payOrders = [{
        payOrderId: 93352
      }];
      // request.creditAccountHeadId = 1901;
      // request.creditReferenceGroupId = 37;
      // request.creditReferenceId = 33768;
      // request.bankAccountId =64

      request.accounts = [
        {
          "id": 0,
          "rexpId": 0,
          "accountHeadId": 1901,
          "referenceGroupId": 37,
          "referenceId": 33768,
          "documentId": 0,
          "amount": 32963758888,
          "descp": 'توضیحات',
          "isEditableAccountHeads": false,
          "isEditableAccountReferences": false,
          "isEditableAccountReferenceGroups": false,
          "isEditableDebit": false,
          "isEditableRow": false,
          "selected": true,
          "rowIndex": 1
        }
      ];
      this.request = request;
    } else {
      this.titleHeader = 'ویرایش سند'
      // let request = new UpdatePayableDocumentsCommand().mapFrom(this.payableDocument)
      // this.request = request;
    }
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.save(),
      // PreDefinedActions.add(),
      PreDefinedActions.refresh(),
      PreDefinedActions.list(),

    ]
    this.actionBar.actions = [
      PreDefinedActions.save(),
      // PreDefinedActions.add(),
      PreDefinedActions.refresh(),
      PreDefinedActions.list(),

    ]

  }

  resolve(params?: any) {

  }

  initialize(params?: any) {

  }

  add(param?: any) {

  }

  get(param?: any) {

  }

  update(param?: any) {

  }

  delete(param?: any) {

  }

  close() {

  }

  print() {

  }

  printSimple() {

  }

  goToVoucherByNumber() {

  }

  async submitCheck() {
    if (this.pageMode === PageModes.Add) {
      // @ts-ignore
      this.request.amount = this.parseFloat(this.request.amount)
      // @ts-ignore
      this.request.currencyRate = this.parseFloat(this.request.currencyRate)
      // @ts-ignore
      this.request.currencyAmount = this.parseFloat(this.request.currencyAmount)
      // @ts-ignore
      this.request.accounts = this.request.accounts.map(item => {


        return {
          id: item.id,
          documentId: item.documentId,
          referenceId: item.referenceId,
          referenceGroupId: item.referenceGroupId,
          accountHeadId: item.accountHeadId,
          rexpId: item.rexpId,
          descp: item.descp,
          amount: this.parseFloat(item.amount)
        }


      });
      await this._mediator.send(<CreatePayableDocumentsCommand>this.request).then(async (res) => {
        console.log('res', res)
      })

    }
  }

  parseFloat(val: any): any {
    if (typeof val == 'number') {
      return val;
    }
    if (val != null)
      return parseFloat(val.replace(/,/g, ''));
    else
      return null;
  }

  handleCustomAction($event: Action) {

  }

  async navigateToPayableDocumentList() {
    await this._router.navigateByUrl('/bursary/payableDocument/list')
  }
}
