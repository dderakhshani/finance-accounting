import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CreateChequeCommand} from "../../../repositories/cheque/commands/create-cheque-command";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {PageModes} from "../../../../../core/enums/page-modes";
import {
  UpdateVouchersHeadCommand
} from "../../../../accounting/repositories/voucher-head/commands/update-vouchers-head-command";
import {
  CreateVouchersHeadCommand
} from "../../../../accounting/repositories/voucher-head/commands/create-vouchers-head-command";
import {Cheque} from "../../../entities/cheque";
import {UpdateChequeCommand} from "../../../repositories/cheque/commands/update-cheque-command";
import {ActivatedRoute, Router} from "@angular/router";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetChequeQuery} from "../../../repositories/cheque/queries/get-cheque-query";
import {Bank} from "../../../entities/bank";
import {BankAccount} from "../../../entities/bank-account";
import {forkJoin} from "rxjs";
import {BankBranch} from "../../../entities/bank-branch";

@Component({
  selector: 'app-add-cheque',
  templateUrl: './add-cheque.component.html',
  styleUrls: ['./add-cheque.component.scss']
})
export class AddChequeComponent extends BaseComponent {


  banks:Bank[] = [];
  bankBranches:BankBranch[] = [];
  bankAccounts:BankAccount[] = [];
  filteredBankAccounts:BankAccount[] = [];

  constructor(
    private route:ActivatedRoute,
    private router:Router,
    private _mediator:Mediator
  ) {
    super(route,router);
    this.request = new CreateChequeCommand()
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

    // forkJoin([
    // ]).subscribe(async ([
    //
    //                   ]) => {

      this.bankAccounts.push({
        id:1,
        title:'hesabe jaari',
        branchId:1
      })
      this.filteredBankAccounts = this.bankAccounts;

      this.banks.push({
        id:1,
        title:'pasargad'
      })
      this.bankBranches.push({
        id:1,
        title:'zafar',
        bankId: 1
      })
      return await this.initialize()
    // })

  }

  async initialize(entity?:Cheque) {

    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateChequeCommand().mapFrom(entity);

    } else {
      this.request = new CreateChequeCommand();
    }
  }

  async add() {
    let response = await this._mediator.send(<CreateChequeCommand>this.request)
    return await this.initialize(response);
  }

  async update() {
    let response = await this._mediator.send(<UpdateChequeCommand>this.request)
    return await this.initialize(response)
  }


  close(): any {
  }

  delete(param?: any): any {
  }

  async get(id: number) {
    return await this._mediator.send(new GetChequeQuery(id)).then(res => {
      return res
    })
  }

  reset() {
    super.reset();
    this.request = new CreateChequeCommand()
  }

  getBankAccountFullDescription(id:number) {
    return this.bankAccounts.find(x => x.id === id)?.title ?? '';
  }

  filterBankAccounts(searchTerm:string) {
    if (searchTerm) {
      this.filteredBankAccounts = this.bankAccounts.filter(x => x.title.includes(searchTerm))
    } else {
      this.filteredBankAccounts = this.bankAccounts;
    }
  }

}
