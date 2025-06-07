
import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { EventEmitter, OnChanges, Output } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { MatMenuTrigger } from '@angular/material/menu';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-combo-code-voucher-groups',
  templateUrl: './combo-code-voucher-groups.component.html',
  styleUrls: ['./combo-code-voucher-groups.component.scss'],

})
export class ComboCodeVoucherGroupsComponent implements OnChanges {
  @ViewChild('input')
  input!: ElementRef<HTMLInputElement>;
  title = new FormControl('');

  public nodes: any[] = [];
  public nodes_filter: any[] = [];
  @Input() viewId: number = 0;
  @Input() code: string[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Input() tabindex: number = 1;
  @Output() SelectId = new EventEmitter<any>();

  
  constructor(private _mediator: Mediator
    , private Service: PagesCommonService
    , private ApiCallService: ApiCallService
    , public _notificationService: NotificationService

  ) {

  }
  
  async ngOnChanges() {

    //--------get data flat and convert to tree by component
    await this.initialize();


    //-----------------------نمایش مقدار اولیه----------------------
    if (this.nodes.length > 0 && this.DefaultId != undefined) {

      var item = this.nodes.find(a => a.id == Number(this.DefaultId));

      var value = item?.title;
      this.title.setValue(value);
      if (this.isDisable)
        this.title.disable()

    }
    if (this.DefaultId == undefined) {
      this.title.setValue(undefined);
    }
  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {

    if (this.viewId>0) {
      this.ApiCallService.getReceiptAllStatus('').then(a => {
        this.nodes = this.ApiCallService.ReceiptStatus.filter(a => a.viewId == this.viewId || this.viewId == 0);
        this.nodes_filter = this.ApiCallService.ReceiptStatus.filter(a => a.viewId == this.viewId || this.viewId == 0);
        if (this.nodes.length == 1) {

          this.onSelectNode(this.nodes[0].id)
        }
      })
    }
    else {
      this.ApiCallService.getReceiptAllStatus('').then(a => {

        for (let i = 0; this.code.length > i; i++) {
          this.ApiCallService.ReceiptStatus.filter(a => a.code.substring(2, 4) == this.code[i]).forEach(res => {
            this.nodes.push(res);
            this.nodes_filter.push(res);
          }
            )

        }
      })
    }

  }
  onSelectNode(id: any) {

    this.SelectId.emit(this.nodes.find(a => a.id == id));
    let value = this.nodes.find(a => a.id == id)?.title;

    this.title.setValue(value);
    this.DefaultId = id;

    if (this.title.value == undefined) {
      this.title.setValue(undefined);
      this.DefaultId = undefined
    }


  }
  onSearchTerm() {
    const filterValue = this.input.nativeElement.value.toLowerCase();
    this.nodes = this.nodes_filter.filter(o => o.title.toLowerCase().includes(filterValue));

  }

  onSelectionChange(event: any) {

    this.onSelectNode(event.option.value);
  }
}



