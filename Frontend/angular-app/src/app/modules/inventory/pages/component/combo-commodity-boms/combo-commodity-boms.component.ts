
import { EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { GetInvoiceActiveCommodityId } from '../../../../purchase/repositories/invoice/queries/invoice/get-invoices-active-commodityId-query';
import { GetInvoiceActiveRequestNo } from '../../../../purchase/repositories/invoice/queries/invoice/get-invoices-active-requestNo-query';
import { GetBomsByCommodityIdQuery } from '../../../repositories/commodity-categories/get-commodity-boms-query';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
  selector: 'app-combo-commodity-boms',
  templateUrl: './combo-commodity-boms.component.html',
  styleUrls: ['./combo-commodity-boms.component.scss']
})
export class ComboCommodityBomsComponent implements OnChanges {

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  openMenu() {
    this.trigger?.openMenu();
  }

  public nodes: any[] = [];
  public nodes_filter: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  //پر کردن اولین کالا برای اینکه بتوانیم پیش فاکتور آن را پیدا کنیم.
  //در حالتی که از سیستم آرانی درخواست های خرید خوانده شود و در سیستم ایفا ثبت نشده باشد.
  @Input() commodityId: any = undefined;//آیدی 
  @Output() SelectId = new EventEmitter<any>();
  @Input() tabindex: number = 1;
  title: string = "";

  searchTerm: string = "";
  constructor(
    private _mediator: Mediator,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,

  ) {

  }
  async ngOnChanges() {
    //-----------------------نمایش مقدار اولیه----------------------
    if ((this.title == "" && this.DefaultId != undefined && this.DefaultId!=0) || (this.title == "" && this.commodityId != undefined)) {

      await this.ReferenceFilter().then(res => {

        if (this.DefaultId != undefined) {
          var value = this.nodes.find(a => a.bomsHeaderId == Number(this.DefaultId))?.title;
          var bomDate = this.nodes.find(a => a.bomsHeaderId == Number(this.DefaultId))?.bomDate;
          if (value != undefined) {
            this.title = value + ' ' + this.Service.toPersianDate(bomDate) + ' ' + this.nodes.find(a => a.bomsHeaderId == Number(this.DefaultId))?.name;;
          }
          else {
            this.title = this.DefaultId;
          }
        }
        //-------------------------------------------------
        if (this.nodes.length == 1) {
          this.onSelectNode(this.nodes[0]?.bomsHeaderId)
        }
      })
    }
    if (this.DefaultId == undefined && this.nodes.length!=1) {
      this.title = '';
    }
    //اگر فرمول ساخت جدیدی به لیست اضافه شده باشد این کامپونت مجددا دیتاها را بخواند
    if (this.DefaultId == 0) {
      this.title = '';
      this.DefaultId = undefined;
      this.ReferenceFilter();
    }

  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {
    
      this.ReferenceFilter()
  }
  //اگر از طریق لیست انتخاب شود.
  onSelectNode(id: any) {
    
    this.SelectId.emit(this.nodes.find(a => a.bomsHeaderId == id));
    var bomDate = this.nodes.find(a => a.bomsHeaderId == id)?.bomDate;
    this.title = this.nodes.find(a => a.bomsHeaderId == id)?.title + ' ' + this.nodes.find(a => a.bomsHeaderId == id)?.name;
    
    this.DefaultId = id;
    if (this.title == undefined) {
      this.title = '';
    }
    this.searchTerm = "";

  }
  //اگر مستقیم مقدار شماره صورتحساب وارد شود و از لیست چیزی انتخاب نشود
  onEnterTitle() {
    this.SelectId.emit(this.title);
  }

  onSearchTerm() {

    if (this.searchTerm) {

      this.nodes = this.nodes_filter.filter(x =>
        x.title.toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    } else {
      this.nodes = [...this.nodes_filter]

    }

  }
  async ReferenceFilter() {

    
    if (this.commodityId != undefined) {
      await this._mediator.send(new GetBomsByCommodityIdQuery(this.commodityId, 0, 25, undefined,'')).then(res => {
        
        this.nodes = res
        this.nodes_filter = res;
        
      })
    }
    
  }

}



