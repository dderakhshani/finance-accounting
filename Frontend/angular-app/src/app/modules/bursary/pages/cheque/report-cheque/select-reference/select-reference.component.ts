import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { AccountReference } from 'src/app/modules/bursary/entities/account-reference';
import { AccountReferencesGroup } from 'src/app/modules/bursary/entities/account-reference-group';
import { AccountReferencesGroupEnums, Documents } from 'src/app/modules/bursary/entities/enums';
import { GetReferenceGroupsByAccountHeadIdQuery } from 'src/app/modules/bursary/repositories/financial-request/account-reference-group/queries/get-reference-groups-bay-account-head';
import {  GetBursaryAccountReferencesQuery } from 'src/app/modules/bursary/repositories/financial-request/referenceAccount/queries/get-account-references-query';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';

@Component({
  selector: 'app-select-reference',
  templateUrl: './select-reference.component.html',
  styleUrls: ['./select-reference.component.scss']
})
export class SelectReferenceComponent extends BaseComponent {

  public documents = Documents;
  creditAccountReferences: AccountReference[] = [];
  creditAccountReferencesGroup: AccountReferencesGroup[] = [];

  
  creditAccountReferenceId!:number
  creditAccountReferenceGroupId!:number


  documentDate !: any
  constructor(private _mediator: Mediator,@Optional() public dialogRef: MatDialogRef<SelectReferenceComponent>, @Inject(MAT_DIALOG_DATA) public value: any, public dialog: MatDialog) {
    super();
   }

  resolve(params?: any) {
    throw new Error('Method not implemented.');
  }
  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
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

 async ngOnInit()  {
 

  await this._mediator.send(new GetReferenceGroupsByAccountHeadIdQuery(1900)).then((res) => {
    this.creditAccountReferencesGroup = res.data
});
 



let filter: SearchQuery[] = [];
filter.push({
  propertyName: 'accountReferencesGroupsIdList',
  comparison: 'inList',
  values: [AccountReferencesGroupEnums.AccountReferencesGroupCode_1001],
  nextOperand: 'or'
})
filter.push({
  propertyName: 'accountReferencesGroupsIdList',
  comparison: 'inList',
  values: [AccountReferencesGroupEnums.AccountReferencesGroupCode_1002],
  nextOperand: 'and'
})

  await this._mediator.send(new GetBursaryAccountReferencesQuery(0, 25, filter)).then((res) => {
    this.creditAccountReferences = res.data
});


  }
 
  accountReferenceGroupDisplayFn(id: number) {
    return this.creditAccountReferencesGroup.find(x => x.id === id)?.title;
  }



  accountReferenceDisplayFn(id: number) {
    return this.creditAccountReferences.find(x => x.id === id)?.title;
  }



  setReference(){
 let result =  this.creditAccountReferenceId+","+this.documentDate._d+","+this.creditAccountReferenceGroupId;
    this.dialogRef.close(result);
  }

}
