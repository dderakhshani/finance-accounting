import { Component, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { IdentityService } from '../../../../identity/repositories/identity.service';
import { invoice } from '../../../entities/invoice';
import { GetInvoiceListIdQuery } from '../../../repositories/invoice/queries/invoice/get-invoice-list-Id-query';
import { GetInvoiceQuery } from '../../../repositories/invoice/queries/invoice/get-invoice-query';


@Component({
  selector: 'app-invoice-details',
  templateUrl: './invoice-details.component.html',
  styleUrls: ['./invoice-details.component.scss']
})
export class InvoiceDetailsComponent extends BaseComponent {


  public Invoice: invoice | undefined = undefined;

  public displayPage: string = "";

  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    private _mediator: Mediator) {
    super(route, router);

  }

  async ngOnInit() {
    await this.resolve()
  }


  async resolve(params?: any) {
    //this.formActions = [


    //  FormActionTypes.list,
    //];

    await this.initialize()

  }

  async initialize(entity?: any) {
    //-----------------------------------

    this.Invoice = await this.getData()
    this.displayPage = this.getQueryParam('displayPage')
  }
  async getData() {

    if (this.getQueryParam('id')) {
      return await this.get(this.getQueryParam('id'));
    }
    else {
      return await this.getListId(this.Service.ListId);
    }
  }
  async get(Id: number) {
    return await this._mediator.send(new GetInvoiceQuery(Id))
  }
  async getListId(ListId: string[]) {
    var req = new GetInvoiceListIdQuery();
    req.ListId = ListId;
    return await this._mediator.send(<GetInvoiceListIdQuery>req);

  }
  async navigateToInvoiceList() {

    await this.router.navigateByUrl('/purchase/contractList')
  }
  async update() {

  }


  add() {

  }
  async edit() {
  }

  async reset() {

  }
  close(): any {
  }

  delete(param?: any): any {
  }

}
