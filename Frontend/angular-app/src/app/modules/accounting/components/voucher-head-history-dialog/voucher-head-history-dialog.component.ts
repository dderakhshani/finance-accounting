import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {da} from "date-fns/locale";
import {EntityEvent} from "../../repositories/events/entity-event";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {GetEventsQuery} from "../../repositories/events/get-events-query";
import {AccountManagerService} from "../../services/account-manager.service";
import {MoneyPipe} from "../../../../core/pipes/money.pipe";

@Component({
  selector: 'app-voucher-head-history-dialog',
  templateUrl: './voucher-head-history-dialog.component.html',
  styleUrls: ['./voucher-head-history-dialog.component.scss']
})
export class VoucherHeadHistoryDialogComponent implements OnInit {

  entityId!: number
  entityType = 'VouchersHead'

  events: EntityEvent[] = []

  constructor(
    private dialogRef: MatDialogRef<VoucherHeadHistoryDialogComponent>,
    private mediator: Mediator,
    private accountManager: AccountManagerService,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.entityId = data.id;
  }

  async ngOnInit() {
    await this.mediator.send(new GetEventsQuery(this.entityId, this.entityType)).then(res => {
      this.events = res;
    })
  }


  getEventDescription(event: EntityEvent) {
    let descrption = '';
    if (!event.payload) event.payload = JSON.parse(event.payloadJSON)
    switch (event.payloadType) {
      case 'CreateVoucherHeadEvent':
        descrption += 'سند شماره: ' + event.payload.voucherNo;
        descrption += ' به شرح: ' + event.payload.voucherDescription
        descrption += ' ثبت شد.'
        return descrption;
      case 'CreateVoucherDetailEvent':
        descrption += 'آرتیکل به شماره ردیف: ' + event.payload.rowIndex
        if (event.payload.accountHeadId) {
          let accountHead = this.accountManager.accountHeads.value.find(x => x.id === event.payload.accountHeadId)
          descrption += ' در معین: ' + [accountHead?.code, accountHead?.title].join("/")
        }
        if (event.payload.accountReferencesGroupId) {
          let group = this.accountManager.accountReferenceGroups.value.find(x => x.id === event.payload.accountReferencesGroupId)
          descrption += ' در گروه: ' + [group?.code, group?.title].join("/");
        }
        if (event.payload.referenceId1) {
          let accountReference = this.accountManager.accountReferences.value.find(x => x.id === event.payload.referenceId1)
          descrption += ' در تفصیل: ' + [accountReference?.code, accountReference?.title].join("/");
        }
        if (event.payload.credit) descrption += ' با مبلغ بستانکاری: ' + new MoneyPipe().transform(event.payload.credit)
        if (event.payload.debit) descrption += ' با مبلغ بدهکاری: ' + new MoneyPipe().transform(event.payload.debit)
        descrption += ' ثبت شد.'
        return descrption
      default:
        return "";
    }

  }

}
