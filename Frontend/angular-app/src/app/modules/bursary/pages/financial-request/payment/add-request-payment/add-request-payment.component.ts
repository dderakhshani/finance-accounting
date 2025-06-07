import { Component, TemplateRef, ViewChild } from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {RequestPayment} from "../../../../entities/request-payment";
import {AccountReference} from "../../../../entities/account-reference";
import {Observable} from "rxjs";
import {
  CreateRequestPaymentCommand
} from "../../../../repositories/financial-request/request-payments/commands/create-request-payment-command";
import {
  UpdateRequestPaymentCommand
} from "../../../../repositories/financial-request/request-payments/commands/update-request-payment-command";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";

import {
  GetRequestPaymentsQuery
} from "../../../../repositories/financial-request/request-payments/queries/get-request-payments-query";
import { map, startWith } from 'rxjs/operators';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
import { FormArray } from '@angular/forms';

import { MatDialog } from '@angular/material/dialog';
import { AddInvoicesComponent } from './add-invoices/add-invoices.component';
import {  GetBursaryAccountReferencesQuery } from 'src/app/modules/bursary/repositories/financial-request/referenceAccount/queries/get-account-references-query';
import { PageModes } from 'src/app/core/enums/page-modes';
import { GetRequestPaymentQuery } from 'src/app/modules/bursary/repositories/financial-request/request-payments/queries/get-request-payment-query';
import { CreateRequestPaymentDocumentHeadCommand } from 'src/app/modules/bursary/repositories/financial-request/request-payment-document-head/commands/create-request-payment-document-head-command';
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-add-request-payment',
  templateUrl: './add-request-payment.component.html',
  styleUrls: ['./add-request-payment.component.scss']
})
export class AddRequestPaymentComponent extends BaseComponent {

  @ViewChild('rowActions', { read: TemplateRef }) rowActions!: TemplateRef<any>;

  @ViewChild('addDocHead', { read: TemplateRef }) addDocHead!: TemplateRef<any>;

  filteredAccountReferences:AccountReference[] = [];
  accountReferences !: AccountReference [] ;
  selectedReferenceAccount : AccountReference | undefined = undefined;
  referencesOption!: Observable<AccountReference[]>;
  tableConfigurations!: TableConfigurations;
  tempRequestsForm!: FormArray;
  isValue : boolean = false;
  closeResult = '';

  constructor(private _mediator:Mediator,
              public dialog: MatDialog) {
    super();
    this.request = new CreateRequestPaymentCommand()
  }

  async ngOnInit() {
    await this.resolve();
  }

 async resolve() {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.save,
      // FormActionTypes.saveandexit,
      FormActionTypes.list
    ]




    return await this._mediator.send(new GetBursaryAccountReferencesQuery(0,0)).then(res =>{
      if (res.data.length>0)
      this.accountReferences = res.data;
      this.searchReferenceAccounts(this.form);
    }
    );


  await this.initialize();
  }

  async initialize(entity?:RequestPayment) {

    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;

      if (!entity)
       entity = await this.get(this.getQueryParam('id'));

  //    this.request = new UpdateRequestPaymentCommand().mapFrom(entity);

    } else {
      this.request = new CreateRequestPaymentCommand();
   }
  }


  openAddDoc(item:RequestPayment) {
    const dialogRef = this.dialog.open(AddInvoicesComponent, {
      width:'80%',
      height:'80%',
      data: {data:JSON.stringify(item)},
    });

    dialogRef.afterClosed().subscribe(result => {

      let item = new CreateRequestPaymentDocumentHeadCommand().mapFrom(result);
        this.addRequestPaymentDocumentHeads(item);
    });
  }


  async add() {
    let response = await this._mediator.send(<CreateRequestPaymentCommand>this.request)
    return await this.initialize(response)
  }

  async update(param?: any) {

    let response = await this._mediator.send(<UpdateRequestPaymentCommand>param)
 //   return await this.initialize(response)
  }

async addRequestPaymentDocumentHeads(param:any)
{
  return await this._mediator.send(<CreateRequestPaymentDocumentHeadCommand>param)
}

  close(): any {
  }

  delete(param?: any): any {
  }

  async get(id: number) {
    return await this._mediator.send(new GetRequestPaymentQuery(id)).then(res => {
      return res
    })
  }

  async getAllByReferenceAccountId(){

    let searchQueries : SearchQuery[] = []
        searchQueries.push(new SearchQuery({
          propertyName: "ReferenceId",
          values: [this.form.controls["referenceId"].value.id],
          comparison: "equal"
        }))

    return await this._mediator.send(new GetRequestPaymentsQuery(0,0,searchQueries)).then(res =>{
      if (res.data.length>0)
      this.isValue = true;
      this.getTempPaymentReport(res.data);
    }
    );
  }

  reset() {
    super.reset();
    this.request = new CreateRequestPaymentCommand()
  }


  searchReferenceAccounts(form : any) {
    this.referencesOption = form.get('referenceId')?.valueChanges.pipe(
        startWith<string | AccountReference>(''),
        map((value) => {
            if (value == null)
                return "";
            if (typeof value === 'string')
                return value;
            else if (typeof value === 'number') {
                this.selectedReferenceAccount = this.accountReferences.find(x => x.id == value);
                return this.selectedReferenceAccount?.title;
            }
            else {

                this.selectedReferenceAccount =  value as AccountReference;
                return this.selectedReferenceAccount.title;
            }
        }),
        map((title) => {
            return title ? this._filter(title as string) : this.accountReferences.slice();
        })
    );
}

private _filter(title: string): AccountReference[] {
  const filterValue = title.toLowerCase();

  return this.accountReferences.filter((option) =>
      option.title.toLowerCase().includes(filterValue)
  );
}

referenceDisplayFn(accountReference: AccountReference): string {
  return accountReference && accountReference.title ? accountReference.title : '';
}

getTempPaymentReport(data:RequestPayment []){

  let colIssue  = new TableColumn(
    'issue',
    'صدور',
    TableColumnDataType.Template,
    '15%',
    true,

  );

  let colAddDocumentHead  = new TableColumn(
    'addDoc',
    'ثبت فاکتور',
    TableColumnDataType.Template,
    '15%',
    true,
    new TableColumnFilter('', TableColumnFilterTypes.Text),
  );


  colIssue.template = this.rowActions;
  colAddDocumentHead.template = this.addDocHead;
  let columns: TableColumn[] = [
    new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
    new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
    new TableColumn(
      'referenceTitle',
      'طرف حساب',
      TableColumnDataType.Text,
      '15%',
      true,
      new TableColumnFilter('referenceTitle', TableColumnFilterTypes.Text),

    ),
    new TableColumn(
      'requestNumber',
      'شماره عملیات مالی',
      TableColumnDataType.Text,
      '15%',
      true,
      new TableColumnFilter('requestNumber', TableColumnFilterTypes.Text),
    ),

    new TableColumn(
      'totalCost',
      'مبلغ',
      TableColumnDataType.Text,
      '15%',
      true,
      new TableColumnFilter('requestNumber', TableColumnFilterTypes.Text),
    ),

    new TableColumn(
      'description',
      'توضیحات',
      TableColumnDataType.Text,
      '15%',
      true,
      new TableColumnFilter('requestNumber', TableColumnFilterTypes.Text),
    ),
    colIssue,
    colAddDocumentHead

  ]
  this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
  this.tempRequestsForm = new FormArray(
    data.map( x => this.createForm(x))
  )
}



}
