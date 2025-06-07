
import { EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { GetInvoiceActiveCommodityId } from '../../../../purchase/repositories/invoice/queries/invoice/get-invoices-active-commodityId-query';
import { GetInvoiceActiveRequestNo } from '../../../../purchase/repositories/invoice/queries/invoice/get-invoices-active-requestNo-query';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
  selector: 'app-combo-invoice',
  templateUrl: './combo-invoice.component.html',
  styleUrls: ['./combo-invoice.component.scss']
})
export class ComboInvoiceComponent implements OnChanges {

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

  //--------آیدی درخواست خرید کالا--------------------------
  @Input() invoiceId: any = undefined;
  //پر کردن اولین کالا برای اینکه بتوانیم پیش فاکتور آن را پیدا کنیم.
  //در حالتی که از سیستم آرانی درخواست های خرید خوانده شود و در سیستم ایفا ثبت نشده باشد.
  @Input() commodityId: any = undefined;//آیدی 
  @Input() refrenceId: any = undefined;
  @Input() documentDate: Date = new Date();
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

    
    if ((this.invoiceId != undefined || this.commodityId != undefined) && (this.title == "" || this.title == undefined)) {
      await this.ReferenceFilter();
    }

    //-----------------------نمایش مقدار اولیه----------------------
    if (this.title == "" && this.DefaultId != undefined) {
     
      await this.ReferenceFilter().then(a => {

        var value = this.nodes.find(a => a.invoiceNo == Number(this.DefaultId))?.invoiceNo;
        if (value != undefined) {
          this.title = value;
          
        }
        else {
          this.title = this.DefaultId;
        }


      })

    }
    if (this.DefaultId == undefined) {
      this.title = '';
    }

  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {
    if (this.invoiceId != undefined) {
      
      await this.ReferenceFilter()
    }
     
     

  }
  //اگر از لیست طریق انتخاب شود.
  onSelectNode(id: any) {

    this.SelectId.emit(this.nodes.find(a => a.id == id));
    this.title = this.nodes.find(a => a.id == id)?.documentNo;
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
        x.documentNo.toString().includes(this.searchTerm)
        || x.invoiceNo.includes(this.searchTerm)
        || x.referenceTitle.includes(this.searchTerm)
        || x.referenceTitle.includes(this.searchTerm)
        || x.codeVoucherGroupTitle.includes(this.searchTerm)

      )
    } else {


      this.nodes = [...this.nodes_filter]

    }

  }
  async ReferenceFilter() {
        
    if (this.nodes_filter == undefined || this.nodes_filter.length == 0) {
      if (this.invoiceId != undefined) {
        
        await this._mediator.send(new GetInvoiceActiveRequestNo(this.invoiceId, this.refrenceId == null ? 0 : this.refrenceId, undefined, undefined, 0, 25, undefined)).then(res => {

          this.nodes = res.data
          this.nodes_filter = res.data;
        })
      }
      if (this.commodityId != undefined) {
       
        await this._mediator.send(new GetInvoiceActiveCommodityId(this.commodityId, this.refrenceId == null ? 0 : this.refrenceId, undefined, this.documentDate, 0, 25, undefined)).then(res => {

          this.nodes = res.data
          this.nodes_filter = res.data;
        })
      }
      
    }

   

  }
  onfocus() {

    setTimeout(function () {
      document.getElementById('searchTerm')?.focus();
    }, 0);
  }
}



