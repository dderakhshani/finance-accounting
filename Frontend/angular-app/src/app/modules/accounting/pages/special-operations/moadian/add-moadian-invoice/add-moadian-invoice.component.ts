import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {ActivatedRoute, Router} from "@angular/router";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {UpdateVouchersHeadCommand} from "../../../../repositories/voucher-head/commands/update-vouchers-head-command";
import {CreateVouchersHeadCommand} from "../../../../repositories/voucher-head/commands/create-vouchers-head-command";
import {
  UpdateMoadianInvoiceHeaderCommand
} from "../../../../repositories/moadian/moadian-invoice-header/commands/update-moadian-invoice-header-command";
import {
  CreateMoadianInvoiceHeaderCommand
} from "../../../../repositories/moadian/moadian-invoice-header/commands/create-moadian-invoice-header-command";
import {GetVoucherHeadQuery} from "../../../../repositories/voucher-head/queries/get-voucher-head-query";
import {
  GetMoadianInvoiceHeaderQuery
} from "../../../../repositories/moadian/moadian-invoice-header/queries/get-moadian-invoice-header-query";
import {
  CreateMoadianInvoiceDetailCommand
} from "../../../../repositories/moadian/moadian-invoice-detail/commands/create-moadian-invoice-detail-command";

@Component({
  selector: 'app-add-moadian-invoice',
  templateUrl: './add-moadian-invoice.component.html',
  styleUrls: ['./add-moadian-invoice.component.scss']
})
export class AddMoadianInvoiceComponent extends BaseComponent{

  systemModes = [
    {
      title:'اصلی',
      value:false
    },
    {
      title:'SANDBOX',
      value:true
    },

  ]
  intyTypes: any[] = [
    {
      title: 'الکترونیکی نوع اول',
      value: 1
    },
    {
      title: 'الکترونیکی نوع دوم',
      value: 2
    },
  ]
  inpTypes: any[] = [
    {
      title: 'فروش',
      value: 1
    },
    {
      title: 'صادرات',
      value: 7
    },
  ]
  insTypes: any[] = [
    {
      title: 'اصلی',
      value: 1
    },
    {
      title: 'اصلاحی',
      value: 2
    },
    {
      title: 'ابطالی',
      value: 3
    },
    {
      title: 'برگشت از فروش',
      value: 4
    },
  ]
  tobTypes: any[] = [
    {
      title: 'حقیقی',
      value: 1
    },
    {
      title: 'حقوقی',
      value: 2
    },
  ]
   statuses :any[] = [];
  headerInfoPanelState = true;
  sellerInfoPanelState = false;
  buyerInfoPanelState = false;
  errorInfoPanelState = false;
  constructor(
    private _mediator: Mediator,
    private _route: ActivatedRoute,
    private _router: Router,
  ) {
    super(_route, _router);
    this.request = new CreateMoadianInvoiceHeaderCommand();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.refresh(),
      PreDefinedActions.list(),
      PreDefinedActions.delete(),
    ]
  }

  async ngOnInit() {

    await this.resolve();
  }
  async resolve() {
    this.statuses = [
      {
        title:'ارسال شده',
        value:'SENT'
      },
      {
        title:'ثبت شده',
        value:'SUCCESS'
      },
      {
        title:'خطا',
        value:'FAILED'
      },
      {
        title:'در انتظار ثبت',
        value:'IN_PROGRESS'
      },
      {
        title:'اطلاعات نامعتبر',
        value:'INVALID_DATA'
      },
      {
        title:'رد شده',
        value:'DECLINED'
      },
      {
        title:'ارسال نشده',
        value:null
      },
    ]
    await this._mediator.send(new GetBaseValuesByUniqueNameQuery('moadianSettings')).then(async (res) => {
      await this.initialize()
    })
  }


  async initialize(entity?: any)  {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));

      this.request = new UpdateMoadianInvoiceHeaderCommand().mapFrom(entity);

    } else {
      let command = new CreateMoadianInvoiceHeaderCommand();
      this.request = command
    }
  }

  async add() {
    let response = await this._mediator.send(<CreateMoadianInvoiceHeaderCommand>this.request);
    await this.initialize(response);
    await this.addQueryParam('id', response.id);

  }

  async update() {
    let response = await this._mediator.send(<UpdateMoadianInvoiceHeaderCommand>this.request);
    await this.initialize(response);
  }
  async get(id: number) {
    return await this._mediator.send(new GetMoadianInvoiceHeaderQuery(id))
  }

  async reset() {
    await this.deleteQueryParam('id');
    return super.reset();
  }

  async navigateToMoadianInvoiceHeadersList() {
    await this._router.navigateByUrl('/accounting/specialOperations/moadianInvoicesList')
  }

  close(): any {
  }

  delete(param?: any): any {
  }

}
