import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { PageModes } from 'src/app/core/enums/page-modes';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { GetBaseValuesByUniqueNameQuery } from 'src/app/modules/admin/repositories/base-value/queries/get-base-values-by-unique-name-query';
import { AccountReference } from 'src/app/modules/bursary/entities/account-reference';
import { AccountReferencesGroup } from 'src/app/modules/bursary/entities/account-reference-group';
import { Bank } from 'src/app/modules/bursary/entities/bank';
import { BaseValue } from 'src/app/modules/bursary/entities/base-value';
import { ChequeSheet } from 'src/app/modules/bursary/entities/cheque-sheet';
import { GetBanksQuery } from 'src/app/modules/bursary/repositories/bank/queries/get-banks-query';
import { GetBursaryAccountReferenceGroupsQuery } from 'src/app/modules/bursary/repositories/financial-request/account-reference-group/queries/get-bursary-reference-groups-query';
import { CreateChequeSheetCommand } from 'src/app/modules/bursary/repositories/financial-request/receipt-cheque/commands/create-cheque-sheet-command';
import { UpdateChequeSheetCommand } from 'src/app/modules/bursary/repositories/financial-request/receipt-cheque/commands/update-cheque-sheet-command';
import { GetAccountReferenceByGroupIdQuery } from 'src/app/modules/bursary/repositories/financial-request/referenceAccount/queries/get-account-references-by-group-id-query';
import { AddChequeAttachmentsComponent } from './cheque-attachments/add-cheque-attachments.component';
import { CreateReceiveChequeAttachmentCommand } from 'src/app/modules/bursary/repositories/cheque/commands/create-receive-cheque-attachment-command';

@Component({
  selector: 'app-receipt-cheques',
  templateUrl: './add-receive-cheque.component.html',
  styleUrls: ['./add-receive-cheque.component.scss']
})
export class AddReceiveChequeComponent extends BaseComponent {

  attachments: CreateReceiveChequeAttachmentCommand[] = [];
  banks: Bank[] = [];
  chequeTypes: BaseValue[] = [];
  accountReferences: AccountReference[] = [];
  filterdChequeOwnerReferences: AccountReference[] = [];

  accountReferenceGroups: AccountReferencesGroup[] = [];
  filterdChequeOwnerGroups: AccountReferencesGroup[] = [];

  filterdChequeReceiveGroups : AccountReferencesGroup[] = [];

  filterdChequeReceiveReferences: AccountReference[] = [];


  constructor(
    private route:ActivatedRoute,
    private router:Router,
    public dialog: MatDialog,
    private _mediator:Mediator,
    @Optional() public dialogRef: MatDialogRef<AddReceiveChequeComponent>,

  ) {
    super(route,router);
    this.request = new CreateChequeSheetCommand();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve(params?: any) {
    forkJoin({
      accountReferenceGroups : this._mediator.send(new GetBursaryAccountReferenceGroupsQuery()),
      banks: this._mediator.send(new GetBanksQuery()),
      chequeTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("ChequeType")),
    }).subscribe(async (data) => {
      this.accountReferenceGroups = data.accountReferenceGroups.data;
      this.banks = data.banks.data;
      this.chequeTypes = data.chequeTypes;
      await this.initialize();
    });
  }

  async initialize(entity?: ChequeSheet) {
    if (entity || this.getQueryParam("id")) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam("id"));
      this.request = new UpdateChequeSheetCommand().mapFrom(
        <ChequeSheet>entity
      );
    } else {
      this.request = new CreateChequeSheetCommand();
    }

    this.form.patchValue({
      receiptDate :new Date(),
      issueDate :new Date()
    })
    this.ownerChequeGroups(this.form)

    this.ownerChequeReferences(this.form);

    this.receiveChequeGroups(this.form);

    this.receiveChequeReferences(this.form);

  }

   async add(param?: any) {

    let response = await this._mediator.send(<CreateChequeSheetCommand>this.request);

     await this.initialize(response);

    this.dialogRef.close();
  }
  get(param?: any) {

    return new ChequeSheet();
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

  ownerChequeReferenceDisplayFn(referenceId: number) {
    return this.accountReferences.find((x) => x.id === referenceId)?.title ?? "" ;
  }
  ownerChequeGroupsDisplayFn(groupId: number) {
    return  this.accountReferenceGroups.find((x) => x.id === groupId)?.title ?? "" ;
  }


  receiveChequeReferenceDisplayFn(referenceId: number) {
    return this.accountReferences.find((x) => x.id === referenceId)?.title ?? "" ;
  }
  receiveChequeGroupsDisplayFn(groupId: number) {
    return  this.accountReferenceGroups.find((x) => x.id === groupId)?.title ?? "" ;
  }

  ownerChequeGroups(form: FormGroup) {
    form.controls[
      "ownerChequeReferenceGroupId"
    ].valueChanges.pipe(
      startWith(""),
      map((accountReferenceGroupTitle) => {

        if (typeof accountReferenceGroupTitle != 'string') {
          this.filterdChequeOwnerGroups = this.accountReferenceGroups;
          return [];
        }

        return accountReferenceGroupTitle
          ? this.accountReferenceGroups.filter((x) =>
            (x.title + x.code)
              ?.toLowerCase()
              ?.includes(accountReferenceGroupTitle?.toLowerCase().trim())
          )
          : this.filterdChequeOwnerGroups = this.accountReferenceGroups;
      }
      )
    ).subscribe(x => {
      this.filterdChequeOwnerGroups = x;
    });
  }

  ownerChequeReferences(form: FormGroup) {
    form.controls[
      "ownerChequeReferenceId"
    ].valueChanges.pipe(
      startWith(""),
      map((accountReferencesTitle) => {

        if (typeof accountReferencesTitle != 'string') {
          this.filterdChequeOwnerReferences = this.accountReferences;
          return [];
        }

        return accountReferencesTitle
          ? this.accountReferences.filter((x) =>
            (x.title + x.code)
              ?.toLowerCase()
              ?.includes(accountReferencesTitle?.toLowerCase().trim())
          )
          : this.filterdChequeOwnerReferences = this.accountReferences;
      }
      )
    ).subscribe(x => {
      this.filterdChequeOwnerReferences = x;
    });
  }

  async getReferenceAccount(groupId: number) {
    return await this._mediator
      .send(new GetAccountReferenceByGroupIdQuery(groupId))
      .then((x) => { (this.accountReferences = x.data) });
  }


  setValueReferences() {
    this.filterdChequeOwnerReferences = this.accountReferences;
  }



  async getReceiveReferenceAccount(groupId: number) {
    return await this._mediator
      .send(new GetAccountReferenceByGroupIdQuery(groupId))
      .then((x) => { (this.accountReferences = x.data) });
  }


  setValueReceiveReferences() {
    this.filterdChequeOwnerReferences = this.accountReferences;
  }


  receiveChequeGroups(form: FormGroup) {
    form.controls[
      "receiveChequeReferenceGroupId"
    ].valueChanges.pipe(
      startWith(""),
      map((accountReferenceGroupTitle) => {

        if (typeof accountReferenceGroupTitle != 'string') {
          this.filterdChequeReceiveGroups = this.accountReferenceGroups;
          return [];
        }

        return accountReferenceGroupTitle
          ? this.accountReferenceGroups.filter((x) =>
            (x.title + x.code)
              ?.toLowerCase()
              ?.includes(accountReferenceGroupTitle?.toLowerCase().trim())
          )
          : this.filterdChequeReceiveGroups = this.accountReferenceGroups;
      }
      )
    ).subscribe(x => {
      this.filterdChequeReceiveGroups = x;
    });
  }

  receiveChequeReferences(form: FormGroup) {
    form.controls[
      "receiveChequeReferenceId"
    ].valueChanges.pipe(
      startWith(""),
      map((accountReferencesTitle) => {

        if (typeof accountReferencesTitle != 'string') {
          this.filterdChequeReceiveReferences = this.accountReferences;
          return [];
        }

        return accountReferencesTitle
          ? this.accountReferences.filter((x) =>
            (x.title + x.code)
              ?.toLowerCase()
              ?.includes(accountReferencesTitle?.toLowerCase().trim())
          )
          : this.filterdChequeReceiveReferences = this.accountReferences;
      }
      )
    ).subscribe(x => {
      this.filterdChequeReceiveReferences = x;
    });
  }

  upload(){

    const dialogRef = this.dialog.open(AddChequeAttachmentsComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(this.form.value.financialRequestAttachments) },
    });

    dialogRef.afterClosed().subscribe(files => {

      files.forEach((item: any) => {
        this.attachments.push(item);
      });


    });
  }

}
