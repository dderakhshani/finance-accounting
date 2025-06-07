import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {BaseValue} from "../../../../../entities/base-value";
import {SaleAgent} from "../../../../../../sales/entities/SaleAgent";
import {AccountReferencesGroup} from "../../../../../../accounting/entities/account-references-group";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {GetSaleAgentsQuery} from "../../../../../../sales/repositories/sale-agents/queries/get-sale-agents-query";
import {
  GetAccountReferencesGroupsQuery
} from "../../../../../../accounting/repositories/account-reference-group/queries/get-account-references-groups-query";
import {SearchQuery} from "../../../../../../../shared/services/search/models/search-query";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {Customer} from "../../../../../../sales/entities/Customer";
import {UpdateCustomerCommand} from "../../../../../../sales/repositories/customers/commands/update-customer-command";
import {CreateCustomerCommand} from "../../../../../../sales/repositories/customers/commands/create-customer-command";
import {GetCustomerQuery} from "../../../../../../sales/repositories/customers/queries/get-customer-query";
import {DeleteCustomerCommand} from "../../../../../../sales/repositories/customers/commands/delete-customer-command";
import {Toastr_Service} from "../../../../../../../shared/services/toastrService/toastr_.service";

@Component({
  selector: 'app-person-customer-dialog',
  templateUrl: './person-customer-dialog.component.html',
  styleUrls: ['./person-customer-dialog.component.scss']
})
export class PersonCustomerDialogComponent extends BaseComponent {

  customerId!: number
  personId!: number
  pageModes = PageModes;
  customerTypes: BaseValue[] = []
  saleAgents: SaleAgent[] = []
  accountReferencesGroups: AccountReferencesGroup[] = []

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<PersonCustomerDialogComponent>,
    private _mediator: Mediator,
    private _toaster: Toastr_Service,
  ) {
    super()
    this.customerId = data.customerId;
    this.pageMode = data.pageMode;
    this.personId = data.personId
    this.request = new CreateCustomerCommand()
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    let sources: Promise<any>[] = [
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('CustomerTypeBaseId')),
      this._mediator.send(new GetSaleAgentsQuery(0, 0)),
      this._mediator.send(new GetAccountReferencesGroupsQuery(0, 0, [new SearchQuery({
        propertyName: 'code',
        comparison: 'equals',
        values: ['31']
      }),
        new SearchQuery({
          propertyName: 'code',
          comparison: 'equals',
          values: ['32']
        })])),
    ];
    forkJoin(sources).subscribe(async ([
                                         customerTypes,
                                         saleAgents,
                                         accountReferencesGroups
                                       ]) => {
      this.customerTypes = customerTypes
      this.saleAgents = saleAgents.data
      this.accountReferencesGroups = accountReferencesGroups.data
      await this.initialize()

    })
  }

  async initialize(entity?: Customer) {
    if (entity || this.customerId) {

      if (!entity) entity = await this.get(this.customerId);

      this.request = new UpdateCustomerCommand().mapFrom(<Customer>entity);
      if (this.pageMode === PageModes.Delete) {
        this.form.disable();
      }

    } else {
      this.pageMode = PageModes.Add;
      let request = new CreateCustomerCommand()
      request.personId = this.personId
      this.request = request;

    }

  }

  async add() {
    let response = await this._mediator.send(<CreateCustomerCommand>this.request)
    this.dialogRef.close()
  }

  async get(customerId: number) {
    return await this._mediator.send(new GetCustomerQuery(customerId))
  }

  async update() {
    let response = await this._mediator.send(<UpdateCustomerCommand>this.request);
    this.dialogRef.close()
  }

  close(): any {
  }

  async delete(param?: any): Promise<any> {
    const entity: Customer = await this.get(this.customerId);
    if (entity.id) {
      this.request = new DeleteCustomerCommand(entity.id);
      await this._mediator.send(<DeleteCustomerCommand>this.request).then(res => {
        this._toaster.showToast({
          type: 'success',
          message: 'عملیات با موفقیت انجام شد'
        })
      },)
    this.dialogRef.close()
    }
    else {
      return this._toaster.showToast({
        type: 'error',
        message: 'شخص انتخاب شده وجود ندارد'
      });
    }
  }






}
